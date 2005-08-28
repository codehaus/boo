package booclipse.nunit;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.IConfigurationElement;
import org.eclipse.core.runtime.IExtension;
import org.eclipse.core.runtime.IExtensionPoint;
import org.eclipse.core.runtime.IExtensionRegistry;
import org.eclipse.core.runtime.ISafeRunnable;
import org.eclipse.core.runtime.Platform;
import org.eclipse.core.runtime.Status;
import org.eclipse.jface.resource.ImageDescriptor;
import org.eclipse.jface.util.ListenerList;
import org.eclipse.ui.IWorkbench;
import org.eclipse.ui.PlatformUI;
import org.eclipse.ui.plugin.AbstractUIPlugin;
import org.osgi.framework.BundleContext;

import booclipse.core.model.IBooAssemblySource;

/**
 * The main plugin class to be used in the desktop.
 */
public class NUnitPlugin extends AbstractUIPlugin {

	public static final String ID_PLUGIN = "booclipse.nunit";

	public static final String ID_EXTPOINT_LISTENERS = "booclipse.nunit.listeners";

	// The shared instance.
	private static NUnitPlugin _plugin;

	ListenerList _listeners;

	/**
	 * The constructor.
	 */
	public NUnitPlugin() {
		_plugin = this;
	}

	/**
	 * This method is called upon plug-in activation
	 */
	public void start(BundleContext context) throws Exception {
		super.start(context);
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
	public static NUnitPlugin getDefault() {
		return _plugin;
	}

	/**
	 * Returns an image descriptor for the image file at the given plug-in
	 * relative path.
	 * 
	 * @param path
	 *            the path
	 * @return the image descriptor
	 */
	public static ImageDescriptor getImageDescriptor(String path) {
		return AbstractUIPlugin.imageDescriptorFromPlugin("booclipse.nunit",
				path);
	}

	public static void logException(Exception e) {
		e.printStackTrace();
		getDefault().getLog().log(
				new Status(Status.ERROR, ID_PLUGIN, -1,
						e.getLocalizedMessage(), e));
	}

	public static void logInfo(String message) {
		getDefault().getLog().log(
				new Status(Status.INFO, ID_PLUGIN, -1, message, null));
	}
	
	public void addTestListener(ITestRunListener listener) {
		getListenerList().add(listener);
	}	

	public void removeTestListener(ITestRunListener listener) {
		getListenerList().add(listener);
	}
	
	static abstract class AbstractSafeRunnable implements ISafeRunnable {
		public void handleException(Throwable exception) {
		}
	}
	
	public void fireTestsStarted(final IBooAssemblySource source, final int count) {
		Object[] listeners = getListenerList().getListeners();
		for (int i = 0; i<listeners.length; ++i) {
			final ITestRunListener each = (ITestRunListener) listeners[i];
			ISafeRunnable run = new AbstractSafeRunnable() {
				public void run() {
					each.testsStarted(source, count);
				}
			};
			Platform.run(run);
		}
	}

	public void fireTestsFinished(final IBooAssemblySource source) {
		Object[] listeners = getListenerList().getListeners();
		for (int i = 0; i<listeners.length; ++i) {
			final ITestRunListener each = (ITestRunListener) listeners[i];
			ISafeRunnable run = new AbstractSafeRunnable() {
				public void run() {
					each.testsFinished(source);
				}
			};
			Platform.run(run);

		}
	}

	public void fireTestStarted(final IBooAssemblySource source, final String fullName) {
		Object[] listeners = getListenerList().getListeners();
		for (int i = 0; i<listeners.length; ++i) {
			final ITestRunListener each = (ITestRunListener) listeners[i];
			ISafeRunnable run = new AbstractSafeRunnable() {
				public void run() {
					each.testStarted(source, fullName);
				}
			};
			Platform.run(run);

		}
	}

	public void fireTestFailed(final IBooAssemblySource source, final String fullName, final String trace) {
		Object[] listeners = getListenerList().getListeners();
		for (int i = 0; i<listeners.length; ++i) {
			final ITestRunListener each = (ITestRunListener) listeners[i];
			ISafeRunnable run = new AbstractSafeRunnable() {
				public void run() {
					each.testFailed(source, fullName, trace);
				}
			};
			Platform.run(run);
		}
	}

	protected ListenerList getListenerList() {
		if (null == _listeners) {
			_listeners = computeListeners();
		}
		return _listeners;
	}

	private ListenerList computeListeners() {
		IExtensionRegistry registry = Platform.getExtensionRegistry();
		IExtensionPoint extensionPoint = registry
				.getExtensionPoint(ID_EXTPOINT_LISTENERS);
		IExtension[] extensions = extensionPoint.getExtensions();
		ListenerList results = new ListenerList();
		for (int i = 0; i < extensions.length; i++) {
			IConfigurationElement[] elements = extensions[i].getConfigurationElements();
			for (int j = 0; j < elements.length; j++) {
				try {
					Object listener = elements[j].createExecutableExtension("class");
					if (listener instanceof ITestRunListener) {
						results.add(listener);
					}
				} catch (CoreException e) {
					e.printStackTrace();
				}
			}
		}
		return results;
	}

	public void fireTestsAboutToStart(IBooAssemblySource _source) {
		showNUnitView();
	}
	
	private void showNUnitView() {
		final IWorkbench workbench= PlatformUI.getWorkbench();
		workbench.getDisplay().asyncExec(new Runnable() {
			public void run() {
				try {					
				workbench.getActiveWorkbenchWindow().getActivePage().showView("booclipse.nunit.views.NUnitView");
				} catch (Exception e) {
					logException(e);
				}
			}
		});
	}

}
