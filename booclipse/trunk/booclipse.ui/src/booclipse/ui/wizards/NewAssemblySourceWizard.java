package booclipse.ui.wizards;

import org.eclipse.core.resources.IContainer;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IWorkspaceRunnable;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.Path;
import org.eclipse.jface.viewers.IStructuredSelection;
import org.eclipse.jface.wizard.Wizard;
import org.eclipse.ui.INewWizard;
import org.eclipse.ui.IWorkbench;

import booclipse.core.BooCore;
import booclipse.core.model.IBooProject;

public class NewAssemblySourceWizard extends Wizard implements INewWizard {

	private NewBooAssemblySourceWizardPage _mainPage;
	private IContainer _selection;

	public boolean performFinish() {
		IWorkspaceRunnable action = new IWorkspaceRunnable() {
			public void run(IProgressMonitor monitor) throws CoreException {
				IBooProject booProject = BooCore.createProject(_selection.getProject());
				
				IPath containerPath = _selection.getProjectRelativePath();
				booProject.addAssemblySource(containerPath.append(_mainPage.getName()));
			}
		};
		try {
			ResourcesPlugin.getWorkspace().run(action, null);
		} catch (CoreException e) {
			e.printStackTrace();
			return false;
		}
		return true;
	}

	public void init(IWorkbench workbench, IStructuredSelection selection) {
		setWindowTitle("Boo Assembly Source");
		_selection = (IContainer)selection.getFirstElement();
	}
	
	public void addPages() {
		super.addPages();
		
		_mainPage = new NewBooAssemblySourceWizardPage();
		addPage(_mainPage);
	}

}
