#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
//	   * Redistributions of source code must retain the above copyright notice,
//	   this list of conditions and the following disclaimer.
//	   * Redistributions in binary form must reproduce the above copyright notice,
//	   this list of conditions and the following disclaimer in the documentation
//	   and/or other materials provided with the distribution.
//	   * Neither the name of Rodrigo B. de Oliveira nor the names of its
//	   contributors may be used to endorse or promote products derived from this
//	   software without specific prior written permission.
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

using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.TypeSystem
{
	using System;
	using System.Reflection;

	public class ExternalType : IType
	{
		protected TypeSystemServices _typeSystemServices;

		private NameResolutionService _nameResolutionService;

		private readonly Type _type;

		IConstructor[] _constructors;

		IType[] _interfaces;

		IEntity[] _members;

		int _typeDepth = -1;

		string _primitiveName;

		string _fullName;

		private string _name;

		public ExternalType(TypeSystemServices tss, Type type)
		{
			if (null == type) throw new ArgumentException("type");
			_typeSystemServices = tss;
			_type = type;
			_nameResolutionService = _typeSystemServices.Context.NameResolutionService;
		}

		public virtual string FullName
		{
			get
			{
				if (null != _fullName) return _fullName;
				return _fullName = BuildFullName();
			}
		}

		internal string PrimitiveName
		{
			get { return _primitiveName; }

			set { _primitiveName = value; }
		}

		public virtual string Name
		{
			get
			{
				if (null != _name) return _name;
				return _name = TypeName();
			}
		}

		private string TypeName()
		{
			return TypeUtilities.TypeName(_type);
		}

		public EntityType EntityType
		{
			get { return EntityType.Type; }
		}

		public IType Type
		{
			get { return this; }
		}

		public virtual bool IsFinal
		{
			get { return _type.IsSealed; }
		}

		public bool IsByRef
		{
			get { return _type.IsByRef; }
		}

		public virtual IEntity DeclaringEntity
		{
			get { return DeclaringType;  }
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

		public bool IsDefined(IType attributeType)
		{
			ExternalType type = attributeType as ExternalType;
			if (null == type) return false;
			return MetadataUtil.IsAttributeDefined(_type, type.ActualType);
		}

		public virtual IType GetElementType()
		{
			return _typeSystemServices.Map(_type.GetElementType() ?? _type);
		}

		public virtual bool IsClass
		{
			get { return _type.IsClass; }
		}

		public bool IsAbstract
		{
			get { return _type.IsAbstract; }
		}

		public bool IsInterface
		{
			get { return _type.IsInterface; }
		}

		public bool IsEnum
		{
			get { return _type.IsEnum; }
		}

		public virtual bool IsValueType
		{
			get { return _type.IsValueType; }
		}

		public bool IsArray
		{
			get { return false; }
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
		
		protected virtual MemberInfo[] GetDefaultMembers()
		{
			MemberInfo[] miarr = ActualType.GetDefaultMembers();
			
			if(this.IsInterface && GetInterfaces() != null)
			{
				System.Collections.Generic.List<MemberInfo> memlist = 
					new System.Collections.Generic.List<MemberInfo>();
				if(miarr != null)
					memlist.AddRange(miarr);
				foreach(ExternalType type in GetInterfaces())
				{
					miarr = type.GetDefaultMembers();
					if(miarr != null)
						memlist.AddRange(miarr);
				}
				
				miarr = memlist.ToArray();
			}
			
			return miarr;
		}

		public IEntity GetDefaultMember()
		{
			return _typeSystemServices.Map(GetDefaultMembers());
		}

		public Type ActualType
		{
			get { return _type; }
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
				_constructors = CreateConstructors();
			return _constructors;
		}

		protected virtual IConstructor[] CreateConstructors()
		{
			ConstructorInfo[] source = _type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			IConstructor[] constructors = new IConstructor[source.Length];
			for (int i=0; i<constructors.Length; ++i)
			{
				constructors[i] = new ExternalConstructor(_typeSystemServices, source[i]);
			}
			return constructors;
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
				IEntity[] members = CreateMembers();
				_members = members;
			}
			return _members;
		}

		protected virtual IEntity[] CreateMembers()
		{
			List<IEntity> result = new List<IEntity>();
			foreach (MemberInfo member in DeclaredMembers())
				result.Add(_typeSystemServices.Map(member));
			foreach (Type nested in _type.GetNestedTypes())
				result.Add(_typeSystemServices.Map(nested));
			return result.ToArray();
		}

		private MemberInfo[] DeclaredMembers()
		{
			return _type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
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
			get { return null; }
		}

		public virtual bool Resolve(List targetList, string name, EntityType flags)
		{
			bool found = _nameResolutionService.Resolve(name, GetMembers(), flags, targetList);

			if (IsInterface)
			{
				if (_typeSystemServices.ObjectType.Resolve(targetList, name, flags))
					found = true;
				foreach (IType baseInterface in GetInterfaces())
					found |= baseInterface.Resolve(targetList, name, flags);
			}
			else
			{
				if (!found || TypeSystemServices.ContainsMethodsOnly(targetList))
				{
					IType baseType = BaseType;
					if (null != baseType)
						found |= baseType.Resolve(targetList, name, flags);
				}
			}
			return found;
		}

		override public string ToString()
		{
			return FullName;
		}

		static int GetTypeDepth(Type type)
		{
			if (type.IsByRef)
			{
				return GetTypeDepth(type.GetElementType());
			}
			else if (type.IsInterface)
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
			if (_primitiveName != null) return _primitiveName;

			// keep builtin names pretty ('ref int' instead of 'ref System.Int32')
			if (_type.IsByRef) return "ref " + GetElementType().FullName;

			return Boo.Lang.Compiler.Util.TypeUtilities.GetFullName(_type);
		}

		ExternalGenericTypeInfo _genericTypeDefinitionInfo = null;
		public virtual IGenericTypeInfo GenericInfo
		{
			get
			{
				if (ActualType.IsGenericTypeDefinition)
				{
					if (_genericTypeDefinitionInfo == null)
					{
						_genericTypeDefinitionInfo = new ExternalGenericTypeInfo(_typeSystemServices, this);
					}
					return _genericTypeDefinitionInfo;
				}
				return null;
			}
		}

		ExternalConstructedTypeInfo _genericTypeInfo = null;
		public virtual IConstructedTypeInfo ConstructedInfo
		{
			get
			{
				if (ActualType.IsGenericType && !ActualType.IsGenericTypeDefinition)
				{
					if (_genericTypeInfo == null)
					{
						_genericTypeInfo = new ExternalConstructedTypeInfo(_typeSystemServices, this);
					}
					return _genericTypeInfo;
				}
				return null;
			}
		}
	}
}
