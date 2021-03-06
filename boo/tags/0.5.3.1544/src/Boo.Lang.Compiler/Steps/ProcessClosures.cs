#region license
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

namespace Boo.Lang.Compiler.Steps
{
	using System.Collections;
	using Boo.Lang;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.TypeSystem;

	public class ProcessClosures : AbstractTransformerCompilerStep
	{
		override public void Run()
		{
			Visit(CompileUnit);
		}
		
		override public void LeaveCallableBlockExpression(CallableBlockExpression node)
		{			
			InternalMethod closureEntity = (InternalMethod)GetEntity(node);
						
			using (ForeignReferenceCollector collector = new ForeignReferenceCollector())
			{	
				collector.CurrentMethod = closureEntity.Method;
				collector.CurrentType = (IType)closureEntity.DeclaringType;
				collector.Initialize(_context);
				collector.Visit(closureEntity.Method.Body);
				
				if (collector.ContainsForeignLocalReferences)
				{	
					BooClassBuilder closureClass = CreateClosureClass(collector, closureEntity);
					collector.AdjustReferences();
					
					ReplaceCurrentNode(
						CodeBuilder.CreateMemberReference(
							collector.CreateConstructorInvocationWithReferencedEntities(
								closureClass.Entity),
							closureEntity));			
				}			
				else
				{
					Expression expression = CodeBuilder.CreateMemberReference(closureEntity);
					TypeSystemServices.GetConcreteExpressionType(expression);
					ReplaceCurrentNode(expression);
				}
			}
		}
		
		BooClassBuilder CreateClosureClass(ForeignReferenceCollector collector, InternalMethod closure)
		{
			Method method = closure.Method;
			TypeDefinition parent = method.DeclaringType;
			parent.Members.Remove(method);
			
			BooClassBuilder builder = collector.CreateSkeletonClass(closure.Name);
			
			parent.Members.Add(builder.ClassDefinition);
			builder.ClassDefinition.Members.Add(method);			
			method.Name = "Invoke";	
			
			if (method.IsStatic)
			{	
				// need to adjust paremeter indexes (parameter 0 is now self)
				foreach (ParameterDeclaration parameter in method.Parameters)
				{
					((InternalParameter)parameter.Entity).Index += 1;
				}
			}
			
			method.Modifiers = TypeMemberModifiers.Public;
			return builder;
		}
	}
}
