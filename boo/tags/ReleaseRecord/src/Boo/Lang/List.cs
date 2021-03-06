#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// mailto:rbo@acm.org
#endregion

using System;
using System.Collections;
using System.Text;

namespace Boo.Lang
{
	public delegate bool Predicate(object item);

	/// <summary>
	/// List.
	/// </summary>
	[Serializable]
	public class List : IList
	{
		protected ArrayList _list;
		
		public List()
		{
			_list = new ArrayList();
		}
		
		public List(IEnumerable enumerable)
		{
			_list = new ArrayList();
			foreach (object item in enumerable)
			{
				_list.Add(item);
			}
		}
		
		public List(int initialCapacity)
		{
			_list = new ArrayList(initialCapacity);
		}		                                     

		public List(params object[] items)
		{
			_list = new ArrayList(items);
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
			
			List result = new List(_list.Count*count);
			for (int i=0; i<count; ++i)
			{
				result.Extend(_list);
			}
			return result;
		}
		
		public int Count
		{
			get
			{
				return _list.Count;
			}
		}
		
		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
		}
		
		public void CopyTo(Array target, int index)
		{
			_list.CopyTo(target, index);
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
				return this;
			}
		}

		public object this[int index]
		{
			get
			{				
				return _list[NormalizeIndex(index)];
			}
			
			set
			{
				_list[NormalizeIndex(index)] = value;
			}
		}

		public List Add(object item)
		{
			_list.Add(item);
			return this;
		}

		public List AddUnique(object item)
		{
			if (!_list.Contains(item))
			{
				_list.Add(item);
			}
			return this;
		}
		
		public List Extend(IEnumerable enumerable)
		{
			foreach (object item in enumerable)
			{
				_list.Add(item);
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
			return _list.ToArray(targetType);
		}
		
		public object[] ToArray()
		{
			return (object[])ToArray(typeof(object));
		}
		
		public List Sort()
		{
			_list.Sort();
			return this;
		}

		public override string ToString()
		{
			return Join(", ");
		}
		
		public string Join(string separator)
		{
			return Builtins.join(this, separator);
		}
		
		public void Clear()
		{
			_list.Clear();
		}
		
		public bool Contains(object item)
		{
			return _list.Contains(item);
		}
		
		public int IndexOf(object item)
		{			
			return _list.IndexOf(item);
		}
		
		void IList.Insert(int index, object item)
		{
			_list.Insert(index, item);
		}
		
		void IList.Remove(object item)
		{			
			_list.Remove(item);
		}
		
		void IList.RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}
		
		int IList.Add(object item)
		{			
			return _list.Add(item);
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

		void InnerCollect(List target, Predicate condition)
		{
			foreach (object item in _list)
			{
				if (condition(item))
				{
					target.Add(item);
				}
			}
		}
		
		int NormalizeIndex(int index)
		{
			if (index < 0)
			{
				index += _list.Count;
			}
			return index;
		}
	}
}
