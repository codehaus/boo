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
package booclipse.core.internal;

import org.eclipse.core.resources.IFolder;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdapterFactory;

import booclipse.core.BooCore;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;

public class BooAssemblySourceAdapterFactory implements IAdapterFactory {
	
	public Class[] getAdapterList() {
		return new Class[] { IBooAssemblySource.class, IBooAssemblyReference.class };
	}
	
	public Object getAdapter(Object adaptableObject, Class adapterType) {
		final IFolder folder = (IFolder)adaptableObject;
		if (!folder.exists()) return null;
		
		try {
			final BooAssemblySource source = BooAssemblySource.get(folder);
			if (adapterType.isAssignableFrom(IBooAssemblySource.class)) return source;
			return BooAssemblyReference.get(source);
		}
		catch (CoreException x) {
			BooCore.logException(x);
		}
		return null;
	}
}