"""
def foo():
	return { print('foo') }

print(foo()())
"""
def foo():
	return { print('foo') }
end

print(foo()())
