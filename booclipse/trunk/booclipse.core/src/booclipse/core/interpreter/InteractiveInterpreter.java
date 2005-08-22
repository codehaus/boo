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
import booclipse.core.compiler.CompilerProposalsMessageHandler;
import booclipse.core.launching.BooLauncher;
import booclipse.core.launching.IProcessMessageHandler;
import booclipse.core.launching.ProcessMessage;
import booclipse.core.launching.ProcessMessenger;
import booclipse.core.model.IBooLaunchConfigurationTypes;

public class InteractiveInterpreter {
	
	ProcessMessenger _messenger;
	
	IInterpreterListener _listener;

	private CompilerProposalsMessageHandler _proposalsHandler;
	
	public InteractiveInterpreter() throws CoreException {
		_messenger = new ProcessMessenger(createLaunchConfiguration());
		_proposalsHandler = new CompilerProposalsMessageHandler();
		_messenger.setMessageHandler("PROPOSALS", _proposalsHandler);
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
	
	public CompilerProposal[] getCompletionProposals(String code, int offset) throws IOException {
		
		CompilerProposal[] proposals = null;
		try {
			Object lock = _proposalsHandler.getMessageLock();
			synchronized (lock) {
				try {
					_messenger.send(createMessage("GET-PROPOSALS", code + "__codecomplete__"));				
					lock.wait(_messenger.getTimeout());
					proposals = _proposalsHandler.getProposals();
				} catch (Exception e) {
					BooCore.logException(e);
				}
			}
		} finally {
			_messenger.setMessageHandler("PROPOSALS", null);
		}
		
		return proposals;
	}

	public void addListener(IInterpreterListener listener) {
		if (null != _listener) throw new IllegalStateException("only a single listener is supported");
		_listener = listener;
	}
	
	public static ILaunchConfiguration createLaunchConfiguration() throws CoreException {
		ILaunchConfigurationType configType = BooLauncher.getLaunchConfigurationType(IBooLaunchConfigurationTypes.ID_INTERPRETER_SUPPORT);
		ILaunchConfigurationWorkingCopy wc = configType.newInstance(null, "interpreter support");
		wc.setAttribute(DebugPlugin.ATTR_CAPTURE_OUTPUT, true);
		return wc;
	}
}
