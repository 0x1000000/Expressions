namespace Expression.Arithmetic
{
    public class ExprOpener : IExprVisitor<Expr>
    {
        public static readonly ExprOpener Instance = new ExprOpener();

        public Expr VisitExprNum(ExprNum expr)
        {
            return expr;
        }

        public Expr VisitExprPlus(ExprPlus expr)
        {
            //x+(y+z) = x+y+z
            if (expr.Right is ExprSubExpr subExprPlus && subExprPlus.SubExpression is ExprPlus plus)
            {
                return new ExprPlus(expr.Left, plus);
            }
            //(x+y)+z = x+y+z
            if (expr.Left is ExprSubExpr subExprL && subExprL.SubExpression is ExprPlus plusL)
            {
                return new ExprPlus(plusL, expr.Right);
            }
            //x+(y-z) = x+y-z
            if (expr.Right is ExprSubExpr subExprMinus && subExprMinus.SubExpression is ExprMinus minus)
            {
                return new ExprPlus(expr.Left, minus);
            }
            //(y-z)+x = y-z+x
            if (expr.Left is ExprSubExpr subExprMinusL && subExprMinusL.SubExpression is ExprMinus minusL)
            {
                return new ExprPlus(minusL, expr.Right);
            }
            //x+(y*z) = x+y*z
            if (expr.Right is ExprSubExpr subExprMul && subExprMul.SubExpression is ExprMul mul)
            {
                return new ExprPlus(expr.Left, mul);
            }
            //(x*y)+z = x*y+z
            if (expr.Left is ExprSubExpr subExprMulL && subExprMulL.SubExpression is ExprMul mulL)
            {
                return new ExprPlus(mulL, expr.Right);
            }
            return expr;
        }

        public Expr VisitExprMinus(ExprMinus expr)
        {
            //x-(y+z) = x-y-z
            if (expr.Right is ExprSubExpr subExprPlus && subExprPlus.SubExpression is ExprPlus plus)
            {
                return new ExprMinus(new ExprMinus(expr.Left, plus.Left.ToSubExprIfComplex()), plus.Right.ToSubExprIfComplex());
            }
            //(x+y)-z = x+y-z
            if (expr.Left is ExprSubExpr subExprPlusL && subExprPlusL.SubExpression is ExprPlus plusL)
            {
                return new ExprMinus(plusL, expr.Right);
            }
            //x-(y-z) = x-y+z
            if (expr.Right is ExprSubExpr subExprMinus && subExprMinus.SubExpression is ExprMinus minus)
            {
                return new ExprPlus(new ExprMinus(expr.Left, minus.Left.ToSubExprIfComplex()), minus.Right.ToSubExprIfComplex());
            }
            //(x-y)-z = x-y-z
            if (expr.Left is ExprSubExpr subExprMinusL && subExprMinusL.SubExpression is ExprMinus minusL)
            {
                return new ExprMinus(minusL, expr.Right);
            }
            //x-(y*z) = x-y*z
            if (expr.Right is ExprSubExpr subExprMul && subExprMul.SubExpression is ExprMul mul)
            {
                return new ExprMinus(expr.Left, mul);
            }
            //(x*y)-z = x*y-z
            if (expr.Left is ExprSubExpr subExprMulL && subExprMulL.SubExpression is ExprMul mulL)
            {
                return new ExprMinus(mulL, expr.Right);
            }

            return expr;
        }

        private static bool ExtractPlus(Expr expr, out ExprPlus? result)
        {
            if (expr is ExprSubExpr subExpr && subExpr.SubExpression is ExprPlus plus)
            {
                result = plus;
                return true;
            }
            if (expr is ExprPlus dPlus )
            {
                result = dPlus;
                return true;
            }
            result = null;
            return false;
        }

        private static bool ExtractMinus(Expr expr, out ExprMinus? result)
        {
            if (expr is ExprSubExpr subExpr && subExpr.SubExpression is ExprMinus minus)
            {
                result = minus;
                return true;
            }
            if (expr is ExprMinus dMinus )
            {
                result = dMinus;
                return true;
            }
            result = null;
            return false;
        }

        public Expr VisitExprMul(ExprMul expr)
        {
            //x*(z+y) = x*z+x*y
            if (ExtractPlus(expr.Right, out var plus))
            {
                return new ExprPlus(new ExprMul(expr.Left, plus!.Left).ToSubExprIfComplex(), new ExprMul(expr.Left, plus.Right).ToSubExprIfComplex()).ToSubExprIfComplex();
            }
            //(z+y)*x = z*x+y*x
            if (ExtractPlus(expr.Left, out var plusL))
            {
                return new ExprPlus(new ExprMul(plusL!.Left, expr.Right).ToSubExprIfComplex(), new ExprMul(plusL.Right, expr.Right).ToSubExprIfComplex()).ToSubExprIfComplex();
            }
            //x*(z-y) = x*z-x*y
            if (ExtractMinus(expr.Right, out var minus))
            {
                return new ExprMinus(new ExprMul(expr.Left, minus!.Left).ToSubExprIfComplex(), new ExprMul(expr.Left, minus.Right).ToSubExprIfComplex()).ToSubExprIfComplex();
            }
            //(z-y)*x = z*x-y*x
            if (ExtractMinus(expr.Left, out var minusL))
            {
                return new ExprMinus(new ExprMul(minusL!.Left, expr.Right).ToSubExprIfComplex(), new ExprMul(minusL.Right, expr.Right).ToSubExprIfComplex()).ToSubExprIfComplex();
            }

            //x*(z*y) = x*z*y
            if (expr.Right is ExprSubExpr subExprMul && subExprMul.SubExpression is ExprMul mul)
            {
                return new ExprMul(expr.Left, mul);
            }
            //(x*z)*y = x*z*y
            if (expr.Left is ExprSubExpr subExprMulL && subExprMulL.SubExpression is ExprMul mulL)
            {
                return new ExprMul(mulL, expr.Right);
            }
            return expr;
        }

        public Expr VisitExprDiv(ExprDiv expr)
        {
            return expr;
        }

        public Expr VisitExprSub(ExprSubExpr expr)
        {
            if (expr.SubExpression is ExprNum exprNum)
            {
                return exprNum;
            }
            if (expr.SubExpression is ExprSubExpr exprSub)
            {
                return exprSub;
            }
            return expr;
        }
    }
}