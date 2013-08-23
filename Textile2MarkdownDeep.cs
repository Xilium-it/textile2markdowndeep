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

		public static string Convert(string textileFormatString) {
			var fReturn = textileFormatString;

			// UL LI: "* <text>" > "* <text>". NOTE: Markdown need white row before list.
			fReturn = Regex.Replace(fReturn, @"^(?!\*\s)(?<row1>.+)[\r]?[\n](?<row2>\*\s)", m => {
				return m.Groups["row1"].Value + "\n\n* ";
			}, RegexOptions.Multiline);

			// OL LI: "# <testo>" > "0. <testo>". NOTE: Markdown need white row before list
			fReturn = Regex.Replace(fReturn, @"^(?!#\s)(?<row1>.+)[\r]?[\n](?<row2>#\s)", m => {
				return m.Groups["row1"].Value + "\n\n0. ";
			}, RegexOptions.Multiline);
			fReturn = Regex.Replace(fReturn, @"^#\s", m => {
				return "0. ";
			}, RegexOptions.Multiline);

			// Hx: ".h<n>" > "#{n}"
			fReturn = Regex.Replace(fReturn, @"^h([1-6])\. ", m => {
				return (new string('#', int.Parse(m.Groups[1].Value))) + " ";
			}, RegexOptions.Multiline);

			// BR: "\n" > "  \n"
			fReturn = Regex.Replace(fReturn, @"\.(\r?\n)(?![\n\r])", m => {
				return ".  " + m.Groups[1].Value;
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

			// TABLE, step 2: add header row
			fReturn = Regex.Replace(fReturn, @"^(?!\|)(?<preTabRow>[^\n\r]*)(?<headRow>\r?\n(?<subHeadRow>\|# [^\n\r]+))+", m => {
				int numCols = m.Groups["subHeadRow"].Value.Where(c => c == '|').Count() - 1;
				var fRet = new System.Text.StringBuilder();
				fRet.Append(m.Value.Replace("|# ", "| "));
				fRet.Append("\n|");
				for (var i = 0; i < numCols; i++) fRet.Append("---|");
				return fRet.ToString();
			}, RegexOptions.Multiline);

			return fReturn;
		}
    }
}
