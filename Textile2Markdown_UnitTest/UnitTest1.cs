using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Textile2Markdown_UnitTest {
	[TestClass]
	public class UnitTest1 {

		[TestMethod]
		public void TitleTest1() {

			Assert.AreEqual("# My H1 title", Xilium.Textile2MarkdownDeep.Convert("h1. My H1 title"));

			Assert.AreEqual("## My H1 title", Xilium.Textile2MarkdownDeep.Convert("h2. My H1 title"));

			Assert.AreEqual("### My H1 title", Xilium.Textile2MarkdownDeep.Convert("h3. My H1 title"));

			Assert.AreEqual("#### My H1 title", Xilium.Textile2MarkdownDeep.Convert("h4. My H1 title"));

			Assert.AreEqual("##### My H1 title", Xilium.Textile2MarkdownDeep.Convert("h5. My H1 title"));

			Assert.AreEqual("###### My H1 title", Xilium.Textile2MarkdownDeep.Convert("h6. My H1 title"));
		}

		[TestMethod]
		public void BR() {

			Assert.AreEqual("First phrase.  \nSecond phrase?  \nThird phrase!  \nLorem ipsum dolor sit amet, consectetur adipiscing elit.", Xilium.Textile2MarkdownDeep.Convert("First phrase.\nSecond phrase?\nThird phrase!\nLorem ipsum dolor sit amet, consectetur adipiscing elit."));
		}

		[TestMethod]
		public void BoldAndEM() {

			Assert.AreEqual("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", Xilium.Textile2MarkdownDeep.Convert("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, **consectetur** adipiscing elit.", Xilium.Textile2MarkdownDeep.Convert("Lorem ipsum dolor sit amet, *consectetur* adipiscing elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, *consectetur* adipiscing elit.", Xilium.Textile2MarkdownDeep.Convert("Lorem ipsum dolor sit amet, _consectetur_ adipiscing elit."));

			Assert.AreEqual("Lorem ipsum dolor sit amet, **consectetur** *adipiscing elit*.", Xilium.Textile2MarkdownDeep.Convert("Lorem ipsum dolor sit amet, **consectetur** _adipiscing elit_."));
		}

		[TestMethod]
		public void Lists() {

			Assert.AreEqual("My list:\n\n* First row;\n* Second row;\n* Last row.\n\nAfter list", Xilium.Textile2MarkdownDeep.Convert("My list:\n* First row;\n* Second row;\n* Last row.\n\nAfter list"));

			Assert.AreEqual("My list:\n\n0. First row;\n0. Second row;\n0. Last row.\n\nAfter list", Xilium.Textile2MarkdownDeep.Convert("My list:\n# First row;\n# Second row;\n# Last row.\n\nAfter list"));

		}

		[TestMethod]
		public void Table1_headerOnly() {
			var strInput = new StringBuilder();
			strInput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			
			var strOutput = new StringBuilder();
			strOutput.AppendLine("| Head 1 | Head 2 | Head 3 |");
			strOutput.AppendLine("|---|---|---|");

			Assert.AreEqual(strOutput.ToString(), Xilium.Textile2MarkdownDeep.Convert(strInput.ToString()));

		}

	}
}
