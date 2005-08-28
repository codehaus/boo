package booclipse.ui.launching;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.ui.DebugUITools;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IFileEditorInput;

import booclipse.core.launching.BooLauncher;
import booclipse.ui.BooUI;

public class BooScriptLaunchShortcut extends AbstractBooLaunchShortcut {

	public void launch(IEditorPart editor, String mode) {
		IFileEditorInput editorInput = (IFileEditorInput) editor
				.getEditorInput();
		launch(editorInput.getFile(), mode);
		
	}

	protected void launch(IFile file, String mode) {
		BooUI.logInfo("Launching '" + file.getLocation() + "' as a script.");
		
		try {
			ILaunchConfiguration configuration = BooLauncher.getScriptLaunchConfiguration(file);
			DebugUITools.launch(configuration, mode);
		} catch (CoreException e) {
			BooUI.logException(e);
		}
	}
}
