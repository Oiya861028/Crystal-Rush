using UnityEngine;
using UnityEngine.Animations.Rigging;
public class CharacterAiming : MonoBehaviour
{
    public float turningSpeed = 15f;
    Camera mainCamera;
    public Rig aimLayer;
    public float aimDuration = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), turningSpeed * Time.fixedDeltaTime);
    }
    private void Update() { 
        if(Input.GetMouseButton(1)){
            aimLayer.weight += Time.deltaTime/aimDuration;
        }
        else {
            aimLayer.weight -= Time.deltaTime/aimDuration;
        }
    }
}
