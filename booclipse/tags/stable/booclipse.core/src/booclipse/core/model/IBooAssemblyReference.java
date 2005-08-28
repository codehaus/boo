package booclipse.core.model;

import org.eclipse.core.runtime.CoreException;


public interface IBooAssemblyReference extends IMemorable {
	
	public static final String LOCAL = "local";
	
	public static final String GAC = "gac";
	
	public static final String ASSEMBLY_SOURCE = "assembly source";

	/**
	 * Assembly friendly name.
	 * @return assembly friendly name
	 */
	String getAssemblyName();
	
	/**
	 * Returns a string representation of the reference suitable to be
	 * passed as a command line argument to the compiler.
	 * 
	 * @return string representation of this reference
	 */
	String getCompilerReference();

	String getType();
	
	boolean accept(IBooAssemblyReferenceVisitor visitor) throws CoreException;
}
