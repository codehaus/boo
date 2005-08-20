package booclipse.core.internal;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;

import org.eclipse.core.resources.IContainer;
import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.resources.IResourceVisitor;
import org.eclipse.core.resources.IWorkspaceRunnable;
import org.eclipse.core.resources.ResourcesPlugin;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IProgressMonitor;
import org.eclipse.core.runtime.QualifiedName;

import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.model.IAssemblySourceReference;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblyReferenceVisitor;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IRemembrance;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;


public class BooAssemblySource implements IBooAssemblySource {
	
	private static final String SETTINGS_CHARSET = "utf-8";

	private static final String SETTINGS_FILE = ".booclipse";
	
	private static final QualifiedName ASSEMBLY_SOURCE_SESSION_KEY = new QualifiedName("booclipse.core.resources", "BooAssemblySourceSession");
	
	public static IBooAssemblySource create(IFolder folder) throws CoreException {
		synchronized (folder) {
			IBooAssemblySource source = BooAssemblySource.get(folder);
			if (null != source) return source;
			source = internalCreate(folder);
			source.save(null);
			return source;
		}
	}
	
	public static BooAssemblySource get(IFolder folder) throws CoreException {
		synchronized (folder) {
			BooAssemblySource source = (BooAssemblySource) folder.getSessionProperty(ASSEMBLY_SOURCE_SESSION_KEY);
			if (null == source) {
				if (isAssemblySource(folder)) {
					source = internalCreate(folder);
				}
			}
			return source;
		}
	}

	private static BooAssemblySource internalCreate(IFolder folder) throws CoreException {
		BooAssemblySource source = new BooAssemblySource(folder);
		folder.setSessionProperty(ASSEMBLY_SOURCE_SESSION_KEY, source);
		return source;
	}
	
	public static boolean isAssemblySource(Object element) {
		try {
			return element instanceof IFolder
				&& BooAssemblySource.isAssemblySource((IFolder)element);
		} catch (CoreException x) {
		}
		return false;
	}

	public static boolean isAssemblySource(IFolder folder) throws CoreException {
		return folder.getFile(SETTINGS_FILE).exists();
//		synchronized (folder) {
//			return folder.getPersistentProperty(ASSEMBLY_SOURCE_PERSIST_KEY) != null;
//		}
	}
	
	private IFolder _folder;

	private IBooAssemblyReference[] _references;
	
	private String _outputType;

