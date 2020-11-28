using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Util
{
    public static class InputManager
    {
        public static float Vertical { get { return Input.GetAxisRaw("Vertical"); } }
        public static float Horizontal { get { return Input.GetAxisRaw("Horizontal"); } }
        public static bool Jump { get { return Input.GetKeyDown(KeyCode.Space); } }
        public static bool Escape { get { return Input.GetKeyDown(KeyCode.Escape); } }
        //public static bool HandBrake { get { return Input.GetKey(KeyCode.LeftShift); } }
    }
}
