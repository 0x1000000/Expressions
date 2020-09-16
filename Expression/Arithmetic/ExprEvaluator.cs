namespace Expression.Arithmetic
{
    public class ExprEvaluator : IExprVisitor<int>
    {
        public static readonly ExprEvaluator Instance = new ExprEvaluator();

        private ExprEvaluator(){}

        public int VisitExprNum(ExprNum expr)
            => expr.Val;

        public int VisitExprPlus(ExprPlus expr) 
            => expr.Left.Accept(this) + expr.Right.Accept(this);

        public int VisitExprMinus(ExprMinus expr)
            => expr.Left.Accept(this) - expr.Right.Accept(this);

        public int VisitExprMul(ExprMul expr)
            => expr.Left.Accept(this) * expr.Right.Accept(this);

        public int VisitExprDiv(ExprDiv expr)
            => expr.Left.Accept(this) / expr.Right.Accept(this);

        public int VisitExprSub(ExprSubExpr expr)
            => expr.SubExpression.Accept(this);
    }
}