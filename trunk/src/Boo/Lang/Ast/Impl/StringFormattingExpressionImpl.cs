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
// astgenerator.boo on 03/25/2004 14:42:29
//

namespace Boo.Lang.Ast.Impl
{	
	using Boo.Lang.Ast;
	using System.Collections;
	using System.Runtime.Serialization;
	
	[System.Serializable]
	public abstract class StringFormattingExpressionImpl : Expression
	{

		protected string _template;
		protected ExpressionCollection _arguments;

		protected StringFormattingExpressionImpl()
		{
			InitializeFields();
		}
		
		protected StringFormattingExpressionImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected StringFormattingExpressionImpl(string template)
		{
			InitializeFields();
			Template = template;
		}
			
		protected StringFormattingExpressionImpl(LexicalInfo lexicalInfo, string template) : base(lexicalInfo)
		{
			InitializeFields();
			Template = template;
		}
			
		new public StringFormattingExpression CloneNode()
		{
			return Clone() as StringFormattingExpression;
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.StringFormattingExpression;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			StringFormattingExpression thisNode = (StringFormattingExpression)this;
			Expression resultingTypedNode = thisNode;
			transformer.OnStringFormattingExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_arguments != null)
			{
				Expression item = existing as Expression;
				if (null != item)
				{
					Expression newItem = (Expression)newNode;
					if (_arguments.Replace(item, newItem))
					{
						return true;
					}
				}
			}

			return false;
		}

		override public object Clone()
		{
			StringFormattingExpression clone = FormatterServices.GetUninitializedObject(typeof(StringFormattingExpression)) as StringFormattingExpression;
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = _properties.Clone() as Hashtable;
			

			clone._template = _template;

			if (null != _arguments)
			{
				clone._arguments = _arguments.CloneNode();
				clone._arguments.InitializeParent(clone);
			}
			
			return clone;
		}
			
		public string Template
		{
			get
			{
				return _template;
			}
			

			set
			{
				_template = value;
			}

		}
		

		public ExpressionCollection Arguments
		{
			get
			{
				return _arguments;
			}
			

			set
			{
				if (_arguments != value)
				{
					_arguments = value;
					if (null != _arguments)
					{
						_arguments.InitializeParent(this);

					}
				}
			}
			

		}
		

		private void InitializeFields()
		{
			_arguments = new ExpressionCollection(this);

		}
	}
}
