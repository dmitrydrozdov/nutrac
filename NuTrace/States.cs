using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NuTrace
{
    public class FindCounterexampleBegin : ParcerState
    {
        public FindCounterexampleBegin(ParcerFSM fsm) : base(fsm)
        {}

        public override void Enter()
        {}

        public override void Do()
        {
            _fsm.ReadNextLine();
            if (_fsm.CounterexampleBeginReached) _fsm.EnterState(new CounterexampleBegin(_fsm));
            else if (_fsm.EndReached) _fsm.EnterState(new Fault(_fsm));
        }
        public override void Exit()
        {}
    }
    class CounterexampleBegin : ParcerState
    {
        public CounterexampleBegin(ParcerFSM fsm)
            : base(fsm)
        { }

        public override void Enter()
        {
            Match m = _fsm.CounterexampleBeginRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new CounterexampleData(m.Groups[1].Value, m.Groups[2].Value));

            _fsm.ReadNextLine();
        }

        public override void Do()
        {
            if (_fsm.CommentLine1Reached) _fsm.EnterState(new CommentLine1(_fsm));
            else _fsm.EnterState(new Fault(_fsm));
        }
        public override void Exit()
        { }
    }
    class CommentLine1 : ParcerState {
        public CommentLine1(ParcerFSM fsm) : base(fsm)
        {
        }
        public override void Enter()
        {
            _fsm.ReadNextLine();
        }
        public override void Do()
        {
            if (_fsm.CommentLine2Reached) _fsm.EnterState(new CommentLine2(_fsm));
            else _fsm.EnterState(new Fault(_fsm));
        }
        public override void Exit()
        { }
    }
    class CommentLine2 : ParcerState
    {
        public CommentLine2(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            _fsm.ReadNextLine();
        }
        public override void Do()
        {
            if (_fsm.CommentLine3Reached) _fsm.EnterState(new CommentLine3(_fsm));
            else _fsm.EnterState(new Fault(_fsm));
        }
        public override void Exit()
        { }
    }
    class CommentLine3 : ParcerState
    {
        public CommentLine3(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            _fsm.ReadNextLine();
        }

        public override void Do()
        {
            if (_fsm.StateReached) _fsm.EnterState(new CounterexampleState(_fsm));
            else if (_fsm.LoopBeginReached) _fsm.EnterState(new LoopBeginState(_fsm));
            else _fsm.EnterState(new Fault(_fsm));
        }
        public override void Exit()
        { }
    }
    class CounterexampleState : ParcerState
    {
        public CounterexampleState(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            Match m = _fsm.StateRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new TraceState(m.Groups[1].Value, ELoopState.NoLoop));

            _fsm.ReadNextLine();
        }

        public override void Do()
        {
            if (_fsm.VariableReached) _fsm.EnterState(new CounterexampleVar(_fsm));
            else if (_fsm.StateReached) _fsm.EnterState(this);
            else if (_fsm.LoopBeginReached) _fsm.EnterState(new LoopBeginState(_fsm));
            else if (_fsm.EndReached) _fsm.EnterState(new CounterexampleEnd(_fsm));
        }
        public override void Exit()
        { }
    }
    class CounterexampleVar : ParcerState
    {
        public CounterexampleVar(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            Match m = _fsm.VariableRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new TraceVariable(m.Groups[1].Value,m.Groups[3].Value));

            _fsm.ReadNextLine();
        }

        public override void Do()
        {
            if (_fsm.VariableReached) _fsm.EnterState(this);
            else if (_fsm.StateReached) _fsm.EnterState(new CounterexampleState(_fsm));
            else if (_fsm.LoopBeginReached) _fsm.EnterState(new LoopBeginState(_fsm));
            else if (_fsm.EndReached) _fsm.EnterState(new CounterexampleEnd(_fsm));
        }
        public override void Exit()
        { }
    }
    class LoopBeginState : ParcerState
    {
        public LoopBeginState(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            _fsm.ReadNextLine();

            Match m = _fsm.StateRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new TraceState(m.Groups[1].Value, ELoopState.LoopBegin));

            _fsm.ReadNextLine();
        }
        public override void Do()
        {
            if (_fsm.VariableReached) _fsm.EnterState(new LoopVariable(_fsm));
            else if (_fsm.StateReached) _fsm.EnterState(new LoopState(_fsm));
            else if (_fsm.EndReached) _fsm.EnterState(new CounterexampleEnd(_fsm));
        }
        public override void Exit()
        { }
    }
    class LoopState : ParcerState
    {
        public LoopState(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            Match m = _fsm.StateRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new TraceState(m.Groups[1].Value, ELoopState.Loop));

            _fsm.ReadNextLine();
        }
        public override void Do()
        {
            if (_fsm.VariableReached) _fsm.EnterState(new LoopVariable(_fsm));
            else if (_fsm.StateReached) _fsm.EnterState(this);
            else if (_fsm.EndReached) _fsm.EnterState(new CounterexampleEnd(_fsm));
        }
        public override void Exit()
        { }
    }
    class LoopVariable : ParcerState
    {
        public LoopVariable(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        {
            Match m = _fsm.VariableRegex.Match(_fsm.LastReadString);
            _fsm.OutQueue.Enqueue(new TraceVariable(m.Groups[1].Value, m.Groups[3].Value));

            _fsm.ReadNextLine();
        }
        public override void Do()
        {
            if (_fsm.VariableReached) _fsm.EnterState(this);
            else if (_fsm.StateReached) _fsm.EnterState(new LoopState(_fsm));
            else if (_fsm.EndReached) _fsm.EnterState(new CounterexampleEnd(_fsm));
            else if (_fsm.LoopBeginReached) _fsm.EnterState(new CounterexampleEnd(_fsm)); // Loop repeat
        }
        public override void Exit()
        { }
    }
    class CounterexampleEnd : ParcerState
    {
        public CounterexampleEnd(ParcerFSM fsm)
            : base(fsm)
        {
        }
        public override void Enter()
        { }

        public override void Do()
        {
            _fsm.Stop();
        }
        public override void Exit()
        { }
    }
    class Fault : ParcerState
    {
        public Fault(ParcerFSM fsm)
            : base(fsm)
        {
        }

        public override void Enter()
        {
            throw new Exception("Counterexample parsing error!");
        }

        public override void Do()
        {
            _fsm.Stop();
        }
        public override void Exit()
        { }
    }
}
