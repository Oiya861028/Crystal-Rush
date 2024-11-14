using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    public void OnRaycastHit(RaycastWeapon Weapon, Vector3 direction) {
        health.TakeDamage(Weapon.dmg);
    }
    
}
