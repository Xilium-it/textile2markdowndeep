using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xilium
{
    public class Textile2MarkdownDeep
    {
		private Textile2MarkdownDeep() {
			
		}

		/// <summary>
		/// Find chars used in row separator
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private static string getRowSep(string text) {
			var match = Regex.Match(text, @"[\r\n]+");
			if (match.Success == false) return "\n";	// not found. Returns default value.
			string fReturn = "";
			for (int i = 0; i < match.Value.Length; i++) {
				char c = match.Value[i];
				if (fReturn.Contains(c)) break;
				fReturn += c;
			}
			return fReturn;
		}

		private static string repeat(string text, int times) {
			var fReturn = new System.Text.StringBuilder();
			for (int i = 0; i < times; i++) {
				fReturn.Append(text);
			}
			return fReturn.ToString();
		}

		private static int charsCount(string text, char find) {
			return text.Where(c => c == find).Count();
		}

		public static string ConvertUlLi(string textileFormatString, string rowSep, string ptnRowSep) {
			// UL LI: "* <text>" > "* <text>". NOTE: Markdown need white row before list.
			return Regex.Replace(textileFormatString, @"^(?!\*\s)(?<row1>.+)" + ptnRowSep + @"(?<row2>\*\s)", m => {
				return m.Groups["row1"].Value + repeat(rowSep, 2) + "* ";
			}, RegexOptions.Multiline);
		}

		public static string ConvertOlLi(string textileFormatString, string rowSep, string ptnRowSep) {
			// OL LI: "# <testo>" > "0. <testo>". NOTE: Markdown need white row before list
			var fReturn = Regex.Replace(textileFormatString, @"^(?!#\s)(?<row1>.+)" + ptnRowSep + @"(?<row2>#\s)", m => {
				return m.Groups["row1"].Value + repeat(rowSep, 2) + "0. ";
			}, RegexOptions.Multiline);
			fReturn = Regex.Replace(fReturn, @"^#\s", m => {
				return "0. ";
			}, RegexOptions.Multiline);
			return fReturn;
		}

		public static string ConvertHx(string textileFormatString, string rowSep, string ptnRowSep) {
			// Hx: "h<n>." > "#{n}"
			return Regex.Replace(textileFormatString, @"^h([1-6])\.[ ]*", m => {
				return (new string('#', int.Parse(m.Groups[1].Value))) + " ";
			}, RegexOptions.Multiline);
		}

		public static string ConvertBr(string textileFormatString, string rowSep, string ptnRowSep) {
			// BR: "...<endPhrase>\n" > "...<endPhrase>  \n"
			return Regex.Replace(textileFormatString, @"([\.?!:])(" + ptnRowSep + @")(?![\n\r|])", m => {
				return m.Groups[1].Value + "  " + rowSep;
			}, RegexOptions.Multiline);
		}

		public static string ConvertStrong(string textileFormatString, string rowSep, string ptnRowSep) {
			// STRONG: "*<text>*", "**<text>**" > "**<text>**"
			return Regex.Replace(textileFormatString, @"(?<=([\n\r\s""!]|^))(?<begin>\*{1,2})(?<val>[^\s*]|[^\s*][^*\n\r\t\v]*[^\s*])\k<begin>", m => {
				return "**" + m.Groups["val"].Value + "**";
			});
		}

		public static string ConvertEn(string textileFormatString, string rowSep, string ptnRowSep) {
			// EN: "_<text>_", "__<text>__" > "*<text>*"
			return Regex.Replace(textileFormatString, @"(?<=([\n\r\s""!]|^))(?<begin>_{1,2})(?<val>[^\s_]|[^\s_][^_\n\r\t\v]*[^\s_])\k<begin>", m => {
				return "*" + m.Groups["val"].Value + "*";
			});
		}

		public static string ConvertCite(string textileFormatString, string rowSep, string ptnRowSep) {
			// CITE: "??<text>??" > "<<cite>><text><</cite>>"
			return Regex.Replace(textileFormatString, @"(?<=([\n\r\s""!]|^))(?<begin>\?{2})(?<val>[^\s?]|[^\s?][^?\n\r\t\v]*[^\s?])\k<begin>", m => {
				return "<cite>" + m.Groups["val"].Value + "</cite>";
			});
		}

		public static string ConvertU(string textileFormatString, string rowSep, string ptnRowSep) {
			// U: "+<text>+" > "*<text>*"
			return Regex.Replace(textileFormatString, @"(?<=([\n\r\s""!]|^))(?<begin>\+{1,2})(?<val>[^\s+]|[^\s+][^+\n\r\t\v]*[^\s+])\k<begin>", m => {
				return "*" + m.Groups["val"].Value + "*";
			});
		}

		public static string ConvertA(string textileFormatString, string rowSep, string ptnRowSep) {
			// A: "\"<text>\":<url>" > "[<text>](<url>)"
			return Regex.Replace(textileFormatString, @"(?<=([\n\r\s]|^))(?<begin>""{1})(?<text>[^\s""]|[^\s""][^""\n\r\t\v]*[^\s""])\k<begin>:(?<url>([a-z]{3,6}://)?[a-z0-9?&=#%$/\-_\.+!*'()]{5,}[a-z0-9#%$\-_+!*'()])", m => {
				return "[" + m.Groups["text"].Value + "](" + m.Groups["url"].Value + ")";
			}, RegexOptions.IgnoreCase);
		}

		public static string ConvertTable(string textileFormatString, string rowSep, string ptnRowSep) {
			var fReturn = textileFormatString;

			// TABLE, step 1: adjust cells (align, colspan, style) "|_. " > "|# ", "|<. " > "|: ", ...
			fReturn = Regex.Replace(fReturn, @"^\|[^\n\r]{3,}|$", m1 => {
				return Regex.Replace(m1.Value, @"\G\|(((?<head>_)|(?<colspan>\\(?<csnum>[2-9]))|(?<align>[=<>]))+\.)? (?<val>[^|]*)", m2 => {
					return "|" +
						(m2.Groups["align"].Success && m2.Groups["align"].Value != ">" ? ":" : "") +
						(m2.Groups["head"].Success ? "#" : "") +
						" " + m2.Groups["val"].Value +
						(m2.Groups["align"].Success && m2.Groups["align"].Value != "<" ? ":" : "") +
						(m2.Groups["colspan"].Success ? new string('|', int.Parse(m2.Groups["csnum"].Value) - 1) : "")
						;
				});
			}, RegexOptions.Multiline);

			// TABLE, step 2, table without header: add header separator row ("|--|--... ...|") before table
			Func<Match, string> fncTable_step2_replace = (Match m) => {
				int numCols = charsCount(m.Groups["bodyRow"].Value, '|') - 1;
				var fRet = new System.Text.StringBuilder();
				if (m.Groups["preTabRow"].Success) {
					fRet.Append(m.Groups["preTabRow"].Value);
					fRet.Append(rowSep);
				}
				fRet.Append("|");
				fRet.Append(repeat("---|", numCols));
				fRet.Append(rowSep);
				fRet.Append(m.Groups["bodyRow"].Value);
				return fRet.ToString();
			};
			// Text starts with header table
			fReturn = Regex.Replace(fReturn, @"^(?<bodyRow>\| [^\n\r]+)", m => fncTable_step2_replace(m), RegexOptions.Singleline);
			// Header table is inside the text
			fReturn = Regex.Replace(fReturn, @"^(?!\|)(?<preTabRow>[^\n\r]*)" + ptnRowSep + @"(?<bodyRow>\| [^\n\r]+)", m => fncTable_step2_replace(m), RegexOptions.Multiline);

			// TABLE, step 3, table with header: add header separator row ("|--|--... ...|") after table header
			Func<Match, string> fncTable_step3_replace = (Match m) => {
				int numCols = charsCount(m.Groups["subHeadRow"].Value, '|') - 1;
				var fRet = new System.Text.StringBuilder();
				fRet.Append(m.Value.Replace("|# ", "| ").Replace("|:# ", "|: "));
				fRet.Append(rowSep);
				fRet.Append("|");
				fRet.Append(repeat("---|", numCols));
				return fRet.ToString();
			};
			// Text starts with header table
			fReturn = Regex.Replace(fReturn, @"^(?<headRow>(" + ptnRowSep + @")?(?<subHeadRow>\|:?# [^\n\r]+))+", m => fncTable_step3_replace(m), RegexOptions.Singleline);
			// Header table is inside the text
			fReturn = Regex.Replace(fReturn, @"^(?!\|)(?<preTabRow>[^\n\r]*)(?<headRow>" + ptnRowSep + @"(?<subHeadRow>\|:?# [^\n\r]+))+", m => fncTable_step3_replace(m), RegexOptions.Multiline);

			return fReturn;
		}

		public static string ConvertAll(string textileFormatString) {
			var fReturn = textileFormatString;

			// Find row separator ("\n" or "\r" or "\r\n" ?)
			var rowSep = getRowSep(textileFormatString);
			var ptnRowSep = Regex.Escape(rowSep);

			fReturn = ConvertUlLi(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertOlLi(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertHx(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertBr(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertInline(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertTable(fReturn, rowSep, ptnRowSep);

			return fReturn;
		}

	    public static string ConvertInline(string textileFormatString) {
			
			// Find row separator ("\n" or "\r" or "\r\n" ?)
			var rowSep = getRowSep(textileFormatString);
			var ptnRowSep = Regex.Escape(rowSep);

			return ConvertInline(textileFormatString, rowSep, ptnRowSep);
	    }

		public static string ConvertInline(string textileFormatString, string rowSep, string ptnRowSep) {
			var fReturn = textileFormatString;

			fReturn = ConvertStrong(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertEn(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertCite(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertU(fReturn, rowSep, ptnRowSep);

			fReturn = ConvertA(fReturn, rowSep, ptnRowSep);

			return fReturn;
		}

    }
}
