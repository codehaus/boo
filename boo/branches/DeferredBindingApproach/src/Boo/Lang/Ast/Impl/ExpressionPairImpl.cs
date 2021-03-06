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
// ast.py script on Mon Jan 19 20:43:01 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class ExpressionPairImpl : Node
	{
		protected Expression _first;
		protected Expression _second;
		
		protected ExpressionPairImpl()
		{
 		}
		
		protected ExpressionPairImpl(Expression first, Expression second)
		{
 			First = first;
			Second = second;
		}
		
		protected ExpressionPairImpl(LexicalInfo lexicalInfo, Expression first, Expression second) : base(lexicalInfo)
		{
 			First = first;				
			Second = second;				
		}
		
		protected ExpressionPairImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.ExpressionPair;
			}
		}
		public Expression First
		{
			get
			{
				return _first;
			}
			
			set
			{
				
				if (_first != value)
				{
					_first = value;
					if (null != _first)
					{
						_first.InitializeParent(this);
					}
				}
			}
		}
		public Expression Second
		{
			get
			{
				return _second;
			}
			
			set
			{
				
				if (_second != value)
				{
					_second = value;
					if (null != _second)
					{
						_second.InitializeParent(this);
					}
				}
			}
		}
		public override void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			ExpressionPair thisNode = (ExpressionPair)this;
			ExpressionPair resultingTypedNode = thisNode;
			transformer.OnExpressionPair(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
