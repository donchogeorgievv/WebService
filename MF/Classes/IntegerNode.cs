namespace MF.Classes
{

    public class IntegerNode : Node
    {
        int integerValue;
        public IntegerNode(int integerValue)
        {
            this.integerValue = integerValue;
        }
        public override int Evaluate(IContext context)
        {
            return integerValue;
        }
    }
}

