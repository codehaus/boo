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

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IProjectDescription;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.resources.IResourceDelta;
import org.eclipse.core.resources.IResourceDeltaVisitor;
import org.eclipse.core.resources.IResourceVisitor;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IPath;
import org.eclipse.core.runtime.QualifiedName;

import booclipse.core.BooCore;
import booclipse.core.foundation.ArrayUtilities;
import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooProject;

public class BooProject implements IBooProject {

	private static final QualifiedName SESSION_KEY = new QualifiedName(
			"booclipse.core.resources", "BooProject");

	public static IBooProject create(IProject project) throws CoreException {
		if (project.hasNature(BooCore.ID_NATURE)) {
			return BooProject.get(project);
		}
		IProjectDescription description = project.getDescription();
		description.setNatureIds((String[]) ArrayUtilities.append(description
				.getNatureIds(), BooCore.ID_NATURE));
		project.setDescription(description, null);
		return BooProject.get(project);
	}

	public static IBooProject get(IProject project) throws CoreException {
		IBooProject p = (IBooProject) project.getSessionProperty(SESSION_KEY);
		if (null == p && project.hasNature(BooCore.ID_NATURE)) {
			p = new BooProject(project);
			project.setSessionProperty(SESSION_KEY, p);
		}
		return p;
	}

	IProject _project;

	BooProject(IProject project) {
		_project = project;
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see booclipse.core.IBooProject#addAssemblySource(org.eclipse.core.runtime.IPath)
	 */
	public IBooAssemblySource addAssemblySource(IPath path)
			throws CoreException {
		IFolder folder = _project.getFolder(path);
		WorkspaceUtilities.createTree(folder);
		return BooAssemblySource.create(folder);
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see booclipse.core.IBooProject#getAssemblySources()
	 */
	public IBooAssemblySource[] getAssemblySources() throws CoreException {
		final List sources = new ArrayList();
		IResourceVisitor visitor = new IResourceVisitor() {
			public boolean visit(IResource resource) throws CoreException {
				if (resource instanceof IFolder) {
					IBooAssemblySource source = BooAssemblySource
							.get((IFolder) resource);
					if (source != null) {
						sources.add(source);
						return false;
					}
				}
				return true;
			}
		};
		_project.accept(visitor, IResource.DEPTH_INFINITE, IResource.FOLDER);
		return toBooAssemblySourceArray(sources);
	}

	private IBooAssemblySource[] toBooAssemblySourceArray(
			final Collection sources) {
		return (IBooAssemblySource[]) sources
				.toArray(new IBooAssemblySource[sources.size()]);
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see booclipse.core.IBooProject#getAffectedAssemblySources(org.eclipse.core.resources.IResourceDelta)
	 */
	public IBooAssemblySource[] getAffectedAssemblySources(IResourceDelta delta)
			throws CoreException {
		final IBooAssemblySource[] sources = getAssemblySources();
		final Set affected = new HashSet();
		delta.accept(new IResourceDeltaVisitor() {
			public boolean visit(IResourceDelta delta) throws CoreException {
				IResource resource = delta.getResource();
				if (IResource.FILE == resource.getType()) {
					IBooAssemblySource parent = BooCore
							.getAssemblySourceContainer(resource);
					if (null != parent && !affected.contains(parent)) {
						affected.add(parent);
						addDependents(affected, sources, parent);
						return false;
					}
				}
				return true;
			}

			private void addDependents(final Set affected, final IBooAssemblySource[] sources, IBooAssemblySource changed) throws CoreException {
				for (int i=0; i<sources.length; ++i) {
					IBooAssemblySource source = sources[i];
					if (BooAssemblySource.references(source, changed)) {
						if (!affected.contains(source)) {
							affected.add(source);
							addDependents(affected, sources, source);
						}
					}
				}
			}
		});
		return toBooAssemblySourceArray(affected);
	}

	public IBooAssemblySource[] getAssemblySourceOrder(
			IBooAssemblySource[] sources) throws CoreException {
		return new TopoSorter(sources).sorted();
	}

	static class TopoSorter {

		private List _sources;

		private List _sorted = new ArrayList();

		public TopoSorter(IBooAssemblySource[] sources) throws CoreException {
			_sources = new ArrayList(Arrays.asList(sources));
			sort();
		}
		
		public IBooAssemblySource[] sorted() {
			return (IBooAssemblySource[]) _sorted.toArray(new IBooAssemblySource[_sorted.size()]);
		}
		
		private void sort() throws CoreException {
			while (!_sources.isEmpty()) {
				int index = nextLeaf();
				if (index < 0) {
					throw new IllegalStateException("reference cycle");
				}
				_sorted.add(_sources.get(index));
				_sources.remove(index);
			}
		}

		private int nextLeaf() throws CoreException {
			for (int i=0; i<_sources.size(); ++i) {
				IBooAssemblySource source = (IBooAssemblySource) _sources.get(i);
				boolean edge = false;
				for (int j=0; j<_sources.size(); ++j) {
					if (BooAssemblySource.references(source, (IBooAssemblySource) _sources.get(j))) {
						edge = true;
						break;
					}
				}
				if (!edge) return i;
			}
			return -1;
		}
	}
}
