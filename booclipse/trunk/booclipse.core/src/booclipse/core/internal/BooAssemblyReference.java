package booclipse.core.internal;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Collection;
import java.util.Map;
import java.util.TreeMap;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.eclipse.core.resources.IFile;
import org.eclipse.core.resources.IFolder;
import org.eclipse.core.resources.IResource;
import org.eclipse.core.runtime.CoreException;
import org.eclipse.core.runtime.QualifiedName;

import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblySource;

public class BooAssemblyReference {
	
	private static final QualifiedName SESSION_KEY = new QualifiedName("booclipse.core.resources", "BooAssemblyReference");
	
	private static final Map _cachedGacReferences = new TreeMap();

	private static boolean _gacInitialized = false;
	
	private BooAssemblyReference() {
	}
	
	public static IBooAssemblyReference get(IFile file) throws CoreException {
		IBooAssemblyReference reference = getCachedReference(file);
		if (null == reference) {
			reference = new LocalAssemblyReference(file);
			cacheReference(file, reference);
		}
		return reference;
	}

	public static IBooAssemblyReference get(IBooAssemblySource source) throws CoreException {
		IFolder folder = source.getFolder();
		IBooAssemblyReference reference = getCachedReference(folder);
		if (null == reference) {
			reference = new AssemblySourceReference(source);
			cacheReference(folder, reference);
		}
		return reference;
	}
	
	private static IBooAssemblyReference getCachedReference(IResource resource) throws CoreException {
		return (IBooAssemblyReference)resource.getSessionProperty(SESSION_KEY);
	}
	
	private static void cacheReference(IResource resource, IBooAssemblyReference reference) throws CoreException {
		resource.setSessionProperty(SESSION_KEY, reference);
	}

	static final Pattern ASSEMBLY_NAME_PATTERN = Pattern.compile(
		"\\s*([^,]+),\\s*Version=([^,]+),\\s*Culture=([^,]+),\\s*PublicKeyToken=([^,]+)($|,\\s*Custom=null)$");

	public static IBooAssemblyReference[] listGlobalAssemblyCache() throws IOException {
		if (!_gacInitialized) {
			initializeGlobalAssemblyCache();
		}
		return toArray(_cachedGacReferences.values());
	}
	
	private static void initializeGlobalAssemblyCache() throws IOException {
		GACRunner runner = new GACRunner("-l");
		Process p = runner.launch();
		BufferedReader reader = new BufferedReader(new InputStreamReader(p.getInputStream()));
		String line = null;
		while (null != (line = reader.readLine())) {
			Matcher m = ASSEMBLY_NAME_PATTERN.matcher(line);
			if (m.matches()) {
				
				String name = m.group(1);
				String version = m.group(2);
				String culture = m.group(3);
				String token = m.group(4);
				
				getGlobalAssemblyCacheReference(name, version, culture, token);
			}
		}
		_gacInitialized  = true;
	}

	static IBooAssemblyReference getGlobalAssemblyCacheReference(String name, String version, String culture, String token) {
		IBooAssemblyReference reference = getCachedReference(name, version, culture, token);
		if (null == reference) {
			reference = createAndCacheReference(name, version, culture, token);
		}
		return reference;
	}

	private static IBooAssemblyReference[] toArray(Collection references) {
		return (IBooAssemblyReference[]) references.toArray(new IBooAssemblyReference[references.size()]);
	}

	private static IBooAssemblyReference createAndCacheReference(String name, String version, String culture, String token) {
		IBooAssemblyReference reference = new GlobalAssemblyCacheReference(name, version, culture, token);
		_cachedGacReferences.put(cacheKey(name, version, culture, token), reference);
		return reference;
	}

	private static IBooAssemblyReference getCachedReference(String name, String version, String culture, String token) {
		return (IBooAssemblyReference) _cachedGacReferences.get(cacheKey(name, version, culture, token));
	}

	private static Object cacheKey(String name, String version, String culture, String token) {
		return name + version + culture + token;
	}
}
