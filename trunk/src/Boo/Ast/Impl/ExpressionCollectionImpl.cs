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
// ast.py script on Mon Jan 12 19:25:19 2004
//
using System;
using Boo.Ast;

namespace Boo.Ast.Impl
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="Expression"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>ExpressionCollection</b> provides an <see cref="System.Collections.ArrayList"/> 
	/// that is strongly typed for <see cref="Expression"/> elements.
	/// </remarks> 
	[Serializable]
	public class ExpressionCollectionImpl : NodeCollection
	{
		protected ExpressionCollectionImpl()
		{
		}
		
		protected ExpressionCollectionImpl(Node parent) : base(parent)
		{
		}
		
		public Expression this[int index]
		{
			get
			{
				return (Expression)InnerList[index];
			}
		}

		public void Add(Expression item)
		{
			base.Add(item);			
		}
		
		public void Add(params Expression[] items)
		{
			base.Add(items);			
		}
		
		public void Add(System.Collections.ICollection items)
		{
			foreach (Expression item in items)
			{
				base.Add(item);
			}
		}
		
		public void Insert(int index, Expression item)
		{
			base.Insert(index, item);
		}
		
		public void Replace(Expression existing, Expression newItem)
		{
			base.Replace(existing, newItem);
		}
		
		public new Expression[] ToArray()
		{
			return (Expression[])InnerList.ToArray(typeof(Expression));
		}
	}
}
