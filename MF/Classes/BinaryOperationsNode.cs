namespace MF.Classes
{
    using System;
    public class BinaryOperationsNode : Node
    {
        Node leftNode;
        Node rightNode;
        Func<int, int, int> operation; //Callback


        public BinaryOperationsNode(Node leftNode, Node rightNode, Func<int, int, int> operation)
        {
            this.leftNode = leftNode;
            this.rightNode = rightNode;
            this.operation = operation;
        }

        public override int Evaluate(IContext contextt)
        {
            var leftNodeValue = leftNode.Evaluate(contextt);
            var rightNodeValue = rightNode.Evaluate(contextt);

            int result = operation(leftNodeValue, rightNodeValue);
            return result;
        }
    }
}
