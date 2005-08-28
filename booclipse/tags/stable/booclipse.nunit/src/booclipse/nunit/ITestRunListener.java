package booclipse.nunit;

import booclipse.core.model.IBooAssemblySource;

public interface ITestRunListener {
	
	void testsStarted(IBooAssemblySource source, int testCount);
	
	void testsFinished(IBooAssemblySource source);
	
	void testStarted(IBooAssemblySource source, String fullName);
	
	void testFailed(IBooAssemblySource source, String fullName, String trace);
}