textile2markdowndeep
====================

Small procedure to convert textile formatted string to markdown formatted string.

This procedure borns to convert `Textile` formatted string to [Xilium-it markdowndeep](../../../markdowndeep) version.  
This MarkdownDeep version implements some feature for table that original markdowndeep version don't provide.

## How to use

```cs

string textile = "h1. Title";
textile += "\nLorem ipsum dolor sit amet, consectetur adipiscing elit.";
textile += "\nMaecenas at adipiscing lectus. Mauris porttitor.";

string markdonw = Xilium.Textile2MarkdownDeep.Convert(textile);

```
