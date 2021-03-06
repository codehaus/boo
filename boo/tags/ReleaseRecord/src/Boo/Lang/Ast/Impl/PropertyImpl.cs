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

//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by the
// ast.py script on Thu Feb 19 02:30:23 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class PropertyImpl : TypeMember
	{
		protected ParameterDeclarationCollection _parameters;
		protected Method _getter;
		protected Method _setter;
		protected TypeReference _type;
		
		protected PropertyImpl()
		{
			_parameters = new ParameterDeclarationCollection(this);
 		}
		
		protected PropertyImpl(Method getter, Method setter, TypeReference type)
		{
			_parameters = new ParameterDeclarationCollection(this);
 			Getter = getter;
			Setter = setter;
			Type = type;
		}
		
		protected PropertyImpl(LexicalInfo lexicalInfo, Method getter, Method setter, TypeReference type) : base(lexicalInfo)
		{
			_parameters = new ParameterDeclarationCollection(this);
 			Getter = getter;				
			Setter = setter;				
			Type = type;				
		}
		
		protected PropertyImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_parameters = new ParameterDeclarationCollection(this);
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.Property;
			}
		}
		public ParameterDeclarationCollection Parameters
		{
			get
			{
				return _parameters;
			}
			
			set
			{
				
				if (_parameters != value)
				{
					_parameters = value;
					if (null != _parameters)
					{
						_parameters.InitializeParent(this);
					}
				}
			}
		}
		public Method Getter
		{
			get
			{
				return _getter;
			}
			
			set
			{
				
				if (_getter != value)
				{
					_getter = value;
					if (null != _getter)
					{
						_getter.InitializeParent(this);
					}
				}
			}
		}
		public Method Setter
		{
			get
			{
				return _setter;
			}
			
			set
			{
				
				if (_setter != value)
				{
					_setter = value;
					if (null != _setter)
					{
						_setter.InitializeParent(this);
					}
				}
			}
		}
		public TypeReference Type
		{
			get
			{
				return _type;
			}
			
			set
			{
				
				if (_type != value)
				{
					_type = value;
					if (null != _type)
					{
						_type.InitializeParent(this);
					}
				}
			}
		}
		new public Property CloneNode()
		{
			return (Property)Clone();
		}
		
		override public object Clone()
		{
			Property clone = (Property)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			if (null != _parameters)
			{
				clone._parameters = (ParameterDeclarationCollection)_parameters.Clone();
			}
			if (null != _getter)
			{
				clone._getter = (Method)_getter.Clone();
			}
			if (null != _setter)
			{
				clone._setter = (Method)_setter.Clone();
			}
			if (null != _type)
			{
				clone._type = (TypeReference)_type.Clone();
			}
			clone._modifiers = _modifiers;
			clone._name = _name;
			if (null != _attributes)
			{
				clone._attributes = (AttributeCollection)_attributes.Clone();
			}
			
			return clone;
		}
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			if (_parameters != null)
			{
				ParameterDeclaration item = existing as ParameterDeclaration;
				if (null != item)
				{
					if (_parameters.Replace(item, (ParameterDeclaration)newNode))
					{
						return true;
					}
				}
			}
			if (_getter == existing)
			{
				this.Getter = (Method)newNode;
				return true;
			}
			if (_setter == existing)
			{
				this.Setter = (Method)newNode;
				return true;
			}
			if (_type == existing)
			{
				this.Type = (TypeReference)newNode;
				return true;
			}
			if (_attributes != null)
			{
				Attribute item = existing as Attribute;
				if (null != item)
				{
					if (_attributes.Replace(item, (Attribute)newNode))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Property thisNode = (Property)this;
			Property resultingTypedNode = thisNode;
			transformer.OnProperty(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
