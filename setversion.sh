#!/bin/bash
sed -i '' "s/<Version>[0-9.]*<\/Version>/<Version>$1<\/Version>/g"\
    Sawmill/Sawmill.csproj\
    Sawmill.Newtonsoft.Json/Sawmill.Newtonsoft.Json.csproj\
    Sawmill.Microsoft.CodeAnalysis/Sawmill.Microsoft.CodeAnalysis.csproj\
    Sawmill.Microsoft.CodeAnalysis.CSharp/Sawmill.Microsoft.CodeAnalysis.CSharp.csproj\
    Sawmill.Microsoft.CodeAnalysis.VisualBasic/Sawmill.Microsoft.CodeAnalysis.VisualBasic.csproj