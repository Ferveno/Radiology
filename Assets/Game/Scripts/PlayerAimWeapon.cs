using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;
using System;
public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
        public Vector3 shellPosition;
    }


    private Transform aimTransform;
    private Transform gunEndPointTransform;
    private Transform aimShellPositionTranform;
    private Animator aimAnimator;
    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
        gunEndPointTransform = aimTransform.Find("GunEndPointPosition");
    }

    private void Update()
    {
        //HandleAiming();
        //HandlShooting();
    }


    public void HandleAiming(Vector3 position)
    {
        Vector3 mousePosition;
        if (position == Vector3.zero)
        {
            mousePosition = UtilsClass.GetMouseWorldPosition();
        }
        else
        {
            mousePosition = position;
        }
        //Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 a = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            a.y = -1f;
        }
        else
        {
            a.y = +1f;
        }

        aimTransform.localScale = a;
    }

    private void HandlShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

            //aimAnimator.SetTrigger("Shoot");
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = gunEndPointTransform.position,
                shootPosition = mousePosition,
            });
        }
    }

   
}
