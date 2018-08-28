using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Community : MonoBehaviour {

    #region basics

    public int ip;
    public int ipCur;
    public int ipReg;
    public string objName;
    public Sprite objSprite;
    public GameObject panel;
    [HideInInspector]
    public CommunityPanel cP;
    public GameObject civicPool;
    public AgentPool aP;
    public float treasure;

    public List<Citizen> citizens = new List<Citizen>();
    public List<Culture> residentCultures = new List<Culture>();

    public float totalWealth;

    #endregion

    #region jobmarket

    public List<Citizen> SearchingForJobList = new List<Citizen>();

    #endregion

    private void Start()
    {
        cP = panel.GetComponent<CommunityPanel>();

        MainMenu.EndRoundTick2 += UpdateCom;
        MainMenu.EndRoundTick3 += Round3;
        MainMenu.EndRoundTick4 += Round4;


    }

    void OnMouseDown()
    {
        
    }

    public void Interact()
    {
        panel.SetActive(true);
        
        cP.refCom = gameObject;
        cP.UpdatePanel();
    }

    public void Close()
    {
        panel.SetActive(false);
        if(cP.currentPan != null)
        {
            cP.currentPan.SetActive(false);
            
        }

        if (cP.refCom != null)
        {
            cP.refCom = null;

        }

        
    }

    public void UpdateCom()
    {
        Debug.Log("Update Community");

        
        totalWealth = 0;

        for(int i = 0; i < citizens.Count; i++)
        {
            Citizen c = citizens[i];

            totalWealth += c.wealth;
        }
    }

    public void Round3()
    {

    }

    public void Round4()
    {
        ipReg = 0;

        for(int i = 0; i < citizens.Count; i++)
        {
            Citizen cit = citizens[i];

            if (cit.gameObject.GetComponent<Agent>() != null  )
            {
                Agent agent = cit.gameObject.GetComponent<Agent>();

                ipReg += Mathf.RoundToInt(agent.iPGen);
                Debug.Log(ipReg);
            }
        }

        ip = ipCur;
        ip += ipReg;
        

        ipCur = ip;

    }
}
