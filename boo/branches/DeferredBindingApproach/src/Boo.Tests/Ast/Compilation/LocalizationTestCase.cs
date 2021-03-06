#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// mailto:rbo@acm.org
#endregion

using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using Boo.Lang.Compiler;

namespace Boo.Tests.Ast.Compiler
{
	/// <summary>	
	/// </summary>
	[TestFixture]
	public class LocalizationTestCase
	{
		const string TestCase = "foo def";
		
		[Test]
		public void TestNeutralCulture()
		{
			AssertCultureDependentMessage("Unexpected token: foo.", CultureInfo.InvariantCulture);
		}

		[Test]
		public void TestEnUsCulture()
		{
			AssertCultureDependentMessage("Unexpected token: foo.", CultureInfo.CreateSpecificCulture("en-US"));
		}

		[Test]
		public void TestPtBrCulture()
		{
			AssertCultureDependentMessage("Token inesperado: foo.", CultureInfo.CreateSpecificCulture("pt-BR"));
		}

		void AssertCultureDependentMessage(string message, CultureInfo culture)
		{
			CultureInfo savedCulture = Thread.CurrentThread.CurrentUICulture;			
			Thread.CurrentThread.CurrentUICulture = culture;

			try
			{
				BooCompiler compiler = new BooCompiler();
				CompilerParameters options = compiler.Parameters;
				options.Input.Add(new Boo.Lang.Compiler.IO.StringInput("testcase", TestCase));
				options.Pipeline.Add(new Boo.Antlr.BooParsingStep());
				
				ErrorCollection errors = compiler.Run().Errors;
	
				Assertion.AssertEquals(1, errors.Count);
				Assertion.AssertEquals(message, errors[0].Message);
			}
			finally
			{
				Thread.CurrentThread.CurrentUICulture = savedCulture;
			}
		}
	}
}
