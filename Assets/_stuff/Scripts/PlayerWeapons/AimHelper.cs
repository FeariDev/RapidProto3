using UnityEngine;

public static class AimHelper
{
    public static Vector3 GetAimDirection(Transform weaponTransform)
    {
        Vector2 aim = Player.Instance.AimDirection;

        // If controller stick is being used
        if (aim.sqrMagnitude > 0.01f)
        {
            return new Vector3(aim.x, aim.y, 0).normalized;
        }
        else
        {
            // Fallback to mouse aiming
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return (mousePos - weaponTransform.position).normalized;
        }
    }
}
