﻿#region license
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
// This file was generated automatically by
// astgenerator.boo on 2/29/2004 12:21:42 AM
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public abstract class EnumMemberImpl : TypeMember
	{

		protected IntegerLiteralExpression _initializer;

		protected EnumMemberImpl()
		{
			InitializeFields();
		}
		
		protected EnumMemberImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected EnumMemberImpl(IntegerLiteralExpression initializer)
		{
			InitializeFields();
			Initializer = initializer;
		}
			
		protected EnumMemberImpl(LexicalInfo lexicalInfo, IntegerLiteralExpression initializer) : base(lexicalInfo)
		{
			InitializeFields();
			Initializer = initializer;
		}
			
		new public Boo.Lang.Ast.EnumMember CloneNode()
		{
			return Clone() as Boo.Lang.Ast.EnumMember;
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.EnumMember;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.EnumMember thisNode = (Boo.Lang.Ast.EnumMember)this;
			Boo.Lang.Ast.EnumMember resultingTypedNode = thisNode;
			transformer.OnEnumMember(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_attributes != null)
			{
				Boo.Lang.Ast.Attribute item = existing as Boo.Lang.Ast.Attribute;
				if (null != item)
				{
					if (_attributes.Replace(item, (Boo.Lang.Ast.Attribute)newNode))
					{
						return true;
					}
				}
			}

			if (_initializer == existing)
			{
				this.Initializer = ((Boo.Lang.Ast.IntegerLiteralExpression)newNode);
				return true;
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.EnumMember clone = (Boo.Lang.Ast.EnumMember)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.EnumMember));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			clone._modifiers = _modifiers;

			clone._name = _name;

			if (null != _attributes)
			{
				clone._attributes = ((AttributeCollection)_attributes.Clone());
			}

			if (null != _initializer)
			{
				clone._initializer = ((IntegerLiteralExpression)_initializer.Clone());
			}
			
			return clone;
		}
			
		public IntegerLiteralExpression Initializer
		{
			get
			{
				return _initializer;
			}
			

			set
			{
				if (_initializer != value)
				{
					_initializer = value;
					if (null != _initializer)
					{
						_initializer.InitializeParent(this);

					}
				}
			}
			

		}
		

		private void InitializeFields()
		{

		}
	}
}
