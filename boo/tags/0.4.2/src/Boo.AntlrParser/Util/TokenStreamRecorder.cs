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

using System;
using System.Collections;
using antlr;

namespace Boo.AntlrParser.Util
{
	/// <summary>
	/// Records a stream of tokens for later playback.
	/// </summary>
	public class TokenStreamRecorder : TokenStream
	{
		TokenStreamSelector _selector;
		Queue _queue = new Queue();
	
		public TokenStreamRecorder(TokenStreamSelector selector)
		{
			_selector = selector;
		}
	
		public int Count
		{
			get
			{
				return _queue.Count;
			}
		}
	
		public void Enqueue(Token token)
		{
			_queue.Enqueue(token);
		}
		
		public Token Dequeue()
		{
			 return (Token)_queue.Dequeue();
		}
	
		public int RecordUntil(TokenStream stream, int ttype)
		{
			int cTokens = 0;
		
			ods("> RecordUntil");
			Token token = stream.nextToken();
			while (ttype != token.Type)
			{			
				if (token.Type < Token.MIN_USER_TYPE)
				{
					break;
				}
			
				ods("  > {0}", token);
				Enqueue(token);			
			
				++cTokens;			
				token = stream.nextToken();			
			}
			ods("< RecordUntil");
			return cTokens;
		}
	
		public Token nextToken()
		{
			if (_queue.Count > 0)
			{
				return Dequeue();
			}
			return _selector.pop().nextToken();
		}
	
		void ods(string s, params object[] args)
		{
			//Console.WriteLine(s, args);
		}
	}
}
