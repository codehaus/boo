package booclipse.core;

import java.io.File;
import java.io.IOException;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IProject;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IAdapterManager;
import org.eclipse.core.runtime.Platform;
import org.eclipse.core.runtime.Plugin;
import org.eclipse.core.runtime.Status;
import org.osgi.framework.BundleContext;

import booclipse.core.internal.BooAssemblyReference;
import booclipse.core.internal.BooAssemblyReferenceAdapterFactory;
import booclipse.core.internal.BooAssemblySource;
import booclipse.core.internal.BooAssemblySourceAdapterFactory;
import booclipse.core.internal.BooProject;
import booclipse.core.internal.BooProjectAdapterFactory;
import booclipse.core.launching.RuntimeRunner;
import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooProject;


/**
 * The main plugin class to be used in the desktop.
 */
public class BooCore extends Plugin {
	
	public static final String ID_PLUGIN = "booclipse.core";
	
	/**
	 * Boo Project Nature ID
	 */
	public static final String ID_NATURE = BooCore.ID_PLUGIN + ".booNature";
	
	/**
	 * Plugin preferences 
	 */	
	public static final String P_RUNTIME_LOCATION = "compilerLocation";

	private static BooCore _plugin;
	
	/**
	 * The constructor.
	 */
	public BooCore() {
		_plugin = this;
	}

	/**
	 * This method is called upon plug-in activation
	 */
	public void start(BundleContext context) throws Exception {
		super.start(context);
		registerAdapters();
		registerPreferenceListeners();
	}

	/**
	 * This method is called when the plug-in is stopped
	 */
	public void stop(BundleContext context) throws Exception {
		super.stop(context);
		_plugin = null;
	}

	/**
	 * Returns the shared instance.
	 */
	public static BooCore getDefault() {
		return _plugin;
	}
	
	public static String getRuntimeLocation() {
		return getDefault().getPluginPreferences().getString(P_RUNTIME_LOCATION);
	}
	
	public static void setRuntimeLocation(String path) throws IOException {
		if (RuntimePathStatus.OK != validateRuntimePath(path)) {
			throw new IllegalArgumentException("path");
		}
		getDefault().getPluginPreferences().setValue(P_RUNTIME_LOCATION, path);
	}
	
	void registerAdapters() {
		IAdapterManager adapterManager = Platform.getAdapterManager();
		adapterManager.registerAdapters(new BooProjectAdapterFactory(), IProject.class);
		adapterManager.registerAdapters(new BooAssemblySourceAdapterFactory(), IFolder.class);
		adapterManager.registerAdapters(new BooAssemblyReferenceAdapterFactory(), IFile.class);
	}
	
	public static void logException(Exception e) {
		e.printStackTrace();
		BooCore plugin = getDefault();
		if (null == plugin) return;
		plugin.getLog().log(new Status(Status.ERROR, ID_PLUGIN, -1, e.getMessage(), e));
	}
	
	public static void logInfo(String message) {
		BooCore plugin = getDefault();
		if (null == plugin) return;
		plugin.getLog().log(new Status(Status.INFO, ID_PLUGIN, -1, message, null));
	}
	
	private void registerPreferenceListeners() {
	}

	public static IBooAssemblySource getAssemblySourceContainer(IResource resource) {
		try {
			return BooAssemblySource.getContainer(resource);
		} catch (CoreException e) {
			logException(e);
		}
		return null;
	}

	public static IBooAssemblySource createAssemblySource(IFolder folder) throws CoreException {
		return BooAssemblySource.create(folder);
	}

	public static IBooAssemblySource getAssemblySource(IFolder folder) throws CoreException {
		return BooAssemblySource.get(folder);
	}

	public static boolean isAssemblySource(Object selectedElement) {
		return BooAssemblySource.isAssemblySource(selectedElement);
	}

	public static IBooProject createProject(IProject project) throws CoreException {
		return BooProject.create(project);
	}

	public static IBooAssemblyReference[] listGlobalAssemblyCache() throws IOException {
		return BooAssemblyReference.listGlobalAssemblyCache();
	}

	public static IBooAssemblyReference createAssemblyReference(IFile file) throws CoreException {
		return BooAssemblyReference.get(file);
	}
	
	public static IBooAssemblyReference createAssemblyReference(IBooAssemblySource source) throws CoreException {
		return BooAssemblyReference.get(source);
	}
	
	public interface RuntimePathStatus {
		static final int OK = 0;
		static final int MISSING_MONO = 1;
		static final int MISSING_BOO = 2;
	}

	public static int validateRuntimePath(String path) throws IOException {
		String runtime = RuntimeRunner.getOSDependentRuntimeExecutable(path);
		return new File(runtime).exists() ? RuntimePathStatus.OK : RuntimePathStatus.MISSING_MONO;
	}
}