using System;
using System.Linq;
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
    private float lastBulletTime;
    private float playerDistance;

    // Start is called before the first frame update
    void Start()
    {
        GameObject AIModel = Instantiate(AIStat.CharacterModel, transform);
        //Get Weapon_Attach_Point
        Transform Weapon_Attach_Point = AIStat.getWeaponAttachPoint();
        GameObject weapon = Instantiate(WeaponStat.WeaponModel, Weapon_Attach_Point.position, Weapon_Attach_Point.rotation);
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
    }
    
}
