namespace Expression.Arithmetic
{
    internal class ExprModifier : IExprVisitor<Expr>
    {
        private readonly IExprVisitor<Expr> _modifier;

        public ExprModifier(IExprVisitor<Expr> modifier)
        {
            this._modifier = modifier;
        }

        public Expr VisitExprNum(ExprNum expr)
        {
            return this._modifier.VisitExprNum(expr);
        }

        public Expr VisitExprPlus(ExprPlus expr)
        {
            var newLeft = expr.Left.Accept(this).Accept(this._modifier);
            var newRight = expr.Right.Accept(this).Accept(this._modifier);
            if (!ReferenceEquals(newLeft, expr.Left) || !ReferenceEquals(newRight, expr.Right))
            {
                return new ExprPlus(newLeft, newRight);
            }
            return expr;
        }

        public Expr VisitExprMinus(ExprMinus expr)
        {
            var newLeft = expr.Left.Accept(this).Accept(this._modifier);
            var newRight = expr.Right.Accept(this).Accept(this._modifier);
            if (!ReferenceEquals(newLeft, expr.Left) || !ReferenceEquals(newRight, expr.Right))
            {
                return new ExprMinus(newLeft, newRight);
            }
            return expr;
        }

        public Expr VisitExprMul(ExprMul expr)
        {
            var newLeft = expr.Left.Accept(this).Accept(this._modifier);
            var newRight = expr.Right.Accept(this).Accept(this._modifier);
            if (!ReferenceEquals(newLeft, expr.Left) || !ReferenceEquals(newRight, expr.Right))
            {
                return new ExprMul(newLeft, newRight);
            }
            return expr;
        }

        public Expr VisitExprDiv(ExprDiv expr)
        {
            var newLeft = expr.Left.Accept(this).Accept(this._modifier);
            var newRight = expr.Right.Accept(this).Accept(this._modifier);
            if (!ReferenceEquals(newLeft, expr.Left) || !ReferenceEquals(newRight, expr.Right))
            {
                return new ExprDiv(newLeft, newRight);
            }
            return expr;
        }

        public Expr VisitExprSub(ExprSubExpr expr)
        {
            var newSub = expr.SubExpression.Accept(this).Accept(this._modifier);

            if (!ReferenceEquals(newSub, expr.SubExpression))
            {
                return new ExprSubExpr(newSub);
            }
            return expr;
        }
    }
}