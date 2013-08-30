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

		public static string Convert(string textileFormatString) {
			var fReturn = textileFormatString;

			// Find row separator ("\n" or "\r" or "\r\n" ?)
			var rowSep = getRowSep(textileFormatString);
			var ptnRowSep = Regex.Escape(rowSep);

			// UL LI: "* <text>" > "* <text>". NOTE: Markdown need white row before list.
			fReturn = Regex.Replace(fReturn, @"^(?!\*\s)(?<row1>.+)" + ptnRowSep + @"(?<row2>\*\s)", m => {
				return m.Groups["row1"].Value +  repeat(rowSep, 2) + "* ";
			}, RegexOptions.Multiline);

			// OL LI: "# <testo>" > "0. <testo>". NOTE: Markdown need white row before list
			fReturn = Regex.Replace(fReturn, @"^(?!#\s)(?<row1>.+)" + ptnRowSep + @"(?<row2>#\s)", m => {
				return m.Groups["row1"].Value + repeat(rowSep, 2) + "0. ";
			}, RegexOptions.Multiline);
			fReturn = Regex.Replace(fReturn, @"^#\s", m => {
				return "0. ";
			}, RegexOptions.Multiline);

			// Hx: ".h<n>" > "#{n}"
			fReturn = Regex.Replace(fReturn, @"^h([1-6])\. ", m => {
				return (new string('#', int.Parse(m.Groups[1].Value))) + " ";
			}, RegexOptions.Multiline);

			// BR: "...<endPhrase>\n" > "...<endPhrase>  \n"
			fReturn = Regex.Replace(fReturn, @"([\.?!:])(" + ptnRowSep + @")(?![\n\r|])", m => {
				return m.Groups[1].Value + "  " + rowSep;
			}, RegexOptions.Multiline);

			// STRONG: "*<text>*", "**<text>**" > "**<text>**"
			fReturn = Regex.Replace(fReturn, @"(?<=([\n\r\s]|^))(?<begin>\*{1,2})(?<val>[^\s*]|[^\s*][^*\n\r\t\v]*[^\s*])\k<begin>", m => {
				return "**" + m.Groups["val"].Value + "**";
			});

			// EN: "_<text>_", "__<text>__" > "*<text>*"
			fReturn = Regex.Replace(fReturn, @"(?<=([\n\r\s]|^))(?<begin>_{1,2})(?<val>[^\s_]|[^\s_][^_\n\r\t\v]*[^\s_])\k<begin>", m => {
				return "*" + m.Groups["val"].Value + "*";
			});

			// U: "+<text>+" > "*<text>*"
			fReturn = Regex.Replace(fReturn, @"(?<=([\n\r\s]|^))(?<begin>\+{1,2})(?<val>[^\s+]|[^\s+][^+\n\r\t\v]*[^\s+])\k<begin>", m => {
				return "*" + m.Groups["val"].Value + "*";
			});

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
    }
}
