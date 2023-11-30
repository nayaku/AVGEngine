using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AVGEngine
{
    public class WaitFor : CustomYieldInstruction
    {
        private Func<bool> _condition;
        public override bool keepWaiting
        {
            get
            {
                return !_condition();
            }
        }

        public WaitFor(Func<bool> condition)
        {
            _condition = condition;
        }
    }
}
