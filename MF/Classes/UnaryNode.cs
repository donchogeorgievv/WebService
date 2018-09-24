namespace MF.Classes
{
    using System;

    //Used to process negatives (example 1 + -3)
    class UnaryNode : Node
    {
        public UnaryNode(Node rightNode, Func<int, int> operation)
        {
            this.rightNode = rightNode;
            this.operation = operation;
        }

        Node rightNode;
        Func<int, int> operation;


        public override int Evaluate(IContext context)
        {
            var rhsVal = rightNode.Evaluate(context); //Get the value of the right node

            var result = operation(rhsVal);
            return result;
            throw new NotImplementedException();
        }
    }
}
