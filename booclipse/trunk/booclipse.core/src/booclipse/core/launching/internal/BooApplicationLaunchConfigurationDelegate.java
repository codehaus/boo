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
import booclipse.core.launching.BooLauncher;
import booclipse.core.launching.RuntimeRunner;
import booclipse.core.model.IBooAssemblySource;

public class BooApplicationLaunchConfigurationDelegate extends AbstractBooLaunchConfigurationDelegate {

	public void launch(ILaunchConfiguration configuration, String mode,
			ILaunch launch, IProgressMonitor monitor) throws CoreException {
		logInfo("BooApplicationLaunchConfigurationDelegate.launch");
		
		IBooAssemblySource source = BooLauncher.getConfiguredAssemblySource(configuration);
		try {
			launch.addProcess(DebugPlugin.newProcess(launch,
					launchApp(source.getOutputFile()), configuration.getName()));
		} catch (IOException e) {
			BooCore.logException(e);
			WorkspaceUtilities.throwCoreException(e);
		}
	}

	private Process launchApp(IFile file) throws IOException {
		RuntimeRunner runner = new RuntimeRunner();
		runner.add(WorkspaceUtilities.getLocation(file));
		return runner.launch();
	}
}
