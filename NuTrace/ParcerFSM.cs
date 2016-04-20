using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NuTrace
{
    public class ParcerFSM
    {
        //workspace
        public const string CommentLine1 = "-- as demonstrated by the following execution sequence";
        public const string CommentLine2 = "Trace Description: BMC Counterexample";
        public const string CommentLine3 = "Trace Type: Counterexample";
        //public const string LoopBeginLine = "-- Loop starts here";

        private string _lastReadString;
        public readonly Regex CounterexampleBeginRegex = new Regex(@"-- specification\s*(.*)\s*is\s*(\w*)");
        public readonly Regex StateRegex = new Regex(@"->\s+State:\s((\d|\.)+)\s+<-");
        public readonly Regex VariableRegex = new Regex(@"((\w|\.)+)\s=\s((\w|-)+)");
        public readonly Regex LoopBeginRegex = new Regex(@"\s*--\s*(Loop starts here)");

        public readonly TextReader InStream;
        public readonly Queue<Object> OutQueue;
        public string LastReadString
        {
            get { return _lastReadString; }
        }

        public bool EndReached
        {
            get { return (_lastReadString == null || _lastReadString == "NuSMV >" || _lastReadString == "nuXmv >"); }
        }
        public bool CounterexampleBeginReached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (CounterexampleBeginRegex.Match(_lastReadString).Success);
            }
        }
        public bool StateReached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (StateRegex.Match(_lastReadString).Success);
            }
        }
        public bool VariableReached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (VariableRegex.Match(_lastReadString).Success);
            }
        }
        public bool LoopBeginReached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (LoopBeginRegex.Match(_lastReadString).Success);
            }
        }
        /*public bool CommentLine1Reached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (_lastReadString == CommentLine1);
            }
        }
        public bool CommentLine2Reached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (_lastReadString == CommentLine2);
            }
        }*/
        public bool CommentLine3Reached
        {
            get
            {
                if (_lastReadString == null) return false;
                return (_lastReadString == CommentLine3);
            }
        }
        //end workspace

        private IState _currentState;
        private bool _run;

        public ParcerFSM(TextReader inStream, Queue<object> outQueue)
        {
            InStream = inStream;
            OutQueue = outQueue;
            _run = true;
        }

        public string ReadNextLine()
        {
            _lastReadString = InStream.ReadLine();
            if (_lastReadString != null) _lastReadString.Trim();
            return _lastReadString;
        }

        public void Stop()
        {
            _run = false;
        }
        public void Run()
        {
            while (_run)
            {
                _currentState.Do();
            }
        }
        public void EnterState(IState state)
        {
            if (_currentState!=null) _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }
}
