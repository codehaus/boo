package booclipse.core;

import org.eclipse.core.runtime.CoreException;

import booclipse.core.model.IAssemblySourceReference;
import booclipse.core.model.IBooAssemblyReferenceVisitor;
import booclipse.core.model.IGlobalAssemblyCacheReference;
import booclipse.core.model.ILocalAssemblyReference;

public class BooAssemblyReferenceVisitor implements
		IBooAssemblyReferenceVisitor {
	
	public boolean visit(ILocalAssemblyReference reference) throws CoreException {
		return true;
	}

	public boolean visit(IGlobalAssemblyCacheReference reference) throws CoreException {
		return true;
	}

	public boolean visit(IAssemblySourceReference reference) throws CoreException {
		return true;
	}

}
