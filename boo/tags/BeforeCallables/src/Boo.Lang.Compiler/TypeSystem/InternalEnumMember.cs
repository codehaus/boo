#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

namespace Boo.Lang.Compiler.TypeSystem
{
	using System;
	using Boo.Lang.Compiler.Ast;
	
	public class InternalEnumMember : IInternalEntity, IField
	{
		TypeSystemServices _typeSystemServices;
		
		EnumMember _member;
		
		public InternalEnumMember(TypeSystemServices tagManager, EnumMember member)
		{
			_typeSystemServices = tagManager;
			_member = member;
		}
		
		public string Name
		{
			get
			{
				return _member.Name;
			}
		}
		
		public string FullName
		{
			get
			{
				return _member.DeclaringType.FullName + "." + _member.Name;
			}
		}
		
		public bool IsStatic
		{
			get
			{
				return true;
			}
		}
		
		public bool IsPublic
		{
			get
			{
				return true;
			}
		}
		
		public bool IsLiteral
		{
			get
			{
				return true;
			}
		}
		
		public EntityType EntityType
		{
			get
			{
				return EntityType.Field;
			}
		}
		
		public IType Type
		{
			get
			{
				return DeclaringType;
			}
		}
		
		public IType DeclaringType
		{
			get
			{
				return (IType)TypeSystemServices.GetEntity(_member.ParentNode);
			}
		}
		
		public object StaticValue
		{
			get
			{
				return _member.Initializer.Value;
			}
		}
		
		public Node Node
		{
			get
			{
				return _member;
			}
		}
	}
}
