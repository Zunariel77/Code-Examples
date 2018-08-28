using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : Photon.MonoBehaviour {

    public GameObject OnlineMenu;
    public GameObject OnlineEntry;
    public GameObject loadingPan;
    public GameObject loadingPan2;
    public GameObject ArrangedMenu;

    public Character_Selection cS;

    public int registChars;

    private void Start()
    {
        if (PhotonNetwork.offlineMode == true) return;

        

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 60;
    }

    public void Connect()
    {
        if (PhotonNetwork.offlineMode == true) return;
        PhotonNetwork.ConnectUsingSettings("v0.01");
    }

    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.offlineMode == true) return;
        Debug.Log("Serververbindung hergestellt!!");
        PhotonNetwork.JoinLobby();
    }

    public void OnJoinedLobby()
    {
        if (PhotonNetwork.offlineMode == true) return;
        loadingPan.SetActive(false);
        OnlineMenu.SetActive(true);
        OnlineEntry.SetActive(false);
        PhotonNetwork.automaticallySyncScene = true;
        Debug.Log("Lobbymitgliedschaft hergestellt!!");

    }



    private void Update()
    {
        //Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
       // Debug.Log("Anzahl der Spieler " + PhotonNetwork.otherPlayers.Length);
    }

    public void CutConnection()
    {
        if (PhotonNetwork.offlineMode == true) return;
        PhotonNetwork.offlineMode = false;
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        PhotonNetwork.automaticallySyncScene = false;

        loadingPan.SetActive(true);
        OnlineMenu.SetActive(false);
        OnlineEntry.SetActive(true);
    }

    
    public void OnDisconnectedFromPhoton()
    {
        if (PhotonNetwork.offlineMode == true) return;
        Debug.Log("Photon Verlassen!!");
        loadingPan.SetActive(false);

    }

    public void CreatePlayerRoom()
    {
        if (PhotonNetwork.offlineMode == true) return;
        PhotonNetwork.JoinRandomRoom();
        loadingPan2.SetActive(true);
        OnlineMenu.SetActive(false);
        ArrangedMenu.SetActive(true);
                      
        cS.selectedChars.Clear();
        GameObject.Find("PlayerList_Panel").gameObject.GetComponent<PlayerList>().playerCharList.Clear();
        GameObject.Find("PlayerList_Panel").gameObject.GetComponent<PlayerList>().playerObjList.Clear();

    }

    public void OnJoinedRoom()
    {
        for (int i = 0; i < cS.charList.Count; i++)
        {
            cS.charList[i].charInt = -1;
        }
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void OnPhotonRandomJoinFailed()
    {
        if (PhotonNetwork.offlineMode == true) return;
        PhotonNetwork.CreateRoom(null);
    }

    public void LeaveThisRoom()
    {
        if (PhotonNetwork.offlineMode == true) return;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.automaticallySyncScene = false;
        SceneManager.LoadScene("Title Screen");

        cS.selectedChars.Clear();
        GameObject.Find("PlayerList_Panel").gameObject.GetComponent<PlayerList>().playerCharList.Clear();
        GameObject.Find("PlayerList_Panel").gameObject.GetComponent<PlayerList>().playerObjList.Clear();

        loadingPan.SetActive(false);
        ArrangedMenu.SetActive(false);
        OnlineMenu.SetActive(true);

        
    }

    [PunRPC]
    public void Register()
    {
        registChars += cS.selectedChars.Count;
    }
}

