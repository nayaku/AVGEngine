using System;
using UnityEngine;

namespace AVGEngine
{
    public class WaitUntilForSeconds : CustomYieldInstruction
    {
        float _timer;
        Func<bool> _condition;
        /// <summary>
        /// 等待直到条件满足，或者超时
        /// </summary>
        /// <param name="condition">条件为真则停止</param>
        /// <param name="seconds">超时时间</param>
        public WaitUntilForSeconds(Func<bool> condition, float seconds)
        {
            _condition = condition;
            _timer = seconds;
        }
        public override bool keepWaiting
        {
            get
            {
                if (_condition())
                {
                    return false;
                }
                else
                {
                    _timer -= Time.deltaTime;
                    return _timer > 0;
                }
            }
        }
    }
}
