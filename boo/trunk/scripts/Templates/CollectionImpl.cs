${header}
namespace Boo.Lang.Compiler.Ast
{
	using System;
	using Boo.Lang.Compiler.Ast;
<%

itemType = "Boo.Lang.Compiler.Ast." + model.GetCollectionItemType(node)

%>	
	[Serializable]
	public partial class ${node.Name} : NodeCollection<${itemType}>
	{
		public ${itemType}Collection PopRange(int begin)
		{
			${itemType}Collection range = new ${itemType}Collection(_parent);
			range.InnerList.Extend(InternalPopRange(begin));
			return range;
		}
	}
}

