package booclipse.ui.tests;

import java.io.InputStream;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IWorkspace;
import org.eclipse.core.resources.IWorkspaceDescription;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.Path;

import booclipse.core.BooCore;
import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooProject;
import junit.framework.TestCase;

public abstract class AbstractBooTestCase extends TestCase{
	
	protected SimpleProject _project;	
	protected IBooProject _booProject;

	protected void setUp() throws Exception {
		assertEquals("runtime location is not set, please set the MONO_HOME environment variable", 
				BooCore.RuntimePathStatus.OK,
				BooCore.validateRuntimePath(BooCore.getRuntimeLocation()));
		
		disableAutoBuilding();
		_project = new SimpleProject("Test");
		_booProject = BooCore.createProject(getProject());
		assertNotNull(_booProject);
	}
	
	protected void tearDown() throws Exception {
		_project.dispose();
	}
	
	protected IFile getFile(String path) {
		return getProject().getFile(path);
	}

	protected IProject getProject() {
		return _project.getProject();
	}
	
	protected IFolder getFolder(String path) {
		return getProject().getFolder(path);
	}
	
	private void disableAutoBuilding() throws CoreException {
		IWorkspace workspace = ResourcesPlugin.getWorkspace();
		IWorkspaceDescription description = workspace.getDescription();
		if (description.isAutoBuilding()) {
			description.setAutoBuilding(false);
			workspace.setDescription(description);
		}
	}

	protected IBooAssemblySource addAssemblySource(String path) throws CoreException {
		return _booProject.addAssemblySource(new Path(path));
	}

	protected IFile copyResourceTo(String resource, String path) throws Exception {
		IFile file = getProject().getFile(new Path(path).append(resource));
		WorkspaceUtilities.createTree((IFolder) file.getParent());
		InputStream source = getClass().getResourceAsStream("/resources/" + resource);
		if (null == source) resourceNotFound(resource);
		try {
			file.create(source, true, null);
		} finally {
			source.close();
		}
		return file;
	}

	private void resourceNotFound(String resource) {
		throw new IllegalArgumentException("Resource '" + resource + "' not found!");
	}

}
