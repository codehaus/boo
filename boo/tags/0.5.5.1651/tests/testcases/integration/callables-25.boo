import System
import NUnit.Framework

def foo():
	return "foo"
	
d1 as Delegate = foo
Assert.AreEqual("foo", d1.DynamicInvoke(null))

d2 as MulticastDelegate = foo
Assert.AreEqual("foo", d2.DynamicInvoke(null))

d3 as ICallable = foo
Assert.AreEqual("foo", d3())
