using System;
using System.Diagnostics;

namespace Expression.Arithmetic
{
    public static class Parser
    {
        public static Expr Parse(string str)
        {
            var arraySegment = str.ToCharArray();
            if (!ParseExpr(arraySegment, out var result))
            {
                throw ParserException.Build("Could not find an arithmetic expression", arraySegment, 0);
            }

            return result ?? throw new Exception("Result cannot be null");
        }

        private static bool ParseExpr(ArraySegment<char> str, out Expr? result)
        {
            LexCheck(str);
            return ParseExprAll(str, out result);
        }

        private static void LexCheck(ArraySegment<char> str)
        {
            for (int i = 0; i < str.Count; i++)
            {
                var ch = str[i];

                bool valid = char.IsDigit(ch) || ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '(' || ch == ')';

                if (!valid)
                {
                    throw ParserException.Build("Incorrect char", str, i);
                }
            }
        }

        private static bool ParseExprAll(ArraySegment<char> str, out Expr? result)
        {

            if (ParseSubExpr(str, out result)) return true;
            if (ParsePlusMinus(str, out result)) return true;
            if (ParseMulDiv(str, out result)) return true;
            if (ParseNum(str, out result)) return true;
            return false;
        }

        private static bool ParseExprMulDiv(ArraySegment<char> str, out Expr? result)
        {
            if (ParseSubExpr(str, out result)) return true;
            if (ParseMulDiv(str, out result)) return true;
            if (ParseNum(str, out result)) return true;
            return false;
        }

        private static bool ParseSubExpr(ArraySegment<char> str, out Expr? result)
        {
            result = null;
            if (str.Count > 0 && str[0] == '(')
            {
                int sum = 1;

                for (int i = 1; i < str.Count; i++)
                {
                    if (str[i] == '(')
                    {
                        sum++;
                    }
                    if (str[i] == ')')
                    {
                        sum--;
                    }

                    if (sum == 0)
                    {
                        if (i + 1 != str.Count)// (....)....
                        {
                            return false;
                        }

                        if (!ParseExprAll(str.Slice(1, i - 1), out var sub))
                        {
                            throw ParserException.Build("Could not recognize any sub expression", str, i);
                        }

                        Debug.Assert(sub != null, nameof(sub) + " != null");
                        result = new ExprSubExpr(sub);
                        return true;
                    }
                }
                throw ParserException.Build("Could not find closing ')'", str, 0);
            }
            return false;
        }

        private static bool ParsePlusMinus(ArraySegment<char> str, out Expr? result)
        {
            result = null;

            var plusIndex = str.LastIndexOf(out var sign, '+', '-');
            if (plusIndex < 0)
            {
                return false;
            }

            if (!ParseExprAll(str.Slice(0, plusIndex), out var left))
            {
                throw ParserException.Build("Could not recognize any left expression", str, 0);
            }
            if (!ParseExprAll(str.Slice(plusIndex + 1), out var right))
            {
                throw ParserException.Build("Could not recognize any right expression", str, plusIndex + 1);
            }

            Debug.Assert(left != null, nameof(left) + " != null");
            Debug.Assert(right != null, nameof(right) + " != null");
            result = sign == '+' ? (Expr)new ExprPlus(left, right) : new ExprMinus(left, right);
            return true;
        }

        private static bool ParseMulDiv(ArraySegment<char> str, out Expr? result)
        {
            result = null;

            var mulIndex = str.LastIndexOf(out var sign, '*', '/');
            if (mulIndex < 0)
            {
                return false;
            }

            if (!ParseExprMulDiv(str.Slice(0, mulIndex), out var left))
            {
                throw ParserException.Build("Could not recognize any left expression", str, mulIndex + 1);
            }
            if (!ParseExprMulDiv(str.Slice(mulIndex + 1), out var right))
            {
                throw ParserException.Build("Could not recognize any right expression", str, mulIndex + 1);
            }

            Debug.Assert(left != null, nameof(left) + " != null");
            Debug.Assert(right != null, nameof(right) + " != null");
            result = sign == '*' ? (Expr)new ExprMul(left, right) : new ExprDiv(left, right);
            return true;
        }

        private static bool ParseNum(ArraySegment<char> str, out Expr result)
        {
            if (!int.TryParse(str, out var value))
            {
                throw ParserException.Build("Could not recognize number", str, 0);
            }
            result = new ExprNum(value);
            return true;
        }

        private static int LastIndexOf(this ArraySegment<char> str, out char res, char ch1, char ch2)
        {
            res = (char)0;
            for (int i = str.Count - 1; i >= 0; i--)
            {
                if ((str[i] == ch1 || str[i] == ch2) && !str.IndexInsideParenthesis(i))
                {
                    res = str[i];
                    return i;
                }
            }
            return -1;
        }

        private static bool IndexInsideParenthesis(this ArraySegment<char> str, int index)
        {
            int sum = 0;
            for (int i = 0; i < index; i++)
            {
                if (str[i] == '(')
                {
                    sum++;
                }
                if (str[i] == ')')
                {
                    sum--;
                }
            }
            return sum != 0;
        }
    }
}