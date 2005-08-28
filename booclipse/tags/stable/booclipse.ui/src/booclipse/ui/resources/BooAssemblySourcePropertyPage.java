/*
 * Boo Development Tools for the Eclipse IDE
 * Copyright (C) 2005 Rodrigo B. de Oliveira (rbo@acm.org)
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 */

package booclipse.ui.resources;

import org.eclipse.core.resources.IFolder;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdaptable;
import org.eclipse.jface.preference.PreferencePage;
import org.eclipse.swt.SWT;
import org.eclipse.swt.layout.RowLayout;
import org.eclipse.swt.widgets.Button;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.swt.widgets.Control;
import org.eclipse.swt.widgets.Group;
import org.eclipse.ui.IWorkbenchPropertyPage;

import booclipse.core.BooCore;
import booclipse.core.model.IBooAssemblySource;
import booclipse.ui.BooUI;

public class BooAssemblySourcePropertyPage extends PreferencePage
	implements IWorkbenchPropertyPage {

	private IAdaptable _element;
	private Button[] _outputTypeButtons;

	public BooAssemblySourcePropertyPage() {
	}

	protected void performDefaults() {
	}
	
	public boolean performOk() {
		IBooAssemblySource source = getAssemblySource();
		if (null == source) return false;
		
		try {
			source.setOutputType(getSelectedOutputType());
			source.save(null);
		} catch (CoreException e) {
			BooUI.logException(e);
			return false;
		}
		
		return true;
	}

	private String getSelectedOutputType() {
		for (int i = 0; i < _outputTypeButtons.length; i++) {
			Button button = _outputTypeButtons[i];
			if (button.getSelection()) {
				return (String) button.getData();
			}
		}
		return null;
	}

	private IBooAssemblySource getAssemblySource() {
		try {
			return BooCore.getAssemblySource((IFolder)_element);
		} catch (CoreException e) {
			BooUI.logException(e);
			setErrorMessage(e.getLocalizedMessage());
		}
		return null;
	}

	public IAdaptable getElement() {
		return _element;
	}

	public void setElement(IAdaptable element) {
		_element = element;
	}

	protected Control createContents(Composite parent) {
		IBooAssemblySource source = getAssemblySource();
		if (null == source) return null;
		
		String outputType = source.getOutputType();
		
		Group group = new Group(parent, SWT.SHADOW_IN);
	    group.setText("Output Type");
	    group.setLayout(new RowLayout(SWT.VERTICAL));
	    
	    String[] labels = new String[] {
	    	"Console Application",
	    	"Windows Application",
	    	"Library",
	    };
	    
	    String[] data = new String[] {
	    	IBooAssemblySource.OutputType.CONSOLE_APPLICATION,
	    	IBooAssemblySource.OutputType.WINDOWS_APPLICATION,
	    	IBooAssemblySource.OutputType.LIBRARY,
	    };
	    
	    _outputTypeButtons = new Button[data.length];
	    for (int i=0; i<data.length; ++i) {
	    	Button button = new Button(group, SWT.RADIO);
	    	button.setText(labels[i]);
	    	button.setData(data[i]);
	    	if (data[i].equals(outputType)) {
	    		button.setSelection(true);
	    	}
	    	_outputTypeButtons[i] = button;
	    }
	    return group;
	}

}