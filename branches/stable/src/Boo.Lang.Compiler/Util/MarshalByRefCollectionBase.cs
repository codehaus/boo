﻿#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Boo.Lang.Compiler.Util
{
	using System;
	using System.Collections;
	
	public class MarshalByRefCollectionBase : System.MarshalByRefObject, ICollection
	{
		protected ArrayList _items = new ArrayList();
		
		public bool IsSynchronized
		{
			get
			{
				return _items.IsSynchronized;
			}
		}
		
		public object SyncRoot
		{
			get
			{
				return _items.SyncRoot;
			}
		}
		
		public int Count
		{
			get
			{
				return _items.Count;
			}
		}
		
		protected ArrayList InnerList
		{
			get
			{
				return _items;
			}
		}
		
		public void Clear()
		{
			_items.Clear();
		}
		
		public void CopyTo(System.Array array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}
		
		public IEnumerator GetEnumerator()
		{
			return new MarshalByRefEnumerator(_items.GetEnumerator());
		}
	}
}
