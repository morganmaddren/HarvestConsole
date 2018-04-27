using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Statistics.Balance
{
    class BalanceStringEvaluator
    {
        BalanceData data;

        public BalanceStringEvaluator(BalanceData data)
        {
            this.data = data;
        }

        public double Evaluate(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            var inputs = System.Text.RegularExpressions.Regex.Split(input, @"([ +\-\*/])")
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(x => x.Trim())
                            .ToList();

            Node tree = Treeify(data, inputs, 0, inputs.Count - 1);

            return tree.Eval();
        }

        private abstract class Node
        {
            public abstract double Eval();
        }

        private class OperationNode : Node
        {
            public Node Left { get; set; }
            public Node Right { get; set; }

            Func<Node, Node, double> operation;

            public OperationNode(Func<Node, Node, double> operation)
            {
                this.operation = operation;
            }

            public override double Eval()
            {
                return operation(Left, Right);
            }
        }

        private class ConstNode : Node
        {
            double value;

            public ConstNode(double value)
            {
                this.value = value;
            }

            public override double Eval()
            {
                return value;
            }
        }

        private class VariableNode : Node
        {
            BalanceData data;
            string key;

            public VariableNode(BalanceData data, string key)
            {
                this.data = data;
                this.key = key;
            }

            public override double Eval()
            {
                return data.Data[key];
            }
        }

        static Node ParseToken(BalanceData data, string s)
        {
            double result;
            if (double.TryParse(s, out result))
            {
                return new ConstNode(result);
            }

            return new VariableNode(data, s);
        }

        static OperationNode ParseOperation(string s)
        {
            switch (s)
            {
                case "+":
                    return new OperationNode((x, y) => x.Eval() + y.Eval());
                case "-":
                    return new OperationNode((x, y) => x.Eval() - y.Eval());
                case "*":
                    return new OperationNode((x, y) => x.Eval() * y.Eval());
                case "/":
                    return new OperationNode((x, y) => x.Eval() / y.Eval());
                default:
                    return null;
            }
        }

        static Node Treeify(BalanceData data, List<string> input, int start, int end)
        {
            if (end < start)
                return null;

            if (end == start)
                return ParseToken(data, input[start]);

            int index = FindHighestPriorityOperationIndex(input, start, end);
            if (index == -1)
                throw new Exception();

            var op = ParseOperation(input[index]);
            op.Left = Treeify(data, input, start, index - 1);
            op.Right = Treeify(data, input, index + 1, end);

            return op;
        }

        static int FindHighestPriorityOperationIndex(List<string> input, int start, int end)
        {
            for (int i = end - 1; i >= start; i--)
            {
                if (input[i] == "+" || input[i] == "-")
                    return i;
            }

            for (int i = end - 1; i >= start; i--)
            {
                if (input[i] == "*" || input[i] == "/")
                    return i;
            }
            
            return -1;
        }
    }
}
