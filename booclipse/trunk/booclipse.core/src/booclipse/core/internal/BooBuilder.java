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

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.Charset;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.eclipse.core.resources.IContainer;
import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IMarker;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.resources.IResourceDelta;
import org.eclipse.core.resources.IncrementalProjectBuilder;
import org.eclipse.core.resources.WorkspaceJob;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.IStatus;
import org.eclipse.core.runtime.Path;
import org.eclipse.core.runtime.Status;
import org.eclipse.core.runtime.Preferences.IPropertyChangeListener;
import org.eclipse.core.runtime.Preferences.PropertyChangeEvent;

import booclipse.core.BooAssemblyReferenceVisitor;
import booclipse.core.BooCore;
import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooProject;
import booclipse.core.model.ILocalAssemblyReference;


public class BooBuilder extends IncrementalProjectBuilder {

	public static final String BUILDER_ID = BooCore.ID_PLUGIN + ".booBuilder";

	private static final String MARKER_TYPE = BooCore.ID_PLUGIN + ".booProblem";
	
	protected void startupOnInitialize() {
		BooCore.getDefault().getPluginPreferences().addPropertyChangeListener(new IPropertyChangeListener() {
			public void propertyChange(PropertyChangeEvent event) {
				if (BooCore.P_RUNTIME_LOCATION.equals(event.getProperty())) {
					scheduleProjectRebuild();
				}
			}
		});
		super.startupOnInitialize();
	}
	
	private void scheduleProjectRebuild() {
		WorkspaceJob job = new WorkspaceJob("boo build") {
			public IStatus runInWorkspace(IProgressMonitor monitor) throws CoreException {
				getProject().build(IncrementalProjectBuilder.FULL_BUILD, monitor);
				return Status.OK_STATUS;
			}
		};
		job.schedule();
	}
	
	private void addMarker(IResource resource, String message, int lineNumber,
			int severity) {
		try {
			IMarker marker = resource.createMarker(MARKER_TYPE);
			marker.setAttribute(IMarker.MESSAGE, message);
			marker.setAttribute(IMarker.SEVERITY, severity);
			if (lineNumber == -1) {
				lineNumber = 1;
			}
			marker.setAttribute(IMarker.LINE_NUMBER, lineNumber);
		} catch (CoreException e) {
			BooCore.logException(e);
		}
	}

	private void addMarker(String path, String message, int lineNumber,
			int severity) {

		String relativePath = path.substring(getProject().getLocation()
				.toOSString().length() + 1);
		relativePath = relativePath.replaceAll("\\\\", "/");
		IFile file = getProject().getFile(relativePath);
		addMarker(file, message, lineNumber, severity);

	}
	
	protected void clean(IProgressMonitor monitor) throws CoreException {
		IBooAssemblySource[] sources = getAssemblySources();
		for (int i=0; i<sources.length; ++i) {
			clean(sources[i], monitor);
		}
	}

	private void clean(IBooAssemblySource source, final IProgressMonitor monitor) throws CoreException {
		IFile outputFile = source.getOutputFile();
		final IContainer outputFolder = outputFile.getParent();
		deleteIfExists(outputFile, monitor);
		source.visitReferences(new BooAssemblyReferenceVisitor() {
			public boolean visit(ILocalAssemblyReference reference) throws CoreException {
				deleteIfExists(outputFolder.getFile(new Path(reference.getFile().getName())), monitor);
				return true;
			}
		});
	}

	private void deleteIfExists(IFile outputFile, IProgressMonitor monitor) throws CoreException {
		if (outputFile.exists()) {
			outputFile.delete(true, monitor);
		}
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see org.eclipse.core.internal.events.InternalBuilder#build(int,
	 *      java.util.Map, org.eclipse.core.runtime.IProgressMonitor)
	 */
	protected IProject[] build(int kind, Map args, IProgressMonitor monitor)
			throws CoreException {
		if (kind == FULL_BUILD) {
			fullBuild(monitor);
		} else {
			IResourceDelta delta = getDelta(getProject());
			if (delta == null) {
				fullBuild(monitor);
			} else {
				incrementalBuild(delta, monitor);
			}
		}
		return null;
	}

	private void deleteMarkers(IFolder folder) {
		try {
			folder.deleteMarkers(MARKER_TYPE, false, IResource.DEPTH_INFINITE);
		} catch (CoreException e) {
			BooCore.logException(e);
		}
	}

