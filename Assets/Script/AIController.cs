using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class AIController : MonoBehaviour
{
    //References
    [SerializeField] private CharacterStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    
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
        AIModel = Instantiate(AIStat.CharacterModel, transform);
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
            if(checkIfPlayerSeen()) AttackPlayer();
        }
        if (playerDistance < chaseRange) {
            chasePlayer();
        }

    }
    private bool checkIfPlayerSeen() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, attackRange))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }
    //Counter variables
    private float lastBulletTime;
    private float sprintCD;
    private float playerDistance;
    void AttackPlayer()
    {
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
    void chasePlayer(){
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
