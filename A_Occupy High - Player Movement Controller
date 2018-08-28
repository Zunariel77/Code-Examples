using UnityEngine;


public class Player_Movement_Controller : Photon.MonoBehaviour
{
    // The following script controls the player character 
    // and triggers the animations. Because Occupy High is 
    // also playable in online multiplayer it uses Photon-functions aswell

    // with this script we have an idle animation when not moving, 
    // we walk when pushing left or right
    // we sprint when pushing two times left or right
    // we can walk sidewards by pushing up or down and while sprinting we can slightly run in diagonale lines
    // we can jump by pushing the jump button
    // we can block and we can roll when we push block and a direction-button at the same time
    // this script also has a function which can throw the char to the ground or through the air for example when getting hit
    // and it can make it stand up when pushing a direction button or jump when lying on the ground


    public float speed; // movement on the x-axis (not Unity axises, but Controller-Input axises)
    public float directionSpeed; // movement on the y-axis 
    public float xPos; // changed by controller-input
    public float yPos; // changed by controller-input
    public float speedMod; 
    public float performing; 
    public float comboCounter; 
    public float impactUp; // is used when character is thrown up into the air
    public float impactBack; // is used when character is thrown back through the air
    public bool frontHit; // is true when char is hit from the front
    public bool Death; // is true when char is dead
    public int JumpInt; 

    public int playerNumber; 
    public int attackInt; 
    public int comboInt;

    public int facing; // is 1 when facing right and -1 when facing left
    public bool down; // is true when the char lies on the ground
    public bool controlDeactive; // is used to turn of control ingame

    //Jump

    private CharacterController controller; 
    private Fighter_Stats_Script fSS;   

    public float verticalVelocity; // brings the char up when greater 0 (for example when jumping)
    public float gravity; // drags the char down once up in the air (consists of base- plus modGravity)
    public float baseGravity; 
    public float modGravity;
    public float jumpForce; // with how much force the char jumps (the higher the number, the higher the jump)
    public float deploymentHeight;
    public int landInt; // activates when landing so landing animation can be triggered for example
    public int sprintInt; // 0 = standing, 1 = walking, 2 = running

    private bool deployed;
    private Animator anim;
    private Rigidbody rb;
    private CapsuleCollider cc;
    public Vector3 moveVector; // is used for throwing the char through the air
    private float jumpRecovery; // refers to the 0.5 seconds when after landing when you can do a "quick-jump"

    public bool running; // is true when running
    private float runningCounter; // supports the sprinting function
    private float cHeight; // is used to change collider capsule height
    private float cRadius; // is used to change collider capsule radius
    private Vector3 cVector; // is used to change collider capsule center
    public bool isHit; 

    private void Start()
    {
        fSS = GetComponent<Fighter_Stats_Script>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        controller = GetComponent<CharacterController>();

        if(transform.rotation.y == 0) 
        {
            facing = 1;
        }
        else
        {
            facing = -1;
        }

        cHeight = controller.height;
        cVector = controller.center;
        cRadius = controller.radius;

        
    }

    

    private void LateUpdate()
    {
        if (isHit == true)
        {
            isHit = false;
        }
    }

