package booclipse.core.internal;

import java.io.IOException;

import org.eclipse.core.resources.IFile;

import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.launching.RuntimeRunner;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;

public class CompilerLauncher extends RuntimeRunner {
	
	public static final String COMPILER_EXECUTABLE = "lib/boo/booc.exe";
	
	public CompilerLauncher() throws IOException {
		add(runtimeRelativePath(COMPILER_EXECUTABLE));
	}
	
	public CompilerLauncher(IBooAssemblySource source) throws IOException {
		this();
		add("-utf8");	
		setOutputFile(source.getOutputFile());
		setOutputType(source.getOutputType());
		addReferences(source.getReferences());
	}
	
	public void setPipeline(String pipeline) {
		add("-p:" + pipeline);
	}
	
	public void setOutputType(String outputType) {
		add("-t:" + outputType);
	}
	
	public void setOutputFile(IFile outputFile) {
		add("-o:" + WorkspaceUtilities.getLocation(outputFile));
	}
	
	public void addReferences(IBooAssemblyReference[] references) {		
		for (int i=0; i<references.length; ++i) {
			add("-r:" + references[i].getCompilerReference());
		}
	}
	
	public void addSourceFiles(IFile[] files) {
		for (int i=0; i<files.length; ++i) {
			add(WorkspaceUtilities.getLocation(files[i]));
		}
	}
}
