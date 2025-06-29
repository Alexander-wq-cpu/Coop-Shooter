using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; private set; }

    public int totalAKMAmmo = 200;
    public int totalShotgunAmmo = 200;

    [SerializeField]private TextMeshProUGUI ammoUI;
    public Weapon activeWeapon;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        ShowAmmo();
    }

    private void ShowAmmo()
    {
        if (activeWeapon.weaponModel == Weapon.WeaponModel.AKM)
            ammoUI.SetText($"{activeWeapon.bulletsLeft}/{totalAKMAmmo}");

        if (activeWeapon.weaponModel == Weapon.WeaponModel.Shotgun)
            ammoUI.SetText($"{activeWeapon.bulletsLeft}/{totalShotgunAmmo}");
    }

}
