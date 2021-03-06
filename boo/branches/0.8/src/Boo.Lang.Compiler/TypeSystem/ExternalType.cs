#region license
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
	using System.Reflection;

	public class ExternalType : IType
	{
		protected TypeSystemServices _typeSystemServices;
		
		Type _type;
		
		IConstructor[] _constructors;
		
		IType[] _interfaces;
		
		IEntity[] _members;
		
		int _typeDepth = -1;
		
		string _primitiveName;
		
		string _fullName;
		
		internal ExternalType(TypeSystemServices tss, Type type)
		{
			if (null == type)
			{
				throw new ArgumentException("type");
			}
			_typeSystemServices = tss;
			_type = type;
		}
		
		public virtual string FullName
		{
			get
			{
				if (null == _fullName)
				{
					_fullName = BuildFullName();
				}
				return _fullName;
			}
		}
		
		internal string PrimitiveName
		{
			get
			{
				return _primitiveName;
			}
			
			set
			{
				_primitiveName = value;
			}
		}
		
		public virtual string Name
		{
			get
			{
				return _type.Name;
			}
		}
		
		public EntityType EntityType
		{
			get
			{
				return EntityType.Type;
			}
		}
		
		public IType Type
		{
			get
			{
				return this;
			}
		}
		
		public virtual bool IsFinal
		{
			get
			{
				return _type.IsSealed;
			}
		}
		
		public bool IsByRef
		{
			get
			{
				return _type.IsByRef;
			}
		}
		
		public IType DeclaringType
		{
			get
			{
				System.Type declaringType = _type.DeclaringType;
				return null != declaringType
					? _typeSystemServices.Map(declaringType)
					: null;
			}
		}
		
		public virtual IType GetElementType()
		{
			return _typeSystemServices.Map(_type.GetElementType());
		}
		
		public bool IsClass
		{
			get
			{
				return _type.IsClass;
			}
		}
		
		public bool IsAbstract
		{
			get
			{
				return _type.IsAbstract;
			}
		}
		
		public bool IsInterface
		{
			get
			{
				return _type.IsInterface;
			}
		}
		
		public bool IsEnum
		{
			get
			{
				return _type.IsEnum;
			}
		}
		
		public bool IsValueType
		{
			get
			{
				return _type.IsValueType;
			}
		}
		
		public bool IsArray
		{
			get
			{
				return false;
			}
		}
		
		public virtual IType BaseType
		{
			get
			{
								
				Type baseType = _type.BaseType;
				return null == baseType
					? null
					: _typeSystemServices.Map(baseType);
			}
		}
		
		public virtual IEntity GetDefaultMember()
		{
			return _typeSystemServices.Map(_type.GetDefaultMembers());
		}
		
		public Type ActualType
		{
			get
			{
				return _type;
			}
		}
		
		public virtual bool IsSubclassOf(IType other)
		{
			ExternalType external = other as ExternalType;
			if (null == external /*|| _typeSystemServices.VoidType == other*/)
			{
				return false;
			}
			
			return _type.IsSubclassOf(external._type) ||
				(external.IsInterface && external._type.IsAssignableFrom(_type))
				;
		}
		
		public virtual bool IsAssignableFrom(IType other)
		{
			ExternalType external = other as ExternalType;
			if (null == external)
			{
				if (EntityType.Null == other.EntityType)
				{
					return !IsValueType;
				}
				return other.IsSubclassOf(this);
			}
			if (other == _typeSystemServices.VoidType)
			{
				return false;
			}
			return _type.IsAssignableFrom(external._type);
		}
		
		public virtual IConstructor[] GetConstructors()
		{
			if (null == _constructors)
			{
				BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				ConstructorInfo[] ctors = _type.GetConstructors(flags);
				_constructors = new IConstructor[ctors.Length];
				for (int i=0; i<_constructors.Length; ++i)
				{
					_constructors[i] = new ExternalConstructor(_typeSystemServices, ctors[i]);
				}
			}
			return _constructors;
		}
		
		public virtual IType[] GetInterfaces()
		{
			if (null == _interfaces)
			{
				Type[] interfaces = _type.GetInterfaces();
				_interfaces = new IType[interfaces.Length];
				for (int i=0; i<_interfaces.Length; ++i)
				{
					_interfaces[i] = _typeSystemServices.Map(interfaces[i]);
				}
			}
			return _interfaces;
		}
		
		public virtual IEntity[] GetMembers()
		{
			if (null == _members)
			{
				BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;// | BindingFlags.FlattenHierarchy;
				MemberInfo[] members = _type.GetMembers(flags);
				Type[] nested = _type.GetNestedTypes();
				_members = new IEntity[members.Length+nested.Length];
				int i = 0;
				for (i=0; i<members.Length; ++i)
				{
					_members[i] = _typeSystemServices.Map(members[i]);
				}
				for (int j=0; j<nested.Length; ++j)
				{
					_members[i++] = _typeSystemServices.Map(nested[j]);
				}
			}
			return _members;
		}
		
		public int GetTypeDepth()
		{
			if (-1 == _typeDepth)
			{
				_typeDepth = GetTypeDepth(_type);
			}
			return _typeDepth;
		}
		
		public virtual INamespace ParentNamespace
		{
			get
			{
				return null;
			}
		}
		
		public virtual bool Resolve(List targetList, string name, EntityType flags)
		{
			bool found = false;
			foreach (IEntity member in GetMembers())
			{
				if (member.Name == name && NameResolutionService.IsFlagSet(flags, member.EntityType))
				{
					targetList.AddUnique(member);
					found = true;
				}
			}
			
			if (IsInterface)
			{
				if (_typeSystemServices.ObjectType.Resolve(targetList, name, flags))
				{
					found = true;
				}
				
				foreach (IType baseInterface in GetInterfaces())
				{
					found |= baseInterface.Resolve(targetList, name, flags);
				}
			}
			else
			{
				if (!found || TypeSystemServices.ContainsMethodsOnly(targetList))
				{
					IType baseType = BaseType;
					if (null != baseType)
					{
						found |= baseType.Resolve(targetList, name, flags);
					}
				}
			}
			return found;
		}
		
		override public string ToString()
		{
			return _primitiveName != null ? _primitiveName : FullName;
		}
		
		static int GetTypeDepth(Type type)
		{
			if (type.IsByRef)
			{
				return GetTypeDepth(type.GetElementType());
			}
			if (type.IsInterface)
			{
				return GetInterfaceDepth(type);
			}
			return GetClassDepth(type);
		}
		
		static int GetClassDepth(Type type)
		{
			int depth = 0;
			Type objectType = Types.Object;
			while (type != objectType)
			{
				type = type.BaseType;
				++depth;
			}
			return depth;
		}
		
		static int GetInterfaceDepth(Type type)
		{
			Type[] interfaces = type.GetInterfaces();
			if (interfaces.Length > 0)
			{
				int current = 0;
				foreach (Type i in interfaces)
				{
					int depth = GetInterfaceDepth(i);
					if (depth > current)
					{
						current = depth;
					}
				}
				return 1+current;
			}
			return 1;
		}
		
		protected virtual string BuildFullName()
		{
			if (_type.IsByRef) return "ref " + this.GetElementType().ToString();
			if (_type.DeclaringType != null) return this.DeclaringType.ToString() + "." + _type.Name;		
			
#if NET_2_0
			// HACK: Some constructed generic types report a FullName of null
			if (_type.FullName == null) 
			{
				string[] argumentNames = Array.ConvertAll<Type, string>(
					_type.GetGenericArguments(),
					delegate(Type t) { return t.FullName; });
				
				return string.Format(
					"{0}[{1}]",
					_type.GetGenericTypeDefinition().FullName,
					string.Join(",", argumentNames));
				
			}			
#endif
			return _type.FullName;
		}
		
#if NET_2_0		
		ExternalGenericTypeDefinitionInfo _genericTypeDefinitionInfo = null;		
		public virtual IGenericTypeDefinitionInfo GenericTypeDefinitionInfo
		{
			get
			{
				if (ActualType.IsGenericTypeDefinition)
				{
					if (_genericTypeDefinitionInfo == null)
					{
						_genericTypeDefinitionInfo = new ExternalGenericTypeDefinitionInfo(_typeSystemServices, this);
					}
					return _genericTypeDefinitionInfo;
				}
				return null;
			}
		}

		ExternalGenericTypeInfo _genericTypeInfo = null;
		public virtual IGenericTypeInfo GenericTypeInfo
		{
			get
			{
				if (ActualType.IsGenericType)
				{
					if (_genericTypeInfo == null)
					{
						_genericTypeInfo = new ExternalGenericTypeInfo(_typeSystemServices, this);
					}
					return _genericTypeInfo;
				}
				return null;
			}
		}	
#else
		IGenericTypeDefinitionInfo IType.GenericTypeDefinitionInfo
		{
			get { return null; }
		}
		
		IGenericTypeInfo IType.GenericTypeInfo
		{
			get { return null; }
		}		
#endif

	}
}
