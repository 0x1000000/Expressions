namespace Expression.Arithmetic
{
    public abstract class Expr
    {
        public abstract TRes Accept<TRes>(IExprVisitor<TRes> visitor);

        public static implicit operator Expr(int d)
            => new ExprNum(d);

        public static ExprPlus operator +(Expr a, Expr b)
            => new ExprPlus(a, b);

        public static ExprMinus operator -(Expr a, Expr b)
            => new ExprMinus(a, b);

        public static ExprMul operator *(Expr a, Expr b)
            => new ExprMul(a, b);

        public static ExprDiv operator /(Expr a, Expr b)
            => new ExprDiv(a, b);

    }

    public class ExprNum : Expr
    {
        public readonly int Val;

        public ExprNum(int val)
        {
            this.Val = val;
        }

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor) 
            => visitor.VisitExprNum(this);
    }

    interface ILeftRight
    {
        Expr Left { get; }

        Expr Right { get; }
    }

    public class ExprPlus : Expr, ILeftRight
    {
        public Expr Left { get; }

        public Expr Right { get; }

        public ExprPlus(Expr left, Expr right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor)
            => visitor.VisitExprPlus(this);
    }

    public class ExprMinus : Expr, ILeftRight
    {
        public Expr Left { get; }

        public Expr Right { get; }

        public ExprMinus(Expr left, Expr right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor)
            => visitor.VisitExprMinus(this);
    }

    public class ExprMul : Expr, ILeftRight
    {
        public Expr Left { get; }

        public Expr Right { get; }

        public ExprMul(Expr left, Expr right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor)
            => visitor.VisitExprMul(this);
    }

    public class ExprDiv : Expr, ILeftRight
    {
        public Expr Left { get; }

        public Expr Right { get; }

        public ExprDiv(Expr left, Expr right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor)
            => visitor.VisitExprDiv(this);
    }

    public class ExprSubExpr : Expr
    {
        public ExprSubExpr(Expr subExpression)
        {
            this.SubExpression = subExpression;
        }

        public readonly Expr SubExpression;

        public override TRes Accept<TRes>(IExprVisitor<TRes> visitor)
            => visitor.VisitExprSub(this);
    }
}