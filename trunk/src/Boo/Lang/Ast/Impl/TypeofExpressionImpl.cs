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
// astgenerator.boo on 3/14/2004 7:21:24 PM
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public abstract class TypeofExpressionImpl : Expression
	{

		protected TypeReference _type;

		protected TypeofExpressionImpl()
		{
			InitializeFields();
		}
		
		protected TypeofExpressionImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected TypeofExpressionImpl(TypeReference type)
		{
			InitializeFields();
			Type = type;
		}
			
		protected TypeofExpressionImpl(LexicalInfo lexicalInfo, TypeReference type) : base(lexicalInfo)
		{
			InitializeFields();
			Type = type;
		}
			
		new public Boo.Lang.Ast.TypeofExpression CloneNode()
		{
			return Clone() as Boo.Lang.Ast.TypeofExpression;
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.TypeofExpression;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.TypeofExpression thisNode = (Boo.Lang.Ast.TypeofExpression)this;
			Boo.Lang.Ast.Expression resultingTypedNode = thisNode;
			transformer.OnTypeofExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_type == existing)
			{
				this.Type = ((Boo.Lang.Ast.TypeReference)newNode);
				return true;
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.TypeofExpression clone = (Boo.Lang.Ast.TypeofExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.TypeofExpression));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			if (null != _type)
			{
				clone._type = ((TypeReference)_type.Clone());
				clone._type.InitializeParent(clone);
			}
			
			return clone;
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
		

		private void InitializeFields()
		{

		}
	}
}
