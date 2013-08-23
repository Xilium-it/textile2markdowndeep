textile2markdowndeep
====================

Small procedure to convert textile formatted string to markdown formatted string.

This procedure borns to convert `Textile` formatted string to [Xilium-it markdowndeep](../../../markdowndeep) version.  
This MarkdownDeep version implements some feature for table that original markdowndeep version don't provide.

## How to use

```cs

string textile = "h1. Title";
textile += "\n";
textile += "\nLorem ipsum dolor sit amet, consectetur adipiscing elit.";
textile += "\nMaecenas at adipiscing lectus. Mauris porttitor.";

string markdonw = Xilium.Textile2MarkdownDeep.Convert(textile);

```

## The MIT License (MIT)

Copyright (c) 2010 Vertino Ltd

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
