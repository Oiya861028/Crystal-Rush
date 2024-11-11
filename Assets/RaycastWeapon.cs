using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer TraceEffect;
    public Transform RaycastOrigin;
    public Transform RaycastDestination;
    public bool isFiring = false;
    public float fireRate = .05f;
    float timeSincegunFired;
    Ray ray;
    RaycastHit hitInfo;
    public void StartFiring() {
        if(timeSincegunFired>=fireRate){
            ray.origin = RaycastOrigin.position;
            ray.direction = RaycastDestination.position - RaycastOrigin.position;
            var tracer = Instantiate(TraceEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);
            if(Physics.Raycast(ray, out hitInfo)) {
                //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
                tracer.transform.position = hitInfo.point;
            }
            timeSincegunFired=0;
            isFiring = true;
            muzzleFlash.Emit(1);
        }
        else{
            timeSincegunFired+=Time.deltaTime;
        }

        
    }
    public void StopFiring() {
        isFiring = false;
    }
}