	BooAssemblySource(IFolder folder) throws CoreException {
		if (null == folder || !folder.exists()) throw new IllegalArgumentException();
		_folder = folder;
		refresh(null);
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#getFolder()
	 */
	public IFolder getFolder() {
		return _folder;
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#getSourceFiles()
	 */
	public IFile[] getSourceFiles() throws CoreException {
		final List files = new ArrayList();
		IResourceVisitor visitor = new IResourceVisitor() {
			public boolean visit(IResource resource) throws CoreException {
				if (isBooFile(resource) && resource.exists()) {
					files.add(resource);
				}
				return true;
			}
		};
		_folder.accept(visitor, IResource.DEPTH_INFINITE, IResource.FILE);
		return (IFile[])files.toArray(new IFile[files.size()]);
	}
	
	public Object getAdapter(Class adapter) {
		if (adapter.isAssignableFrom(IFolder.class)) {
			return _folder;
		}
		return null;
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#setReferences(booclipse.core.IBooAssemblyReference[])
	 */
	public void setReferences(IBooAssemblyReference[] references) {
		if (null == references) throw new IllegalArgumentException("references");
		_references = references;
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#getReferences()
	 */
	public IBooAssemblyReference[] getReferences() {
		return _references;
	}

	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#getOutputType()
	 */
	public String getOutputType() {
		return _outputType;
	}
	
	public void setOutputType(String outputType) {
		if (null == outputType) throw new IllegalArgumentException();
		if (!outputType.equals(OutputType.CONSOLE_APPLICATION)
			&& !outputType.equals(OutputType.WINDOWS_APPLICATION)
			&& !outputType.equals(OutputType.LIBRARY)) {
			throw new IllegalArgumentException("outputType");
		}
		_outputType = outputType;
	}

	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#getOutputFile()
	 */
	public IFile getOutputFile() {
		return _folder.getProject().getFolder("bin").getFile(_folder.getName() + getOutputAssemblyExtension());
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#refresh(org.eclipse.core.runtime.IProgressMonitor)
	 */
	public void refresh(IProgressMonitor monitor) throws CoreException {
		IWorkspaceRunnable action = new IWorkspaceRunnable() {
			public void run(IProgressMonitor monitor) throws CoreException {
				IFile file = getSettingsFile();
				file.refreshLocal(IResource.DEPTH_ZERO, monitor);
				if (!file.exists()) {
					useDefaultSettings();
					save(monitor); 
				} else {
					loadSettingsFrom(file);
				}
			}
		};
		ResourcesPlugin.getWorkspace().run(action, monitor);
	}
	
	static class AssemblySourceRemembrance {
		public String outputType;
		public IRemembrance[] references;
		public AssemblySourceRemembrance(BooAssemblySource source) {
			outputType = source._outputType;
			references = new IRemembrance[source._references.length];
			for (int i=0; i<references.length; ++i) {
				references[i] = source._references[i].getRemembrance();
			}
		}
		
		public IBooAssemblyReference[] activateReferences() throws CoreException {
			IBooAssemblyReference[] asmReferences = new IBooAssemblyReference[references.length];
			for (int i=0; i<asmReferences.length; ++i) {
				asmReferences[i] = (IBooAssemblyReference) references[i].activate();
			}
			return asmReferences;
			                                                                
		}
	}
	
	/* (non-Javadoc)
	 * @see booclipse.core.IBooAssemblySource#save()
	 */
	public void save(IProgressMonitor monitor) throws CoreException {
		XStream stream = createXStream();
		String xml = stream.toXML(new AssemblySourceRemembrance(this));
		IFile file = getSettingsFile();
		if (!file.exists()) {
			file.create(encode(xml), true, monitor);
			file.setCharset(SETTINGS_CHARSET, monitor);
		} else {
			file.setContents(encode(xml), true, true, monitor);
		}
	}

	private void loadSettingsFrom(IFile file) throws CoreException {
		AssemblySourceRemembrance remembrance = (AssemblySourceRemembrance) createXStream().fromXML(decode(file));
		_outputType = remembrance.outputType;
		_references = remembrance.activateReferences();
	}

	private Reader decode(IFile file) throws CoreException {
		try {
			return new InputStreamReader(file.getContents(), file.getCharset());
		} catch (IOException e) {
			e.printStackTrace();
			WorkspaceUtilities.throwCoreException(e);
		}
		return null;
	}

	private void useDefaultSettings() {
		_references = new IBooAssemblyReference[0];
		_outputType = OutputType.CONSOLE_APPLICATION;
	}

	private String getOutputAssemblyExtension() {
		return OutputType.LIBRARY.equals(getOutputType()) ? ".dll" : ".exe";
	}
	
	boolean isBooFile(IResource resource) {
		if (IResource.FILE != resource.getType()) return false;
		String extension = resource.getFileExtension();
		return extension == null
			? false
			: extension.equalsIgnoreCase("boo");
	}

	private IFile getSettingsFile() {
		return _folder.getFile(SETTINGS_FILE);
	}
	
	private XStream createXStream() {
		XStream stream = new XStream(new DomDriver());
		stream.alias("settings", AssemblySourceRemembrance.class);
		return stream;
	}

	private InputStream encode(String xml) throws CoreException {
		try {
			return new ByteArrayInputStream(xml.getBytes(SETTINGS_CHARSET));
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
			WorkspaceUtilities.throwCoreException(e);
		}
		return null;
	}

	public static IBooAssemblySource getContainer(IResource resource) throws CoreException {
		IContainer parent = resource.getParent();
		while (null != parent && IResource.FOLDER == parent.getType()) {
			BooAssemblySource source = get((IFolder)parent);
			if (null != source) return source;
			parent = parent.getParent();
		}
		return null;
	}

	public boolean visitReferences(IBooAssemblyReferenceVisitor visitor) throws CoreException {
		for (int i=0; i<_references.length; ++i) {
			if (!_references[i].accept(visitor)) return false;
		}
		return true;
	}
	
	public String toString() {
		return _folder.getFullPath().toString();
	}

	public static boolean references(IBooAssemblySource l,
			final IBooAssemblySource r) throws CoreException {
		IBooAssemblyReference[] references = l.getReferences();
		for (int i = 0; i < references.length; ++i) {
			IBooAssemblyReference reference = references[i];
			if (reference instanceof IAssemblySourceReference) {
				if (r == ((IAssemblySourceReference) reference)
						.getAssemblySource()) {
					return true;
				}
			}
		}
		return false;
	}
}
