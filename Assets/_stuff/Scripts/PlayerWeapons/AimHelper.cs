using UnityEngine;

public static class AimHelper
{
    public static Vector3 GetAimDirection(Transform weaponTransform)
    {
        Vector3 aimTarget = Vector3.zero;

        // 🕹 If using controller + virtual cursor, use its position
        if (VirtualCursor.WorldPosition != Vector3.zero)
        {
            aimTarget = VirtualCursor.WorldPosition;
        }
        else
        {
            // 🖱 fallback to real mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            aimTarget = mousePos;
        }

        Vector3 dir = (aimTarget - weaponTransform.position).normalized;
        return dir;
    }
}