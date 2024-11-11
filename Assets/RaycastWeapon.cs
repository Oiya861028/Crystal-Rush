using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public Transform RaycastOrigin;
    public Transform RaycastDestination;
    public bool isFiring = false;
    Ray ray;
    RaycastHit hitInfo;
    public void StartFiring() {
        ray.origin = RaycastOrigin.position;
        ray.direction = RaycastDestination.position - RaycastOrigin.position;
        if(Physics.Raycast(ray, out hitInfo)) {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
        }

        isFiring = true;
        muzzleFlash.Emit(1);
    }
    public void StopFiring() {
        isFiring = false;
    }
}