    void Update()
    {
        if (down == false) // here we decrease the size of the capsule collider when the char lies on the ground
        {
            controller.height = cHeight;
            controller.center = cVector;
            controller.radius = cRadius;
        }
        else
        {
            controller.radius = 0.3f;
            controller.height = 0.24f;
            controller.center = new Vector3(0, 0.25f, 0.09f);
        }

        if (facing == 1 && transform.rotation != Quaternion.Euler(0, 0, 0)) // here we change the facing direction of the char once "facing" changes
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (facing == -1 && transform.rotation != Quaternion.Euler(0, 180, 0))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (!photonView.isMine) 
        {
            
            
            return;

            Debug.Log("Hallo!!");
        }
        


        gravity = baseGravity + modGravity;

        if(Death == true) // turns off controller functions when the character has 0 health
        {
            if(controller.enabled == true && controller.isGrounded)
            {
                controller.enabled = false;
            }
            
            performing = 2;
            down = true;
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector.y = verticalVelocity;
            if (verticalVelocity < -100) verticalVelocity = -100;
        }
        else
        {
            if (controller.enabled == false)
            {
                controller.enabled = true;
            }
        }

        if (controller.isGrounded) // is true when the char is on the ground and not flying through the air
        {
            anim.SetBool("isGrounded", true);

        }
        else
        {
            performing -= 1 * Time.deltaTime;
            if (performing < 0)
            {
                performing = 0;
                attackInt = 0;
                comboInt = 0;

                comboCounter = 0;
            }
            anim.SetBool("isGrounded", false);
        }

        if (controller.isGrounded)
        {
            if (anim.GetBool("isJumping") == true)
            {
                anim.SetBool("isJumping", false);
                jumpRecovery = 0.5f;
            }
            if(anim.GetBool("isQuickJumping") == true) 
            {
                anim.SetBool("isQuickJumping", false);               
            }

        }

        if (jumpRecovery > 0)
        {
            jumpRecovery -= 1 * Time.deltaTime;
        }

        if (anim.GetBool("Blocking") == true) 
        {
            fSS.isDefending = true;
        }
        else
        {
            fSS.isDefending = false;
        }

        

        if (runningCounter > 0)
        {
            runningCounter -= 1 * Time.deltaTime;
        }


        if (controller.isGrounded)
        {
            if (landInt != 0)
            {

                
                landInt = 0;
                verticalVelocity = 0;
            }

            
            if (fSS.AILevel == 0)
            {
                moveVector = Vector3.zero;
                yPos = 0;
                xPos = 0;
            }
            

            

            if (down == false)
            {
                

                if (performing <= 0) // actions can only be done when the char isn't performing something at the moment
                {
                    if (controlDeactive == false)
                    {


                        if (Input.GetAxis("VerticalP" + playerNumber) != 0 && fSS.AILevel == 0) // here we can walk up or down on the field when pushing Up or Down
                        {
                            float tempFloat = Input.GetAxis("VerticalP" + playerNumber);

                            if (facing == -1)
                            {
                                yPos = -tempFloat;
                            }
                            else
                            {
                                yPos = tempFloat;
                            }
                        }

                        

                        if (Input.GetAxis("HorizontalP" + playerNumber) != 0 && fSS.AILevel == 0)  // here we change our facing direction when pushing Right or Left
                        {
                            float tempFloat = Input.GetAxis("HorizontalP" + playerNumber);
                            
                            xPos = tempFloat;

                            if (running == false && runningCounter <= 0 && sprintInt == 0) // when we push 2 times Left or Right the char sprints
                            {
                                
                                running = true;
                                sprintInt = 1;
                            }
                            else
                            {
                                if (running == false && runningCounter > 0)
                                {
                                    
                                    running = true;
                                    sprintInt = 2;
                                }
                            }
                            
                        }
                        else
                        {                       
                            if (running == true && fSS.AILevel == 0)
                            {
                                running = false;
                                
                                if(anim.GetFloat("WalkSpeed") > 1.5f)
                                {
                                    runningCounter = 0;
                                }
                                else
                                {
                                    runningCounter = 1f;
                                }
                                              
                            }                            
                        }
                        
                        

                        if(Input.GetAxis("HorizontalP" + playerNumber) == 0 && fSS.AILevel == 0) 
                        {
                                                       
                            if (sprintInt >= 1 && speed == 0.0f)
                            {
                                sprintInt = 0;
                            }                                   
                            
                        }

                        if (xPos != 0)
                        {
                            if(sprintInt <= 1)
                            {
                                speed = 1;
                            }
                            else
                            {
                                if (sprintInt == 2)
                                {
                                    speed = 2;
                                }
                            }
                            
                        }
                        else
                        {
                            
                            speed = 0;


                            if (anim.GetFloat("WalkSpeed") < 0.1 && anim.GetFloat("WalkSpeed") > -0.1)
                            {
                                anim.SetFloat("WalkSpeed", 0);
                            }
                            
                        }

                        if (yPos == 0 && fSS.AILevel == 0) // we want the char while moving down to become slower and then move up when the player pushes Up
                        {
                            if (anim.GetFloat("WalkDirection") < 0.1 && anim.GetFloat("WalkDirection") > -0.1)
                            {
                                anim.SetFloat("WalkDirection", 0);
                            }
                        }

                        if (xPos != 0 || yPos != 0)
                        {

                            anim.SetBool("Moving", true);
                        }
                        else
                        {

                            anim.SetBool("Moving", false);

                        }

                        //=================

                        speedMod = 0;

                        if (anim.GetFloat("WalkSpeed") != speed) 
                        {
                            float tempFloat = anim.GetFloat("WalkSpeed");

                            if (speed > tempFloat)
                            {
                                speedMod = 4f;

                            }
                            else
                            {
                                if (speed < tempFloat)
                                {
                                    speedMod = -4f;

                                }
                            }

                            //Right
                            if (speed > 0 && xPos > 0 && transform.rotation != Quaternion.Euler(0, 0, 0))
                            {
                                if(anim.GetFloat("WalkSpeed") < 3)
                                {
                                    if (anim.GetFloat("WalkSpeed") > 0.1f)
                                    {
                                        speedMod = -4f;
                                    }
                                    else
                                    {
                                        transform.rotation = Quaternion.Euler(0, 0, 0);
                                        facing = 1;
                                        sprintInt = 0;
                                        anim.SetFloat("WalkDirection", 0);
                                        directionSpeed = 0;
                                    }
                                }
                                

                                
                            }
                            else
                            {
                                //Left
                                if (speed > 0 && xPos < 0 && transform.rotation != Quaternion.Euler(0, 180, 0))
                                {
                                    if (anim.GetFloat("WalkSpeed") < 3f)
                                    {
                                        if (anim.GetFloat("WalkSpeed") > 0.1f)
                                        {
                                            speedMod = -4f;
                                        }
                                        else
                                        {
                                            transform.rotation = Quaternion.Euler(0, 180, 0);
                                            facing = -1;
                                            sprintInt = 0;
                                            anim.SetFloat("WalkDirection", 0);
                                            directionSpeed = 0;
                                        }
                                    }
                                    


                                }

                            }



                            tempFloat += speedMod * Time.deltaTime;



                            anim.SetFloat("WalkSpeed", tempFloat);
                        }


                        if (yPos != 0)
                        {
                            if (directionSpeed < yPos)
                            {
                                directionSpeed += 4 * Time.deltaTime;
                            }
                            else
                            {
                                if (directionSpeed > yPos)
                                {
                                    directionSpeed -= 4 * Time.deltaTime;
                                }
                            }
                        }
                        else
                        {
                            directionSpeed = 0;
                        }


                        if (anim.GetFloat("WalkDirection") != directionSpeed)
                        {

                            anim.SetFloat("WalkDirection", directionSpeed);
                        }

                        // Jump

                        if (fSS.AILevel == 0)
                        {
                            verticalVelocity = -gravity * Time.deltaTime;

                            if (verticalVelocity < 0)
                            {
                                verticalVelocity = 0;
                            }
                            if (Input.GetButtonDown("JumpP" + playerNumber) && anim.GetBool("isJumping") == false && jumpRecovery <= 0)
                            {
                                if (speed <= 1.3)
                                {
                                    HighJump();
                                }
                                else
                                {
                                    if (speed > 1.3f)
                                    {
                                        QuickJump();
                                    }
                                }
                            }
                            else
                            {
                                if (Input.GetButtonDown("JumpP" + playerNumber) && anim.GetBool("isJumping") == false && jumpRecovery > 0.15 && fSS.AILevel == 0)
                                {
                                    QuickJump();
                                }
                                else
                                {
                                    if (Input.GetButton("BlockP" + playerNumber) && fSS.AILevel == 0)
                                    {
                                        if (anim.GetBool("Blocking") == false)
                                        {
                                            anim.SetBool("Blocking", true);

                                        }
                                    }
                                    else
                                    {
                                        if (anim.GetBool("Blocking") == true)
                                        {
                                            anim.SetBool("Blocking", false);

                                        }
                                    }

                                }
                            }
                        }
                        else // we differentiate between a "High Jump" which brings the char up in the air and a "Quick Jump" which is a long low jump
                        {
                            if(JumpInt == 1) // we want a high jump when simply jumping
                            {
                                
                                HighJump();
                            }
                            else
                            {
                                if (JumpInt == 2) // and we want a quick jump when sprinting or shortly after landing a high jump
                                {
                                    QuickJump();
                                }
                            }
                            JumpInt = 0;
                        }                       
                    }
                    else
                    {
                        verticalVelocity = -gravity * Time.deltaTime;                       
                    }                                
                }
                else
                {
                    if(Death == false) // when a char is dead, certain functions get turned off
                    {
                        performing -= 1 * Time.deltaTime;
                        if (performing < 0)
                        {
                            performing = 0;
                            attackInt = 0;
                            comboInt = 0;

                            comboCounter = 0;
                        }

                    }



                }

                

            }
            else
            {
                if(performing > 0 && Death == false)
                {
                    performing -= 1 * Time.deltaTime;
                    if (performing <= 0)
                    {
                        performing = 0;
                        attackInt = 0;
                        comboInt = 0;

                        comboCounter = 0;
                    }
                }
                else
                {
                    StandingUp();

                    
                }

                
            }

            
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
            
            if(Input.GetButtonDown("JumpP" + playerNumber) && down == true && fSS.AILevel == 0)
            {
                AirSave(); // an "Air Save" can be executed when the char gets thrown through the air and the player pushes "Jump". 
                // that way the char performs a dive roll or a flip mid air to land on his feet again
            }
        }

        if (impactUp > 0 || impactBack > 0)
        {
            moveVector = Vector3.zero;
            yPos = 0;
            xPos = 0;        


            Falling(impactUp, impactBack, frontHit);
            impactUp = 0;
            impactBack = 0;
           
        }


        moveVector.y = verticalVelocity;


        controller.Move(moveVector * Time.deltaTime);

        if(fSS.currentHealth <= 0)
        {
            Death = true;
        }
    }

