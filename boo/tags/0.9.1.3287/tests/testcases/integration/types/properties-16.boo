"""
A.Foo.set
B.Foo.set
A.Foo
"""
import NUnit.Framework

class A:

	virtual Foo:
		get:
			return "A.Foo"
		set:
			print("A.Foo.set")
			
class B(A):

	override Foo:
		get:
			return "B: ${super()}"
			
		set:
			print("B.Foo.set")
			print(super.Foo)

a = A()
a.Foo = "foo"
Assert.AreEqual("A.Foo", a.Foo)


a = B()
a.Foo = "foo"
Assert.AreEqual("B: A.Foo", a.Foo)
			
