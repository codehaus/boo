package booclipse.core.launching.internal;

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

import booclipse.core.BooCore;
import booclipse.core.launching.RuntimeRunner;

public class InterpreterLaunchConfigurationDelegate implements ILaunchConfigurationDelegate {

	public void launch(ILaunchConfiguration configuration, String mode, ILaunch launch, IProgressMonitor monitor) throws CoreException {
		
		try {
			RuntimeRunner runner = new RuntimeRunner();
			runner.add(getInterpreterLocation());
			runner.add(Integer.toString(configuration.getAttribute("port", 0xB00)));
			launch.addProcess(DebugPlugin.newProcess(launch, runner.launch(), configuration.getName()));
		} catch (IOException e) {
			BooCore.logException(e);
		}	
	}
	
	private String getInterpreterLocation() throws IOException {
		URL url = BooCore.getDefault().find(new Path("bin/interpreter.exe"));
		return new File(Platform.asLocalURL(url).getFile()).getCanonicalPath();
	}

}
