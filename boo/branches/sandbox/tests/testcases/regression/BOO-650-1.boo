"""
Homer
Eric
Eric
"""

import System
import System.Collections
import System.Reflection

[System.Runtime.CompilerServices.CompilerGlobalScope]
class MyClass:
    public static def Main(argv as (string)):
        c = FooCollection() //FooCollection's enumeratoritemtype visited now
        c.Add(Foo("Homer"))
        c.Add(Foo("Eric"))
        for foo in c:
                print(foo.Name)
        print c[1].Name

class Foo:
        [getter(Name)]
        _name as string

        def constructor(name as string):
                _name = name 
		

[EnumeratorItemType(Foo)]
[DefaultMember("Item")]
class FooCollection(ICollection):

        _items = []

        SyncRoot:
                get:
                        return _items.SyncRoot

        Count:
                get:
                        return len(_items)

        IsSynchronized:
                get:
                        return _items.IsSynchronized

        def CopyTo(target as Array, index as int):
                _items.CopyTo(target, index)

        def GetEnumerator():
                return _items.GetEnumerator()

        def Add([required] item as Foo):
                _items.Add(item)

        Item(index as int) as Foo:
                get:
                        return _items[index] 

