package booclipse.ui.wizards;

import org.eclipse.jface.wizard.WizardPage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.GridLayout;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Label;
import org.eclipse.swt.widgets.Text;

public class NewBooAssemblySourceWizardPage extends WizardPage {
	
	private Text _name;

	public NewBooAssemblySourceWizardPage() {
		super("General");
	}

	public void createControl(Composite parent) {
		
		Composite container = new Composite(parent, SWT.NULL);
		
		GridLayout layout = new GridLayout();
		container.setLayout(layout);
		layout.numColumns = 2;
		
		Label label = new Label(container, SWT.NONE);
		label.setText("Name: ");
		
		_name = new Text(container, SWT.BORDER|SWT.FILL);
		
		setControl(container);
	}
	
	public String getName() {
		return _name.getText();
	}

}
