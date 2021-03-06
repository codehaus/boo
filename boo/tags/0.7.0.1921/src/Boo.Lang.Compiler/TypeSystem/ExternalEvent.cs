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

namespace Boo.Lang.Compiler.TypeSystem
{
	public class ExternalEvent : IEvent
	{
		TypeSystemServices _typeSystemServices;
		
		System.Reflection.EventInfo _event;
		
		public ExternalEvent(TypeSystemServices tagManager, System.Reflection.EventInfo event_)
		{
			_typeSystemServices = tagManager;
			_event = event_;
		}
		
		public IType DeclaringType
		{
			get
			{
				return _typeSystemServices.Map(_event.DeclaringType);
			}
		}
		
		public IMethod GetAddMethod()
		{
			return (IMethod)_typeSystemServices.Map(_event.GetAddMethod());
		}
		
		public IMethod GetRemoveMethod()
		{
			return (IMethod)_typeSystemServices.Map(_event.GetRemoveMethod());
		}
		
		public IMethod GetRaiseMethod()
		{
			return (IMethod)_typeSystemServices.Map(_event.GetRaiseMethod());
		}
		
		public System.Reflection.EventInfo EventInfo
		{
			get
			{
				return _event;
			}
		}
		
		public bool IsPublic
		{
			get
			{
				return _event.GetAddMethod(true).IsPublic;
			}
		}
		
		public string Name
		{
			get
			{
				return _event.Name;
			}
		}
		
		public string FullName
		{
			get
			{
				return _event.DeclaringType.FullName + "." + _event.Name;
			}
		}
		
		public EntityType EntityType
		{
			get
			{
				return EntityType.Event;
			}
		}
		
		public IType Type
		{
			get
			{
				return _typeSystemServices.Map(_event.EventHandlerType);
			}
		}
		
		public bool IsStatic
		{
			get
			{
				return _event.GetAddMethod().IsStatic;
			}
		}

		public bool IsAbstract
		{
			get
			{
				return _event.GetAddMethod().IsAbstract;
			}
		}

		public bool IsVirtual
		{
			get
			{
				return _event.GetAddMethod().IsVirtual;
			}
		}
		
		override public string ToString()
		{
			return _event.ToString();
		}

		public bool IsDuckTyped
		{
			get { return false; }
		}
	}
}
