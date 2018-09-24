namespace MF.Classes
{
    using System;
    using System.IO;

    public class Parser
    {
        TokenReader tknReader;
        public Parser(TokenReader tknReader)
        {
            this.tknReader = tknReader;
        }

        // Parse an entire expression and check EOF was reached
        public Node ParseAnExpression()
        {
            return ParseAdditionSubstraction();
        }

        private Node ParseAdditionSubstraction()
        {
            Node leftNode = ParseMultiplicationDivision();

            while (true)
            {

                Func<int, int, int> operation = null;  //Identify  the operator

                if (tknReader.Token == Token.Addition)
                {
                    operation = (a, b) => a + b;
                }
                else if (tknReader.Token == Token.Substraction)
                {
                    operation = (a, b) => a - b;
                }

                if (operation == null)
                {
                    return leftNode; //No binary operator found
                }

                //parse the right side of the expression
                tknReader.ReadNextToken();
                Node rightNode = ParseMultiplicationDivision();

                //Create a binary node and use it as the left hand side from now on
                leftNode = new BinaryOperationsNode(leftNode, rightNode, operation);
            }
        }

        private Node ParseMultiplicationDivision()
        {
            Node leftNode = ParseUnaryOperators();

            while (true)
            {
                Func<int, int, int> operation = null;
                if (tknReader.Token == Token.Multiplication)
                {
                    operation = (a, b) => a * b;
                }
                else if (tknReader.Token == Token.Division)
                {
                    operation = (a, b) => a / b;
                }

                if (operation == null)
                {
                    return leftNode; //No bin operator found
                }

                tknReader.ReadNextToken();
                Node rightNode = ParseUnaryOperators();
                leftNode = new BinaryOperationsNode(leftNode, rightNode, operation);
            }
        }

        private Node ParseUnaryOperators()
        {
            while (true)
            {
                if (tknReader.Token == Token.Addition)
                {
                    //Positive unary operator so it can be ignored 
                    tknReader.ReadNextToken();
                    continue;
                }
                if (tknReader.Token == Token.Substraction) //Negative operator
                {
                    tknReader.ReadNextToken();
                    Node rightNode = ParseUnaryOperators();
                    return new UnaryNode(rightNode, (x) => -x);
                }
                return ParseLeaf();
            }
        }

        public static Node Parse(string str)
        {
            return Parse(new TokenReader(new StringReader(str)));
        }

        public static Node Parse(TokenReader tknReader)
        {
            Parser parser = new Parser(tknReader);
            return parser.ParseAnExpression();
        }

        //Parse a leaf node
        private Node ParseLeaf()
        {
            if (tknReader.Token == Token.Integer)
            {
                IntegerNode intNode = new IntegerNode(tknReader.IntegerConstant);
                tknReader.ReadNextToken();
                return intNode;
            }
            return null;
        }
    }
}
