package booclipse.core.launching;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.debug.core.DebugPlugin;
import org.eclipse.debug.core.ILaunchConfiguration;
import org.eclipse.debug.core.ILaunchConfigurationType;
import org.eclipse.debug.core.ILaunchConfigurationWorkingCopy;
import org.eclipse.debug.core.ILaunchManager;

import booclipse.core.foundation.WorkspaceUtilities;
import booclipse.core.internal.BooAssemblySource;
import booclipse.core.model.IBooAssemblySource;
import booclipse.core.model.IBooLaunchConfigurationConstants;
import booclipse.core.model.IBooLaunchConfigurationTypes;

public class BooLauncher {

	public static ILaunchConfiguration getAppLaunchConfiguration(
			IBooAssemblySource source) throws CoreException {
		ILaunchConfiguration configuration = findAppLaunchConfiguration(source);
		return null == configuration
				? createAppLaunchConfiguration(source)
				: configuration;
	}

	private static ILaunchConfiguration createAppLaunchConfiguration(IBooAssemblySource source) throws CoreException {
		final ILaunchConfigurationType configType = getAppLaunchConfigType();
		return createAssemblySourceLaunchConfiguration(source, configType);
	}

	public static ILaunchConfiguration createAssemblySourceLaunchConfiguration(IBooAssemblySource source, final ILaunchConfigurationType configType) throws CoreException {
		return createResourceLaunchConfiguration(configType,
				IBooLaunchConfigurationConstants.ATTR_ASSEMBLY_SOURCE_PATH, source.getFolder());
	}
	
	public static IBooAssemblySource getConfiguredAssemblySource(ILaunchConfiguration configuration) throws CoreException {
		return BooAssemblySource.get(getAssemblySourceFolder(configuration));
	}

	private static IFolder getAssemblySourceFolder(ILaunchConfiguration configuration)
			throws CoreException {
		return WorkspaceUtilities.getFolder(configuration.getAttribute(
				IBooLaunchConfigurationConstants.ATTR_ASSEMBLY_SOURCE_PATH, ""));
	}

	private static ILaunchConfiguration findAppLaunchConfiguration(
			IBooAssemblySource source) throws CoreException {
		final ILaunchConfigurationType configType = getAppLaunchConfigType();
		return findAssemblySourceLaunchConfiguration(source, configType);
	}

	public static ILaunchConfiguration findAssemblySourceLaunchConfiguration(IBooAssemblySource source, final ILaunchConfigurationType configType) throws CoreException {
		return findLaunchConfiguration(configType,
				IBooLaunchConfigurationConstants.ATTR_ASSEMBLY_SOURCE_PATH,
				WorkspaceUtilities.getPortablePath(source.getFolder()));
	}

	public static ILaunchConfiguration getScriptLaunchConfiguration(IFile file)
			throws CoreException {
		ILaunchConfiguration configuration = findScriptLaunchConfiguration(file);
		return null == configuration
				? createScriptConfiguration(file)
				: configuration;
	}

	private static ILaunchConfiguration createScriptConfiguration(IFile file)
			throws CoreException {
		return createResourceLaunchConfiguration(getScriptLaunchConfigType(),
				IBooLaunchConfigurationConstants.ATTR_SCRIPT_PATH, file);
	}

	private static ILaunchConfiguration createResourceLaunchConfiguration(
			ILaunchConfigurationType configType, String pathAttributeName,
			IResource resource) throws CoreException {
		String path = WorkspaceUtilities.getPortablePath(resource);
		ILaunchConfigurationWorkingCopy wc = configType.newInstance(null,
				generateUniqueLaunchConfigurationName(path));
		wc.setAttribute(pathAttributeName, path);
		wc.setAttribute(DebugPlugin.ATTR_CAPTURE_OUTPUT, true);
		return wc.doSave();
	}

	private static String generateUniqueLaunchConfigurationName(String path) {
		return DebugPlugin.getDefault().getLaunchManager()
				.generateUniqueLaunchConfigurationNameFrom(path);
	}

	private static ILaunchConfiguration findScriptLaunchConfiguration(IFile file)
			throws CoreException {
		return findLaunchConfiguration(getScriptLaunchConfigType(),
				IBooLaunchConfigurationConstants.ATTR_SCRIPT_PATH,
				WorkspaceUtilities.getPortablePath(file));
	}

	private static ILaunchConfiguration findLaunchConfiguration(
			ILaunchConfigurationType configType, String attributeName,
			String attributeValue) throws CoreException {
		ILaunchConfiguration[] existing = getLaunchManager()
				.getLaunchConfigurations(configType);
		for (int i = 0; i < existing.length; ++i) {
			if (attributeValue.equals(existing[i].getAttribute(attributeName,
					""))) {
				return existing[i];
			}
		}
		return null;
	}

	private static ILaunchConfigurationType getAppLaunchConfigType() {
		return getLaunchConfigurationType(IBooLaunchConfigurationTypes.ID_BOO_APP);
	}

	private static ILaunchConfigurationType getScriptLaunchConfigType() {
		return getLaunchConfigurationType(IBooLaunchConfigurationTypes.ID_BOO_SCRIPT);
	}

	public static ILaunchConfigurationType getLaunchConfigurationType(
			String configTypeId) {
		return getLaunchManager().getLaunchConfigurationType(configTypeId);
	}

	private static ILaunchManager getLaunchManager() {
		return DebugPlugin.getDefault().getLaunchManager();
	}

}
