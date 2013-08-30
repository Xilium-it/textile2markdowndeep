textile2markdowndeep
====================

Small procedure to convert **textile** formatted string to **markdown** formatted string.

This procedure borns to convert `Textile` formatted string to [Xilium MarkdownDeep](../../../MarkdownDeep) version.  
This MarkdownDeep version implements some feature for table that original markdowndeep version don't provide.

## How to use

This is an example code to use the method:

```cs

string textile = "h1. Title";
textile += "\n";
textile += "\nLorem ipsum dolor sit amet, *consectetur* adipiscing elit.";
textile += "\nMaecenas _at adipiscing_ lectus. Mauris porttitor.";
textile += "\n";
textile += "\n|_\3. My table with colspan 3 |";
textile += "\n| 8.00 | breakfast | lorem... |";
textile += "\n| 12.00 | lounch | lorem... |";
textile += "\n| 16.00 | TV | lorem... |";
textile += "\n| 19.00 | dinner | lorem... |";

string markdonw = Xilium.Textile2MarkdownDeep.Convert(textile);

```

### Output

```
# Title

Lorem ipsum dolor sit amet, **consectetur** adipiscing elit.
Maecenas *at adipiscing* lectus. Mauris porttitor.

| My table with colspan 3 |||
|---|---|---|
| 8.00 | breakfast | lorem... |
| 12.00 | lounch | lorem... |
| 16.00 | TV | lorem... |
| 19.00 | dinner | lorem... |

```


## The MIT License (MIT)

Copyright (c) 2013 Xilium di Flavio Spezi - Italy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
