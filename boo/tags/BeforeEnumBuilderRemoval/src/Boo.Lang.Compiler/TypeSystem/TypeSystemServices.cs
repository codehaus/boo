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

namespace Boo.Lang.Compiler.TypeSystem
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Text;
	using Boo.Lang.Compiler;	
	using Boo.Lang.Compiler.Ast;

	public class TypeSystemServices
	{			
		public DuckTypeImpl DuckType;
		
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
		
		public ExternalType IEnumeratorType;
		
		public ExternalType ICollectionType;
		
		public ExternalType IListType;
		
		public ExternalType IDictionaryType;
		
		System.Collections.Hashtable _primitives = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _entityCache = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _arrayCache = new System.Collections.Hashtable();
		
		System.Collections.Hashtable _anonymousCallableTypes = new System.Collections.Hashtable();
		
		public static readonly IType ErrorEntity = Boo.Lang.Compiler.TypeSystem.Error.Default;
		
		StringBuilder _buffer = new StringBuilder();
		
		Boo.Lang.Compiler.Ast.Module _anonymousTypesModule;
		
		CompilerContext _context;
		
		public TypeSystemServices() : this(new CompilerContext())
		{
		}
		
		public TypeSystemServices(CompilerContext context)		
		{			
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			
			_context = context;
			
			Cache(typeof(Boo.Lang.Builtins.duck), DuckType = new DuckTypeImpl(this));
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
			Cache(IEnumeratorType = new ExternalType(this, typeof(System.Collections.IEnumerator)));
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
		
		public bool IsCallable(IType type)
		{
			return (TypeType == type) ||
				(ICallableType.IsAssignableFrom(type)) ||
				(type is Boo.Lang.Compiler.TypeSystem.ICallableType);
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
		
		public IType GetExpressionType(Expression node)
		{			
			IType type = node.ExpressionType;
			if (null == type)
			{
				throw CompilerErrorFactory.InvalidNode(node);
			}
			return type;
		}
		
		public IType GetConcreteExpressionType(Expression expression)
		{
			IType type = GetExpressionType(expression);
			AnonymousCallableType anonymousType = type as AnonymousCallableType;
			if (null != anonymousType)
			{
				if (null == anonymousType.ConcreteType)
				{
					CreateConcreteCallableType(expression, anonymousType);
				}
				expression.ExpressionType = anonymousType.ConcreteType;
				return anonymousType.ConcreteType;
			}
			return type;
		}
		
		public ParameterDeclaration CreateParameterDeclaration(int index, string name, IType type)
		{
			ParameterDeclaration parameter = new ParameterDeclaration(name, CreateTypeReference(type));
			parameter.Entity = new InternalParameter(parameter, index);
			return parameter;
		}
		
		public Boo.Lang.Compiler.Ast.Module GetAnonymousTypesModule()
		{
			if (null == _anonymousTypesModule)
			{
				_anonymousTypesModule = new Boo.Lang.Compiler.Ast.Module();
				_anonymousTypesModule.Entity = new ModuleEntity(_context.NameResolutionService, 
																this,
																_anonymousTypesModule);
				_context.CompileUnit.Modules.Add(_anonymousTypesModule);
			}
			return _anonymousTypesModule;
		}
		
		public ClassDefinition CreateCallableDefinition(string name)
		{
			ClassDefinition cd = new ClassDefinition();
			cd.BaseTypes.Add(CreateTypeReference(this.MulticastDelegateType));
			cd.BaseTypes.Add(CreateTypeReference(this.ICallableType));
			cd.Name = name;
			cd.Modifiers = TypeMemberModifiers.Final;
			cd.Members.Add(CreateCallableConstructor());
			cd.Members.Add(CreateCallMethod());			
			cd.Entity = new InternalCallableType(this, cd);
			return cd;
		}
		
		Method CreateCallMethod()
		{
			Method method = new Method("Call");
			method.Modifiers = TypeMemberModifiers.Public|TypeMemberModifiers.Virtual;
			method.Parameters.Add(CreateParameterDeclaration(1, "args", ObjectArrayType));
			method.ReturnType = CreateTypeReference(ObjectType);
			method.Entity = new InternalMethod(this, method);
			return method;
		}
		
		Constructor CreateCallableConstructor()
		{
			Constructor constructor = new Constructor();
			constructor.Modifiers = TypeMemberModifiers.Public;
			constructor.ImplementationFlags = MethodImplementationFlags.Runtime;
			constructor.Parameters.Add(
						CreateParameterDeclaration(1, "instance", ObjectType));
			constructor.Parameters.Add(
						CreateParameterDeclaration(2, "method", IntPtrType));
			constructor.Entity = new InternalConstructor(this, constructor);						
			return constructor;
		}
		
		public static bool IsCallableTypeAssignableFrom(ICallableType lhs, IType rhs)
		{
			if (lhs == rhs || Null.Default == rhs)
			{
				return true;
			}
			
			ICallableType other = rhs as ICallableType;
			if (null != other)
			{			
				return lhs.GetSignature() == other.GetSignature(); 
			}
			return false;
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
		
		public IMethod Map(MethodInfo method)
		{
			object key = GetCacheKey(method);
			IMethod entity = (IMethod)_entityCache[key];
			if (null == entity)
			{
				entity = new ExternalMethod(this, method);
				_entityCache[key] = entity;
			}
			return entity;
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
			IEntity tag = (IEntity)_entityCache[GetCacheKey(mi)];
			if (null == tag)
			{			
				switch (mi.MemberType)
				{
					case MemberTypes.Method:
					{
						return Map((System.Reflection.MethodInfo)mi);
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
				_entityCache.Add(GetCacheKey(mi), tag);
			}
			return tag;
		}
		
		public string GetSignature(IMethod method)
		{
			_buffer.Length = 0;
			_buffer.Append(method.FullName);
			_buffer.Append("(");
			
			IParameter[] parameters = method.GetParameters();
			for (int i=0; i<parameters.Length; ++i)
			{
				if (i > 0) { _buffer.Append(", "); }
				_buffer.Append(parameters[i].Type.FullName);
			}
			_buffer.Append(")");
			return _buffer.ToString();
		}
		
		public object GetCacheKey(System.Reflection.MemberInfo mi)
		{
			return mi;
		}
		
		public IEntity ResolvePrimitive(string name)
		{
			return (IEntity)_primitives[name];
		}
		
		public bool IsPrimitive(string name)
		{
			return _primitives.ContainsKey(name);
		}
		
		/// <summary>
		/// checks if the passed type will be equivalente to
		/// System.Object in runtime (accounting for the presence
		/// of duck typing.
		/// </summary>
		public bool IsSystemObject(IType type)
		{
			return type == ObjectType || type == DuckType;
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
			AddBuiltin(BuiltinFunction.Len);
			AddBuiltin(BuiltinFunction.AddressOf);
			AddBuiltin(BuiltinFunction.Eval);
		}
		
		void AddPrimitiveType(string name, ExternalType type)
		{
			_primitives[name] = type;
		}
		
		void AddBuiltin(BuiltinFunction function)
		{
			_primitives[function.Name] = function;
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
		
		Method CreateBeginInvokeMethod(AnonymousCallableType anonymousType)
		{
			Method method = CreateRuntimeMethod("BeginInvoke", Map(typeof(IAsyncResult)),
												anonymousType.GetSignature().Parameters);
												
			int delta=method.Parameters.Count;
			method.Parameters.Add(
					CreateParameterDeclaration(delta+1, "callback", Map(typeof(AsyncCallback))));
			method.Parameters.Add(
					CreateParameterDeclaration(delta+1, "asyncState", ObjectType));
			return method;
		}
		
		Method CreateEndInvokeMethod(AnonymousCallableType anonymousType)
		{
			CallableSignature signature = anonymousType.GetSignature();
			Method method = CreateRuntimeMethod("EndInvoke", signature.ReturnType);
			int delta=method.Parameters.Count;
			method.Parameters.Add(
				CreateParameterDeclaration(delta+1, "result", Map(typeof(IAsyncResult))));
			return method;
		}
		
		Method CreateInvokeMethod(AnonymousCallableType anonymousType)
		{
			CallableSignature signature = anonymousType.GetSignature();
			return CreateRuntimeMethod("Invoke", signature.ReturnType, signature.Parameters);
		}
		
		Method CreateRuntimeMethod(string name, IType returnType, IParameter[] parameters)
		{
			Method method = CreateRuntimeMethod(name, returnType);
			for (int i=0; i<parameters.Length; ++i)
			{
				method.Parameters.Add(CreateParameterDeclaration(i,
									"arg" + i,
									parameters[i].Type));
			}
			return method;
		}
		
		public IConstructor GetDefaultConstructor(IType type)
		{
			IConstructor[] constructors = type.GetConstructors();
			for (int i=0; i<constructors.Length; ++i)
			{
				IConstructor constructor = constructors[i];
				if (0 == constructor.GetParameters().Length)
				{
					return constructor;
				}
			}
			return null;
		}
		
		public MethodInvocationExpression CreateConstructorInvocation(IConstructor constructor, Expression arg)
		{
			MethodInvocationExpression mie = CreateConstructorInvocation(constructor);
			mie.LexicalInfo = arg.LexicalInfo;
			mie.Arguments.Add(arg);
			return mie;
		}
		
		public MethodInvocationExpression CreateConstructorInvocation(IConstructor constructor)
		{
			MethodInvocationExpression mie = new MethodInvocationExpression();
			mie.Target = new ReferenceExpression(constructor.DeclaringType.FullName);			
			mie.Target.Entity = constructor;
			mie.ExpressionType = constructor.DeclaringType;
			return mie;
		}
		
		public Statement CreateSuperConstructorInvocation(IType baseType)
		{			
			IConstructor defaultConstructor = GetDefaultConstructor(baseType);
			Debug.Assert(null != defaultConstructor);
			
			MethodInvocationExpression call = new MethodInvocationExpression(new SuperLiteralExpression());			
			call.Target.Entity = defaultConstructor;
			call.ExpressionType = VoidType;
			
			return new ExpressionStatement(call);
		}
		
		public Method CreateVirtualMethod(string name, IType returnType)
		{
			Method method = new Method(name);
			method.Modifiers = TypeMemberModifiers.Public|TypeMemberModifiers.Virtual;
			method.ReturnType = CreateTypeReference(returnType);
			method.Entity = new InternalMethod(this, method);
			return method;
		}
		
		Method CreateRuntimeMethod(string name, IType returnType)
		{
			Method method = CreateVirtualMethod(name, returnType);
			method.ImplementationFlags = MethodImplementationFlags.Runtime;			
			return method;
		}
		
		void CreateConcreteCallableType(Node sourceNode, AnonymousCallableType anonymousType)
		{
			Boo.Lang.Compiler.Ast.Module module = GetAnonymousTypesModule();
			
			string name = string.Format("__anonymous{0}__", module.Members.Count);
			ClassDefinition cd = CreateCallableDefinition(name);
			cd.Modifiers |= TypeMemberModifiers.Public;
			cd.LexicalInfo = sourceNode.LexicalInfo;
			
			cd.Members.Add(CreateInvokeMethod(anonymousType));
			cd.Members.Add(CreateBeginInvokeMethod(anonymousType));
			cd.Members.Add(CreateEndInvokeMethod(anonymousType));
			_anonymousTypesModule.Members.Add(cd);
			
			anonymousType.ConcreteType = (IType)cd.Entity;
		}
		
		private static void InvalidNode(Node node)
		{
			throw CompilerErrorFactory.InvalidNode(node);
		}

		public class DuckTypeImpl : ExternalType
		{	
			public DuckTypeImpl(TypeSystemServices typeSystemServices) : 
				base(typeSystemServices, Types.Object)
			{
			}
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
