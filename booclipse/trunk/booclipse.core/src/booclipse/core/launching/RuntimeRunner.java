package booclipse.core.launching;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.runtime.Platform;

import booclipse.core.BooCore;
import booclipse.core.foundation.ArrayUtilities;


public class RuntimeRunner {
	
	public static final String RUNTIME_EXECUTABLE = "bin/mono";

	private List _cmd = new ArrayList();
	
	private File _workingDir;
	
	public RuntimeRunner() throws IOException {
		assertRuntimeLocation();
		add(getOSDependentRuntimeExecutable(getRuntimeLocation()));
		add("--debug");
	}

	public void add(String arg) {
		_cmd.add(arg);
	}
	
	public void setWorkingDir(File dir) {
		_workingDir = dir;
	}
	
	public String runtimeRelativePath(String relativePath) throws IOException {
		String parent = getRuntimeLocation();
		return combinePath(parent, relativePath);
	}

	public String getRuntimeLocation() {
		return BooCore.getRuntimeLocation();
	}
	
	public Process launch() throws IOException {
		final String[] cmdLine = (String[]) _cmd.toArray(new String[_cmd.size()]);
		BooCore.logInfo(ArrayUtilities.join(cmdLine));
		return Runtime.getRuntime().exec(cmdLine, null, _workingDir);
	}
	
	private void assertRuntimeLocation() throws IOException {
		String runtimeLocation = getRuntimeLocation();
		if (null == runtimeLocation) {
			throw new IOException("runtime location is not set!");
		}
	}
	
	public static String getOSDependentRuntimeExecutable(String runtimeLocation) throws IOException {
		String path = combinePath(runtimeLocation, RUNTIME_EXECUTABLE);
		return Platform.OS_WIN32.equals(Platform.getOS())
			? path + ".exe"
			: path;
	}
	
	private static String combinePath(String parent, String relativePath) throws IOException {
		return new File(parent, relativePath).getCanonicalPath();
	}
}
