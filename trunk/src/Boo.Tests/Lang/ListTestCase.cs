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

using System;
using NUnit.Framework;
using Boo.Lang;

namespace Boo.Tests.Lang
{
	/// <summary>	
	/// </summary>
	[TestFixture]
	public class ListTestCase : Assertion
	{
		List _list;

		[SetUp]
		public void SetUp()
		{
			_list = new List().Add("um").Add("dois").Add("tres");
		}

		[Test]
		public void TestCount()
		{
			AssertEquals(3, _list.Count);
		}
		
		[Test]
		public void Remove()
		{
			_list.Remove("dois");
			AssertItems("um", "tres");
			_list.Remove("um");
			AssertItems("tres");
			_list.Remove("tres");
			AssertItems();
		}
		
		[Test]
		public void RemoveAt()
		{
			_list.RemoveAt(2);
			AssertItems("um", "dois");
			_list.RemoveAt(0);
			AssertItems("dois");
			_list.RemoveAt(-1);
			AssertItems();
		}
		
		[Test]
		public void Insert()
		{
			_list.Insert(-1, "foo");
			AssertItems("um", "dois", "foo", "tres");
			_list.Insert(0, "bar");
			AssertItems("bar", "um", "dois", "foo", "tres");
			_list.Insert(1, "baz");
			AssertItems("bar", "baz", "um", "dois", "foo", "tres");
		}

		[Test]
		public void TestAddUnique()
		{
			_list.AddUnique("dois");
			AssertItems("um", "dois", "tres");
		}

		[Test]
		public void TestToString()
		{
			AssertEquals("um, dois, tres", _list.ToString());
		}

		void AssertItems(params object[] items)
		{			
			AssertEquals("Count", items.Length, _list.Count);
			for (int i=0; i<items.Length; ++i)
			{
				AssertEquals("[" + i + "]", items[i], _list[i]);
			}
		}
	}
}
