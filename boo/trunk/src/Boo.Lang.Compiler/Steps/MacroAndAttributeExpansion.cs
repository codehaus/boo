#region license
// Copyright (c) 2004-2009, Rodrigo B. de Oliveira (rbo@acm.org)
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
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps.MacroProcessing;

namespace Boo.Lang.Compiler.Steps
{
	public class MacroAndAttributeExpansion : AbstractCompilerStep
	{
		private BindAndApplyAttributes _attributes = new BindAndApplyAttributes();
		private MacroExpander _macroExpander = new MacroExpander();

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_attributes.Initialize(context);
			_macroExpander.Initialize(context);
		}

		public override void Run()
		{
			ExpandExternalMacros();
			ExpandInternalMacros();
		}

		private void ExpandInternalMacros()
		{
			_macroExpander.ExpandingInternalMacros = true;
			RunExpansionIterations();
			
		}

		private void ExpandExternalMacros()
		{
			_macroExpander.ExpandingInternalMacros = false;
			RunExpansionIterations();
		}

		private void RunExpansionIterations()
		{
			int iteration = 0;
			while (true)
			{
				bool expanded = ApplyAttributesAndExpandMacros();
				if (!expanded)
					break;
				
				BubbleResultingTypeMemberStatementsUp();

				++iteration;
				if (iteration > Parameters.MaxExpansionIterations)
					throw new CompilerError("Too many expansions.");
			}
		}

		private void BubbleResultingTypeMemberStatementsUp()
		{
			CompileUnit.Accept(new TypeMemberStatementBubbler());
		}

		class TypeMemberStatementBubbler : DepthFirstTransformer, ITypeMemberStatementVisitor
		{
			private TypeDefinition _current = null;

			protected override void OnNode(Node node)
			{
				TypeDefinition typeDefinition = node as TypeDefinition;
				if (null == typeDefinition)
				{
					base.OnNode(node);
					return;
				}

				TypeDefinition previous = _current;
				try
				{
					_current = typeDefinition;
					base.OnNode(node);
				}
				finally
				{
					_current = previous;
				}
			}

			#region Implementation of ITypeMemberStatementVisitor

			public void OnTypeMemberStatement(TypeMemberStatement node)
			{
				_current.Members.Add(node.TypeMember);
				Visit(node.TypeMember);
				RemoveCurrentNode();
			}

			#endregion
		}

		private bool ApplyAttributesAndExpandMacros()
		{
			bool attributesApplied = _attributes.BindAndApply();
			bool macrosExpanded = _macroExpander.ExpandAll();
			return attributesApplied || macrosExpanded;
		}
	}
}
