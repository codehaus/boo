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

namespace Boo.Lang.Compiler.Bindings
{
	public abstract class AbstractTypeBinding : ITypeBinding, INamespace
	{	
		public abstract string Name
		{
			get;
		}
		
		public abstract BindingType BindingType
		{
			get;
		}
		
		public string FullName
		{
			get
			{
				return Name;
			}
		}
		
		public virtual ITypeBinding BoundType
		{
			get
			{
				return this;
			}
		}
		
		public virtual bool IsClass
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
				return false;
			}
		}
		
		public ITypeBinding GetElementType()
		{
			return null;
		}
		
		public ITypeBinding BaseType
		{
			get
			{
				return null;
			}
		}
		
		public IBinding GetDefaultMember()
		{
			return null;
		}
		
		public virtual bool IsSubclassOf(ITypeBinding other)
		{
			return false;
		}
		
		public virtual bool IsAssignableFrom(ITypeBinding other)
		{
			return false;
		}
		
		public IConstructorBinding[] GetConstructors()
		{
			return new IConstructorBinding[0];
		}
		
		public IBinding Resolve(string name)
		{
			return null;
		}
		
		public override string ToString()
		{
			return Name;
		}
	}
	
	public class NullBinding : AbstractTypeBinding
	{
		public static NullBinding Default = new NullBinding();
		
		private NullBinding()
		{
		}
		
		public override string Name
		{
			get
			{
				return "null";
			}
		}
		
		public override BindingType BindingType
		{
			get
			{
				return BindingType.Null;
			}
		}
	}
	
	public class UnknownBinding : AbstractTypeBinding
	{
		public static UnknownBinding Default = new UnknownBinding();
		
		private UnknownBinding()
		{
		}
		
		public override string Name
		{
			get
			{
				return "unknown";
			}
		}
		
		public override BindingType BindingType
		{
			get
			{
				return BindingType.Unknown;
			}
		}
	}
	
	public class ErrorBinding : AbstractTypeBinding
	{
		public static ErrorBinding Default = new ErrorBinding();
		
		private ErrorBinding()
		{			
		}	
		
		public override string Name
		{
			get
			{
				return "error";
			}
		}
		
		public override BindingType BindingType
		{
			get
			{
				return BindingType.Error;
			}
		}
	}
}
