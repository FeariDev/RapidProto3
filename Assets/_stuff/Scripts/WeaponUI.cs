using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [Header("Weapon UI Elements (1-4)")]
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;

    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color activeColor = Color.HSVToRGB(146, 22, 81);

    private void OnEnable()
    {
        Player.Instance.attack.OnWeaponSwitched += UpdateWeaponUI;
    }

    private void OnDisable()
    {
        Player.Instance.attack.OnWeaponSwitched -= UpdateWeaponUI;
    }

    void UpdateWeaponUI(int weaponIndex)
    {
        slot1.color = defaultColor;
        slot2.color = defaultColor;
        slot3.color = defaultColor;
        slot4.color = defaultColor;

        switch (weaponIndex)
        {
            case 1: slot1.color = activeColor; break;
            case 2: slot2.color = activeColor; break;
            case 3: slot3.color = activeColor; break;
            case 4: slot4.color = activeColor; break;
        }
    }
}
