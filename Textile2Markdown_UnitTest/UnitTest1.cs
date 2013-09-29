using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Textile2Markdown_UnitTest {
	[TestClass]
	public class UnitTest1 {

		private const string genericPhrase = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
		private const string rowSep = "\r\n";

		[TestMethod]
		public void TitleTest1() {

			Assert.AreEqual("# My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h1. My H1 title"));

			Assert.AreEqual("## My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h2. My H1 title"));

			Assert.AreEqual("### My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h3. My H1 title"));

			Assert.AreEqual("#### My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h4. My H1 title"));

			Assert.AreEqual("##### My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h5. My H1 title"));

			Assert.AreEqual("###### My H1 title", Xilium.Textile2MarkdownDeep.ConvertAll("h6. My H1 title"));
		}

		[TestMethod]
		public void BR() {

			Assert.AreEqual("First phrase.  \nSecond phrase?  \nThird phrase!  \nLorem ipsum dolor sit amet, consectetur adipiscing elit.", Xilium.Textile2MarkdownDeep.ConvertAll("First phrase.\nSecond phrase?\nThird phrase!\nLorem ipsum dolor sit amet, consectetur adipiscing elit."));
		}

		[TestMethod]
		public void BoldAndEMAndCITE() {

			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, **consectetur** adipiscing elit, <cite>bla bla bla</cite>.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, *consectetur* adipiscing elit, ??bla bla bla??."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, *consectetur* adipiscing elit, <cite>bla bla bla</cite>.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, _consectetur_ adipiscing elit, ??bla bla bla??."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, **consectetur** *adipiscing elit*, <cite>bla bla bla</cite>.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, **consectetur** _adipiscing elit_, ??bla bla bla??."));
		}

		[TestMethod]
		public void Link() {

			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, [consectetur adipiscing](http://mysite.net) elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, \"consectetur adipiscing\":http://mysite.net elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, [**consectetur adipiscing**](http://mysite.net) elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, \"*consectetur adipiscing*\":http://mysite.net elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, [consectetur **adipiscing**](http://mysite.net) elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, \"consectetur *adipiscing*\":http://mysite.net elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, [**consectetur** adipiscing](http://mysite.net) elit.", Xilium.Textile2MarkdownDeep.ConvertAll("Lorem ipsum dolor sit amet, \"*consectetur* adipiscing\":http://mysite.net elit."));

		}

		[TestMethod]
		public void Lists() {

			Assert.AreEqual("My list:\n\n* First row;\n* Second row;\n* Last row.\n\nAfter list", Xilium.Textile2MarkdownDeep.ConvertAll("My list:\n* First row;\n* Second row;\n* Last row.\n\nAfter list"));

			Assert.AreEqual("My list:\n\n0. First row;\n0. Second row;\n0. Last row.\n\nAfter list", Xilium.Textile2MarkdownDeep.ConvertAll("My list:\n# First row;\n# Second row;\n# Last row.\n\nAfter list"));

		}

		[TestMethod]
		public void Table1_rows1() {
			var strInput = new StringBuilder();
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("|---|---|---|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}


		[TestMethod]
		public void Table1_rows5() {
			var strInput = new StringBuilder();
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("|---|---|---|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		[TestMethod]
		public void Table2_heads1() {
			var strInput = new StringBuilder();
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("|---|---|---|");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		public void Table2_heads3() {
			var strInput = new StringBuilder();
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("|---|---|---|");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		[TestMethod]
		public void Table3_heads1rows1() {
			var strInput = new StringBuilder();
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("|---|---|---|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		[TestMethod]
		public void Table3_heads1rows5() {
			var strInput = new StringBuilder();
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_. Head 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("|---|---|---|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		[TestMethod]
		public void Table4_advance() {
			var strInput = new StringBuilder();
			strInput.AppendLine("|_\\3. Head colspan1 |");
			strInput.AppendLine("|_. Head 1 |_. Head 2 |_>. Head right |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("|\\2. Row 1+2 | Row 3 |");
			strInput.AppendLine("|<. Row left |=. Row center |>. Row right |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strInput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head colspan1 |||");
			strOutput.AppendLine("| Head 1 | Head 2 | Head right :|");
			strOutput.AppendLine("|---|---|---|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1+2 || Row 3 |");
			strOutput.AppendLine("|: Row left |: Row center :| Row right :|");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");
			strOutput.AppendLine("| Row 1 | Row 2 | Row 3 |");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with table");

			strInput.Insert(0, genericPhrase + rowSep);
			strOutput.Insert(0, genericPhrase + rowSep);
			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.ConvertAll(strInput.ToString()), "Text starts with generic phrase");

		}

		/*
		[TestMethod]
		public void GetRowSep() {
			var strInput = new StringBuilder();
			strInput.AppendLine("FirstRow");
			strInput.AppendLine("SecondRow");

			Assert.AreEqual(Regex.Escape(Xilium.Textile2MarkdownDeep.getRowSep(strInput.ToString())), Regex.Escape(rowSep));
		}
		*/
	}
}
