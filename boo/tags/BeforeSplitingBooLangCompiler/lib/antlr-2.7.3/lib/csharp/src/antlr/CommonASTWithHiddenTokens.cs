using System;

namespace antlr
{
	/*ANTLR Translator Generator
	* Project led by Terence Parr at http://www.jGuru.com
	* Software rights: http://www.antlr.org/RIGHTS.html
	*
	* $Id: $
	*/

	//
	// ANTLR C# Code Generator by Micheal Jordan
	//                            Kunle Odutola       : kunle UNDERSCORE odutola AT hotmail DOT com
	//                            Anthony Oguntimehin
	//
	// With many thanks to Eric V. Smith from the ANTLR list.
	//

	/*A CommonAST whose initialization copies hidden token
	*  information from the Token used to create a node.
	*/

	public class CommonASTWithHiddenTokens : CommonAST
	{
		protected internal CommonHiddenStreamToken hiddenBefore, hiddenAfter; // references to hidden tokens
		
		public CommonASTWithHiddenTokens() : base()
		{
		}
		
		public CommonASTWithHiddenTokens(Token tok) : base(tok)
		{
		}
		
		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		protected CommonASTWithHiddenTokens(CommonASTWithHiddenTokens another) : base(another)
		{
			hiddenBefore	= another.hiddenBefore;
			hiddenAfter		= another.hiddenAfter;
		}

		public virtual CommonHiddenStreamToken getHiddenAfter()
		{
			return hiddenAfter;
		}
		
		public virtual CommonHiddenStreamToken getHiddenBefore()
		{
			return hiddenBefore;
		}
		
		override public void  initialize(Token tok)
		{
			CommonHiddenStreamToken t = (CommonHiddenStreamToken) tok;
			base.initialize(t);
			hiddenBefore = t.getHiddenBefore();
			hiddenAfter = t.getHiddenAfter();
		}

		#region Implementation of ICloneable
		[Obsolete("Deprecated since version 2.7.2. Use ASTFactory.dup() instead.", false)]
		override public object Clone()
		{
			return new CommonASTWithHiddenTokens(this);
		}
		#endregion
	}
}