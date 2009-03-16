"""
public class Test(object):

	protected static X as int

	protected x as int

	public static def Bar() as void:
		Test.X = 42

	public def Foo() as void:
		self.x = 23

	public Value as int:
		public get:
			return self.x

	public def GetAndSet(ref x as object, new as object) as void:
		x = self.x
		self.x = new

	public def constructor():
		super()

[System.Runtime.CompilerServices.CompilerGlobalScopeAttribute]
public final transient class Omitted-target-1Module(object):

	public static def Do() as void:
		System.Console.WriteLine('done')

	private static def Main(argv as (string)) as void:
		Omitted-target-1Module.Do()

	private def constructor():
		super()
"""
class Test:
	static X as int
	x as int

	static def Bar():
		.X = 42

	def Foo():
		.x = 23

	Value:
		get: return .x

	def GetAndSet(ref x, new):
		x = .x
		.x = new


def Do():
	print "done"

.Do()

