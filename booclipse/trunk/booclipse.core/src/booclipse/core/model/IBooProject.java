package booclipse.core.model;

import org.eclipse.core.resources.IResourceDelta;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;

public interface IBooProject {

	IBooAssemblySource addAssemblySource(IPath path)
			throws CoreException;

	IBooAssemblySource[] getAssemblySources()
			throws CoreException;

	IBooAssemblySource[] getAffectedAssemblySources(
			IResourceDelta delta) throws CoreException;

	IBooAssemblySource[] getAssemblySourceOrder(IBooAssemblySource[] sources) throws CoreException;

}