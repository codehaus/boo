package booclipse.core.launching;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.net.InetAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.HashMap;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.ISafeRunnable;
import org.eclipse.core.runtime.IStatus;
import org.eclipse.core.runtime.Platform;
import org.eclipse.core.runtime.Status;
import org.eclipse.core.runtime.jobs.Job;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.core.ILaunchConfigurationWorkingCopy;

import booclipse.core.BooCore;
import booclipse.core.model.IBooLaunchConfigurationConstants;

public class ProcessMessenger {
	
	private final Object _socketMutex = new Object();
	
	private Socket _socket;
	
	private final Map _handlers = new HashMap();

	private ILaunchConfiguration _configuration;
	
	private int _timeout = 5000;
	
	public ProcessMessenger(ILaunchConfiguration configuration) {
		_configuration = configuration;
	}
	
	public void setTimeout(int timeout) {
		_timeout = timeout;
	}
	
	public int getTimeout() {
		return _timeout;
	}
	
	public synchronized void setMessageHandler(String messageName, IProcessMessageHandler handler) {
		if (null == handler) {
			_handlers.remove(messageName);
		} else {
			_handlers.put(messageName, handler);
		}
	}
	
	public void send(String name, String payload) throws IOException {
		if (null == name || null == payload) throw new IllegalArgumentException();
		send(new ProcessMessage(name, payload));
	}
	
	public void send(ProcessMessage message) throws IOException {
		synchronized (_socketMutex) {
			if (null == _socket) {
				launch();
				try {
					// always wait a little longer
					// than the socket timeout
					_socketMutex.wait(_timeout*2);
				} catch (InterruptedException x) {
					BooCore.logException(x);
					throw new RuntimeException(x);
				}
				if (null == _socket) {
					throw new IOException("no connection from process");
				}
			}
			doSend(message);
		}
	}

	private void doSend(ProcessMessage message) throws IOException {
		message.writeTo(buffered(_socket.getOutputStream()));
	}

	public void unload() {
		synchronized (_socketMutex) {
			if (null == _socket) return;
			try {
				doSend(new ProcessMessage("QUIT", ""));
				try {
					_socketMutex.wait(500);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				if (null != _socket) {
					_socket.close();
				}
			} catch (IOException e) {
				BooCore.logException(e);
			}
			finally {
				_socket = null;
			}
		}
	}
	
	public void dispose() {
		try {
			unload();
		} catch (Exception x) {
			BooCore.logException(x);
		}
	}
	
	private void launch() throws IOException {
		
		Job job = new Job("ProcessMessenger [" + _configuration.getName() + "]") {
			protected IStatus run(IProgressMonitor monitor) {
				try {
					final int portNumber = findAvailablePort();
					launchConfiguration(portNumber, monitor);
					listen(monitor, portNumber);
				} catch (Exception x) {
					BooCore.logException(x);
				}
				return Status.OK_STATUS;
			}

			private void launchConfiguration(int portNumber, IProgressMonitor monitor) throws CoreException {
				ILaunchConfigurationWorkingCopy workingCopy = _configuration.getWorkingCopy();
				workingCopy.setAttribute(IBooLaunchConfigurationConstants.ATTR_PROCESS_MESSENGER_PORT, portNumber);
				workingCopy.launch("run", monitor);
			}
		};
		job.setPriority(Job.LONG);
		job.setSystem(true);
		job.schedule();
	}
	
	private int findAvailablePort() {
		for (int i=0xB00; i<0xBFF; ++i) {
			if (isPortAvailable(i)) return i;
		}
		return 0xABBA;
	}
	
	private static boolean isPortAvailable(int port) {
		try {
			ServerSocket a = new ServerSocket(port-1);
			try {
				ServerSocket b = new ServerSocket(port);
				b.close();
				return true;
			} catch (IOException ioe) {
			}
			finally {
				a.close();
			}
		} catch (IOException ioe) {
		}
		return false;
	}
	
	private void listen(IProgressMonitor monitor, int portNumber) {
		try {
			InetAddress address = InetAddress.getByName("127.0.0.1");
			ServerSocket server = new ServerSocket(portNumber, 50, address);
			server.setSoTimeout(_timeout);
			try {
				synchronized (_socketMutex) {
					_socket = server.accept();
					_socketMutex.notify();
				}
				try {
					while (!monitor.isCanceled()) {
						ProcessMessage message = readMessage(monitor);
						if (null == message) break;
						if (message.name.equals("QUIT")) {
							synchronized (_socketMutex) {
								_socketMutex.notify();
							}
							break;
						}
						handle(message);
					}
				} finally {
					unload();
				}
			} finally {
				server.close();
			}
		} catch (IOException e) {
			BooCore.logException(e);
		}
	}
	
	private synchronized void handle(final ProcessMessage message) {
		final IProcessMessageHandler handler = (IProcessMessageHandler) _handlers.get(message.name); 
		if (null == handler) return;
		Platform.run(new ISafeRunnable() {
			public void handleException(Throwable exception) {
			}

			public void run() throws Exception {
				handler.handle(message);
			}
		});
	}

	private ProcessMessage readMessage(IProgressMonitor monitor) throws IOException {
		BufferedReader reader = buffered(_socket.getInputStream());
		StringWriter buffer = new StringWriter();
		PrintWriter writer = new PrintWriter(buffer);
		String name = reader.readLine();
		while (true) {
			if (monitor.isCanceled()) return null;
			String line = reader.readLine();
			if (null == line) return null;
			if (line.equals(ProcessMessage.END_MARKER)) break;
			if (line.endsWith(ProcessMessage.END_MARKER)) {
				writer.println(line.substring(0, line.length() - ProcessMessage.END_MARKER.length()));
				break;
			}	
			writer.println(line);
		}
		return new ProcessMessage(name, buffer.getBuffer().toString());
	}

	private BufferedWriter buffered(final OutputStream outputStream) {
		return new BufferedWriter(new OutputStreamWriter(outputStream));
	}

	private BufferedReader buffered(final InputStream inputStream) {
		return new BufferedReader(new InputStreamReader(inputStream));
	}		
}
