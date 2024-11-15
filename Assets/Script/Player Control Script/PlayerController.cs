using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    //Inputs 
    private Vector3 PlayerMovementInputs;
    private Vector2 PlayerMouseInputs;
    private Vector3 Velocity;
    private Vector3 RootMotion;
    private float xRot; 


    //Object References
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController Controller;
    [SerializeField] Animator animator;
    [Space]
    //Variables for Player
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpDamp;
    
    private float groundTimer;
    [SerializeField] private float sensitivity;
    [SerializeField] private float Gravity = -9.81f;
    Boolean isJumping;
    [Space]
    //Assesories
    [SerializeField] private Weapon weapon; 
    
    void OnEnable() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        PlayerMovementInputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //Get Movement Input
        PlayerMouseInputs = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Get Mouse Input
        animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
        animator.SetFloat("InputY", Input.GetAxis("Vertical"));
        //Move Jumping and Camera
        movePlayer();
        //moveCamera();
        updateIsSprinting();
    }
    private void OnAnimatorMove() {
        RootMotion += animator.deltaPosition;
    }
    void FixedUpdate() {
        Controller.Move(RootMotion*speed);
        RootMotion = Vector3.zero;
    }
    private void movePlayer() {
        Vector3 MoveVector = Vector3.zero;
        //Control walking and running
        // float currentSpeed = speed;
        // float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
        // currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, transitionSpeed * Time.deltaTime);
        // MoveVector *= currentSpeed;
        //Control Jumping
        Boolean isGrounded = Controller.isGrounded;
        if(isGrounded)
            groundTimer = 0.2f; //Allows for jumping to occur even coming down ramps and stairs
        if(groundTimer>0) 
            groundTimer -= Time.deltaTime;  //Updating groundTimer
        if(isGrounded && Velocity.y<0) {
            Velocity.y = 0 ; //Set falling speed to 0 if falling has come in contact with ground
        }
        Velocity.y -= -2f*Gravity*Time.deltaTime; //Always applying gravity to make sure character moves down when going down a ramp or something
        if(isGrounded && Input.GetKey(KeyCode.Space)){ //If jump key is pressed and is grounded then we apply up force
            if(groundTimer > 0){ //Must recently be grounded to jump
                groundTimer = 0; //Set groundTimer to 0 so we can not jump while in air
                Velocity.y = jumpForce;
            }
        } 
        MoveVector.y = Velocity.y; //Combine movement and jump together

        Controller.Move(MoveVector * Time.deltaTime); //Move character based on the vectors described
        

    }
    int isSprintingParam = Animator.StringToHash("IsSprinting");
    void updateIsSprinting(){
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool(isSprintingParam, isSprinting);
    }
    // private void moveCamera() { 
    //     xRot-= PlayerMouseInputs.y * sensitivity; //xRot controls looking up and down 
    //     xRot = Mathf.Clamp(xRot,-90,90); //Limit the amount you can rotate to prevent looking into character
    //     transform.Rotate(0f, PlayerMouseInputs.x*sensitivity, 0f); //Rotate the camera and player sideways
    //     playerCamera.transform.localRotation = Quaternion.Euler(xRot,0,0); //Set the Up/down rotation
    // }
}
