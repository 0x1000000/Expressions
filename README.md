# Code for article "Abstract Syntax Tree TODO"

Abstract Syntax Tree Models for basic arithmetic expressions. 

They allow printing, evaluation, modification:

```
Expr expr = Parser.Parse("1+(5-2)*2");

Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

expr = 2*expr - 3;

Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

var exprSimple = expr.OpenParentheses();

Console.WriteLine($"{expr.Print()} = {exprSimple.Print()} = {exprSimple.Evaluate()}");

```
