"""
public final transient class Equality0Module(System.Object):

	private static def __Main__() as System.Void:
		o1 = System.Object()
		o2 = System.Object()
		Boo.Lang.Builtins.print(System.String.op_Equality('foo', 'bar'))
		Boo.Lang.Builtins.print((3 == 3.0))
		Boo.Lang.Builtins.print(System.Object.Equals(o1, o2))
		Boo.Lang.Builtins.print(System.Object.Equals('foo', o2))
		Boo.Lang.Builtins.print(System.Object.Equals(3.0, o1))

	private def constructor():
		super()
"""
o1 = object()
o2 = object()
print('foo' == 'bar')
print(3 == 3.0)
print(o1 == o2)
print('foo' == o2)
print(3.0 == o1)
