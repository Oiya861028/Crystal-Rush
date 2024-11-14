using System;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ProjectileStat", menuName = "ScriptableObject/Projectile")]
public class Projectile:ScriptableObject{

    public GameObject model;
    public float baseDamage = 10;
    public String projectileName;
}