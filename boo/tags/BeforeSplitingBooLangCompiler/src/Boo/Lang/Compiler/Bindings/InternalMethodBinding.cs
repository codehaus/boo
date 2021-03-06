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
using Boo.Lang.Ast;

namespace Boo.Lang.Compiler.Bindings
{
	public class InternalMethodBinding : AbstractInternalBinding, IMethodBinding, INamespace
	{
		BindingManager _bindingManager;
		
		Boo.Lang.Ast.Method _method;
		
		IMethodBinding _override;
		
		ITypeBinding _declaringType;
		
		public ExpressionCollection ReturnExpressions;
		
		public ExpressionCollection SuperExpressions;
		
		internal InternalMethodBinding(BindingManager manager, Method method) : this(manager, method, false)
		{
		}
		
		internal InternalMethodBinding(BindingManager manager, Boo.Lang.Ast.Method method, bool visited) : base(visited)
		{			
			_bindingManager = manager;
			_method = method;
			if (method.NodeType != NodeType.Constructor)
			{
				SuperExpressions = new ExpressionCollection();
				ReturnExpressions = new ExpressionCollection();
				if (null == _method.ReturnType)
				{
					if (_method.DeclaringType.NodeType == NodeType.ClassDefinition)
					{
						_method.ReturnType = new SimpleTypeReference("unknown");
						BindingManager.Bind(_method.ReturnType, UnknownBinding.Default);
					}
					else
					{
						_method.ReturnType = new SimpleTypeReference("System.Void");
						BindingManager.Bind(_method.ReturnType, _bindingManager.VoidTypeBinding);
					}
				}
			}
		}
		
		public ITypeBinding DeclaringType
		{
			get
			{
				if (null == _declaringType)
				{
					_declaringType = (ITypeBinding)BindingManager.GetBinding(_method.DeclaringType);
				}
				return _declaringType;
			}
		}
		
		public bool IsStatic
		{
			get
			{
				return _method.IsStatic;
			}
		}
		
		public bool IsPublic
		{
			get
			{
				return _method.IsPublic;
			}
		}
		
		public bool IsVirtual
		{
			get
			{
				return !_method.IsFinal;
			}
		}
		
		public string Name
		{
			get
			{
				return _method.Name;
			}
		}
		
		public string FullName
		{
			get
			{
				return _method.DeclaringType.FullName + "." + _method.Name;
			}
		}
		
		public virtual BindingType BindingType
		{
			get
			{
				return BindingType.Method;
			}
		}
		
		public ITypeBinding BoundType
		{
			get
			{
				return ReturnType;
			}
		}
		
		public int ParameterCount
		{
			get
			{
				return _method.Parameters.Count;
			}
		}
		
		public Method Method
		{
			get
			{
				return _method;
			}
		}
		
		override public Node Node
		{
			get
			{
				return _method;
			}
		}
		
		public IMethodBinding Override
		{
			get
			{
				return _override;
			}
			
			set
			{
				_override = value;
			}
		}
		
		public ITypeBinding GetParameterType(int parameterIndex)
		{
			return _bindingManager.GetBoundType(_method.Parameters[parameterIndex].Type);
		}
		
		public ITypeBinding ReturnType
		{
			get
			{					
				return _bindingManager.GetBoundType(_method.ReturnType);
			}
		}
		
		public INamespace ParentNamespace
		{
			get
			{
				return DeclaringType;
			}
		}
		
		public IBinding Resolve(string name)
		{
			foreach (Local local in _method.Locals)
			{
				if (local.PrivateScope)
				{
					continue;
				}
				
				if (name == local.Name)
				{
					return BindingManager.GetBinding(local);
				}
			}
			
			foreach (ParameterDeclaration parameter in _method.Parameters)
			{
				if (name == parameter.Name)
				{
					return BindingManager.GetBinding(parameter);
				}
			}
			return null;
		}
		
		override public string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			builder.Append(_method.FullName);
			builder.Append("(");
			
			int i=0;
			foreach (ParameterDeclaration parameter in _method.Parameters)
			{
				if (i > 0)
				{
					builder.Append(", ");					
				}
				else
				{
					++i;
				}
				if (null == parameter.Type)
				{
					builder.Append("System.Object");
				}
				else
				{
					builder.Append(parameter.Type.ToString());
				}
			}
			
			builder.Append(")");
			return builder.ToString();
		}
	}
	
	public class InternalConstructorBinding : InternalMethodBinding, IConstructorBinding
	{
		bool _hasSuperCall = false;
		
		public InternalConstructorBinding(BindingManager bindingManager,
		                                  Constructor constructor) : base(bindingManager, constructor)
		  {
		  }
		  
		public InternalConstructorBinding(BindingManager bindingManager,
		                                  Constructor constructor,
										  bool visited) : base(bindingManager, constructor, visited)
		  {
		  }
		  
		public bool HasSuperCall
		{
			get
			{
				return _hasSuperCall;
			}
			
			set
			{
				_hasSuperCall = value;
			}
		}
	      
	    override public BindingType BindingType
	    {
	    	get
	    	{
	    		return BindingType.Constructor;
	    	}
	    }
	}
}
