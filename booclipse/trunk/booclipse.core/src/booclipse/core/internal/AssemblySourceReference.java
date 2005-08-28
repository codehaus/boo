package booclipse.core.internal;

import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.Path;

import booclipse.core.model.IAssemblySourceReference;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblyReferenceVisitor;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IMemorable;
import booclipse.core.model.IRemembrance;

public class AssemblySourceReference implements IAssemblySourceReference {
	
	private IBooAssemblySource _source;

	AssemblySourceReference(IBooAssemblySource source) {
		_source = source;
	}

	public IBooAssemblySource getAssemblySource() {
		return _source;
	}

	public String getAssemblyName() {
		return _source.getFolder().getName();
	}

	public String getCompilerReference() {
		return _source.getOutputFile().getLocation().toOSString();
	}

	public String getType() {
		return IBooAssemblyReference.ASSEMBLY_SOURCE;
	}

	public boolean accept(IBooAssemblyReferenceVisitor visitor)
			throws CoreException {
		return visitor.visit(this);
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
			IFolder folder = ResourcesPlugin.getWorkspace().getRoot().getFolder(new Path(this.path));
			BooAssemblySource source = BooAssemblySource.get(folder);
			return BooAssemblyReference.get(source);
		}
	}

	public IRemembrance getRemembrance() {
		return new Remembrance(_source.getFolder().getFullPath().toPortableString());
	}

}
