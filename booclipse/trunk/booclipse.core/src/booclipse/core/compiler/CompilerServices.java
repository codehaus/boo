package booclipse.core.compiler;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.StringReader;

import org.eclipse.core.runtime.CoreException;

import booclipse.core.interpreter.InteractiveInterpreter;
import booclipse.core.launching.IProcessMessageHandler;
import booclipse.core.launching.ProcessMessage;
import booclipse.core.launching.ProcessMessenger;

public class CompilerServices {

	public static synchronized CompilerServices getInstance() throws CoreException {
		if (null == _instance) {
			_instance = new CompilerServices();
		}
		return _instance;
	}
	
	private static CompilerServices _instance;
	
	private ProcessMessenger _messenger;
	
	private Object _outlineMutex = new Object();
	
	OutlineNode _outline;

	private CompilerServices() throws CoreException {
		_messenger = new ProcessMessenger(InteractiveInterpreter.createLaunchConfiguration());
		_messenger.setMessageHandler("OUTLINE-RESPONSE", new IProcessMessageHandler() {
			public void handle(ProcessMessage message) {
				updateOutline(message.payload);
			}
		});
	}

	public OutlineNode getOutline(String text) throws IOException {
		synchronized (_outlineMutex) {
			_messenger.send("GET-OUTLINE", text);
			try {
				_outlineMutex.wait(3000);
			} catch (InterruptedException e) {
				e.printStackTrace();
				return null;
			}
		}
		return _outline;
	}
	
	void updateOutline(String outline) {
		synchronized (_outlineMutex) {
			_outline = parseOutline(outline);
			_outlineMutex.notify();
		}
	}

	private OutlineNode parseOutline(String text) {
		
		OutlineNode node = new OutlineNode();
		
		BufferedReader reader = new BufferedReader(new StringReader(text));
		String line = null;
		try {
			while (null != (line = reader.readLine())) {
				if (line.equals("BEGIN-NODE")) {
					node = node.create();
				} else if (line.equals("END-NODE")) {
					node = node.parent();
				} else {
					String[] parts = line.split(":");
					node.type(parts[0]);
					node.name(parts[1]);
					node.line(Integer.parseInt(parts[2]));
				}
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
		return node;
	}

	public void dispose() {
		_messenger.dispose();
	}

}
