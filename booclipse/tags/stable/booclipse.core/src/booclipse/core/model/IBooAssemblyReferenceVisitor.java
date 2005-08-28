package booclipse.core.model;

import org.eclipse.core.runtime.CoreException;

public interface IBooAssemblyReferenceVisitor {
	boolean visit(ILocalAssemblyReference reference) throws CoreException;
	boolean visit(IGlobalAssemblyCacheReference reference) throws CoreException;
	boolean visit(IAssemblySourceReference reference) throws CoreException;
}
