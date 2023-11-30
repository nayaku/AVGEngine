using UnityEngine;

namespace AVGEngine
{
    public class InputControls
    {
        public static bool NextDialog()
        {
            return Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.Return) 
                || Input.GetMouseButtonDown(0);
        }
    }
}
