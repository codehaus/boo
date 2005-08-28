package booclipse.core.model;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdaptable;
import org.eclipse.core.runtime.IProgressMonitor;

public interface IBooAssemblySource extends IAdaptable {
	
	public interface OutputType {
		public static final String CONSOLE_APPLICATION = "exe";
	
		public static final String WINDOWS_APPLICATION = "winexe";
	
		public static final String LIBRARY = "library";
	}

	IFolder getFolder();

	IFile[] getSourceFiles() throws CoreException;

	void setReferences(IBooAssemblyReference[] references);

	IBooAssemblyReference[] getReferences();
	
	boolean visitReferences(IBooAssemblyReferenceVisitor visitor) throws CoreException;

	String getOutputType();

	void setOutputType(String outputType);

	IFile getOutputFile();

	void refresh(IProgressMonitor monitor) throws CoreException;

	void save(IProgressMonitor monitor) throws CoreException;
}