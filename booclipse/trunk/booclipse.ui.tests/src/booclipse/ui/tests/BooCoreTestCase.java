package booclipse.ui.tests;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.runtime.Platform;

import booclipse.core.BooCore;
import booclipse.core.model.IAssemblySourceReference;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.ILocalAssemblyReference;

public class BooCoreTestCase extends AbstractBooTestCase {

	public void testCreateAssemblyReference() throws Exception {
		IBooAssemblySource bar = addAssemblySource("src/Bar");
		IAssemblySourceReference barReference = (IAssemblySourceReference) BooCore.createAssemblyReference(bar);
		assertNotNull(barReference);
		assertSame(bar, barReference.getAssemblySource());
		assertSame(barReference, BooCore.createAssemblyReference(bar));
	}
	
	public void testFileAdapters() throws Exception {
		assertNull(getAdapter(getFile("lib/TestClass.dll"), IBooAssemblyReference.class));
		
		final IFile resource = copyResourceTo("TestClass.dll", "lib");
		
		ILocalAssemblyReference reference = (ILocalAssemblyReference)getAdapter(resource, IBooAssemblyReference.class);
		assertNotNull(reference);
		assertSame(resource, reference.getFile());
		assertSame(reference, getAdapter(resource, IBooAssemblyReference.class));
	}
	
	public void testAssemblySourceAdapters() throws Exception {
		final String path = "src/Bar";
		
		final IFolder folder = getFolder(path);
		assertNull(getAdapter(folder, IBooAssemblyReference.class));
		
		final IBooAssemblySource source = addAssemblySource(path);
		IAssemblySourceReference reference = (IAssemblySourceReference) getAdapter(folder, IBooAssemblyReference.class);
		assertNotNull(reference);
		assertSame(source, reference.getAssemblySource());
		assertSame(reference, getAdapter(folder, IBooAssemblyReference.class));
	}

	private Object getAdapter(Object adaptable, Class adapterClass) {
		return Platform.getAdapterManager().getAdapter(adaptable, adapterClass);
	}
}
