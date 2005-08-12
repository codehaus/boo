package booclipse.ui.decorators;

import org.eclipse.jface.viewers.IDecoration;
import org.eclipse.jface.viewers.ILabelProviderListener;
import org.eclipse.jface.viewers.ILightweightLabelDecorator;

import booclipse.core.BooCore;
import booclipse.ui.BooUI;
import booclipse.ui.IBooUIConstants;

public class BooAssemblySourceDecorator implements ILightweightLabelDecorator {

	public void decorate(Object element, IDecoration decoration) {
		if (BooCore.isAssemblySource(element)) {
			decoration.addOverlay(BooUI.getImageDescriptor(IBooUIConstants.ASSEMBLY_SOURCE_DECORATOR));
		}
	}

	public void addListener(ILabelProviderListener listener) {
	}

	public void dispose() {
		
	}

	public boolean isLabelProperty(Object element, String property) {
		return false;
	}

	public void removeListener(ILabelProviderListener listener) {

	}

}
