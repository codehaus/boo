﻿#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Boo.Lang.Compiler.TypeSystem
{
	using System;
	using System.Collections;
	using Boo.Lang.Compiler.Ast;
	
	public abstract class AbstractInternalType : IInternalEntity, IType, INamespace
	{		
		protected TypeSystemServices _typeSystemServices;
		
		protected TypeDefinition _typeDefinition;
		
		protected IEntity[] _members;
		
		protected IType[] _interfaces;
		
		protected INamespace _parentNamespace;
		
		protected Boo.Lang.List _buffer = new Boo.Lang.List();
		
		protected AbstractInternalType(TypeSystemServices typeSystemServices, TypeDefinition typeDefinition)
		{
			_typeSystemServices = typeSystemServices;
			_typeDefinition = typeDefinition;			
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
		
		public Node Node
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
				if (null == _parentNamespace)
				{
					_parentNamespace = (INamespace)TypeSystemServices.GetEntity(_typeDefinition.ParentNode);
				}
				return _parentNamespace;
			}
		}
		
		public virtual bool Resolve(Boo.Lang.List targetList, string name, EntityType flags)
		{			
			bool found = false;
			
			foreach (IEntity tag in GetMembers())
			{
				if (tag.Name == name && NameResolutionService.IsFlagSet(flags, tag.EntityType))
				{
					targetList.Add(tag);
					found = true;
				}
			}
			
			return found;
		}
		
		public virtual IType BaseType
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
		
		public IType Type
		{
			get
			{
				return this;
			}
		}
		
		public bool IsByRef
		{
			get
			{
				return false;
			}
		}
		
		public IType GetElementType()
		{
			return null;
		}
		
		public bool IsClass
		{
			get
			{
				return NodeType.ClassDefinition == _typeDefinition.NodeType;
			}
		}
		
		public bool IsAbstract
		{
			get
			{
				return _typeDefinition.IsAbstract;
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
		
		public virtual int GetTypeDepth()
		{
			return 1;
		}
		
		public IEntity GetDefaultMember()
		{
			IType defaultMemberAttribute = _typeSystemServices.Map(typeof(System.Reflection.DefaultMemberAttribute));
			foreach (Boo.Lang.Compiler.Ast.Attribute attribute in _typeDefinition.Attributes)
			{
				IConstructor tag = TypeSystemServices.GetEntity(attribute) as IConstructor;
				if (null != tag)
				{
					if (defaultMemberAttribute == tag.DeclaringType)
					{
						StringLiteralExpression memberName = attribute.Arguments[0] as StringLiteralExpression;
						if (null != memberName)
						{
							_buffer.Clear();
							Resolve(_buffer, memberName.Value, EntityType.Any);
							return NameResolutionService.GetEntityFromList(_buffer);
						}
					}
				}
			}
			if (null != BaseType)
			{
				return BaseType.GetDefaultMember();
			}
			return null;
		}
		
		public virtual EntityType EntityType
		{
			get
			{
				return EntityType.Type;
			}
		}
		
		public virtual bool IsSubclassOf(IType other)
		{
			return false;
		}
		
		public virtual bool IsAssignableFrom(IType other)
		{
			return this == other ||
					(!this.IsValueType && Null.Default == other) ||
					other.IsSubclassOf(this);
		}
		
		public virtual IConstructor[] GetConstructors()
		{
			return new IConstructor[0];
		}
		
		public IType[] GetInterfaces()
		{
			if (null == _interfaces)
			{
				_buffer.Clear();
				
				foreach (TypeReference baseType in _typeDefinition.BaseTypes)
				{
					IType tag = TypeSystemServices.GetType(baseType);
					if (tag.IsInterface)
					{
						_buffer.AddUnique(tag);
					}
				}
				
				_interfaces = (IType[])_buffer.ToArray(typeof(IType));
			}
			return _interfaces;
		}
		
		public virtual IEntity[] GetMembers()
		{
			if (null == _members)
			{
				_buffer.Clear();
				foreach (TypeMember member in _typeDefinition.Members)
				{
					IEntity tag = TypeSystemServices.GetEntity(member);
					_buffer.Add(tag);
				}

				_members = (IEntity[])_buffer.ToArray(typeof(IEntity));
				_buffer.Clear();				
			}
			return _members;
		}
		
		override public string ToString()
		{
			return FullName;
		}
	}

}
