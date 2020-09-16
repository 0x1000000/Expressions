using System;
using System.Diagnostics;
using Expression.Arithmetic;
using NUnit.Framework;

namespace Expression.Test
{
    [TestFixture]
    public class ArithmeticTests
    {
        [Test]
        public void Doc()
        {
            Expr expr = Parser.Parse("1+(5-2)*2");

            Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

            expr = 2 * expr - 3;

            Console.WriteLine($"{expr.Print()} = {expr.Evaluate()}");

            var exprSimple = expr.OpenParentheses();

            Console.WriteLine($"{expr.Print()} = {exprSimple.Print()} = {exprSimple.Evaluate()}");
        }

        [Test]
        public void Arithmetic()
        {
            AssertInput(11, "11");
            AssertInput(11, "(11)");
            AssertInput(11-7+5, "11-7+5");
            AssertInput(11-7+5, "11-7+5");
            AssertInput(2*3+3*5*2+12, "2*3+3*5*2+12");
            AssertInput(2*3-3*5*2+12, "2*3-3*5*2+12");
            AssertInput((1+1), "(1+1)");
            AssertInput((1+2)*(3+4), "(1+2)*(3+4)");
            AssertInput((1+2)-(3+4), "(1+2)-(3+4)");
            AssertInput(1+2-(3+4), "1+2-(3+4)");
            AssertInput(7-(2+5), "7-(2+5)");
            AssertInput(7-2-5, "7-2-5");
            AssertInput(4/2, "4/2");
            AssertInput(4/2*3+1, "4/2*3+1");
            AssertInput(4/2*(3+1), "4/2*(3+1)");
            AssertInput(4/(8/(1+3))-(1*1 + 2/2), "4/(8/(1+3))-(1*1+2/2)");
            AssertInput(7-5*(7+8), "7-5*(7+8)");
            AssertInput((7-5*(7+8))*2+(7*(3-2)), "(7-5*(7+8))*2+(7*(3-2))");

            void AssertInput(int expected, string input)
            {
                var expr = Parser.Parse(input);
                Assert.AreEqual(expected, expr.Evaluate(), message: $"Expected\"{input}\" to be {expected}");
                Assert.AreEqual(input, expr.Print());
            }
        }

        [Test]
        public void Err1()
        {
            Assert.Throws<ParserException>(() => {
                    Parser.Parse("1++2");
                }, "Could not recognize number Col. 2 Input: 1+->+2");
        }

        [Test]
        public void Err2()
        {
            Assert.Throws<ParserException>(() => {
                    Parser.Parse("(1+2)*((2+1)");
                }, "Could not find closing ')' Col. 6 Input: (1+2)*->((2+1)");
        }

        [Test]
        public void TestOpenParentheses()
        {
            AssertInput("2*3+2*5+1", "2*(3+5)+1", 17);
            AssertInput("7*3+7*5", "7*(3+5)", 56);
            AssertInput("3*7+5*7", "(3+5)*7", 56);
            AssertInput("7*3+2*3+7*5+2*5", "(7+2)*(3+5)", 72);
            AssertInput("7*3*5-7*3*4+2*3*5-2*3*4+7*5*5-7*5*4+2*5*5-2*5*4", "(7+2)*(3+5)*(5-4)", 72);

            AssertInput("7*5-7*3", "7*(5-3)", 14);
            AssertInput("5*2-3*2", "(5-3)*2", 4);
            AssertInput("7*5-7*3+2*5-2*3", "(7+2)*(5-3)", 18);

            AssertInput("7*5*3", "7*(5*3)", 105);
            AssertInput("7*5*3", "(7*5)*3", 105);
            AssertInput("7*5*3", "(7*5)*(3)", 105);
            AssertInput("7+3+5", "(7+3)+5", 15);
            AssertInput("7+3+5", "7+(3+5)", 15);
            AssertInput("7+2+3+5", "(7+2)+(3+5)", 17);
            AssertInput("7+3-5", "7+(3-5)", 5);
            AssertInput("3-5+7", "(3-5)+7", 5);
            AssertInput("7-2-5", "7-(2+5)", 0);
            AssertInput("7+2-5", "(7+2)-5", 4);
            AssertInput("7-5+2", "7-(5-2)", 4);
            AssertInput("7-5+2+3", "7-(5-2-3)", 7);
            AssertInput("7-5-2", "(7-5)-2", 0);
            AssertInput("7*2+3", "(7*2)+3", 17);
            AssertInput("7+2*3", "7+(2*3)", 13);
            AssertInput("7*6+2*6+3*6+4*6+5*6", "(7+2+3+4+5)*6", 126);
            AssertInput("7*6+2*6-3*6+4*6-5*6", "(7+2-3+4-5)*6", 30);
            AssertInput("1-17-3-5*2", "1-(17+3+5*2)", -29);

            AssertInput("7*2-5*7*2-5*8*2+7*3-7*2", "(7-5*(7+8))*2+(7*(3-2))", -129);
            AssertInput("7*2-5*7*2-5*8*2+19*2-12*5*2+3*2+7*3-7*2-17-3-5*2", "(7-5*(7+8)+19-(12*5-3))*2+(7*(3-2)-(17+3+5*2))", -235);

            void AssertInput(string expected, string input, int res)
            {
                var fromInput = Parser.Parse(input);
                var result = fromInput.OpenParentheses();
                Assert.AreEqual(expected, result.Print());
                Assert.AreEqual(Parser.Parse(expected).Evaluate(), result.Evaluate());
                Assert.AreEqual(fromInput.Evaluate(), result.Evaluate());
                Assert.AreEqual(res, result.Evaluate());
            }
        }

        [Test]
        public void Print_CheckPlusMinusParenthesizes()
        {
            Expr e = 2 * ((Expr)1 + 3);
            Assert.AreEqual("2*(1+3)", e.Print());
            Assert.AreEqual(8, e.Evaluate());

            e = (Expr)3 - 2 - ((Expr)1 + 3);
            Assert.AreEqual("3-2-(1+3)", e.Print());
            Assert.AreEqual(-3, e.Evaluate());

            e = ((Expr)10-2)/((Expr)1 + 3);
            Assert.AreEqual("(10-2)/(1+3)", e.Print());
            Assert.AreEqual(2, e.Evaluate());

            e = 8/((Expr)1 + 3);
            Assert.AreEqual("8/(1+3)", e.Print());
            Assert.AreEqual(2, e.Evaluate());
        }

    }
}