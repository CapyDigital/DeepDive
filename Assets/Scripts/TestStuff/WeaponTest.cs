using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTest : MonoBehaviour
{
    public void Shoot()
    {
        Debug.Log("Shot weapon");
    }

    public void GrabWeapon()
    {
        Debug.Log("Grabbed weapon");
    }

    public void DropWeapon()
    {
        Debug.Log("Dropped weapon");
    }
}
