using System;

namespace Expression.Arithmetic
{
    public static class ExprExtension
    {
        public static int Evaluate(this Expr expr) 
            => expr.Accept(ExprEvaluator.Instance);

        public static string Print(this Expr expr) 
            => expr.Accept(ExprPrinter.Instance);

        public static Expr Modify(this Expr expr, IExprVisitor<Expr> modifier)
        {
            return expr.Accept(new ExprModifier(modifier)).Accept(modifier);
        }

        public static Expr OpenParentheses(this Expr expr)
        {
            int watchDog = 0;
            Expr before;
            var after = expr;
            do
            {
                watchDog++;
                if (watchDog == 100000)
                {
                    throw new Exception("Too many iterations");
                }
                before = after;
                after = before.Modify(ExprOpener.Instance);
                if (after is ExprSubExpr s)
                {
                    after = s.SubExpression;
                }
            } while (after != before);

            return after;
        }

        internal static Expr ToSubExprIfComplex(this Expr expr)
        {
            if (expr is ILeftRight)
            {
                return new ExprSubExpr(expr);
            }
            return expr;
        }
    }
}