package booclipse.core.launching.internal;

import java.io.IOException;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.debug.core.DebugPlugin;
import org.eclipse.debug.core.ILaunch;
import org.eclipse.debug.core.ILaunchConfiguration;

import booclipse.core.BooCore;
import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.internal.CompilerLauncher;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooLaunchConfigurationConstants;

public class BooScriptLaunchConfigurationDelegate extends AbstractBooLaunchConfigurationDelegate {

	public void launch(ILaunchConfiguration configuration, String mode,
			ILaunch launch, IProgressMonitor monitor) throws CoreException {
		System.out.println("launch");
		try {

			launch.addProcess(DebugPlugin.newProcess(launch,
					launchScript(configuration), configuration.getName()));
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	private Process launchScript(ILaunchConfiguration configuration)
			throws CoreException, IOException {
		IFile scriptFile = getScriptFile(configuration);

		CompilerLauncher launcher = new CompilerLauncher();
		launcher.setPipeline("run");
		launcher.addSourceFiles(new IFile[] { scriptFile });
		launcher.setWorkingDir(scriptFile.getParent().getLocation().toFile());

		IBooAssemblySource container = BooCore.getAssemblySourceContainer(scriptFile);
		if (null != container) launcher.addReferences(container.getReferences());
		return launcher.launch();
	}

	private IFile getScriptFile(ILaunchConfiguration configuration)
			throws CoreException {
		return WorkspaceUtilities.getFile(configuration.getAttribute(
				IBooLaunchConfigurationConstants.ATTR_SCRIPT_PATH, ""));
	}

}
