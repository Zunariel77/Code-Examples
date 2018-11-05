using UnityEngine;

public class AIScript : Photon.MonoBehaviour {

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // The following script is used in case AI-players are active in the game. 
    // What the script does is scanning the battleground for the next nearby enemy,
    // targeting this enemy and then making a decision about the strategy to engage with this enemy.
    // Each character has for that purpose an individual AI-script with different preset strategies.
    // (Also dependant on the difficuluty level)

    // This is a basic script for testing purposes and not the final version.
    // It already allows us to fight against the AI and experience a challenge especially on a higher AI-Level.
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    private GameObject parentObj; //The Character Object
    private SphereCollider AIcol; //The Collider-Component used for scanning the environment
    private Player_Movement_Controller pControl; //The Controller-Component
    private Fighter_Stats_Script fSS; //The reference to the stats of the character (health, energy, teamnumber, etc.)
    private Animator anim; //reference to the animator
    private CharacterController controller; //reference to the character-controller-component
    private Fighter_Battle_Script fBS; //reference to the script which stores all the attacks and skills the character can execute



    private float AIRange = 50; //The maximum range the collider scans for enemy-targets
    private float AIchance; //The "reaction time" (lower the higher the difficulty level)
    [SerializeField]
    private float AITimer; //A timer which is set up once a decision was made and then ticks down to zero. At zero an action gets executed
    private int decisionInt; //The decision-integer determines the action which will finally be executed
    private float moveCounter; //An additional counter which ticks down per second and makes sure that no further decisions are made as long as it is >0
    private float downCounter; //A counter which gets set up once the character falls to the ground. When it reaches 0 the character stands up again.

    private int teamInt; //The number of the team. Potential targets are only fighters which belong to other teams.
    [SerializeField]
    private float targDistY; //Actually the X-position of the target. It is Y and not X because we pretend that the battleground is a 2D-floor looked at from above to simplify it.
    [SerializeField]
    private float targDistX; //Actually the Z-position of the target.
    public GameObject target; //The object of the targetted enemy-fighter gets stored here.
    
    [SerializeField]
    private float yPos; //Actually the X position of this character
    [SerializeField]
    private float xPos; //Actually the Z position of this character

    public float actRange = 2.5f; 

