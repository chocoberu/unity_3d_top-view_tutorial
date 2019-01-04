using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    void Start()
    {
        if (startingGun != null)
            EquipGun(startingGun);
    }
    public void EquipGun(Gun gunToEquip)
    {
        if(equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip,weaponHold.transform.position,weaponHold.transform.rotation) as Gun;
        equippedGun.transform.parent = weaponHold; // 총 오브젝트가 플레이어를 따라 움직이도록 weaponHold의 자식으로 넣어야함
    }
    public void Shoot()
    {
        if (equippedGun != null)
            equippedGun.Shoot();
    }
}
