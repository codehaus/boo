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
// astgenerator.boo on 2/27/2004 1:11:12 AM
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public abstract class StatementModifierImpl : Node
	{

		protected StatementModifierType _type;
		protected Expression _condition;

		protected StatementModifierImpl()
		{
			InitializeFields();
		}
		
		protected StatementModifierImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected StatementModifierImpl(StatementModifierType type, Expression condition)
		{
			InitializeFields();
			Type = type;
			Condition = condition;
		}
			
		protected StatementModifierImpl(LexicalInfo lexicalInfo, StatementModifierType type, Expression condition) : base(lexicalInfo)
		{
			InitializeFields();
			Type = type;
			Condition = condition;
		}
			
		new public Boo.Lang.Ast.StatementModifier CloneNode()
		{
			return (Boo.Lang.Ast.StatementModifier)Clone();
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.StatementModifier;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.StatementModifier thisNode = (Boo.Lang.Ast.StatementModifier)this;
			Boo.Lang.Ast.StatementModifier resultingTypedNode = thisNode;
			transformer.OnStatementModifier(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_condition == existing)
			{
				this.Condition = (Boo.Lang.Ast.Expression)newNode;
				return true;
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.StatementModifier clone = (Boo.Lang.Ast.StatementModifier)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.StatementModifier));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			clone._type = _type;

			if (null != _condition)
			{
				clone._condition = (Expression)_condition.Clone();
			}
			
			return clone;
		}
			
		public StatementModifierType Type
		{
			get
			{
				return _type;
			}
			

			set
			{
				_type = value;
			}

		}
		

		public Expression Condition
		{
			get
			{
				return _condition;
			}
			

			set
			{
				if (_condition != value)
				{
					_condition = value;
					if (null != _condition)
					{
						_condition.InitializeParent(this);

					}
				}
			}
			

		}
		

		private void InitializeFields()
		{

		}
	}
}
