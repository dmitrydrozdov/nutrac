using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuTrace
{
    public class ParcerState : IState
    {
        protected ParcerFSM _fsm;

        public ParcerState(ParcerFSM fsm)
        {
            _fsm = fsm;
        }
        public virtual void Enter()
        {
            throw new NotImplementedException();
        }

        public virtual void Do()
        {
            throw new NotImplementedException();
        }

        public virtual void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
