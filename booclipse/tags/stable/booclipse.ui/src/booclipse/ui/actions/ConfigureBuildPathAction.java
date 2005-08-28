package booclipse.ui.actions;

import org.eclipse.ui.IViewPart;

import booclipse.ui.BooUI;
import booclipse.ui.IBooUIConstants;

public class ConfigureBuildPathAction extends FolderAction {
	public ConfigureBuildPathAction(IViewPart view) {
		super(view, "Add Reference...", BooUI.getImageDescriptor(IBooUIConstants.ASSEMBLY_REFERENCE));
	}
}
