"""
2, 0, 6
"""
struct Foo:
	value as int
	
	override def ToString():
		return value.ToString()
	
foos = array(Foo, 3)
i = 0
for foo in foos:
	++i
	if i == 2: continue
	foo.value += 2*i	
	
print join(foos, ', ')
