"""
a = ast:
	return 42

b = ast:
	while (not foo):
		print 'bar'
"""
a = ast:
	return 42

b = ast:
	while not foo:
		print 'bar'
