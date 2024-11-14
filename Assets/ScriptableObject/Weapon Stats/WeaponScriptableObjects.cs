using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponStat",menuName = "ScriptableObject/Weapon")]
public class Weapon:ScriptableObject {
    public float damage = 10f;
    public float reloadTime = 2f;
    public AudioClip attackSound;
    public float bulletSpeed = 5f;
    public GameObject WeaponModel;
    public Projectile projectileModel;
    
    

}