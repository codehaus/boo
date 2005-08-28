package booclipse.core.internal;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdapterFactory;

import booclipse.core.BooCore;
import booclipse.core.model.IBooAssemblyReference;

public class BooAssemblyReferenceAdapterFactory implements IAdapterFactory {

	public Object getAdapter(Object adaptableObject, Class adapterType) {
		IFile file = (IFile)adaptableObject;
		if (!file.exists()) return null;
		try {
			return BooAssemblyReference.get(file);
		} catch (CoreException e) {
			BooCore.logException(e);
		}
		return null;
	}

	public Class[] getAdapterList() {
		return new Class[] { IBooAssemblyReference.class };
	}

}
