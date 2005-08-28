package booclipse.ui.tests;


import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IncrementalProjectBuilder;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.Path;

import booclipse.core.BooCore;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;

public class BooBuilderTestCase extends AbstractBooTestCase {
	
	private IBooAssemblySource _assemblySource;

	protected void setUp() throws Exception {
		super.setUp();
		
		IFile file = copyResourceTo("TestClass.dll", "lib");
		IBooAssemblyReference reference = BooCore.createAssemblyReference(file);
		_assemblySource = _booProject.addAssemblySource(new Path("src/Test"));
		_assemblySource.setReferences(new IBooAssemblyReference[] { reference });
		copyResourceTo("Program.boo", "src/Test");
		build();
	}
	
	public void testBuild() throws Exception {
		
		assertTrue(_assemblySource.getOutputFile().exists());
		assertTrue(_assemblySource.getOutputFile().isDerived());
		
		IFile copiedRef = getFile("bin/TestClass.dll");
		assertTrue("referenced local files must be copied to the output folder", copiedRef.exists());
		assertTrue("copied references must be marked as derived files", copiedRef.isDerived());
	}
	
	public void testClean() throws Exception {
		_project.getProject().build(IncrementalProjectBuilder.CLEAN_BUILD, null);
		
		assertFalse(_assemblySource.getOutputFile().exists());
		assertFalse(getFile("bin/TestClass.dll").exists());
	}
	
	public void testAssemblySourceBuildOrder() throws Exception {
		IBooAssemblySource bar = addAssemblySource("src/Bar");
		bar.setOutputType(IBooAssemblySource.OutputType.LIBRARY);
		
		IBooAssemblySource foo = addAssemblySource("src/Foo");

		foo.setReferences(new IBooAssemblyReference[] { BooCore.createAssemblyReference(bar) });
		
		copyResourceTo("Bar.boo", "src/Bar");
		copyResourceTo("Foo.boo", "src/Foo");
		
		build();
		assertTrue(foo.getOutputFile().exists());
		assertTrue(bar.getOutputFile().exists());
	}

	private void build() throws CoreException {
		_project.getProject().build(IncrementalProjectBuilder.FULL_BUILD, null);
	}


}
