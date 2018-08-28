using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Photon.MonoBehaviour {

    public Text playText;
    public Button playBtn;
    public Button teamNrBtn;
    public Image playImg;
    public Image playCharImg;
    public Image playCharImgBg;
   

    public string playerName;
    public GameObject character;
    public string message;
    public string charName;

    public bool ready;
    public bool selected;
    public int teamInt;
    public int controlInt;
    public int charInt;

    public Text teamText;
    public Text numberText;
    public Character_Selection cS;

    public PhotonView pv;
    [SerializeField]
    private PlayerList pL;


    private void Start()
    {
        float xFac = Screen.width;
        float yFac = Screen.height;

        xFac /= 1600;
        yFac /= 900;
        
        transform.localScale = new Vector3(yFac, yFac, yFac);

        pv = GetComponent<PhotonView>();
        playerName = pv.owner.NickName;
        transform.name = "PL_"+playerName;
        transform.parent = GameObject.Find("PlayerList_Panel").gameObject.transform;
        pL = GameObject.Find("PlayerList_Panel").gameObject.GetComponent<PlayerList>();
        cS = GameObject.Find("Character_Selection").gameObject.GetComponent<Character_Selection>();
        playText.text = playerName;

        teamInt = 1;
        
        ready = false;

        
        pL.playerObjList.Add(gameObject);
        

        if (pv.isMine == false)
        {
            playCharImgBg.color = Color.clear;
            teamNrBtn.interactable = false;
            playBtn.interactable = false;
            
        }
        else
        {
            pL.loadPan.SetActive(false);
            pL.playerCharList.Add(gameObject.GetComponent<PlayerStats>());
            cS.selectedChars.Add(gameObject);
            SelectChar();
        }
        controlInt = cS.selectedChars.Count;

        
        pv.RPC("GlobalUpdate", PhotonTargets.All);

        

        
    }

    public void OnDestroy()
    {
        
        
    }

    [PunRPC]
    public void GetReady()
    {
        if (ready == true)
        {
            ready = false;
        }
        else
        {
            ready = true;

        }
    }

    [PunRPC]
    public void GlobalUpdate()
    {
        pv = GetComponent<PhotonView>();

        if (pv.isMine)
        {
            pv.RPC("UpdatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void UpdatePlayer()
    {
        if(charName != "")
        {
            character = GameObject.Find(charName);
        }

        message = "RPC Call Received";

        playText.text = playerName;

        if (ready)
        {
            playImg.color = Color.green;
        }
        else
        {
            playImg.color = Color.red;
        }

        if(character != null)
        {
            playCharImg.sprite = character.GetComponent<Character_Prefab>().picture;
            charInt = character.GetComponent<Character_Prefab>().charInt;
        }
        else
        {
            playCharImg.sprite = null;
            charInt = -1;
        }


        teamText.text = teamInt.ToString();
        numberText.text = controlInt.ToString();

        if(pv.isMine)
        {
            if (selected == true)
            {
                playCharImgBg.color = Color.green;
            }
            else
            {
                playCharImgBg.color = Color.red;
            }
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(ready);
            stream.SendNext(selected);
            stream.SendNext(teamInt);
            stream.SendNext(controlInt);
            stream.SendNext(charName);
            stream.SendNext(charInt);

        }
        else
        {
            ready = (bool)stream.ReceiveNext();
            selected = (bool)stream.ReceiveNext();
            teamInt = (int)stream.ReceiveNext();
            controlInt = (int)stream.ReceiveNext();
            charName = (string)stream.ReceiveNext();
            charInt = (int)stream.ReceiveNext();

        }

    }

    
    public void ChangeTeam()
    {
        if(teamInt < 8)
        {
            teamInt++;
        }
        else
        {
            teamInt = 1;
        }
        pv.RPC("UpdateTeam", PhotonTargets.All, teamInt);
        pv.RPC("GlobalUpdate", PhotonTargets.All);
    }

    [PunRPC]
    public void UpdateTeam(int tI)
    {
        teamInt = tI;
    }

    public void DestroyThis()
    {
        if(cS.selectedChars.Contains(gameObject))
        {
            cS.selectedChars.Remove(gameObject);
        }

        if (pL.playerObjList.Contains(gameObject))
        {
            pL.playerObjList.Remove(gameObject);
        }

        if (selected)
        {
            pL.playerCharList[pL.playerCharList.Count - 1].SelectChar();
        }

        pv.RPC("RpcDestroyThis", PhotonTargets.All);
    }

    [PunRPC]
    public void RpcDestroyThis()
    {
        pL.playerCharList.Remove(gameObject.GetComponent<PlayerStats>());
        pL.playerObjList.Remove(gameObject);

        if(pv.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }        
    }

    public void SelectChar()
    {
        Debug.Log(pL.playerObjList.Count);
        for(int i = 0; i < pL.playerObjList.Count; i++ )
        {
            PlayerStats pS = pL.playerObjList[i].GetComponent<PlayerStats>();
            if(pS.photonView.isMine == true)
            {
                pS.selected = false;
            }
            pS.photonView.RPC("GlobalUpdate", PhotonTargets.All);
        }

        selected = true;
        pL.currentSelect = GetComponent<PlayerStats>();

        photonView.RPC("GlobalUpdate", PhotonTargets.All);

    }

    [PunRPC]
    public void UpdateCharacter(string chara)
    {
        charName = chara;
    }
}
