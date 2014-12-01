using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuTrace
{
    public class Storage
    {
        public readonly List<NuTraceState> States = new List<NuTraceState>();
        public readonly List<NuTraceVariable> Variables = new List<NuTraceVariable>();

        public Storage(Queue<Object> parcedData)
        {
            TraceState currentState = null;
            while (parcedData.Any())
            {
                //TODO: analyze multiple counterexamples
                Object curData = parcedData.Dequeue();
                if (curData.GetType() == typeof(TraceState))
                {
                    currentState = (TraceState)curData;
                    States.Add(new NuTraceState(currentState));
                }
                else if (curData.GetType() == typeof (TraceVariable))
                {
                    TraceVariable v = (TraceVariable) curData;
                    Variables.Add(new NuTraceVariable(v.Variable, v.Value, currentState.Label));
                }
            }
        }
    }

    public class NuTraceState
    {
        public readonly string Label;
        //public readonly string Next;
        //public readonly string Previous;
        public readonly ELoopState Loop;

        public NuTraceState(string label, ELoopState loop)
        {
            Label = label;
            Loop = loop;
        }
        public NuTraceState(TraceState state)
        {
            Label = state.Label;
            Loop = state.Loop;
        }
    }

    public class NuTraceVariable
    {
        public readonly string Variable;
        public readonly string Value;
        public readonly string StateLabel;

        public NuTraceVariable(string variable, string value, string stateLabel)
        {
            Variable = variable;
            Value = value;
            StateLabel = stateLabel;
        }
    }
}