    public void HighJump()
    {
        
        verticalVelocity = jumpForce;
        anim.SetTrigger("Jump");
        anim.SetBool("isJumping", true);

        moveVector.x = (yPos * -3f) * facing;
        moveVector.z = (xPos * 5f);

        landInt = 1;
    }

    public void StandingUp()
    {
        if (controlDeactive == false)
        {
            if (controller.isGrounded && fSS.AILevel == 0 && fSS.currentHealth > 0)
            {
                if (Input.GetButtonDown("JumpP" + playerNumber) )
                {
                    anim.SetTrigger("StandUp");
                }
                else
                {
                    if (Input.GetAxis("HorizontalP" + playerNumber) != 0 && Input.GetAxis("VerticalP" + playerNumber) == 0)
                    {
                        photonView.RPC("UpdateStands", PhotonTargets.All);
                        anim.SetTrigger("FrwdRoll");
                    }
                    else
                    {
                        if ((facing == 1 && Input.GetAxis("VerticalP" + playerNumber) > 0) || (facing == -1 && Input.GetAxis("VerticalP" + playerNumber) < 0))
                        {
                            photonView.RPC("UpdateStands", PhotonTargets.All);                            
                            anim.SetTrigger("RollLeft");
                        }
                        else
                        {
                            if ((facing == -1 && Input.GetAxis("VerticalP" + playerNumber) > 0) || (facing == 1 && Input.GetAxis("VerticalP" + playerNumber) < 0))
                            {
                                photonView.RPC("UpdateStands", PhotonTargets.All);
                                anim.SetTrigger("RollRight");
                            }
                        }
                    }
                }
            }

                
        }
        else
        {
            if (fSS.currentHealth > 0)
            {
                photonView.RPC("UpdateStands", PhotonTargets.All);
                anim.SetTrigger("StandUp");
            }

        }
    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(down);
            stream.SendNext(facing);
        }
        else
        {
            down = (bool)stream.ReceiveNext();
            facing = (int)stream.ReceiveNext();
        }
    }


    [PunRPC]
    public void UpdateStands()
    {
        down = false;
    }

    [PunRPC]
    public void UpdateFalls()
    {
        down = true;
    }

    public void QuickJump()
    {
        verticalVelocity = jumpForce * 0.7f;
        anim.SetTrigger("Jump");
        anim.SetBool("isQuickJumping", true);
        sprintInt = 0;

        moveVector.x = (yPos * -3f) * facing;
        moveVector.z = (xPos * 10f);

        landInt = 1;
    }

    public void Falling(float impactUp, float impactBack, bool frontHitSent) 
    {
        // the function gets fueled with 2 floats which determine the impact of the hit and the bool informs us about the direction (front or back)

        float tempFloat = 0;

        if (impactBack > 25) impactBack = 25;
        if (impactUp > 25) impactUp = 25;

        performing = ((impactBack + impactUp) * 0.03f) + 0.3f;
        

        if (controller.isGrounded == true)
        {
            tempFloat = impactUp * 0.7f;
        }
        else
        {
            tempFloat = impactUp * 0.1f;
        }
        
        verticalVelocity += impactUp * 0.7f;

        if (frontHit == true)
        {
            photonView.RPC("UpdateFalls", PhotonTargets.All);
            anim.SetTrigger("HitBackwards");           
        }
        else
        {
            photonView.RPC("UpdateFalls", PhotonTargets.All);
            anim.SetTrigger("HitFront");
            impactBack *= -1.5f;           
        }
        
        moveVector.x = 0;
        moveVector.z = (impactBack * -1f) * facing;

        landInt = 1;
    }

    public void AirSave()
    {
        down = true;
        anim.SetTrigger("AirSaveRoll");
        performing = 0.5f;
    }


    public void StandUp()
    {
        if(down == true)
        {
            down = false;
            
        }
    }

    public void Turn()
    {
        

        if(facing == 1)
        {
            Debug.Log("Turn 0");
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if(facing == -1)
            {
                Debug.Log("Turn 180");
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            
        }
        
    }
        
}

