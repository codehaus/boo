using Boo.Lang.Compiler.TypeSystem;
using NUnit.Framework;
using Boo.Lang.Compiler;

namespace BooCompiler.Tests.TypeSystem
{
	[TestFixture]
	public class NameResolutionServiceTestFixture
	{
		[Test]
		public void NameMatchingCanBeCustomized()
		{
			CompilerParameters parameters = new CompilerParameters();
			string code = @"
l = []
l.ADD(42)
l.add(42)
print JOIN(l, "", "")
";
			parameters.Input.Add(new Boo.Lang.Compiler.IO.StringInput("code", code));
			parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.ResolveExpressions();
			parameters.Pipeline.Insert(0, new ActionStep(delegate(CompilerContext context)
			                                             	{
			                                             		context.NameResolutionService.EntityNameMatcher = MatchIgnoringCase;
			                                             	}));
			CompilerContext result = new Boo.Lang.Compiler.BooCompiler(parameters).Run();
			Assert.AreEqual(0, result.Errors.Count, result.Errors.ToString());
		}

		private bool MatchIgnoringCase(IEntity candidate, string name)
		{
			return 0 == string.Compare(candidate.Name, name, true);
		}
	}
}