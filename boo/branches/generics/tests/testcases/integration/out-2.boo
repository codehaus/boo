import BooCompiler.Tests
import NUnit.Framework

class Foo:
	public value = 0
	public reference

f = Foo()
for i in -1, 0, 5:
	ByRef.ReturnValue(i, f.value)
	Assert.AreEqual(i, f.value)
	

for o in object(), "", object():
	ByRef.ReturnRef(o, f.reference)
	Assert.AreSame(o, f.reference)
