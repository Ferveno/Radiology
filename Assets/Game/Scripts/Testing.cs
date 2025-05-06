using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon playerAimWeapon;

    // Start is called before the first frame update
    void Start()
    {
        playerAimWeapon.OnShoot += PlayerAimWeapon_OnShoot;
    }

    private void PlayerAimWeapon_OnShoot(object sender,PlayerAimWeapon.OnShootEventArgs e)
    {
        //Vector3 shootDir = e.shootPosition - e.gunEndPointPosition.normalized;
        //UtilsClass.ApplyRotationToVector(shootDir, 90f);
    }

}
