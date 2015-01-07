﻿using System;
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
        private string _variable;
        private string _value;
        private string _stateLabel;

        public string Variable
        {
            get { return _variable; }
        }
        public string Value
        {
            get { return _value; }
        }
        public string StateLabel
        {
            get { return _stateLabel; }
        }

        public NuTraceVariable(string variable, string value, string stateLabel)
        {
            _variable = variable;
            _value = value;
            _stateLabel = stateLabel;
        }
    }
}
