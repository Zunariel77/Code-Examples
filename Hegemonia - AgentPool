using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPool : MonoBehaviour {

    public Community com;
   
   
    public List<AgentClasses> classList = new List<AgentClasses>();

    private void Start()
    {
        GameObject aSObj = GameObject.Find("Agent System");

        AgentSystem aS = aSObj.GetComponent<AgentSystem>();

        for(int i = 0; i < classList.Count; i++)
        {
            AgentClasses aC = classList[i];

            aC.com = com;


        }
    }

    public List<string> orderList = new List<string>();
    

    [System.Serializable]
    public class satisfaction
    {
        public float quality;
        public string satisLvl;
    }

    public List<satisfaction> satisfactList = new List<satisfaction>();
}
