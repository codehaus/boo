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
// ast.py script on Fri Feb 06 11:56:14 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class MacroStatementImpl : Statement
	{
		protected string _name;
		protected ExpressionCollection _arguments;
		protected Block _block;
		
		protected MacroStatementImpl()
		{
			_arguments = new ExpressionCollection(this);
			Block = new Block();
 		}
		
		protected MacroStatementImpl(string name)
		{
			_arguments = new ExpressionCollection(this);
			Block = new Block();
 			Name = name;
		}
		
		protected MacroStatementImpl(LexicalInfo lexicalInfo, string name) : base(lexicalInfo)
		{
			_arguments = new ExpressionCollection(this);
			Block = new Block();
 			Name = name;				
		}
		
		protected MacroStatementImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_arguments = new ExpressionCollection(this);
			Block = new Block();
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.MacroStatement;
			}
		}
		public string Name
		{
			get
			{
				return _name;
			}
			
			set
			{
				
				_name = value;
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
		public Block Block
		{
			get
			{
				return _block;
			}
			
			set
			{
				
				if (_block != value)
				{
					_block = value;
					if (null != _block)
					{
						_block.InitializeParent(this);
					}
				}
			}
		}
		public override void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			MacroStatement thisNode = (MacroStatement)this;
			Statement resultingTypedNode = thisNode;
			transformer.OnMacroStatement(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