    private void Start()
    {
        parentObj = transform.parent.gameObject;

        AIcol = GetComponent<SphereCollider>();
        pControl = parentObj.GetComponent<Player_Movement_Controller>();
        fBS = parentObj.GetComponent<Fighter_Battle_Script>();
        fSS = parentObj.GetComponent<Fighter_Stats_Script>();
        anim = parentObj.GetComponent<Animator>();
        controller = parentObj.GetComponent<CharacterController>();

        teamInt = fSS.teamInt;

        if(fSS.AILevel == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // Because this game can also be played online it uses some PhotonNetwork-Coding
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        if (!fSS.photonView.isMine) return; //when the object isnt owned by the client, but by another client in the network the script does nothing

        if(controller.isGrounded) // The moveVector lets the character fly through the air (for example when it got striked heavily). When the character is on the ground, the vector is zero.
        {
            pControl.moveVector = Vector3.zero;
        }

        if (fSS.currentHealth <= 0) // This basically turns of the script when the character is defeated.
        {
            target = null;
            pControl.xPos = 0;
            pControl.yPos = 0;
            return;
        }

        if(pControl.down && downCounter == 0) // This orders the character to stand up after a certain time when he fell to the ground.
        {
            downCounter = 4;
        }
        else
        {
            if(downCounter > 0)
            {
                downCounter -= 1 * Time.deltaTime;
                if(downCounter<= 0.3f && fSS.currentHealth > 0)
                {
                    downCounter = 0;
                    pControl.down = false;
                    if(Random.Range(0,2) > 1)
                    {
                        anim.SetTrigger("FrwdRoll");
                        pControl.performing = 0.5f;
                    }
                    else
                    {
                        anim.SetTrigger("StandUp");
                        pControl.performing = 1f;
                    }
                    
                }
            }
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // The AI-character executes its decisions after a "reaction time" (AIchance) which is reduced by the difficulty level (AILevel)
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        AIchance = 2.2f - (fSS.AILevel * 0.20f);

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // AIcol is a simple collider which gets bigger until it reaches a maximum range before it gets set to 0.01 again. 
        // The moment it collides with a valid enemy target close to the character it sets the "target"-GameObject to this enemy character
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        AIcol.radius += 1f;
        if (AIcol.radius > AIRange) { AIcol.radius = 0.01f; }

        if (target != null && (target.GetComponent<Fighter_Stats_Script>().currentHealth <= 0 || target.GetComponent<Fighter_Stats_Script>().isInvu))
        {
            target = null;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // when there is a target the character tries get close to the same X-position (which from the top perspective is Y)
        // and then closer and closer to the target on the Z-position. 
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        if (target != null )
        {
            targDistY = transform.position.x - target.transform.position.x;
            targDistX = transform.position.z - target.transform.position.z;
        }

        if(target == null)
        {
            xPos = 0;
            yPos = 0;
            targDistX = 0;
            targDistY = 0;
            pControl.xPos = 0;
            pControl.yPos = 0;
            return;
        }

        if(moveCounter <= 0)
        {
            if (targDistY > 0.3f || targDistY < -0.3f)
            {

                yPos = targDistY;
                if (yPos > 1) yPos = 1;
                if (yPos < -1) yPos = -1;

                float tempFloat = yPos;

                if (pControl.facing == -1)
                {
                    yPos = -tempFloat;
                }
                else
                {
                    yPos = tempFloat;
                }

                pControl.yPos = yPos;

            }
            else
            {
                targDistY = 0;
                pControl.yPos = 0;
            }

            Turning();

            

            if (targDistX > actRange || targDistX < -actRange)
            {

                xPos = targDistX;
                if (xPos > 1) xPos = 1;
                if (xPos < -1) xPos = -1;

                pControl.xPos = -xPos;

            }
            else
            {
                targDistX = 0;
                pControl.xPos = 0;
            }

            // ============ Running =============

            if(targDistX >= 7 || targDistX <= -7)
            {
                if (pControl.speed > 0.5f)
                {
                    pControl.sprintInt = 2;
                    pControl.running = true;
                }
                else
                {
                    pControl.sprintInt = 0;
                    pControl.running = false;
                }
            }
            else
            
            //===================================

            if(xPos < 0.1f && xPos > -0.1f)
            {
                anim.SetBool("Moving", false);
            }
        }
        else
        {
            moveCounter -= 1 * Time.deltaTime;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        // Decision and Action!
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


        if (anim.GetBool("isGrounded") == true)
        {
            if (AITimer <= 0)
            {
                anim.SetBool("Blocking", false);
                if (pControl.performing <= 0.05f)
                {
                    if (decisionInt == 0)
                    {                        
                        DecisionMaking();
                    }
                    else
                    {
                        if (fSS.currentHealth > 0)
                        {                            
                            DoAction();
                        }

                    }
                }
                else
                {
                    if (pControl.performing > 0.05f && pControl.comboCounter >= pControl.performing)
                    {
                        ComboAction();
                    }
                }
            }
            else
            {
                AITimer -= 1 * Time.deltaTime;


            }
        }
        else
        {
            if(pControl.down == false)
            {
                JumpAttack();
            }
            
        }
        
        if (pControl.down)
        {
            yPos = 0;
            xPos = 0;
            pControl.speed = 0;
            
        }

    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // Decisions are based on the position of the character, the target and a random-roll to make AI characters more unpredictable. 
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void DecisionMaking()
    {
        if (fSS.currentHealth <= 0)
        {
            target = null;
            return;
        }


        float tempFloat;

        

        if (anim.GetBool("isGrounded"))
        {
            if (pControl.down == false)
            {
                // Close Range while not running

                if ((targDistY > 4 || targDistY < -4) && anim.GetFloat("WalkSpeed") < 1.1f && pControl.running == false && pControl.performing <= 0)
                {
                    tempFloat = Random.Range(0, 2);
                    if (tempFloat < 1)
                    {
                        pControl.JumpInt = 1;
                    }
                    else
                    {
                        pControl.JumpInt = 0;
                        Debug.Log("Rolling");
                        anim.SetBool("Blocking", true);
                        pControl.yPos = -targDistY;
                        if (pControl.yPos > 1) pControl.yPos = 1;
                        if (pControl.yPos < -1) pControl.yPos = -1;

                        pControl.xPos = 0;
                        moveCounter = 1;
                        pControl.performing = 0.8f;

                    }

                }
                else
                {
                    if (pControl.running == true && (targDistX < 5 || targDistX > -5))
                    {
                        // Close Range while running

                        if (targDistY > -0.4f && targDistY < 0.4f)
                        {
                            Debug.Log("Decision 2");
                            decisionInt = 2;
                            AICounter();

                        }
                        else
                        {
                            pControl.sprintInt = 0;
                            pControl.running = false;
                        }


                    }
                    else
                    {
                        // Mid Range while not running

                        if (pControl.running == false && (targDistX < 8 && targDistX > 4) || (targDistX > -8 && targDistX < -4))
                        {
                            // Midrange Combat
                            if (targDistY > -0.4f && targDistY < 0.4f)
                            {
                                tempFloat = Random.Range(0, 2);
                                if (tempFloat < 1)
                                {

                                    decisionInt = 3;
                                    AITimer = 0;
                                }
                                else
                                {
                                    decisionInt = 10;
                                    AICounter();
                                }
                            }
                            else
                            {
                                // Mid Range while target is not on the same Y

                                if (targDistY > -5f && targDistY < 5f)
                                {
                                    tempFloat = Random.Range(0, 2);
                                    if (tempFloat < 1)
                                    {
                                        decisionInt = 4;
                                        AICounter();
                                    }
                                    else
                                    {
                                        if (tempFloat < 2)
                                        {
                                            decisionInt = 11;
                                            AICounter();
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            // Close Range while not running

                            if (pControl.running == false && (targDistX < 2 || targDistX > -2))
                            {
                                

                                if (targDistY > -0.4f && targDistY < 0.4f)
                                {
                                    tempFloat = Random.Range(0, 3);
                                    if (tempFloat < 1)
                                    {

                                        decisionInt = 5;
                                        AICounter();

                                    }
                                    else
                                    {
                                        if (tempFloat < 2)
                                        {

                                            decisionInt = 6;
                                            AICounter();

                                        }
                                        else
                                        {
                                            if (tempFloat < 3)
                                            {

                                                decisionInt = 7;
                                                AICounter();

                                            }
                                        }
                                    }
                                }



                            }
                            else
                            {
                                // Mid-Close Range while not running

                                if (pControl.running == false && (targDistX < 4 || targDistX > -4))
                                {
                                    

                                    if (targDistY > -0.4f && targDistY < 0.4f)
                                    {
                                        tempFloat = Random.Range(0, 3);
                                        if (tempFloat < 1)
                                        {

                                            decisionInt = 8;
                                            AICounter();

                                        }
                                        else
                                        {
                                            if (tempFloat < 2)
                                            {

                                                decisionInt = 9;
                                                AICounter();

                                            }
                                            else
                                            {
                                                if (tempFloat < 3)
                                                {

                                                    decisionInt = 10;
                                                    AICounter();

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (targDistY > -4f && targDistY < 4f)
                                        {
                                            tempFloat = Random.Range(0, 2);
                                            if (tempFloat < 1)
                                            {
                                                decisionInt = 11;
                                                AICounter();
                                            }
                                            else
                                            {
                                                if (tempFloat < 2)
                                                {
                                                    decisionInt = 4;
                                                    AICounter();
                                                }
                                            }
                                        }        
                                    }    
                                }
                                    
                            }
                        }
                    }
                }
            }
            else
            {
                decisionInt = 1;
                AITimer = Random.Range((10 - fSS.AILevel) * 0.01f, (10 - fSS.AILevel) * 0.3f);
                
            }
        }


    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // This returns a target when the collider hits an enemy nearby.
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void OnTriggerEnter(Collider col)
    {
        if (!fSS.photonView.isMine) return;

        GameObject somebody = col.gameObject;


        if (somebody.layer == LayerMask.NameToLayer("Character"))
        {
            if (somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt && somebody.GetComponent<Fighter_Stats_Script>().currentHealth > 0 && !fSS.isInvu)
            {
                if (somebody != target && target != null)
                {
                    Vector3 gbTrans = gameObject.transform.position;
                    Vector3 tTrans = target.transform.position;
                    Vector3 sbTrans = somebody.transform.position;

                    float gbtDist = Vector3.Distance(gbTrans, tTrans);
                    float gbsbDist = Vector3.Distance(gbTrans, sbTrans);

                    if (gbtDist > gbsbDist)
                    {
                        target = somebody;
                    }
                }
                else
                {
                    target = somebody;
                }
            }
        }


    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // After a decision was made and a randomly set timer ticked down to zero an action is executed based on the skills of the character.
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void DoAction()
    {


        if (fSS.currentHealth <= 0)
        {
            target = null;
            return;
        }


        float tempFloat;

        Debug.Log(decisionInt);

        if (decisionInt == 1)
        {
            if (pControl.down == true)
            {
                
                tempFloat = Random.Range(0, 2);
                if(tempFloat < fSS.AILevel * 0.2f)
                {
                    pControl.down = false;
                    anim.SetTrigger("FrwdRoll");
                    pControl.performing = 0.2f;
                }
                else
                {
                    anim.SetTrigger("StandUp");
                    pControl.performing = 1.2f;
                }
                
            }
        }
        else
        {
            if (decisionInt == 2)
            {
                tempFloat = Random.Range(0, 4);
                if (tempFloat < 1)
                {
                    
                    pControl.JumpInt = 2;
                }
                else
                {
                    if (tempFloat < 2.5f)
                    {
                        fBS.DiveRoll();
                    }
                    else
                    {
                        if (tempFloat < 3.5f)
                        {
                            fBS.FlipExplosionAttack();
                        }
                        else
                        {
                            pControl.sprintInt = 0;
                            pControl.running = false;
                        }
                    }
                }
            }
            else
            {
                if (decisionInt == 3)
                {
                    if (fSS.currentEnergy > 20)
                    {
                        fBS.ShootProjectile();
                    }
                    
                }
                else
                {
                    if (decisionInt == 4)
                    {
                        if (fSS.currentEnergy > 60)
                        {
                            fBS.MissileExplosionAttack();
                        }

                    }
                    else
                    {
                        if (decisionInt == 5)
                        {
                            fBS.BasicAttack();

                        }
                        else
                        {
                            if (decisionInt == 6)
                            {
                                fBS.FlipKick();

                            }
                            else
                            {
                                if (decisionInt == 7)
                                {
                                    fBS.DoubleJumpKick();

                                }
                                else
                                {
                                    if (decisionInt == 8)
                                    {
                                        fBS.ButterflyKick();

                                    }
                                    else
                                    {
                                        if (decisionInt == 9)
                                        {
                                            fBS.DiveRoll();

                                        }
                                        else
                                        {
                                            if (decisionInt == 10)
                                            {
                                                fBS.ComboSlash();

                                            }
                                            else
                                            {
                                                if (decisionInt == 11)
                                                {
                                                    if (fSS.currentEnergy > 45)
                                                    {
                                                        fBS.AOEArmada();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
        decisionInt = 0;

    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // when the character is jumping and not on the ground there is a different set of actions being taken in consideration. 
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void JumpAttack()
    {
        if (fSS.currentHealth <= 0)
        {
            target = null;
            return;
        }        

        if (targDistX > -4f && targDistX < 4f)
        {
            if (targDistY > -1f && targDistY < 1f)
            {
                float tempFloat = Random.Range(0, fSS.AILevel);               
                if (tempFloat < 2.5)
                {
                    pControl.performing = 1;                    
                }
                else
                {
                    if (anim.GetBool("isJumping"))
                    {
                        if (tempFloat < 5)
                        {

                            fBS.BasicJumpAttack();
                        }
                        else
                        {
                            pControl.performing = 0.40f;
                            fBS.JumpProjectile();
                        }
                    }
                    else
                    {
                        if (anim.GetBool("isQuickJumping"))
                        {
                            if (tempFloat < 5)
                            {
                                fBS.BasicFlyingAttack();
                            }
                            else
                            {
                                if (fSS.currentEnergy >= 40)
                                {
                                    fBS.SpecialFlyingPunch();
                                }                                    
                            }
                        }
                    }                    
                }
            }
        }            
    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // certain actions can be followed up by other attacks to execute a combo
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void ComboAction()
    {
        if (fSS.currentHealth <= 0)
        {
            target = null;
            return;
        }


        if (pControl.attackInt == 1 && (targDistX < 2 && targDistX > -2 && targDistY < 1 && targDistY > -1))
        {
            if (pControl.comboInt <= 7)
            {
                fBS.BasicAttack();
            }
            
        }
        else
        {
            if (pControl.attackInt == 9 && targDistY < 1 && targDistY > -1)
            {
                if (fSS.currentEnergy >= 15)
                {
                    fBS.ShootProjectile();
                }
            }
            else
            {
                if (pControl.comboInt == 1 && (targDistX < 4 && targDistX > -4 && targDistY < 1 && targDistY > -1))
                {
                    if (fSS.currentEnergy >= 15)
                    {
                        fBS.ComboSlash();
                    }
                }
                else
                {
                    if (pControl.comboInt == 2 && (targDistX < 4 && targDistX > -4 && targDistY < 1 && targDistY > -1))
                    {
                        if (fSS.currentEnergy >= 15)
                        {
                            fBS.ComboSlash();
                        }
                    }
                }
            }
        }
    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // This makes the character always looking into the direction of its target
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public void Turning()
    {
        if (fSS.currentHealth <= 0)
        {
            target = null;
            return;
        }


        if (targDistX < -0.01f && parentObj.transform.rotation != Quaternion.Euler(0, 0, 0) && pControl.down == false && pControl.performing < 0.05f)
        {
            if (anim.GetFloat("WalkSpeed") > 0.1f)
            {
                pControl.speedMod = -4f;
            }
            else
            {
                parentObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                pControl.facing = 1;
                pControl.sprintInt = 0;
                anim.SetFloat("WalkDirection", 0);
                pControl.directionSpeed = 0;
            }
        }

        //Left
        if (targDistX > 0.01f && parentObj.transform.rotation != Quaternion.Euler(0, 180, 0) && pControl.down == false && pControl.performing < 0.05f)
        {
            if (anim.GetFloat("WalkSpeed") > 0.1f)
            {
                pControl.speedMod = -4f;
            }
            else
            {
                parentObj.transform.rotation = Quaternion.Euler(0, 180, 0);
                pControl.facing = -1;
                pControl.sprintInt = 0;
                anim.SetFloat("WalkDirection", 0);
                pControl.directionSpeed = 0;
            }
        }
    }

    public void AICounter()
    {
        AITimer = Random.Range((10 - fSS.AILevel) * 0.01f, (10 - fSS.AILevel) * 0.5f);
    }
}
