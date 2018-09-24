namespace MF.Classes
{
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Class to scan through the input and identify the different elements - operators and constants
    /// </summary>
    public class TokenReader
    {
        public TokenReader(TextReader txtReader)
        {
            this.txtReader = txtReader;
            ReadNextChar();
            ReadNextToken();
        }
        TextReader txtReader;
        char currentChar;
        Token currentToken;
        int integerConstant;

        public Token Token { get { return currentToken; } }
        public int IntegerConstant { get { return integerConstant; } }

        /// <summary>
        /// Method to read the next char of the stream
        /// Set to '\0' in for end of stream
        /// </summary>
        void ReadNextChar()
        {
            int ch = txtReader.Read();
            if (ch < 0)
            {
                currentChar = '\0';
            }
            else
            {
                currentChar = (char)ch;
            }
        }
        public void ReadNextToken()
        {

            //Binary Operators
            while (char.IsWhiteSpace(currentChar))
            {
                ReadNextChar();
            }

            switch (currentChar)
            {
                case '\0':
                    return;
                case '+':
                    ReadNextChar();
                    currentToken = Token.Addition;
                    return;
                case '-':
                    ReadNextChar();
                    currentToken = Token.Substraction;
                    return;
                case '*':
                    ReadNextChar();
                    currentToken = Token.Multiplication;
                    return;
                case '/':
                    ReadNextChar();
                    currentToken = Token.Division;
                    return;

            }

            //Integers
            if (IsDigit(currentChar))
            {
                string result = string.Empty;
                //The number will most probably consist of multiple digits
                while (IsDigit(currentChar))
                {
                    result += currentChar;
                    ReadNextChar();
                }
                integerConstant = int.Parse(result);
                currentToken = Token.Integer;
            }

            //Looking for the abs() and sizeof() unary operators
            if (char.IsLetter(currentChar))
            {
                string result = string.Empty;
                while (IsValidInThisContext(currentChar))
                {
                    result += currentChar;
                    ReadNextChar();
                }

                Regex absValueRegEx = new Regex(@"(abs\([-?0-9]*\))");    //Matching abs() with any number of digits
                Regex siezofRegEx = new Regex(@"(sizeof\(.*\))");       //Matching sizeof() with any characters

                if (absValueRegEx.IsMatch(result.ToLower()))
                {
                    string value = Regex.Match(result, @"\(([^)]*)\)").Groups[1].Value; //Regex to extract the value in the parentheses
                    int intValue = int.Parse(value);
                    if (intValue < 0)
                    {
                        integerConstant = intValue * -1;
                    }
                    else
                    {
                        integerConstant = intValue;
                    }
                    currentToken = Token.Integer;
                }
                else if (siezofRegEx.IsMatch(result.ToLower()))
                {
                    string value = Regex.Match(result, @"\(([^)]*)\)").Groups[1].Value; //Regex to extract the value in the parentheses
                    integerConstant = value.Length;
                    System.Console.WriteLine("cal" + value.Length);
                    currentToken = Token.Integer;
                }
            }

        }


        /// <summary>
        /// Check if a given char is a digit
        /// Not using C# built-in funciton may return flase positive in some cases. 
        /// </summary>
        /// <param name="inputChar"></param>
        /// <returns>boolean if the char is a digit</returns>
        private static bool IsDigit(char inputChar)
        {
            if (inputChar >= '0' && inputChar <= '9')
                return true;
            return false;
        }

        /// <summary>
        /// Static helper method to allow numbers and white spaces to be stored in strings when working with string constrants
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        bool IsValidInThisContext(char ch)
        {
            if (ch == '(' || ch == ')' || ch == '-' || ch == ' ' || char.IsLetter(ch) || IsDigit(ch))
                return true;
            return false;
        }

    }
}
