# Code for article "Abstract Syntax Tree TODO"

Abstract Syntax Tree Models for basic arithmetic expressions. 

They allow printing, evaluation, modification:

```cs
Expr expr = Parser.Parse("1+(5-2)*2");

Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

expr = 2*expr - 3;

Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

var exprSimple = expr.OpenParentheses();

Console.WriteLine($"{expr.Print()} = {exprSimple.Print()} = {exprSimple.Evaluate()}");

```

Result:
```
1+(5-2)*2 = 7

2*(1+(5-2)*2)-3 = 11

2*(1+(5-2)*2)-3 = 2*1+2*5*2-2*2*2-3 = 11
```
