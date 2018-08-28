using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    private PhotonView pv;
    private bool tempBool;
    
    [SerializeField]
    private int playerCount;
    [SerializeField]
    public List<GameObject> playerObjList = new List<GameObject>();
    public int maxPlayer = 8;
    public GameObject charPan;
    public GameObject prefab;
    public GameObject loadPan;

    private Character_Pool charPool;
    public List<GameObject> charList = new List<GameObject>();

    public List<PlayerStats> playerCharList = new List<PlayerStats>();
    public PlayerStats currentSelect;
    public Character_Selection cS;

    private string selectStage;
    private NetworkManager nM;

    public Text errText;
    

    private void Start()
    {
        selectStage = "GameScene/Scenes/OlderFighterTest";

        pv = GetComponent<PhotonView>();

        charPool = GameObject.Find("Character_Pool").gameObject.GetComponent<Character_Pool>();

        nM = GameObject.Find("NetworkManager").gameObject.GetComponent<NetworkManager>();
        cS = GameObject.Find("Character_Selection").gameObject.GetComponent<Character_Selection>();
    }

    private void OnJoinedRoom()
    {
        

        if(playerObjList.Count < maxPlayer)
        {
            
            CreatePlayer();
            
        }
        else
        {
            nM.CutConnection();
            GameObject.Find("DODL").gameObject.GetComponent<DODL>().LoadingPanel.SetActive(false);
        }
        
        
    }

    
   

    public void CreatePlayer()
    {
        GameObject obj = PhotonNetwork.Instantiate("PlayerListObj", Vector3.zero, Quaternion.identity, 0);

        
    }

    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        
        


    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }
    
    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        

    }

    

    private void Update()
    {
        for(int i = 0; i < playerObjList.Count; i++)
        {
            if(playerObjList[i] == null)
            {
                playerObjList.Remove(playerObjList[i]);
            }
        }

        for (int i = 0; i < cS.selectedChars.Count; i++)
        {
            if (cS.selectedChars[i] == null)
            {
                cS.selectedChars.Remove(cS.selectedChars[i]);
            }
        }


        if (playerCount != playerObjList.Count)
        {

            playerCount = playerObjList.Count;

            for (int i = 0; i < playerObjList.Count; i++)
            {
                playerObjList[i].GetComponent<PlayerStats>().photonView.RPC("GlobalUpdate", PhotonTargets.All);
            }

            if (PhotonNetwork.isMasterClient)
            {
                if (playerCount >= maxPlayer)
                {
                    PhotonNetwork.room.IsVisible = false;
                    PhotonNetwork.room.IsOpen = false;
                }
                else
                {
                    PhotonNetwork.room.IsVisible = true;
                    PhotonNetwork.room.IsOpen = true;
                }
            }
        }     
        
       
    }

    public void GetReady()
    {
        for(int i = 0; i < playerObjList.Count; i++)
        {
            PlayerStats pS = playerObjList[i].GetComponent<PlayerStats>();


            if(pS.photonView.isMine)
            {
                pS.photonView.GetComponent<PhotonView>().RPC("GetReady", PhotonTargets.All);

                pS.photonView.GetComponent<PhotonView>().RPC("UpdatePlayer", PhotonTargets.All);
            }
            
        }
        
        tempBool = false;

        for (int i = 0; i < playerObjList.Count; i++)
        {
            if (playerObjList[i].GetComponent<PlayerStats>().ready == false)
            {
                tempBool = true;
            }
        }

        if (tempBool == false)
        {
            if(PhotonNetwork.playerList.Length > 1)
            {
                pv.RPC("StartLevel", PhotonTargets.All);
            }
            else
            {
                errText.text = "There has to be more than 1 Client connected to this game to start!";
            }
        }
    }

    public void StageSelect(string stage)
    {
        selectStage = stage;
    }

    [PunRPC]
    public void StartLevel()
    {
        nM.registChars = 0;
        nM.photonView.RPC("Register", PhotonTargets.All);
        GameObject.Find("DODL").gameObject.GetComponent<DODL>().LoadingPanel.SetActive(true);

        cS.OverrideList();

        GameObject.Find("Arranged_Mode_Online").gameObject.SetActive(false);
        Debug.Log("Tried to load level!");

        if(PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel(selectStage);
        }
        

    }

    public void AddCharacter()
    {
        if (playerCount < maxPlayer && cS.selectedChars.Count < 4)
        {
            if(playerCharList.Count > 0)
            {
                for (int i = 0; i < playerCharList.Count; i++)
                {
                    playerCharList[i].ready = false;
                    playerCharList[i].pv.RPC("GlobalUpdate", PhotonTargets.All);

                }
                CreatePlayer();
            }           
        }
    }

    public void RemoveCharacter()
    {
        if(playerCharList.Count > 1)
        {            
            playerCharList[playerCharList.Count - 1].DestroyThis();
        }
        
    }

    public void CurrentSelect(string chara)
    {
        currentSelect.charName = chara;
        currentSelect.pv.RPC("UpdateCharacter", PhotonTargets.All, chara);
        currentSelect.pv.RPC("GlobalUpdate", PhotonTargets.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            
        }
        else
        {
            
            
        }

    }

}
