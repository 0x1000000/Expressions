namespace Expression.Arithmetic
{
    public class ExprPrinter : IExprVisitor<string>
    {
        public static readonly ExprPrinter Instance = new ExprPrinter();

        private ExprPrinter() { }

        public string VisitExprNum(ExprNum expr) 
            => expr.Val.ToString();

        public string VisitExprPlus(ExprPlus expr) 
            => $"{expr.Left.Accept(this)}+{expr.Right.Accept(this)}";

        public string VisitExprMinus(ExprMinus expr) 
            => $"{expr.Left.Accept(this)}-{this.CheckPlusMinusParenthesizes(expr.Right)}";

        public string VisitExprMul(ExprMul expr) 
            => $"{this.CheckPlusMinusParenthesizes(expr.Left)}*{this.CheckPlusMinusParenthesizes(expr.Right)}";

        public string VisitExprDiv(ExprDiv expr) 
            => $"{this.CheckPlusMinusParenthesizes(expr.Left)}/{this.CheckPlusMinusParenthesizes(expr.Right)}";

        public string VisitExprSub(ExprSubExpr expr) 
            => $"({expr.SubExpression.Accept(this)})";

        private string CheckPlusMinusParenthesizes(Expr exp) 
            => exp is ExprPlus || exp is ExprMinus 
                ? $"({exp.Accept(this)})" 
                : exp.Accept(this);
    }
}