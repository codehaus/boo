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
// ast.py script on Wed Feb 11 12:57:26 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class AssertStatementImpl : Statement
	{
		protected Expression _condition;
		protected Expression _message;
		
		protected AssertStatementImpl()
		{
 		}
		
		protected AssertStatementImpl(Expression condition, Expression message)
		{
 			Condition = condition;
			Message = message;
		}
		
		protected AssertStatementImpl(LexicalInfo lexicalInfo, Expression condition, Expression message) : base(lexicalInfo)
		{
 			Condition = condition;				
			Message = message;				
		}
		
		protected AssertStatementImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.AssertStatement;
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
		public Expression Message
		{
			get
			{
				return _message;
			}
			
			set
			{
				
				if (_message != value)
				{
					_message = value;
					if (null != _message)
					{
						_message.InitializeParent(this);
					}
				}
			}
		}
		new public AssertStatement CloneNode()
		{
			return (AssertStatement)Clone();
		}
		
		override public object Clone()
		{
			AssertStatement clone = (AssertStatement)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			if (null != _condition)
			{
				clone._condition = (Expression)_condition.Clone();
			}
			if (null != _message)
			{
				clone._message = (Expression)_message.Clone();
			}
			if (null != _modifier)
			{
				clone._modifier = (StatementModifier)_modifier.Clone();
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
			if (_message == existing)
			{
				this.Message = (Expression)newNode;
				return true;
			}
			if (_modifier == existing)
			{
				this.Modifier = (StatementModifier)newNode;
				return true;
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			AssertStatement thisNode = (AssertStatement)this;
			Statement resultingTypedNode = thisNode;
			transformer.OnAssertStatement(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
