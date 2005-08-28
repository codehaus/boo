package booclipse.core.interpreter;

import java.io.IOException;

import org.eclipse.core.runtime.CoreException;

import booclipse.core.compiler.AbstractBooServiceClient;
import booclipse.core.launching.IProcessMessageHandler;
import booclipse.core.launching.ProcessMessage;

public class InteractiveInterpreter extends AbstractBooServiceClient {
	
	IInterpreterListener _listener;
	
	public InteractiveInterpreter() throws CoreException {
		setMessageHandler("EVAL-FINISHED", new IProcessMessageHandler() {
			public void handle(ProcessMessage message) {
				if (null == _listener) return;
				_listener.evalFinished(message.payload);
			}
		});
	}
	
	public void eval(String code) throws IOException {
		send("EVAL", code);
	}

	protected String getProposalsResponseMessageId() {
		return "INTERPRETER-PROPOSALS";
	}

	protected String getProposalsMessageId() {
		return "GET-INTERPRETER-PROPOSALS";
	}

	public void addListener(IInterpreterListener listener) {
		if (null != _listener) throw new IllegalStateException("only a single listener is supported");
		_listener = listener;
	}
	
}
