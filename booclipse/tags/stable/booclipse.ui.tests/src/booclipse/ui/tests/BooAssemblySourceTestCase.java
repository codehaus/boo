package booclipse.ui.tests;

import org.eclipse.core.runtime.Path;

import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;

public class BooAssemblySourceTestCase extends AbstractBooTestCase {
	
	IBooAssemblySource _assemblySource;
	
	protected void setUp() throws Exception {
		super.setUp();
		_assemblySource = _booProject.addAssemblySource(new Path("src/Test"));
	}
	
	public void testDefaultReferences() throws Exception {
		IBooAssemblyReference[] references = _assemblySource.getReferences();
		assertNotNull(references);
		assertEquals(0, references.length);
	}
	
	public void testDefaultOutputType() throws Exception {
		assertEquals(IBooAssemblySource.OutputType.CONSOLE_APPLICATION, _assemblySource.getOutputType());
	}
	
	public void testOutputFile() throws Exception {
		assertEquals(getFile("bin/Test.exe"), _assemblySource.getOutputFile());
		
		_assemblySource.setOutputType(IBooAssemblySource.OutputType.LIBRARY);
		assertEquals(getFile("bin/Test.dll"), _assemblySource.getOutputFile());
	}
}
