${header}
namespace Boo.Lang.Compiler.Ast
{
	using System;
	
	[Serializable]
	public class ${node.Name} : Boo.Lang.Compiler.Ast.Impl.${node.Name}Impl
	{
		public ${node.Name}()
		{
		}
		
		public ${node.Name}(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
		}
	}
}

