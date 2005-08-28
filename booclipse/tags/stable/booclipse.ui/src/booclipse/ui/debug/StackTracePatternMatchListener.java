package booclipse.ui.debug;

import org.eclipse.core.resources.IFile;
import org.eclipse.debug.ui.console.FileLink;
import org.eclipse.jface.text.BadLocationException;
import org.eclipse.ui.console.IPatternMatchListenerDelegate;
import org.eclipse.ui.console.PatternMatchEvent;
import org.eclipse.ui.console.TextConsole;

import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.ui.BooUI;

public class StackTracePatternMatchListener implements IPatternMatchListenerDelegate {

	private TextConsole _console;

	public void connect(TextConsole console) {
		_console = console;
	}

	public void disconnect() {
		_console = null;
	}

	public void matchFound(PatternMatchEvent event) {
		
		final int offset = event.getOffset() + 4;
		final int length = event.getLength() - 5;
		
		String match = null;
		try {
			match = _console.getDocument().get(offset, length);
		} catch (BadLocationException e) {
			BooUI.logException(e);
			return;
		}
		
		int position = match.lastIndexOf(':');
		String fname = match.substring(0, position);
		final int lineNumber = Integer.parseInt(match.substring(position+1));
		final IFile file = WorkspaceUtilities.getFileForLocation(fname);
		if (null != file) {
			try {
				FileLink link = new FileLink(file, null, -1, -1, lineNumber);
				_console.addHyperlink(link, offset, length);
			} catch (BadLocationException e) {
				BooUI.logException(e);
			}
		}		
	}
}
