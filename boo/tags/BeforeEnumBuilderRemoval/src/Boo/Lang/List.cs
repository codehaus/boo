﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo Barreto de Oliveira
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

namespace Boo.Lang
{
	using System;
	using System.Collections;
	using System.Text;

	// callable Predicate(item) as bool
	public delegate bool Predicate(object item);

	/// <summary>
	/// List.
	/// </summary>
	[Serializable]
	public class List : IList
	{	
		static readonly object[] EmptyObjectArray = new object[0];
		
		protected object[] _items;
		
		protected int _count;
		
		public List()
		{
			_items = EmptyObjectArray;
			_count = 0;
		}
		
		public List(IEnumerable enumerable) : this()
		{
			Extend(enumerable);
		}
		
		public List(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity");
			}
			_items = new object[initialCapacity];
			_count = 0;
		}
		
		public List(object[] items, bool takeOwnership)
		{
			if (null == items)
			{
				throw new ArgumentNullException("items");
			}
			
			if (takeOwnership)
			{
				_items = items;
			}
			else
			{
				_items = (object[])items.Clone();
			}
			_count = items.Length;
		}
		
		public static List operator*(List lhs, int count)
		{
			return lhs.Multiply(count);
		}
		
		public static List operator*(int count, List rhs)
		{
			return rhs.Multiply(count);
		}
		
		public static List operator+(List lhs, IEnumerable rhs)
		{
			List result = new List(lhs.Count);
			result.Extend(lhs);
			result.Extend(rhs);
			return result;
		}
		
		public List Multiply(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			
			object[] items = new object[_count*count];			
			for (int i=0; i<count; ++i)
			{
				Array.Copy(_items, 0, items, i*_count, _count);
			}
			return new List(items, true);
		}
		
		public int Count
		{
			get
			{
				return _count;
			}
		}
		
		public IEnumerator GetEnumerator()
		{
			return new ListEnumerator(this);
		}
		
