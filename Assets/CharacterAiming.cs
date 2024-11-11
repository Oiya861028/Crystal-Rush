using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CharacterAiming : MonoBehaviour
{
    public float turningSpeed = 15f;
    public Rig aimLayer;
    public float aimDuration = 0.2f;
    Camera mainCamera;
    RaycastWeapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<RaycastWeapon>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), turningSpeed * Time.fixedDeltaTime);
    }
    private void LateUpdate() { 
        if(aimLayer){
            if(Input.GetButton("Fire2")) {
                aimLayer.weight += Time.deltaTime/aimDuration;
            }
            else {
                aimLayer.weight -= Time.deltaTime/aimDuration;
            }
        }
        
        if(Input.GetButton("Fire1")) {
            weapon.StartFiring();
        }
        if(weapon.isFiring) {
            weapon.UpdateFiring(Time.deltaTime);
        }
        weapon.UpdateBullet(Time.deltaTime);
        
        if(Input.GetButtonUp("Fire1")){
            weapon.StopFiring();
        }
    }
}
