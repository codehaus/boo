package booclipse.core.interpreter;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.debug.core.DebugPlugin;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.core.ILaunchConfigurationType;
import org.eclipse.debug.core.ILaunchConfigurationWorkingCopy;

import booclipse.core.BooCore;
import booclipse.core.launching.BooLauncher;
import booclipse.core.launching.IProcessMessageHandler;
import booclipse.core.launching.ProcessMessage;
import booclipse.core.launching.ProcessMessenger;
import booclipse.core.model.IBooLaunchConfigurationTypes;

public class InteractiveInterpreter {

	ProcessMessenger _messenger;
	
	IInterpreterListener _listener;
	
	public InteractiveInterpreter() throws CoreException {
		_messenger = new ProcessMessenger(createLaunchConfiguration(), 0xB00);
		_messenger.setMessageHandler("EVAL-FINISHED", new IProcessMessageHandler() {
			public void handle(ProcessMessage message) {
				if (null == _listener) return;
				_listener.evalFinished(message.payload);
			}
		});
	}

	public void eval(String code) throws IOException {
		_messenger.send(createMessage("EVAL", code));
	}

	private ProcessMessage createMessage(String name, String code) {
		return new ProcessMessage(name, code);
	}

	public void unload() {
		try {
			_messenger.unload();
		} catch (Exception x) {
			BooCore.logException(x);
		}
	}

	public void dispose() {
		_messenger.dispose();
	}
	
	public InterpreterProposal[] getCompletionProposals(String code, int offset) throws IOException {
		final ArrayList proposals = new ArrayList();
		_messenger.setMessageHandler("PROPOSALS", new IProcessMessageHandler() {
			public void handle(ProcessMessage response) {
				synchronized (proposals) {
					BufferedReader reader = new BufferedReader(new StringReader(response.payload));	
					String line;
					try {
						while (null != (line = reader.readLine())) {
							String[] parts = line.split(":");
							proposals.add(new InterpreterProposal(parts[0], parts[1], parts[2]));
						}
					} catch (IOException unexpected) {
						BooCore.logException(unexpected);
					}
					proposals.notify();
				}
			}
		});
		
		try {
			synchronized (proposals) {
				try {
					_messenger.send(createMessage("GET-PROPOSALS", code + "__codecomplete__"));				
					proposals.wait(_messenger.getTimeout());
				} catch (Exception e) {
					BooCore.logException(e);
				}
			}
		} finally {
			_messenger.setMessageHandler("PROPOSALS", null);
		}
		
		return (InterpreterProposal[]) proposals.toArray(new InterpreterProposal[proposals.size()]);
	}

	public void addListener(IInterpreterListener listener) {
		if (null != _listener) throw new IllegalStateException("only a single listener is supported");
		_listener = listener;
	}
	
	private ILaunchConfiguration createLaunchConfiguration() throws CoreException {
		ILaunchConfigurationType configType = BooLauncher.getLaunchConfigurationType(IBooLaunchConfigurationTypes.ID_INTERPRETER_SUPPORT);
		ILaunchConfigurationWorkingCopy wc = configType.newInstance(null, "interpreter support");
		wc.setAttribute(DebugPlugin.ATTR_CAPTURE_OUTPUT, true);
		return wc;
	}
}
