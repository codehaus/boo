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
	public class StatementCollectionImpl : NodeCollection
	{
		protected StatementCollectionImpl()
		{
		}
		
		protected StatementCollectionImpl(Node parent) : base(parent)
		{
		}
		
		public Boo.Lang.Ast.Statement this[int index]
		{
			get
			{
				return (Boo.Lang.Ast.Statement)InnerList[index];
			}
		}

		public void Add(Boo.Lang.Ast.Statement item)
		{
			base.Add(item);			
		}
		
		public void Extend(params Boo.Lang.Ast.Statement[] items)
		{
			base.Add(items);			
		}
		
		public void Extend(System.Collections.ICollection items)
		{
			foreach (Boo.Lang.Ast.Statement item in items)
			{
				base.Add(item);
			}
		}
		
		public void ExtendWithClones(System.Collections.ICollection items)
		{
			foreach (Boo.Lang.Ast.Statement item in items)
			{
				base.Add(item.CloneNode());
			}
		}
		
		public void Insert(int index, Boo.Lang.Ast.Statement item)
		{
			base.Insert(index, item);
		}
		
		public bool Replace(Boo.Lang.Ast.Statement existing, Boo.Lang.Ast.Statement newItem)
		{
			return base.Replace(existing, newItem);
		}
		
		public new Boo.Lang.Ast.Statement[] ToArray()
		{
			return (Boo.Lang.Ast.Statement[])InnerList.ToArray(typeof(Boo.Lang.Ast.Statement));
		}
	}
}
