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
// ast.py script on Fri Feb 13 19:16:59 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class StatementModifierImpl : Node
	{
		protected StatementModifierType _type;
		protected Expression _condition;
		
		protected StatementModifierImpl()
		{
 		}
		
		protected StatementModifierImpl(StatementModifierType type, Expression condition)
		{
 			Type = type;
			Condition = condition;
		}
		
		protected StatementModifierImpl(LexicalInfo lexicalInfo, StatementModifierType type, Expression condition) : base(lexicalInfo)
		{
 			Type = type;				
			Condition = condition;				
		}
		
		protected StatementModifierImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.StatementModifier;
			}
		}
		public StatementModifierType Type
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
				}
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
		new public StatementModifier CloneNode()
		{
			return (StatementModifier)Clone();
		}
		
		override public object Clone()
		{
			StatementModifier clone = (StatementModifier)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
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
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			if (_condition == existing)
			{
				this.Condition = (Expression)newNode;
				return true;
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			StatementModifier thisNode = (StatementModifier)this;
			StatementModifier resultingTypedNode = thisNode;
			transformer.OnStatementModifier(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
