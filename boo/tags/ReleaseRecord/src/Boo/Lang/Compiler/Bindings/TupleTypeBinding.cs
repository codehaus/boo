﻿#region license
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

namespace Boo.Lang.Compiler.Bindings
{
	using System;
	
	public class TupleTypeBinding : ITypeBinding, INamespace
	{	
		BindingManager _bindingManager;
		
		ITypeBinding _elementType;
		
		ITypeBinding _array;
		
		public TupleTypeBinding(BindingManager bindingManager, ITypeBinding elementType)
		{
			_bindingManager = bindingManager;
			_array = bindingManager.ArrayTypeBinding;
			_elementType = elementType;
		}
		
		public string Name
		{
			get
			{
				return string.Format("({0})", _elementType.FullName);
			}
		}
		
		public BindingType BindingType
		{
			get			
			{
				return BindingType.Tuple;
			}
		}
		
		public string FullName
		{
			get
			{
				return Name;
			}
		}
		
		public ITypeBinding BoundType
		{
			get
			{
				return this;
			}
		}
		
		public bool IsClass
		{
			get
			{
				return false;
			}
		}
		
		public bool IsEnum
		{
			get
			{
				return false;
			}
		}
		
		public bool IsValueType
		{
			get
			{
				return false;
			}
		}
		
		public bool IsArray
		{
			get
			{
				return true;
			}
		}
		
		public ITypeBinding GetElementType()
		{
			return _elementType;
		}
		
		public ITypeBinding BaseType
		{
			get
			{
				return _array;
			}
		}
		
		public IBinding GetDefaultMember()
		{
			return null;
		}
		
		public virtual bool IsSubclassOf(ITypeBinding other)
		{
			return other.IsAssignableFrom(_array);
		}
		
		public virtual bool IsAssignableFrom(ITypeBinding other)
		{			
			if (other == this)
			{
				return true;
			}
			
			if (other.IsArray)
			{
				ITypeBinding otherElementType = other.GetElementType();
				if (_elementType.IsValueType || otherElementType.IsValueType)
				{
					return _elementType == otherElementType;
				}
				return _elementType.IsAssignableFrom(otherElementType);
			}
			return false;
		}
		
		public IConstructorBinding[] GetConstructors()
		{
			return new IConstructorBinding[0];
		}
		
		public IBinding Resolve(string name)
		{
			return _array.Resolve(name);
		}
		
		public override string ToString()
		{
			return Name;
		}
	}
}
