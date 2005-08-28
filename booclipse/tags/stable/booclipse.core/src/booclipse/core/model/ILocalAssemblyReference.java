package booclipse.core.model;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.IAdaptable;

public interface ILocalAssemblyReference extends IBooAssemblyReference, IAdaptable {
	IFile getFile();
}
