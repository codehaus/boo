using System;

namespace Boo.Ast
{
	/// <summary>
	/// Uma AST para n�s que armazenam par�metros como atributos e
	/// invoca��es de m�todo.
	/// </summary>
	public interface INodeWithArguments
	{
		ExpressionCollection Arguments
		{
			get;
		}

		ExpressionPairCollection NamedArguments
		{
			get;
		}
	}
}
