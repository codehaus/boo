"""
public class Person(System.Object):

	protected _name as System.String

	public Name as System.String:
		public get:
			return self._name
		public set:
			self._name = value

	public def constructor():
		super()

[Boo.Lang.BooModuleAttribute]
public final transient class Assign_propertyModule(System.Object):

	private static def __Main__(argv as (System.String)) as System.Void:
		p = Person()
		p.Name = 'boo'
		Boo.Lang.Builtins.print(p.get_Name())

	private def constructor():
		super()
"""
class Person:
	_name as string
	
	Name:
		get:
			return _name
		set:
			_name = value
			
p = Person()
p.Name = "boo"
print(p.Name)
