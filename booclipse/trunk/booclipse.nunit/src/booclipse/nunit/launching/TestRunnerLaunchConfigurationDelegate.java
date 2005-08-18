package booclipse.nunit.launching;

import java.io.File;
import java.io.IOException;
import java.net.URL;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.Path;
import org.eclipse.core.runtime.Platform;
import org.eclipse.debug.core.DebugPlugin;
import org.eclipse.debug.core.ILaunch;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.core.model.ILaunchConfigurationDelegate;

import booclipse.core.launching.BooLauncher;
import booclipse.core.launching.RuntimeRunner;
import booclipse.nunit.NUnitPlugin;

public class TestRunnerLaunchConfigurationDelegate implements ILaunchConfigurationDelegate {

	public void launch(ILaunchConfiguration configuration, String mode, ILaunch launch, IProgressMonitor monitor) throws CoreException {
		
		try {
			RuntimeRunner runner = new RuntimeRunner();
			runner.add(getTestRunnerLocation());
			runner.add(BooLauncher.getProcessMessengerPort(configuration));
			launch.addProcess(DebugPlugin.newProcess(launch, runner.launch(), configuration.getName()));
		} catch (IOException e) {
			NUnitPlugin.logException(e);
		}	
	}
	
	private String getTestRunnerLocation() throws IOException {
		URL url = NUnitPlugin.getDefault().find(new Path("bin/TestClientRunner.exe"));
		return new File(Platform.asLocalURL(url).getFile()).getCanonicalPath();
	}

}