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
// ast.py script on Mon Jan 19 20:43:00 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class GivenStatementImpl : Statement
	{
		protected Expression _expression;
		protected WhenClauseCollection _whenClauses;
		protected Block _otherwiseBlock;
		
		protected GivenStatementImpl()
		{
			_whenClauses = new WhenClauseCollection(this);
 		}
		
		protected GivenStatementImpl(Expression expression, Block otherwiseBlock)
		{
			_whenClauses = new WhenClauseCollection(this);
 			Expression = expression;
			OtherwiseBlock = otherwiseBlock;
		}
		
		protected GivenStatementImpl(LexicalInfo lexicalInfo, Expression expression, Block otherwiseBlock) : base(lexicalInfo)
		{
			_whenClauses = new WhenClauseCollection(this);
 			Expression = expression;				
			OtherwiseBlock = otherwiseBlock;				
		}
		
		protected GivenStatementImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_whenClauses = new WhenClauseCollection(this);
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.GivenStatement;
			}
		}
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			
			set
			{
				
				if (_expression != value)
				{
					_expression = value;
					if (null != _expression)
					{
						_expression.InitializeParent(this);
					}
				}
			}
		}
		public WhenClauseCollection WhenClauses
		{
			get
			{
				return _whenClauses;
			}
			
			set
			{
				
				if (_whenClauses != value)
				{
					_whenClauses = value;
					if (null != _whenClauses)
					{
						_whenClauses.InitializeParent(this);
					}
				}
			}
		}
		public Block OtherwiseBlock
		{
			get
			{
				return _otherwiseBlock;
			}
			
			set
			{
				
				if (_otherwiseBlock != value)
				{
					_otherwiseBlock = value;
					if (null != _otherwiseBlock)
					{
						_otherwiseBlock.InitializeParent(this);
					}
				}
			}
		}
		public override void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			GivenStatement thisNode = (GivenStatement)this;
			Statement resultingTypedNode = thisNode;
			transformer.OnGivenStatement(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
