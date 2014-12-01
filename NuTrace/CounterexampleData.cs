using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuTrace
{

    class CounterexampleData
    {
        public CounterexampleData(string spec, string result)
        {
            Spec = spec;
            Result = result;
        }

        public readonly string Spec;
        public readonly string Result;
    }

    public enum ELoopState
    {
        NoLoop,
        LoopBegin,
        Loop
    }

    public class TraceState
    {
        public TraceState(string label, ELoopState loop)
        {
            Label = label;
            Loop = loop;
        }

        public readonly string Label;
        public readonly ELoopState Loop;
    }

    class TraceVariable
    {
        public readonly string Variable;
        public readonly string Value;

        public TraceVariable(string variable, string value)
        {
            Variable = variable;
            Value = value;
        }
    }
}
