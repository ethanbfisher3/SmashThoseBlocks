using UnityEngine;

namespace Utils
{
	public class Raycaster
    {
        public static bool RaycastCursor(out RaycastHit hit) => Physics.Raycast(
            Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
    }
}