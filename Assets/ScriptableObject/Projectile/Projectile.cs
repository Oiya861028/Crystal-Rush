using System;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ProjectileStat", menuName = "ScriptableObject/Projectile")]
public class Projectile:ScriptableObject{

    public GameObject model;
    public Material hitEffect;
    public float baseDamage = 10f;
    public float projectileSpeed = 100f;
    public String projectileName;
}