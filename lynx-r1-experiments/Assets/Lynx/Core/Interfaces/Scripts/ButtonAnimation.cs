//   ==============================================================================
//   | Lynx Interfaces (2023)                                                     |
//   |======================================                                      |
//   | Button Animation Class                                                     |
//   | This class contains some parameters for button animation methods.          |
//   ==============================================================================

using System;
using UnityEngine;

namespace Lynx.UI
{
    [Serializable]
    public class ButtonAnimation
    {
        public Transform moveRoot;
        public Vector3 moveBasePos;
        public Vector3 moveDelta;
        public float moveDuration;
        public bool isUsingScale; 

        public ButtonAnimation()
        {
            this.moveRoot = null;
            this.moveBasePos = Vector3.zero;
            this.moveDelta = Vector3.forward;
            this.moveDuration = 0.5f;
            this.isUsingScale = false;
        }

        public ButtonAnimation(Transform moveRoot, Vector3 moveBasePos, Vector3 moveDelta, float moveDuration, bool isUsingScale)
        {
            this.moveRoot = moveRoot;
            this.moveBasePos = moveBasePos;
            this.moveDelta = moveDelta;
            this.moveDuration = moveDuration;
            this.isUsingScale = isUsingScale;
        }
    }
}
