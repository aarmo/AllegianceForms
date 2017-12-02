using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllegianceForms.Engine.LogicFlow
{
    public class LogicFlow
    {
        public Dictionary<string, object> Memory = new Dictionary<string, object>();

    }

    public enum EBoolOperationType
    {
        And, Or, Xor
    }

    public enum ECompareOperationType
    {
        Equal, NotEqual, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
    }

    public enum EArithmeticOperationType
    {
        Add, Subtract, Multiply, Divide, Modulas
    }

    public enum EValueType
    {
        Number, Text, Boolean
    }

    public enum EMemoryScopeType
    {
        Flow, Game, Permanent
    }

    public class Operation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class GetMemoryOperation
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public EMemoryScopeType Scope = EMemoryScopeType.Flow;

        public object Execute(LogicFlow thisFlow, StrategyGame game)
        {
            switch (Scope)
            {
                case EMemoryScopeType.Game:
                    return game.AllUnits[0];

                default:
                    return thisFlow.Memory[Name];
            }
        }
    }

    public class ValueOperation
    {
        public EValueType Type = EValueType.Text;

        public string Input { get; set; }

        public object Execute()
        {
            switch (Type)
            {
                case EValueType.Number: 
                    double d;
                    double.TryParse(Input, out d);
                    return d;

                case EValueType.Boolean:
                    bool b;
                    bool.TryParse(Input, out b);
                    return b;

                default:
                    return Input;
            }
        }
    }

    public class BooleanOperation
    {
        public EBoolOperationType Type = EBoolOperationType.Or;

        public bool Input1 { get; set; }

        public bool Input2 { get; set; }

        public bool Execute()
        {
            switch (Type)
            {
                case EBoolOperationType.And: return Input1 && Input2;
                case EBoolOperationType.Xor: return Input1 ^ Input2;
                default:
                    return Input1 || Input2;
            }
        }
    }

    public class NotOperation
    {
        public bool Input1 { get; set; }

        public bool Execute()
        {
            return !Input1;
        }
    }

    public class ArithmeticOperation
    {
        public EArithmeticOperationType Type = EArithmeticOperationType.Add;

        public double Input1 { get; set; }
        public double Input2 { get; set; }

        public double Execute()
        {
            switch (Type)
            {
                case EArithmeticOperationType.Subtract: return Input1 - Input2;
                case EArithmeticOperationType.Multiply: return Input1 * Input2;
                case EArithmeticOperationType.Divide: return Input1 / Input2;
                case EArithmeticOperationType.Modulas: return Input1 % Input2;

                default:
                    return Input1 + Input2;
            }
        }
    }

    public class ComparisonOperation
    {
        public ECompareOperationType Type = ECompareOperationType.Equal;

        public IComparable Input1 { get; set; }
        public IComparable Input2 { get; set; }

        public bool Execute()
        {
            var c = Input1.CompareTo(Input2);

            switch (Type)
            {
                case ECompareOperationType.NotEqual: return c != 0;
                case ECompareOperationType.GreaterThan: return c > 0;
                case ECompareOperationType.LessThan: return c < 0;
                case ECompareOperationType.GreaterThanOrEqual: return c >= 0;
                case ECompareOperationType.LessThanOrEqual: return c <= 0;
                default:
                    return c == 0;
            }
        }
    }
}
