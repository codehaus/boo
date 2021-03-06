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
	using System.Reflection;
	using Boo.Lang.Compiler;	
	using Boo.Lang.Compiler.Ast;

	public class TypeSystemServices
	{			
		public ExternalType ExceptionType;
		
		public ExternalType ApplicationExceptionType;
		
		public ExternalType MulticastDelegateType;
		
		public ExternalType IntPtrType;
		
		public ExternalType ObjectType;
		
		public ExternalType EnumType;
		
		public ExternalType ArrayType;
		
		public ExternalType TypeType;
		
		public IType ObjectArrayType;
	
		public ExternalType VoidType;
		
		public ExternalType StringType;
		
		public ExternalType BoolType;
		
		public ExternalType ByteType;
		
		public ExternalType ShortType;
		
		public ExternalType IntType;
		
		public ExternalType LongType;
		
		public ExternalType SingleType;
		
		public ExternalType DoubleType;
		
		public ExternalType TimeSpanType;
		
		public ExternalType DateTimeType;
		
		public ExternalType RuntimeServicesType;
		
		public ExternalType BuiltinsType;
		
		public ExternalType ListType;
		
		public ExternalType HashType;
		
		public ExternalType ICallableType;
		
		public ExternalType IEnumerableType;
		
		public ExternalType ICollectionType;
		
		public ExternalType IListType;
		
		public ExternalType IDictionaryType;
		
		System.Collections.Hashtable _primitives = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _entityCache = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _arrayCache = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _anonymousCallableTypes = new System.Collections.Hashtable();
		
		static readonly IEntity _lenInfo = new BuiltinFunction(BuiltinFunctionType.Len);
		
		public static readonly IType ErrorEntity = Boo.Lang.Compiler.TypeSystem.Error.Default;
		
		public TypeSystemServices()		
		{			
			Cache(VoidType = new VoidTypeImpl(this));
			Cache(ObjectType = new ExternalType(this, Types.Object));
			Cache(EnumType = new ExternalType(this, typeof(System.Enum)));
			Cache(ArrayType = new ExternalType(this, Types.Array));
			Cache(TypeType = new ExternalType(this, Types.Type));
			Cache(StringType = new ExternalType(this, Types.String));
			Cache(BoolType = new ExternalType(this, Types.Bool));
			Cache(ByteType = new ExternalType(this, Types.Byte));
			Cache(ShortType = new ExternalType(this, Types.Short));
			Cache(IntType = new ExternalType(this, Types.Int));
			Cache(LongType = new ExternalType(this, Types.Long));
			Cache(SingleType = new ExternalType(this, Types.Single));
			Cache(DoubleType = new ExternalType(this, Types.Double));
			Cache(TimeSpanType = new ExternalType(this, Types.TimeSpan));
			Cache(DateTimeType = new ExternalType(this, Types.DateTime));
			Cache(RuntimeServicesType = new ExternalType(this, Types.RuntimeServices));
			Cache(BuiltinsType = new ExternalType(this, Types.Builtins));
			Cache(ListType = new ExternalType(this, Types.List));
			Cache(HashType = new ExternalType(this, Types.Hash));
			Cache(ICallableType = new ExternalType(this, Types.ICallable));
			Cache(IEnumerableType = new ExternalType(this, Types.IEnumerable));
			Cache(ICollectionType = new ExternalType(this, Types.ICollection));
			Cache(IListType = new ExternalType(this, Types.IList));
			Cache(IDictionaryType = new ExternalType(this, Types.IDictionary));
			Cache(ApplicationExceptionType = new ExternalType(this, Types.ApplicationException));
			Cache(ExceptionType = new ExternalType(this, Types.Exception));
			Cache(IntPtrType = new ExternalType(this, Types.IntPtr));
			Cache(MulticastDelegateType = new ExternalType(this, Types.MulticastDelegate));
			
			ObjectArrayType = GetArrayType(ObjectType);
			
			PreparePrimitives();
		}
		
		public Boo.Lang.Compiler.Ast.TypeReference CreateTypeReference(IType tag)
		{
			TypeReference typeReference = null;
			
			if (tag.IsArray)
			{
				IType elementType = ((IArrayType)tag).GetElementType();
				typeReference = new ArrayTypeReference(CreateTypeReference(elementType));
			}
			else
			{				
				typeReference = new SimpleTypeReference(tag.FullName);				
			}
			
			typeReference.Entity = tag;
			return typeReference;
		}
		
		public IType GetPromotedNumberType(IType left, IType right)
		{
			if (left == DoubleType ||
				right == DoubleType)
			{
				return DoubleType;
			}
			if (left == SingleType ||
				right == SingleType)
			{
				return SingleType;
			}
			if (left == LongType ||
				right == LongType)
			{
				return LongType;
			}
			if (left == ShortType ||
				right == ShortType)
			{
				return ShortType;
			}
			return left;
		}
		
		public ICallableType GetCallableType(IMethod method)
		{
			CallableSignature signature = new CallableSignature(method);
			ICallableType type = (ICallableType)_anonymousCallableTypes[signature];
			if (null == type)
			{
				type = new AnonymousCallableType(this, signature);
				_anonymousCallableTypes.Add(signature, type);
			}
			return type;
		}
		
		public static bool CheckOverrideSignature(IMethod impl, IMethod baseMethod)
		{
			IParameter[] implParameters = impl.GetParameters();
			IParameter[] baseParameters = baseMethod.GetParameters();
			
			if (implParameters.Length == baseParameters.Length)
			{
				for (int i=0; i<implParameters.Length; ++i)
				{
					if (implParameters[i].Type != baseParameters[i].Type)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		
		public static string GetSignature(IMethod tag)
		{			
			System.Text.StringBuilder sb = new System.Text.StringBuilder(tag.DeclaringType.FullName);
			sb.Append(".");
			sb.Append(tag.Name);
			sb.Append("(");
			
			IParameter[] parameters = tag.GetParameters();
			for (int i=0; i<parameters.Length; ++i)
			{				
				if (i>0) 
				{
					sb.Append(", ");
				}
				sb.Append(parameters[i].Type.FullName);
			}
			sb.Append(")");
			
			/*
			IType rt = tag.ReturnType;
			if (null != rt)
			{
				sb.Append(" as ");
				sb.Append(rt.FullName);
			}
			*/
			return sb.ToString();
		}
		
		public static bool IsUnknown(Expression node)
		{
			IType type = node.ExpressionType;
			if (null != type)
			{
				return IsUnknown(type);
			}
			return false;
		}
		
		public static bool IsUnknown(IType tag)
		{
			return EntityType.Unknown == tag.EntityType;
		}
		
		public static bool IsError(Expression node)
		{				 
			IType type = node.ExpressionType;
			if (null != type)
			{
				return IsError(type);
			}
			return false;
		}
		
		public static bool IsErrorAny(ExpressionCollection collection)
		{
			foreach (Expression n in collection)
			{
				if (IsError(n))
				{
					return true;
				}
			}
			return false;
		}
		
		public bool IsBuiltin(IEntity tag)
		{
			if (EntityType.Method == tag.EntityType)
			{
				return BuiltinsType == ((IMethod)tag).DeclaringType;
			}
			return false;
		}		
		
		public static bool IsError(IEntity tag)
		{
			return EntityType.Error == tag.EntityType;
		}
		
		public static IEntity GetEntity(Node node)
		{
			if (null == node)
			{
				throw new ArgumentNullException("node");
			}
			
			IEntity tag = node.Entity;
			if (null == tag)
			{
				InvalidNode(node);
			}
			return tag;
		}	
		
		public static IType GetType(Node node)
		{
			return ((ITypedEntity)GetEntity(node)).Type;
		}
		
		public IType Map(System.Type type)
		{				
			ExternalType tag = (ExternalType)_entityCache[type];
			if (null == tag)
			{
				if (type.IsArray)
				{
					return GetArrayType(Map(type.GetElementType()));
				}				
				else
				{
					if (type.IsSubclassOf(Types.MulticastDelegate))
					{
						tag = new ExternalCallableType(this, type);
					}
					else
					{
						tag = new ExternalType(this, type);
					}
				}
				Cache(tag);
			}
			return tag;
		}
		
		public IArrayType GetArrayType(IType elementType)
		{
			IArrayType tag = (IArrayType)_arrayCache[elementType];
			if (null == tag)
			{
				tag = new ArrayType(this, elementType);
				_arrayCache.Add(elementType, tag);
			}
			return tag;
		}
		
		public IParameter[] Map(Boo.Lang.Compiler.Ast.ParameterDeclarationCollection parameters)
		{
			IParameter[] mapped = new IParameter[parameters.Count];
			for (int i=0; i<mapped.Length; ++i)
			{
				mapped[i] = (IParameter)GetEntity(parameters[i]);
			}
			return mapped;
		}
		
		public IParameter[] Map(System.Reflection.ParameterInfo[] parameters)
		{			
			IParameter[] mapped = new IParameter[parameters.Length];
			for (int i=0; i<parameters.Length; ++i)
			{
				mapped[i] = new ExternalParameter(this, parameters[i]);
			}
			return mapped;
		}
		
		public IEntity Map(System.Reflection.MemberInfo[] info)
		{
			if (info.Length > 1)
			{
				IEntity[] tags = new IEntity[info.Length];
				for (int i=0; i<tags.Length; ++i)
				{
					tags[i] = Map(info[i]);
				}
				return new Ambiguous(tags);
			}
			if (info.Length > 0)
			{
				return Map(info[0]);
			}
			return null;
		}
		
		public IEntity Map(System.Reflection.MemberInfo mi)
		{
			IEntity tag = (IEntity)_entityCache[mi];
			if (null == tag)
			{			
				switch (mi.MemberType)
				{
					case MemberTypes.Method:
					{
						tag = new ExternalMethod(this, (System.Reflection.MethodInfo)mi);
						break;
					}
					
					case MemberTypes.Constructor:
					{
						tag = new ExternalConstructor(this, (System.Reflection.ConstructorInfo)mi);
						break;
					}
					
					case MemberTypes.Field:
					{
						tag = new ExternalField(this, (System.Reflection.FieldInfo)mi);
						break;
					}
					
					case MemberTypes.Property:
					{
						tag = new ExternalProperty(this, (System.Reflection.PropertyInfo)mi);
						break;
					}
					
					case MemberTypes.Event:
					{
						tag = new ExternalEvent(this, (System.Reflection.EventInfo)mi);
						break;
					}
					
					case MemberTypes.NestedType:
					{
						return Map((System.Type)mi);
					}
					
					default:
					{
						throw new NotImplementedException(mi.ToString());
					}
				}
				_entityCache.Add(mi, tag);
			}
			return tag;
		}
		
		public IEntity ResolvePrimitive(string name)
		{
			return (IEntity)_primitives[name];
		}
		
		public bool IsPrimitive(string name)
		{
			return _primitives.ContainsKey(name);
		}
		
		void PreparePrimitives()
		{
			AddPrimitiveType("void", VoidType);
			AddPrimitiveType("bool", BoolType);
			AddPrimitiveType("date", DateTimeType);
			AddPrimitiveType("string", StringType);
			AddPrimitiveType("object", ObjectType);
			AddPrimitiveType("byte", ByteType);
			AddPrimitiveType("int", IntType);
			AddPrimitiveType("long", LongType);
			AddPrimitiveType("single", SingleType);
			AddPrimitiveType("double", DoubleType);
			AddPrimitive("len", _lenInfo);
		}
		
		void AddPrimitiveType(string name, ExternalType type)
		{
			_primitives[name] = type;
		}
		
		void AddPrimitive(string name, IEntity tag)
		{
			_primitives[name] = tag;
		}
		
		void Cache(ExternalType tag)
		{
			_entityCache[tag.ActualType] = tag;
		}
		
		void Cache(object key, IType tag)
		{
			_entityCache[key] = tag;
		}
		
		private static void InvalidNode(Node node)
		{
			throw CompilerErrorFactory.InvalidNode(node);
		}		
		
		#region VoidTypeImpl
		class VoidTypeImpl : ExternalType
		{			
			internal VoidTypeImpl(TypeSystemServices manager) : base(manager, Types.Void)
			{				
			}		
			
			override public bool Resolve(Boo.Lang.List targetList, string name, EntityType flags)
			{	
				return false;
			}	
		}

		#endregion
	}
}
