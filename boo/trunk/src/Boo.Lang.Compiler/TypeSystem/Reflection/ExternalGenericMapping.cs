#region license
// Copyright (c) 2003, 2004, 2005 Rodrigo B. de Oliveira (rbo@acm.org)
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

using Boo.Lang.Compiler.TypeSystem.Generics;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.TypeSystem
{
	using System.Reflection;

	public class ExternalGenericMapping : GenericMapping
	{
		ExternalType _constructedType;

		public ExternalGenericMapping(ExternalType constructedType, IType[] arguments) : base(constructedType, arguments)
		{
			_constructedType = constructedType;
		}

		protected override IMember CreateMappedMember(IMember source)
		{
			ExternalType targetType = _constructedType;
			return FindByMetadataToken(source, _constructedType);
		}

		public override IMember UnMap(IMember mapped)
		{
			IMember unmapped = base.UnMap(mapped);
			if (unmapped != null) return unmapped;
			return FindByMetadataToken(mapped, _constructedType.ConstructedInfo.GenericDefinition as ExternalType);	
		}

		private IMember FindByMetadataToken(IMember source, ExternalType targetType)
		{
			// HACK: since the API doesn't provide a way to correlate members on a generic type 
			// with the mapped members on a constructed type, we have to rely on them sharing the same
			// metadata token.

			MemberInfo sourceMemberInfo = ((IExternalEntity)source).MemberInfo;
			MemberFilter filter = delegate(MemberInfo candidate, object metadataToken)
			{
				return candidate.MetadataToken.Equals(metadataToken);
			};

			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			bindingFlags |= (source.IsStatic ? BindingFlags.Static : BindingFlags.Instance);

			MemberInfo[] mappedMemberInfos = targetType.ActualType.FindMembers(
				sourceMemberInfo.MemberType, bindingFlags, filter, sourceMemberInfo.MetadataToken);

			return (IMember)TypeSystemServices.Map(mappedMemberInfos[0]);
		}
	}
}

