using System;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
//This script has been outdated, AIController will be broken down into their respective components
//More specifically, this script will be broken into AISpawner and AIBehaviors, which the second is attached to each individual AI
public class AIController : MonoBehaviour
{
    //References
    [SerializeField] private BasicStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    
    [SerializeField] private Transform player; //player location
    
    [SerializeField] private float attackRange = 5;
    [SerializeField] private float chaseRange = 10;
    [SerializeField] private Weapon WeaponStat;
    [Space]
    [SerializeField] private bool LeftHand = true; //Which hand to attach weapon to

    GameObject AIModel;
    // Start is called before the first frame update
    void Start()
    {
        AIModel = Instantiate(AIStat.Model, transform.position, quaternion.identity);
        //Get Weapon_Attach_Point
        GameObject Weapon = Instantiate(WeaponStat.WeaponModel, getWeaponAttachPoint(LeftHand).transform);
        lastBulletTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance < attackRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, attackRange))
            {
                if (hit.transform == player)
                {
                    AttackPlayer();
                }
            }
        }
        else if (playerDistance < chaseRange) {
            ChasePlayer();
        }

    }
    
    //Counter variables
    private float lastBulletTime;
    private float sprintCD;
    private float playerDistance;
    void AttackPlayer()
    {
        Debug.Log(Time.time);
        transform.LookAt(player);
        if(Time.deltaTime-lastBulletTime > WeaponStat.reloadTime){
            GameObject projectile = Instantiate(WeaponStat.projectileModel.model, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = (player.position - transform.position).normalized * WeaponStat.bulletSpeed;

            Destroy(projectile, 2f);  // Destroy the projectile after 2 seconds
        }
        else{
            lastBulletTime+= Time.deltaTime;
        }
    }
    void ChasePlayer(){
        //move toward player
        
        transform.position = Vector3.MoveTowards(transform.position, player.position, AIStat.Speed * Time.deltaTime);
        transform.LookAt(player);
    }
    GameObject getWeaponAttachPoint(bool isLeftHand){
        if(isLeftHand){
            return RecursiveFindChild(AIModel, "LeftHand");
        }
        else {
            return RecursiveFindChild(AIModel, "RightHand");
        }
    }
    GameObject RecursiveFindChild(GameObject parent, string childName) {
        foreach (Transform child in parent.transform)
        {
            if(child.name == childName)
            {
                return child.gameObject;
            }
            else
            {
                GameObject found = RecursiveFindChild(child.gameObject, childName);
                if (found != null)
                {
                        return found;
                }
            }
        }
        return null;
    }
}
