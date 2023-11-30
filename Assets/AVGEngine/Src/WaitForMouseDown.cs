using UnityEngine;

namespace AVGEngine
{
    public class WaitForMouseDown : CustomYieldInstruction
    {
        private int _button;
        public override bool keepWaiting
        {
            get
            {
                return !Input.GetMouseButtonDown(_button);
            }
        }

        public WaitForMouseDown(int button = 0)
        {
            _button = button;
        }
    }
}
