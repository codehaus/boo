using System;
using Boo.Ast;

namespace Boo.Lang
{
	/// <summary>
	/// Assegura que uma refer�ncia nula n�o seja passada como
	/// par�metro para um m�todo.
	/// </summary>
	/// <example>
	/// <pre>
	/// def constructor([required] name as string):
	///		_name = name
	/// </pre>
	/// </example>
	//[AstAttributeTarget(typeof(ParameterDeclaration))]
	public class RequiredAttribute : AstAttribute
	{
		public RequiredAttribute()
		{
		}

		public override void Apply(Boo.Ast.Node node)
		{
			ParameterDeclaration pd = node as ParameterDeclaration;
			if (null == pd)
			{
				throw new ApplicationException(ResourceManager.Format("InvalidNodeForAttribute", "ParameterDeclaration"));
			}

			// raise ArgumentNullException("<pd.Name>") unless <pd.Name>
			MethodInvocationExpression x = new MethodInvocationExpression();
			x.Target = new ReferenceExpression("ArgumentNullException");
			x.Arguments.Add(new StringLiteralExpression(pd.Name));
			RaiseStatement rs = new RaiseStatement(x);

			rs.Modifier = new StatementModifier(
				StatementModifierType.Unless,
				new ReferenceExpression(pd.Name)
				);

			// associa mensagens de erro com a posi��o
			// do par�metro no c�digo fonte
			rs.LexicalInfo = LexicalInfo;

			Method method = (Method)pd.ParentNode;
			method.Body.Statements.Insert(0, rs);
		}
	}
}