	protected void fullBuild(final IProgressMonitor monitor)
			throws CoreException {
		BooCore.logInfo("full build requested");
		compile(getAssemblySources(), monitor);
	}

	private IBooAssemblySource[] getAssemblySources() throws CoreException {
		return getBooProject().getAssemblySources();
	}

	private IBooProject getBooProject() throws CoreException {
		return BooProject.get(getProject());
	}

	protected void incrementalBuild(IResourceDelta delta,
			IProgressMonitor monitor) throws CoreException {
		BooCore.logInfo("incremental build requested");
		
		compile(getBooProject().getAffectedAssemblySources(delta), monitor);
	}

	void compile(IBooAssemblySource[] sources, IProgressMonitor monitor) throws CoreException {
		sources = getBooProject().getAssemblySourceOrder(sources);
		for (int i=0; i<sources.length; ++i) {
			compile(sources[i], monitor);
		}
	}
	
	void compile(IBooAssemblySource source, IProgressMonitor monitor) throws CoreException {
		try {
			IFile[] files = source.getSourceFiles();
			if (0 == files.length) return;
			
			IFile file = source.getOutputFile();
			WorkspaceUtilities.ensureDerivedParentExists(file);
			
			Process p = launchCompiler(source, files);
			if (0 == reportErrors(source, p)) {
				file.refreshLocal(IResource.DEPTH_ZERO, monitor);
				if (file.exists()) {
					file.setDerived(true);
				}
				copyLocalReferences(source, (IFolder)file.getParent(), monitor);
			}
		} catch (Exception e) {
			addMarker(source.getFolder(), e.getMessage(), -1, IMarker.SEVERITY_ERROR);
			BooCore.logException(e);
		}
	}
	
	private void copyLocalReferences(IBooAssemblySource source, final IFolder folder, final IProgressMonitor monitor) throws CoreException {
		source.visitReferences(new BooAssemblyReferenceVisitor() {
			public boolean visit(ILocalAssemblyReference reference) throws CoreException {
				copyLocalReference(reference, folder, monitor);
				return true;
			}
		});
	}

	private void copyLocalReference(ILocalAssemblyReference reference, IFolder folder, IProgressMonitor monitor) throws CoreException {
		IFile sourceFile = reference.getFile();
		String name = sourceFile.getName();
		IFile targetFile = folder.getFile(name);
		if (targetFile.exists()) {
			if (!isNewer(sourceFile, targetFile)) return;
			targetFile.delete(true, monitor);
		}
		sourceFile.copy(targetFile.getFullPath(), true, monitor);
		targetFile.setDerived(true);
	}

	boolean isNewer(IFile sourceFile, IFile targetFile) {
		return sourceFile.getModificationStamp() > targetFile.getModificationStamp();
	}

	private Process launchCompiler(IBooAssemblySource source, IFile[] files) throws IOException, CoreException {
		CompilerLauncher launcher = new CompilerLauncher(source);
		launcher.addSourceFiles(files);
		return launcher.launch();
	}
	

	Pattern LINE_ERROR_PATTERN = Pattern
			.compile("(.+)\\((\\d+),\\d+\\):\\s(BC\\w\\d+):\\s(.+)");
	
	Pattern GLOBAL_ERROR_PATTERN = Pattern
			.compile("^BCE.+");

	int reportErrors(IBooAssemblySource source, Process p) throws IOException {
		deleteMarkers(source.getFolder());
		
		BufferedReader reader = new BufferedReader(new InputStreamReader(p
				.getInputStream(), Charset.forName("utf-8")));
		
		int errorCount = 0;
		String line = null;
		while (null != (line = reader.readLine())) {
			Matcher matcher = LINE_ERROR_PATTERN.matcher(line);
			if (matcher.matches()) {
				String path = matcher.group(1);
				int lineNumber = Integer.parseInt(matcher.group(2));
				String code = matcher.group(3);
				String message = matcher.group(4);
				addMarker(path, message, lineNumber, code.startsWith("BCE")
						? IMarker.SEVERITY_ERROR
						: IMarker.SEVERITY_WARNING);
				++errorCount;
			} else {
				if (GLOBAL_ERROR_PATTERN.matcher(line).matches()) {
					addMarker(source.getFolder(), line, -1, IMarker.SEVERITY_ERROR);
				} else {
					System.err.println(line);
				}
			}
		}
		return errorCount;
	}
}
