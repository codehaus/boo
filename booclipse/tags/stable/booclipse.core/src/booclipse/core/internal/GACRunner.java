package booclipse.core.internal;

import java.io.IOException;

import booclipse.core.launching.RuntimeRunner;

public class GACRunner extends RuntimeRunner {
	
	public static final String PATH_GACUTIL = "lib/mono/1.0/gacutil.exe";
	
	public GACRunner(String cmd) throws IOException {
		add(runtimeRelativePath(PATH_GACUTIL));
		add(cmd);
	}

}
