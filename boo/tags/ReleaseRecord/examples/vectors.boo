class Vector3:
	_x as double
	_y as double
	_z as double

	def constructor():
		r = System.Random()
		_x = r.Next()
		_y = r.Next()
		_z = r.Next()

	def Distance(other as Vector3):
		dx = _x - other._x
		dy = _y - other._y
		dz = _z - other._z
		return System.Math.Sqrt(dx*dx+dy*dy+dz*dz)
		#return 1.0
		
def createTuple(count as int):
	l = []
	for i in range(count):
		l.Add(Vector3())
	return l.ToArray(Vector3) as (Vector3)

// array as (Vector3) = tuple(Vector3() for i in range(length))
array = createTuple(25000)

start = date.Now

total = 0.0
count = 0

for v1 in array:
	for v2 in array:
		total += v2.Distance(v1)
		++count

elapsed = date.Now.Subtract(start) 
print("Total... ${total}.")

// a good ips value is: 13.000.000
print("Done ${count} in ${elapsed.TotalSeconds} secs - ${count/elapsed.TotalSeconds} ips.")