		public void CopyTo(Array target, int index)
		{
			Array.Copy(_items, 0, target, index, _count);
		}
		
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}
		
		public object SyncRoot
		{
			get
			{
				return _items;
			}
		}

		public object this[int index]
		{
			get
			{				
				return _items[CheckIndex(NormalizeIndex(index))];
			}
			
			set
			{
				_items[CheckIndex(NormalizeIndex(index))] = value;
			}
		}
		
		public List Push(object item)
		{
			return Add(item);
		}

		public List Add(object item)
		{
			EnsureCapacity(_count+1);
			_items[_count] = item;
			++_count;
			return this;
		}

		public List AddUnique(object item)
		{
			if (!Contains(item))
			{
				Add(item);
			}
			return this;
		}
		
		public List Extend(IEnumerable enumerable)
		{
			foreach (object item in enumerable)
			{
				Add(item);
			}
			return this;
		}

		public List Collect(Predicate condition)
		{
			if (null == condition)
			{
				throw new ArgumentNullException("condition");
			}

			List newList = new List();
			InnerCollect(newList, condition);			
			return newList;
		}		

		public List Collect(List target, Predicate condition)
		{
			if (null == target)
			{
				throw new ArgumentNullException("target");
			}

			if (null == condition)
			{
				throw new ArgumentNullException("condition");
			}

			InnerCollect(target, condition);
			return target;
		}
		
		public Array ToArray(System.Type targetType)
		{
			Array target = Array.CreateInstance(targetType, _count);
			CopyTo(target, 0);
			return target;
		}
		
		public object[] ToArray()
		{
			return (object[])ToArray(typeof(object));
		}
		
		public List Sort()
		{
			Array.Sort(_items, 0, _count, BooComparer.Default);
			return this;
		}
		
		public List Sort(IComparer comparer)
		{
			Array.Sort(_items, 0, _count, comparer);
			return this;
		}

		override public string ToString()
		{
			return Join(", ");
		}
		
		public string Join(string separator)
		{
			return Builtins.join(this, separator);
		}
		
		override public int GetHashCode()
		{
			int hash = _count;
			
			for (int i=0; i<_count; ++i)
			{
				object item = _items[i];
				if (null != item)
				{
					hash ^= item.GetHashCode();
				}
			}
			return hash;
		}
		
		override public bool Equals(object other)
		{
			if (other == this)
			{
				return true;
			}
			
			List rhs = other as List;
			if (null == rhs)
			{
				return false;
			}
			
			if (_count != rhs.Count)
			{
				return false;
			}
			
			for (int i=0; i<_count; ++i)
			{
				if (!object.Equals(_items[i], rhs[i]))
				{
					return false;
				}
			}
			return true;
		}
		
		public void Clear()
		{
			for (int i=0; i<_count; ++i)
			{
				_items[i] = null;
			}
			_count = 0;
		}
		
		public List GetRange(int begin)
		{
			return InnerGetRange(AdjustIndex(NormalizeIndex(begin)), _count);
		}
		
		public List GetRange(int begin, int end)
		{
			return InnerGetRange(
					AdjustIndex(NormalizeIndex(begin)),
					AdjustIndex(NormalizeIndex(end)));
		}
		
		public bool Contains(object item)
		{
			for (int i=0; i<_count; ++i)
			{
				if (object.Equals(_items[i], item))
				{
					return true;
				}
			}
			return false;
		}
		
		public int IndexOf(object item)
		{			
			for (int i=0; i<_count; ++i)
			{
				if (object.Equals(_items[i], item))
				{
					return i;
				}
			}
			return -1;
		}
		
		public List Insert(int index, object item)
		{
			int actual = NormalizeIndex(index);			
			EnsureCapacity(Math.Max(_count, actual) + 1);
			
			if (actual < _count)
			{
				Array.Copy(_items, actual, _items, actual+1, _count-actual);
			}
			
			_items[actual] = item;
			++_count;
			return this;
		}
		
		public object Pop()
		{	
			return Pop(-1);
		}
		
		public object Pop(int index)
		{
			int actualIndex = CheckIndex(NormalizeIndex(index));
			object item = _items[actualIndex];
			InnerRemoveAt(actualIndex);
			return item;
		}
		
		public List Remove(object item)
		{
			InnerRemove(item);
			return this;
		}
		
		public List RemoveAt(int index)
		{
			InnerRemoveAt(CheckIndex(NormalizeIndex(index)));
			return this;
		}
		
		void IList.Insert(int index, object item)
		{
			Insert(index, item);
		}
		
		void IList.Remove(object item)
		{			
			InnerRemove(item);
		}
		
		void IList.RemoveAt(int index)
		{
			InnerRemoveAt(CheckIndex(NormalizeIndex(index)));
		}
		
		int IList.Add(object item)
		{			
			Add(item);
			return _count-1;
		}
		
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}
		
		void EnsureCapacity(int minCapacity)
		{
			if (minCapacity > _items.Length)
			{
				object[] items = NewArray(minCapacity);
				Array.Copy(_items, 0, items, 0, _count);
				_items = items;
			}
		}
		
		object[] NewArray(int minCapacity)
		{
			int newLen = Math.Max(1, _items.Length)*2;				
			return new object[Math.Max(newLen, minCapacity)];
		}		
		
		void InnerRemoveAt(int index)
		{
			--_count;			
			_items[index] = null;
			if (index != _count)
			{
				Array.Copy(_items, index+1, _items, index, _count-index);
			}
		}
		
		void InnerRemove(object item)
		{
			int index = IndexOf(item);
			if (index != -1)
			{
				InnerRemoveAt(index);
			}
		}

		void InnerCollect(List target, Predicate condition)
		{
			for (int i=0; i<_count; ++i)
			{
				object item = _items[i];
				if (condition(item))
				{
					target.Add(item);
				}
			}
		}
		
		List InnerGetRange(int begin, int end)
		{
			int targetLen = end-begin;
			if (targetLen > 0)
			{
				object[] target = new object[targetLen];
				Array.Copy(_items, begin, target, 0, targetLen);
				return new List(target, true);
			}
			return new List();
		}
		
		int AdjustIndex(int index)
		{
			if (index > _count)
			{
				return _count;
			}
			if (index < 0)
			{
				return 0;
			}
			return index;
		}
		
		int CheckIndex(int index)
		{
			if (index >= _count)
			{
				throw new IndexOutOfRangeException();
			}
			return index;
		}
		
		int NormalizeIndex(int index)
		{
			if (index < 0)
			{
				index += _count;
			}			
			return index;
		}
		
		class ListEnumerator : IEnumerator
		{
			List _list;
			object[] _items;
			int _count;
			int _index;
			object _current;
			
			public ListEnumerator(List list)
			{
				_list = list;
				_items = list._items;
				_count = list._count;
				_index = 0;
			}
			
			public void Reset()
			{
				_index = 0;
			}
			
			public bool MoveNext()
			{
				if (_count != _list.Count || _items != _list._items)
				{					
					throw new InvalidOperationException(Boo.ResourceManager.GetString("ListWasModified"));
				}
				
				if (_index < _count)
				{
					_current = _items[_index];
					++_index;
					return true;
				}
				return false;
			}
			
			public object Current
			{
				get
				{
					return _current;
				}
			}
		}
	}
}
