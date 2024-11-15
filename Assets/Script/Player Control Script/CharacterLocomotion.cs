using System;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    CharacterController Controller;
    [SerializeField] private float jumpForce;
    [SerializeField] private float Gravity = -9.81f;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float sprintSpeedMultiplier; 
    private Vector3 Velocity = Vector3.zero;
    private float groundTimer;
    Vector2 input;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        updateIsSprinting();
        checkJump();
        
    }

    private void checkJump()
    {
        bool isGrounded = Controller.isGrounded;
        if(isGrounded)
            groundTimer = 0.2f; //Allows for jumping to occur even coming down ramps and stairs
        if(groundTimer>0) 
            groundTimer -= Time.deltaTime;  //Updating groundTimer
        if(isGrounded && Velocity.y<0) {
            Velocity.y = 0 ; //Set falling speed to 0 if touched ground
        }
        Velocity.y -= -2f*Gravity*Time.deltaTime; //Always applying gravity to make sure character moves down when going down a ramp or something
        if(isGrounded && Input.GetKey(KeyCode.Space)){ //If jump key is pressed and is grounded then we apply up force
            if(groundTimer > 0){ //Must recently be grounded to jump
                groundTimer = 0; //Set groundTimer to 0 so we can not jump while in air
                Velocity.y = jumpForce;
            }
        } 

        Controller.Move(Velocity * Time.deltaTime); //Move character based on the vectors described
    }

    int isSprintingParam = Animator.StringToHash("IsSprinting");
    int sprintMultiplierParam = Animator.StringToHash("sprintMultiplier");
    void updateIsSprinting(){
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool(isSprintingParam, isSprinting);
        if(isSprinting){
            animator.SetFloat(sprintMultiplierParam, sprintSpeedMultiplier);
        }else{
            animator.SetFloat(sprintMultiplierParam, normalSpeed);
        }
    }
}
