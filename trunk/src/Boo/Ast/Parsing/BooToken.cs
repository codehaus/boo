using System;

namespace Boo.Ast.Parsing
{
	/// <summary>
	/// O token armazena tamb�m o nome do arquivo que
	/// o gerou para diagn�stico.
	/// </summary>
	public class BooToken : antlr.CommonToken
	{
		protected string _fname;

		public BooToken()
		{
		}

		public BooToken(antlr.Token original, int type, string text)
		{
			setType(type);
			setText(text);
			setFilename(original.getFilename());
			setLine(original.getLine());
			setColumn(original.getColumn());
		}

		public override void setFilename(string name)
		{
			_fname = name;
		}

		public override string getFilename()
		{
			return _fname;
		}

	}
}
