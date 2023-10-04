//   ==============================================================================
//   | Lynx Interfaces (2023)                                                     |
//   |======================================                                      |
//   | Vector Methods                                                             |
//   | This class contains some vectors methods.                                  |
//   ==============================================================================

using UnityEngine;

namespace Lynx.UI
{
    public class LynxVectors
    {
        /// <summary>
        /// Call this function to inverse a Vector3.
        /// </summary>
        /// <param name="vector">Vector3 to inverse.</param>
        public static Vector3 InverseVector(Vector3 vector)
        {
            Vector3 inversedVector = new Vector3();
            inversedVector.x = 1f / vector.x;
            inversedVector.y = 1f / vector.y;
            inversedVector.z = 1f / vector.z;
            return inversedVector;
        }
    }
}
