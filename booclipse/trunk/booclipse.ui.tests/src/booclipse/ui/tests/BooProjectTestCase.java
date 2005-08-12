package booclipse.ui.tests;

import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IResourceChangeEvent;
import org.eclipse.core.resources.IResourceChangeListener;
import org.eclipse.core.resources.IResourceDelta;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.WorkspaceJob;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.IStatus;
import org.eclipse.core.runtime.Status;

import booclipse.core.BooCore;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooProject;

public class BooProjectTestCase extends AbstractBooTestCase {

	public void testProjectAdapter() {
		IBooProject adapted = (IBooProject) _project.getProject().getAdapter(IBooProject.class);
		assertSame(_booProject, adapted);
	}
	
	public void testAddAssemblySource() throws Exception {
		assertEquals(0, _booProject.getAssemblySources().length);
		
		IBooAssemblySource foo = addAssemblySource("src/Foo");
		assertNotNull(foo);
		IFolder folder = foo.getFolder();
		assertNotNull(folder);
		assertTrue(folder.exists());
		
		IBooAssemblySource[] sources = _booProject.getAssemblySources();
		assertEquals(1, sources.length);
		assertSame(foo, sources[0]);
		
		assertSame(foo, folder.getAdapter(IBooAssemblySource.class));
		
		IBooAssemblySource bar = addAssemblySource("src/Bar");
		sources = _booProject.getAssemblySources();
		assertEquals(2, sources.length);
		assertContains(foo, sources);
		assertContains(bar, sources);
	}
	
	public void testAssemblySourceOrder() throws Exception {
		IBooAssemblySource foo = addAssemblySource("src/Foo");
		IBooAssemblySource bar = addAssemblySource("src/Bar");
		IBooAssemblySource baz = addAssemblySource("src/Baz");
		IBooAssemblySource bang = addAssemblySource("src/Bang");

		foo.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(bar) });
		baz.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(foo) });
		bang.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(foo) });
		
		// (baz, bang) -> foo -> bar
		// build order should be:
		// bang, bar, foo, baz
		
		IBooAssemblySource[] order = _booProject.getAssemblySourceOrder(new IBooAssemblySource[] { bang, foo, bar, baz });
		assertSame(bar, order[0]);
		assertSame(foo, order[1]);
		assertSame(bang, order[2]);
		assertSame(baz, order[3]);
	}
	
	static class ResourceChangeListener implements IResourceChangeListener
	{
		private IResourceDelta _delta;
		
		public IResourceDelta getDelta() {
			return _delta;
		}
		
		public void resourceChanged(IResourceChangeEvent event) {
			synchronized (this) {
				try {
					_delta = event.getDelta();
				} finally {
					notify();
				}
			}
		}
	}
	
	public void testGetAffectedAssemblySources() throws Exception {
		final IBooAssemblySource foo = addAssemblySource("src/Foo");
		final IBooAssemblySource bar = addAssemblySource("src/Bar");
		final IBooAssemblySource baz = addAssemblySource("src/Baz");
		foo.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(bar) });
		baz.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(foo) });
		
		final ResourceChangeListener listener = new ResourceChangeListener();
		final IWorkspace workspace = getProject().getWorkspace();
		workspace.addResourceChangeListener(listener);
		try {
			synchronized (listener) {
				WorkspaceJob job = new WorkspaceJob("addFile") {
					public IStatus runInWorkspace(IProgressMonitor monitor) throws CoreException {
						try {
							copyResourceTo("Bar.boo", "src/Bar");
						} catch (Exception e) {
							fail(e.toString());
						}
						return Status.OK_STATUS;
					}
				};
				job.schedule();
				listener.wait();
			}
		} finally {
			workspace.removeResourceChangeListener(listener);
		}
		
		assertNotNull(listener.getDelta());
		
		IBooAssemblySource[] sources = _booProject.getAffectedAssemblySources(listener.getDelta());
		assertNotNull(sources);
		assertEquals(3, sources.length);
		assertContains(foo, sources);
		assertContains(bar, sources);
		assertContains(baz, sources);
	}

	void assertContains(Object element, Object[] array) {
		for (int i=0; i<array.length; ++i) {
			if (element == array[i]) return;
		}
		fail("Element '" + element + "' not found.");
	}
}
