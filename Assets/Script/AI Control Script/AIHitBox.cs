using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitBox : MonoBehaviour
{
    public AIHealth health;
    public void OnRaycastHit(RaycastWeapon Weapon) {
        health.TakeDamage(Weapon.dmg);
    }
    
}
