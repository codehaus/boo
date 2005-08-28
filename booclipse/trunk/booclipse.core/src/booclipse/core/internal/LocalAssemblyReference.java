package booclipse.core.internal;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.Path;

import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblyReferenceVisitor;
import booclipse.core.model.ILocalAssemblyReference;
import booclipse.core.model.IMemorable;
import booclipse.core.model.IRemembrance;

public class LocalAssemblyReference implements ILocalAssemblyReference {
	
	IFile _reference;

	LocalAssemblyReference(IFile reference) {
		_reference = reference;
	}

	public Object getAdapter(Class adapter) {
		if (adapter.isAssignableFrom(IFile.class)) {
			return _reference;
		}
		return null;
	}

	public String getAssemblyName() {
		return _reference.getName();
	}

	public IFile getFile() {
		return _reference;
	}

	public String getCompilerReference() {
		return _reference.getLocation().toOSString();
	}

	public String getType() {
		return IBooAssemblyReference.LOCAL;
	}
	
	static class Remembrance implements IRemembrance {
		
		public String path;

		public Remembrance(String path) {
			this.path = path;
		}
		
		/**
		 * public no arg constructor for xstream deserialization
		 * on less capable virtual machines.
		 */
		public Remembrance() {
		}

		public IMemorable activate() throws CoreException {
			return BooAssemblyReference.get(ResourcesPlugin.getWorkspace().getRoot().getFile(new Path(this.path)));
		}
	}
	
	public IRemembrance getRemembrance() {
		return new Remembrance(_reference.getFullPath().toPortableString());
	}

	public boolean accept(IBooAssemblyReferenceVisitor visitor) throws CoreException {
		return visitor.visit(this);
	}
}
