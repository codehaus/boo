"""
BaseClass.Method0
A.Method0
BaseClass.Method0

"""
import Boo.Tests.Lang.Compiler from Boo.Tests

class A(BaseClass):
	def Method0():
		super()
		print("A.Method0") #overriden method
		super() #base class method
		
b as BaseClass = A()
b.Method0()

