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
// ast.py script on Fri Feb 13 19:16:50 2004
//
using System;
using Boo.Lang.Ast;

namespace Boo.Lang.Ast.Impl
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="Module"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>ModuleCollection</b> provides an <see cref="System.Collections.ArrayList"/> 
	/// that is strongly typed for <see cref="Module"/> elements.
	/// </remarks> 
	[Serializable]
	public class ModuleCollectionImpl : NodeCollection
	{
		protected ModuleCollectionImpl()
		{
		}
		
		protected ModuleCollectionImpl(Node parent) : base(parent)
		{
		}
		
		public Module this[int index]
		{
			get
			{
				return (Module)InnerList[index];
			}
		}

		public void Add(Module item)
		{
			base.Add(item);			
		}
		
		public void Add(params Module[] items)
		{
			base.Add(items);			
		}
		
		public void Add(System.Collections.ICollection items)
		{
			foreach (Module item in items)
			{
				base.Add(item);
			}
		}
		
		public void AddClones(System.Collections.ICollection items)
		{
			foreach (Module item in items)
			{
				base.Add(item.CloneNode());
			}
		}
		
		public void Insert(int index, Module item)
		{
			base.Insert(index, item);
		}
		
		public bool Replace(Module existing, Module newItem)
		{
			return base.Replace(existing, newItem);
		}
		
		public new Module[] ToArray()
		{
			return (Module[])InnerList.ToArray(typeof(Module));
		}
	}
}
