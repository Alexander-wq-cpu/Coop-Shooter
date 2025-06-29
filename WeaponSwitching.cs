using System;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

  
    void Update()
    {
        int previousWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeapon = 2;

        if (previousWeapon != selectedWeapon)
            SelectWeapon();
    }
    private void SelectWeapon()
    {
        int weaponIndex = 0;
        foreach(Transform weapon in transform)
        {
            if (weaponIndex == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                AmmoManager.Instance.activeWeapon = weapon.GetComponent<Weapon>();
            }
            else
                weapon.gameObject.SetActive(false);

            weaponIndex++;
        }
    }

}
