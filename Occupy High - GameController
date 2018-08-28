using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : Photon.MonoBehaviour {

    public List<GameObject> playerList = new List<GameObject>();
    public List<GameObject> GLplayerList = new List<GameObject>();
    public List<GameObject> UIList = new List<GameObject>();
    public List<GameObject> scoreList = new List<GameObject>();
    public int gameInt;
    public GameObject charPool;
    public GameObject charSelection;
    public GameObject playerPool;
    public GameObject transformPool;

    public GameObject UIprefab;
    public GameObject ScorePrefab;
    

    public GameObject cam;
    public float lowest = -50;
    public float highest = 50;

    public Vector3 target;
    public Vector3 highestZ;
    public Vector3 lowestZ;

    public float timer;
    public int countDown;
    public GameObject countDownObj;

    public GameObject finishObj;
    public float finishTimer;
    public int victoryTeam;
    public int deathCounter;
    public int loadedPlayer;
    public GameObject scoreBoard;
    public GameObject ScoreBoardPP;
    private bool GameStarted;

    public int GlPlayerNumber;

    private bool testBool;
    private bool playerAlive;
    private int camTarget;
    private bool freeCamActive;
    public bool gameStarted;

    private void LateUpdate()
    {
        if (gameStarted == false) return;

        if(gameInt == 0)
        {
            highestZ.z = lowest;
            lowestZ.z = highest;

            playerAlive = false;

            if (playerList.Count > 0)
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    GameObject tempObj = playerList[i];
                    Vector3 tempTrans = tempObj.transform.position;
                    if(tempObj.GetComponent<Fighter_Stats_Script>().currentHealth > 0)
                    {
                        if (tempTrans.z >= highestZ.z)
                        {
                            highestZ = tempTrans;
                        }

                        if (tempTrans.z <= lowestZ.z)
                        {
                            lowestZ = tempTrans;
                        }

                        playerAlive = true;
                    }
                    
                }
            }

            if(playerAlive == true)
            {
                target = cam.transform.position;

                float z = (highestZ.z + lowestZ.z) * 0.5f;

                target.z = z;
            }
            else
            {         
                if(freeCamActive == false)
                {
                    freeCamActive = true;
                    ChooseCamTarget(true);
                }

            

            
                
                if (GLplayerList[camTarget].GetComponent<Fighter_Stats_Script>().currentHealth <= 0)
                {
                    ChooseCamTarget(true);
                }

                GameObject tempObj = GLplayerList[camTarget];
                Vector3 tempTrans = tempObj.transform.position;
                target.z = tempTrans.z;
                    
            }
           
            cam.transform.position = target;
        }


        
        if(playerAlive == false)
        {
            if(Input.GetButtonDown("AttackP" + 1) == true)
            {
                ChooseCamTarget(true);
            }
            else
            {
                if (Input.GetButton("BlockP" + 1) == true)
                {
                    ChooseCamTarget(false);
                }
            }
        }

    }

    public void ChooseCamTarget(bool up)
    {
        bool foundTarg = false;

        if(up == true)
        {
            for (int i = 0; i < GLplayerList.Count; i++)
            {
                GameObject tempObj = GLplayerList[i];
                if (foundTarg == false && i > camTarget)
                {
                    if (i < GLplayerList.Count)
                    {
                        if (GLplayerList[i].GetComponent<Fighter_Stats_Script>().currentHealth > 0 && GLplayerList[i].GetComponent<Fighter_Stats_Script>().photonView.isMine == false)
                        {
                            foundTarg = true;
                            camTarget = i;
                        }
                    }
                    else
                    {
                        camTarget = 0;
                        ChooseCamTarget(up);
                    }
                }
            }
        }
        else
        {
            for (int i = GLplayerList.Count-1; i >= 0; i--)
            {
                GameObject tempObj = GLplayerList[i];
                if (foundTarg == false && i < camTarget)
                {
                    if (i >= 0)
                    {
                        if (GLplayerList[i].GetComponent<Fighter_Stats_Script>().currentHealth > 0 && GLplayerList[i].GetComponent<Fighter_Stats_Script>().photonView.isMine == false)
                        {
                            foundTarg = true;
                            camTarget = i;
                        }
                    }
                    else
                    {
                        camTarget = GLplayerList.Count-1;
                        ChooseCamTarget(up);
                    }
                }
            }
        }

        
    }

    [PunRPC]
    public void RegisterPlayer()
    {
        loadedPlayer += 1;
    }

    private void Start()
    {
        photonView.RPC("RegisterPlayer", PhotonTargets.All);

        if (GameObject.Find("PhotonTest").gameObject.activeSelf == true)
        {
            GameObject.Find("PhotonTest").gameObject.GetComponent<PhotonTest>().Connect();
        }


    }

    public void StartTest()
    {
        
        playerList.Clear();
       

        charPool = Instantiate(charPool);
        charSelection = GameObject.Find("Character_Selection").gameObject;

        for (int i = 0; i < charSelection.GetComponent<Character_Selection>().charList.Count; i++)
        {
            Character_Stats cS = charSelection.GetComponent<Character_Selection>().charList[i].GetComponent<Character_Stats>();
            

            if(cS.charInt >= 0)
            {
                if (charPool.GetComponent<Character_Pool>().charList[cS.charInt] != null)
                {
                    CreateCharacter(charPool.GetComponent<Character_Pool>().charList[cS.charInt], cS.charInt, cS.teamInt, cS.playerState, i);
                }
            }           
        }   
        
        
    }

    public void CreateCharacter(GameObject prefab, int prefabNr, int teamInt, int playerState, int playerNumber)
    {
        GameObject tempPlayer = PhotonNetwork.Instantiate(prefab.GetComponent<Character_Prefab>().prefab.transform.name, Vector3.zero, Quaternion.identity, 0);
        
        tempPlayer.GetComponent<Player_Movement_Controller>().playerNumber = playerNumber + 1;
        tempPlayer.GetComponent<Fighter_Stats_Script>().AILevel = playerState;
        tempPlayer.GetComponent<Fighter_Stats_Script>().prefabNr = prefabNr;
        tempPlayer.GetComponent<Fighter_Stats_Script>().teamInt = teamInt;
        playerList.Add(tempPlayer);

    }

    [PunRPC]
    public void StartGame(bool gS)
    {
        GameObject.Find("DODL").gameObject.GetComponent<DODL>().LoadingPanel.SetActive(false);
        GameStarted = gS;
    }
    
    private void Update()
    {
        if (PhotonNetwork.isMasterClient && GameStarted == false)
        {
            if (loadedPlayer == PhotonNetwork.playerList.Length)
            {
                

                photonView.RPC("StartGame", PhotonTargets.All, true);
                StartCoroutine(waitFunc2(1));
            }

            
        }

        
        if (GameStarted == true && testBool == false)
        {
            

            testBool = true;
            StartTest();

        }
       
            
        if(GameStarted == true)
        {
            if (countDown >= 0)
            {
                
                CountDown();
            }
            else
            {
                if (countDownObj.activeSelf == true)
                {
                    countDownObj.SetActive(false);

                }
            }


        }


    }

    public void CountDown()
    {
        if (countDownObj.activeSelf == false)
        {
            countDownObj.SetActive(true);
        }

        if (timer <= 0)
        {
            timer = 1;
            countDown -= 1;

            if (countDown > 0)
            {
                countDownObj.GetComponent<TextMeshProUGUI>().text = countDown.ToString();
                countDownObj.GetComponent<Animator>().SetTrigger("Trigger");
            }
            else
            {
                countDownObj.GetComponent<TextMeshProUGUI>().text = "Fight!!";
                for(int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].GetComponent<Player_Movement_Controller>().performing = 0;
                }

                
            }



        }
        else
        {
            timer -= 1 * Time.deltaTime;
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<Player_Movement_Controller>().performing = 100;
            }
        }
    }

    [PunRPC]
    public void Finish()
    {
        finishObj.SetActive(true);
        finishObj.GetComponent<Animator>().SetTrigger("Trigger");

        for (int i = 0; i < GLplayerList.Count; i++)
        {
            if(GLplayerList[i].GetComponent<Fighter_Stats_Script>().currentHealth > 0)
            {
                GLplayerList[i].GetComponent<Fighter_Stats_Script>().isInvu = true;
            }
        }

        StartCoroutine(waitFunc1(4));

        
    }

    public void DeathTest()
    {
        if(!PhotonNetwork.isMasterClient)return;


        int tempInt = 0;
        bool tempBool = false;

        for(int i = 0; i < GLplayerList.Count; i++)
        {
            GameObject player = GLplayerList[i];

            Fighter_Stats_Script fSS = player.GetComponent<Fighter_Stats_Script>();

            if (tempInt != 0)
            {
                if(tempInt != fSS.teamInt)
                {
                    if(fSS.currentHealth > 0)
                    {
                        tempInt = fSS.teamInt;
                        tempBool = true;
                    }
                    
                }
            }
            else
            {
                if (fSS.currentHealth > 0)
                {
                    tempInt = fSS.teamInt;
                    
                }
            }

            
        }

        victoryTeam = tempInt;

        if(!tempBool)
        {
            photonView.RPC("Finish", PhotonTargets.All);
        }
    }

    

    IEnumerator waitFunc1(float wait)
    {
        yield return new WaitForSeconds(wait);

        finishObj.SetActive(false);

        scoreBoard.SetActive(true);

        if(PhotonNetwork.offlineMode == false)
        {
            scoreBoard.transform.Find("Button_Restart").GetComponent<Button>().interactable = false;
            scoreBoard.transform.Find("Button_Exit").GetComponent<Button>().interactable = false;
        }

        if(PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < GLplayerList.Count; i++)
            {
                Fighter_Stats_Script fSS = GLplayerList[i].GetComponent<Fighter_Stats_Script>();

                fSS.photonView.RPC("UpdateScore", PhotonTargets.All);

                

            }
        }        
    }

    IEnumerator waitFunc2(float wait)
    {
        yield return new WaitForSeconds(wait);

        for(int i = 0; i < GLplayerList.Count; i++)
        {
            GameObject tempPlayer = GLplayerList[i];

            Debug.Log("Try to call SetPosition!!");
            tempPlayer.GetComponent<Fighter_Stats_Script>().photonView.RPC("SetPosition", PhotonTargets.All, i);
        }
    }
}
