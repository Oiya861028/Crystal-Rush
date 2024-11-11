using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;

    }
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer TraceEffect;
    public Transform RaycastOrigin;
    public Transform RaycastDestination;
    public bool isFiring = false;
    public int fireRate = 30;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float maxLifeTime = 3.0f;
    float timeSincegunFired;
    Ray ray;
    RaycastHit hitInfo;
    List<Bullet> bullets = new List<Bullet>();

    Vector3 getPosition(Bullet bullet){
        //x = vit+1/2gt^2;
        Vector3 gravity = Vector3.down * bulletDrop;
        return bullet.initialPosition + bullet.initialVelocity*bullet.time + 0.5f*gravity*bullet.time*bullet.time;
    }
    Bullet CreateBullet(Vector3 position, Vector3 velocity){
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(TraceEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }
    
    public void StartFiring()
    {
        muzzleFlash.Emit(1);
        isFiring = true;
        timeSincegunFired = 0f;
        FireBullet();

    }
    public void UpdateFiring(float deltaTime) {
        timeSincegunFired += deltaTime;
        float fireInterval = 1.0f / fireRate;
        while(timeSincegunFired >= 0.0f) {
            FireBullet();
            timeSincegunFired -= fireInterval;
        }
    }
    public void UpdateBullet(float deltaTime) {
        simulateBullet(deltaTime);
        DestroyBullets();
    }
    private void simulateBullet(float deltaTime) {
        bullets.ForEach(bullet => {
            Vector3 p0 = getPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = getPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    private void DestroyBullets(){
        bullets.RemoveAll(bullet => bullet.time>=maxLifeTime);
    }
    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end-start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
        } else{
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {   
        
        Vector3 velocity = (RaycastDestination.position - RaycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(RaycastOrigin.position, velocity);
        bullets.Add(bullet);
       
    }

    public void StopFiring() {
        isFiring = false;
    }
}
