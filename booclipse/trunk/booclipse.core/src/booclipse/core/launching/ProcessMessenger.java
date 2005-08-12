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

import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.ISafeRunnable;
import org.eclipse.core.runtime.IStatus;
import org.eclipse.core.runtime.Platform;
import org.eclipse.core.runtime.Status;
import org.eclipse.core.runtime.jobs.Job;
import org.eclipse.debug.core.ILaunchConfiguration;

import booclipse.core.BooCore;

public class ProcessMessenger {
	
	private final Object _socketMutex = new Object();
	
	private Socket _socket;

	private int _portNumber;
	
	private final Map _handlers = new HashMap();

	private ILaunchConfiguration _configuration;
	
	private int _timeout = 3000;
	
	public ProcessMessenger(ILaunchConfiguration configuration, int portNumber) {
		_configuration = configuration;
		_portNumber = portNumber;
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
					_socketMutex.wait(_timeout);
				} catch (InterruptedException x) {
					BooCore.logException(x);
					throw new RuntimeException(x);
				}
				if (null == _socket) {
					throw new IOException("no connection from process");
				}
			}
			message.writeTo(buffered(_socket.getOutputStream()));
		}
	}

	public void unload() {
		synchronized (_socketMutex) {
			if (null == _socket) return;
			try {
				_socket.close();
			} catch (IOException e) {
				BooCore.logException(e);
			} finally {
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
		
		// TODO: stop job when process is stopped
		// TODO: stop process when job is stopped
		Job job = new Job("ProcessMessenger [" + _configuration.getName() + "]") {
			protected IStatus run(IProgressMonitor monitor) {
				try {
					_configuration.launch("run", monitor);
					connect(monitor);
				} catch (Exception x) {
					//return new Status(Status.ERROR, BooCore.ID_PLUGIN, -1, x.getMessage(), x);
					BooCore.logException(x);
				}
				return Status.OK_STATUS;
			}
		};
		job.setPriority(Job.LONG);
		job.setSystem(true);
		job.schedule();
	}

	private void connect(IProgressMonitor monitor) {
		try {
			InetAddress address = InetAddress.getByName("127.0.0.1");
			//ServerSocket server = new ServerSocket(_portNumber, 50, InetAddress.getLocalHost());
			ServerSocket server = new ServerSocket(_portNumber, 50, address);
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
