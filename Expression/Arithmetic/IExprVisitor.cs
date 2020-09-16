namespace Expression.Arithmetic
{
    public interface IExprVisitor<out TRes>
    {
        TRes VisitExprNum(ExprNum expr);

        TRes VisitExprPlus(ExprPlus expr);
        TRes VisitExprMinus(ExprMinus expr);

        TRes VisitExprMul(ExprMul expr);
        TRes VisitExprDiv(ExprDiv expr);

        TRes VisitExprSub(ExprSubExpr expr);
    }
}