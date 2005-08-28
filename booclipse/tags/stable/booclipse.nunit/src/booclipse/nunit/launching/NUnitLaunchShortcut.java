package booclipse.nunit.launching;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.core.ILaunchConfigurationType;
import org.eclipse.debug.ui.DebugUITools;
import org.eclipse.debug.ui.ILaunchShortcut;
import org.eclipse.jface.viewers.ISelection;
import org.eclipse.ui.IEditorPart;
import org.eclipse.ui.IFileEditorInput;

import booclipse.core.BooCore;
import booclipse.core.launching.BooLauncher;
import booclipse.core.model.IBooAssemblySource;
import booclipse.nunit.INUnitLaunchConfigurationTypes;
import booclipse.nunit.NUnitPlugin;

public class NUnitLaunchShortcut implements ILaunchShortcut {

	public void launch(ISelection selection, String mode) {
		// TODO Auto-generated method stub

	}

	public void launch(IEditorPart editor, String mode) {
		IFileEditorInput editorInput = (IFileEditorInput) editor.getEditorInput();
		
		try {
			run(editorInput.getFile(), mode);
		} catch (CoreException e) {
			NUnitPlugin.logException(e);
		}
	}

	private void run(IFile file, String mode) throws CoreException {
		final IBooAssemblySource source = BooCore.getAssemblySourceContainer(file);
		
		ILaunchConfigurationType configType = BooLauncher.getLaunchConfigurationType(INUnitLaunchConfigurationTypes.ID_NUNIT);
		ILaunchConfiguration configuration = BooLauncher.findAssemblySourceLaunchConfiguration(source, configType);
		if (null == configuration) {
			configuration = BooLauncher.createAssemblySourceLaunchConfiguration(source, configType);
		}
		DebugUITools.launch(configuration, mode);
	}

}
