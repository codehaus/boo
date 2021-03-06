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
	using System;
	using System.Collections;
	using Boo.Lang.Ast;
	
	public abstract class AbstractInternalBinding : IInternalBinding
	{
		protected bool _visited;
		
		public AbstractInternalBinding()
		{
			_visited = false;
		}
		
		public AbstractInternalBinding(bool visited)
		{
			_visited = visited;
		}
		
		public abstract Node Node
		{
			get;
		}
		
		public bool Visited
		{
			get
			{
				return _visited;
			}
			
			set
			{
				_visited = value;
			}
		}		
	}
	
	public abstract class AbstractInternalTypeBinding : AbstractInternalBinding, ITypeBinding, INamespace
	{		
		protected BindingManager _bindingManager;
		
		protected TypeDefinition _typeDefinition;
		
		protected IBinding[] _members;
		
		protected INamespace _parentNamespace;
		
		protected ArrayList _memberBuffer = new ArrayList();
		
		protected AbstractInternalTypeBinding(BindingManager bindingManager, TypeDefinition typeDefinition)
		{
			_bindingManager = bindingManager;
			_typeDefinition = typeDefinition;
			_parentNamespace = (INamespace)BindingManager.GetBinding(_typeDefinition.ParentNode);
		}
		
		public string FullName
		{
			get
			{
				return _typeDefinition.FullName;
			}
		}
		
		public string Name
		{
			get
			{
				return _typeDefinition.Name;
			}
		}	
		
		override public Node Node
		{
			get
			{
				return _typeDefinition;
			}
		}
		
		public virtual INamespace ParentNamespace
		{
			get
			{
				return _parentNamespace;
			}
		}
		
		public virtual IBinding Resolve(string name)
		{			
			_memberBuffer.Clear();			
			
			foreach (IBinding binding in GetMembers())
			{
				if (binding.Name == name)
				{
					_memberBuffer.Add(binding);
				}
			}
			
			if (0 == _memberBuffer.Count)
			{
				foreach (TypeReference baseType in _typeDefinition.BaseTypes)
				{
					IBinding binding = _bindingManager.GetBoundType(baseType).Resolve(name);
					if (null != binding)
					{
						_memberBuffer.Add(binding);
					}
				}
			}
			
			if (_memberBuffer.Count > 0)
			{
				if (_memberBuffer.Count > 1)
				{
					return new AmbiguousBinding((IBinding[])_memberBuffer.ToArray(typeof(IBinding)));
				}
				else
				{
					return (IBinding)_memberBuffer[0];
				}
			}
			return null;
		}
		
		IBinding CreateCorrectBinding(TypeMember member)
		{
			switch (member.NodeType)
			{
				case NodeType.Method:
				{
					return new InternalMethodBinding(_bindingManager, (Method)member);
				}
				
				case NodeType.Constructor:
				{
					return new InternalConstructorBinding(_bindingManager, (Constructor)member);
				}
				
				case NodeType.Field:
				{
					return new InternalFieldBinding(_bindingManager, (Field)member);
				}
				
				case NodeType.EnumDefinition:
				{
					return new EnumTypeBinding(_bindingManager, (EnumDefinition)member);
				}
				
				case NodeType.EnumMember:
				{
					return new InternalEnumMemberBinding(_bindingManager, (EnumMember)member);
				}
				
				case NodeType.Property:
				{
					return new InternalPropertyBinding(_bindingManager, (Property)member);
				}
			}
			throw new NotImplementedException(member.GetType().ToString());
		}
		
		public virtual ITypeBinding BaseType
		{
			get
			{
				return null;
			}
		}
		
		public TypeDefinition TypeDefinition
		{
			get
			{
				return _typeDefinition;
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
				return NodeType.ClassDefinition == _typeDefinition.NodeType;
			}
		}
		
		public bool IsInterface
		{
			get
			{
				return NodeType.InterfaceDefinition == _typeDefinition.NodeType;
			}
		}
		
		public bool IsEnum
		{
			get
			{
				return NodeType.EnumDefinition == _typeDefinition.NodeType;
			}
		}
		
		public bool IsValueType
		{
			get
			{
				return IsEnum;
			}
		}
		
		public bool IsArray
		{
			get
			{
				return false;
			}
		}
		
		public int GetArrayRank()
		{
			return 0;
		}
		
		public ITypeBinding GetElementType()
		{
			return null;
		}
		
		public IBinding GetDefaultMember()
		{
			ITypeBinding defaultMemberAttribute = _bindingManager.AsTypeBinding(typeof(System.Reflection.DefaultMemberAttribute));
			foreach (Boo.Lang.Ast.Attribute attribute in _typeDefinition.Attributes)
			{
				IConstructorBinding binding = BindingManager.GetBinding(attribute) as IConstructorBinding;
				if (null != binding)
				{
					if (defaultMemberAttribute == binding.DeclaringType)
					{
						StringLiteralExpression memberName = attribute.Arguments[0] as StringLiteralExpression;
						if (null != memberName)
						{
							return Resolve(memberName.Value);
						}
					}
				}
			}
			return null;
		}
		
		public virtual BindingType BindingType
		{
			get
			{
				return BindingType.Type;
			}
		}
		
		public virtual bool IsSubclassOf(ITypeBinding other)
		{
			return false;
		}
		
		public virtual bool IsAssignableFrom(ITypeBinding other)
		{
			return this == other ||
					(!this.IsValueType && NullBinding.Default == other) ||
					other.IsSubclassOf(this);
		}
		
		public virtual IConstructorBinding[] GetConstructors()
		{
			return new IConstructorBinding[0];
		}
		
		public virtual IBinding[] GetMembers()
		{
			if (null == _members)
			{
				_memberBuffer.Clear();
				foreach (TypeMember member in _typeDefinition.Members)
				{
					IBinding binding = BindingManager.GetOptionalBinding(member);
					if (null == binding)
					{						
						binding = CreateCorrectBinding(member);
						BindingManager.Bind(member, binding);
					}	
					
					if (BindingType.Type == binding.BindingType)
					{
						binding = _bindingManager.AsTypeReference((ITypeBinding)binding);
					}
					_memberBuffer.Add(binding);
				}

				_members = (IBinding[])_memberBuffer.ToArray(typeof(IBinding));
				_memberBuffer.Clear();				
			}
			return _members;
		}
		
		override public string ToString()
		{
			return FullName;
		}
	}

}
