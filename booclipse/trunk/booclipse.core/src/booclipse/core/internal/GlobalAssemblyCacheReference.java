package booclipse.core.internal;

import org.eclipse.core.runtime.CoreException;

import booclipse.core.model.IBooAssemblyReference;
import booclipse.core.model.IBooAssemblyReferenceVisitor;
import booclipse.core.model.IGlobalAssemblyCacheReference;
import booclipse.core.model.IMemorable;
import booclipse.core.model.IRemembrance;

public class GlobalAssemblyCacheReference implements IGlobalAssemblyCacheReference {

	private String _name;
	private String _version;
	private String _culture;
	private String _token;

	public GlobalAssemblyCacheReference(String name, String version, String culture, String token) {
		_name = name;
		_version = version;
		_culture = culture;
		_token = token;
	}

	public String getAssemblyName() {
		return _name;
	}

	public String getCompilerReference() {
		return _name + ", Version=" + _version + ", Culture=" + _culture + ", PublicKeyToken=" + _token;
	}

	public String getType() {
		return IBooAssemblyReference.GAC;
	}

	public String getVersion() {
		return _version;
	}

	public String getToken() {
		return _token;
	}
	
	static public class Remembrance implements IRemembrance {
		public String name;
		public String version;
		public String culture;
		public String token;
		
		public Remembrance(String name, String version, String culture, String token) {
			this.name = name;
			this.version = version;
			this.culture = culture;
			this.token = token;
		}
		
		/**
		 * public no arg constructor for xstream deserialization
		 * on less capable virtual machines.
		 */
		public Remembrance() {
		}

		public IMemorable activate() throws CoreException {
			return BooAssemblyReference.getGlobalAssemblyCacheReference(name, version, culture, token);
		}
	}
		
	public IRemembrance getRemembrance() {
		return new Remembrance(_name, _version, _culture, _token);
	}

	public boolean accept(IBooAssemblyReferenceVisitor visitor) throws CoreException {
		return visitor.visit(this);
	}	
}
