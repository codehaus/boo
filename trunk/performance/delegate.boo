def applyDelegate(delegate as System.Delegate, iterator):
	for item in iterator:
		delegate.DynamicInvoke((item,))
		
def applyCallable(fn as ICallable, iterator):
	for item in iterator:
		fn(item)
		
def timeit(name, fn as ICallable):
	start = date.Now
	fn(foo, range(100000))
	print("${name} took ${date.Now-start}")
		
def foo(item):
	pass
	
timeit("DynamicInvoke", applyDelegate)
timeit("Call", applyCallable)
	
