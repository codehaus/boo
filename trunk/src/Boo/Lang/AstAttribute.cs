using System;

namespace Boo.Lang
{
	/// <summary>
	/// Um atributo que pode ser aplicado a um n� da AST de forma
	/// a transform�-lo.
	/// </summary>
	public abstract class AstAttribute
	{
		protected Boo.Ast.Attribute _attribute;

		public Boo.Ast.Attribute Attribute
		{
			get
			{
				return _attribute;
			}

			set
			{
				if (null == value)
				{
					throw new ArgumentNullException("value");
				}
				_attribute = value;
			}
		}

		public Boo.Ast.LexicalInfo LexicalInfo
		{
			get
			{
				return _attribute.LexicalInfo;
			}
		}

		public abstract void Apply(Boo.Ast.Node node);
	}
}
