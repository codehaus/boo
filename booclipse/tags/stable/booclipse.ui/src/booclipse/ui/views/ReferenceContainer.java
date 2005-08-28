package booclipse.ui.views;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdaptable;

import booclipse.core.model.IBooAssemblySource;

public class ReferenceContainer implements IAdaptable {

	private IBooAssemblySource _source;

	ReferenceContainer(IBooAssemblySource source) {
		_source = source;
	}

	public Object getAdapter(Class adapter) {
		if (IBooAssemblySource.class == adapter) return _source;
		return null;
	}

	public boolean hasChildren() throws CoreException {
		return _source.getReferences().length > 0;
	}

	public Object[] getChildren() throws CoreException {
		return _source.getReferences();
	}

}
