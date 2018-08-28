using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentManagement : MonoBehaviour {

    public GameObject window;
    public int selectedAgent;
    public AgentPool aP;
    public AgentClasses aC;

    public Community com;

    #region UIElements
    [Space]
    [Space]
    public TextMeshProUGUI title;
    public TextMeshProUGUI recruCounter;
    public TextMeshProUGUI satisfactionTxt;
    public TextMeshProUGUI sexTxt;
    public TMP_InputField wageInput;
    public TMP_InputField educInput;
    public TMP_InputField minAgeInput;
    public TMP_InputField maxAgeInput;
    public Toggle womenTogg;
    public Toggle menTogg;

    public GameObject cultureContent;

    public List<GameObject> contentList1 = new List<GameObject>();


    public GameObject prefab;
    public GameObject prefab2;
    public GameObject prefab3;

    public AgentWorkerPanelBtn aWs;

    public GameObject middleSide;
    public GameObject rightSide;

    #endregion

    #region Middle UI
    [Space]
    [Space]
    public List<GameObject> workerList = new List<GameObject>();
    public GameObject workerContent;

    public TextMeshProUGUI agentLvl;
    public TMP_InputField wageChangeInput;
    public TextMeshProUGUI iPGenTxt;
    public GameObject iPGenImg;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI refEducTxt;
    public TextMeshProUGUI refAgeTxt;
    public TextMeshProUGUI orderTxt;

    #endregion


    #region rightSide
    [Space]
    [Space]
    public Toggle lowerTogg;
    public Toggle middleTogg;
    public Toggle eliteTogg;
    public Toggle religiousTogg;
    public Toggle RefwomenTogg;
    public Toggle RefmenTogg;
    public GameObject refAgentContent;

    public List<GameObject> contentList2 = new List<GameObject>();

    #endregion


    private void Start()
    {
        Interaction.Interact1 += Interacted;

        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        window.SetActive(false);
    }

    public void OpenWindow(int index)
    {
        if(aC != null && aC.refA != null)
        {
            aC.refA = null;
        }
        

        selectedAgent = index;
        if(window.activeSelf == false)
        {
            window.SetActive(true);
        }

        UpdateWindow();
        UpdateRightSide();
    }

    public void CloseWindow()
    {
        window.SetActive(false);
    }

    public void Interacted()
    {
        com = Interaction.com;
    }

    public void UpdateWindow()
    {
        aP = com.aP;

        aC = aP.classList[selectedAgent];

        

        title.text = aC.aName;
        recruCounter.text = aC.recruInt.ToString();
        wageInput.text = aC.wageOffer.ToString();
        educInput.text = aC.minEduc.ToString();
        minAgeInput.text = aC.minAge.ToString();
        maxAgeInput.text = aC.maxAge.ToString();
        womenTogg.isOn = aC.women;
        menTogg.isOn = aC.men;

        

        if (contentList1.Count > 0)
        {
            foreach (GameObject go in contentList1)
            {
                Destroy(go);
            }
        }

        contentList1.Clear();

        for(int i = 0; i < com.residentCultures.Count; i++)
        {
            Culture cult = com.residentCultures[i];

            GameObject cPrefab = Instantiate(prefab, cultureContent.transform);
            contentList1.Add(cPrefab);
          
            AgentPanelBtn aPBtn = cPrefab.GetComponent<AgentPanelBtn>();

            aPBtn.cult = cult;

            aPBtn.tog.isOn = false;
            aPBtn.img.sprite = cult.cIcon;
            aPBtn.aM = this;
            aPBtn.aC = aC;

            

            if(aC.recruCultures.Contains(cult))
            {
                aPBtn.tog.isOn = true;
            }
        }

        if (workerList.Count > 0)
        {
            foreach (GameObject go in workerList)
            {
                Destroy(go);
            }
        }      

        workerList.Clear();

        for(int a = 0; a < aC.workerList.Count; a++)
        {
            Citizen cit = aC.workerList[a];

            GameObject instant = Instantiate(prefab2, workerContent.transform);
            workerList.Add(instant);

            AgentWorkerPanelBtn aW = instant.GetComponent<AgentWorkerPanelBtn>();

            aW.agent = cit.gameObject.GetComponent<Agent>();

            aW.wages.text = "W:"+ cit.income.ToString();
            aW.lvl.text = "Lvl " + aW.agent.agentLvl.ToString();
            aW.cultImg.sprite = cit.culture.cIcon;
            aW.aM = this;
            aW.active = false;
            aW.img.color = Color.red;
        }
    }

    public void RecruCounter(int index)
    {
        if(index == 0)
        {
            if(aC.recruInt<10)
            aC.recruInt += 1;
        }
        else
        {
            if(aC.recruInt > 0)
            aC.recruInt -= 1;
        }

        recruCounter.text = aC.recruInt.ToString();
    }

    public void WageOffer()
    {
        float offer = float.Parse(wageInput.text);
        aC.wageOffer = offer;
    }

    public void MinEducation()
    {
        float offer = float.Parse(educInput.text);
        aC.minEduc = offer;
    }

    public void MinAge()
    {
        float offer = float.Parse(minAgeInput.text);
        

        aC.minAge = offer;
    }

    public void MaxAge()
    {
        float offer = float.Parse(maxAgeInput.text);


        aC.maxAge = offer;
    }

    public void MaxAgeDe()
    {
        float offer = float.Parse(maxAgeInput.text);

        if (offer < 18)
        {
            offer = 18;
            maxAgeInput.text = 18.ToString();
        }
        else
        {
            if (offer > 99)
            {
                offer = 99;
                maxAgeInput.text = 99.ToString();
            }
        }
    }

    public void MinAgeDe()
    {
        float offer = float.Parse(minAgeInput.text);
        if (offer < 18)
        {
            offer = 18;
            minAgeInput.text = 18.ToString();
        }
        else
        {
            if (offer > 99)
            {
                offer = 99;
                minAgeInput.text = 99.ToString();
            }
        }
    }

   

    public void Men()
    {
        bool change = menTogg.isOn;
        aC.men = change;
    }

    public void Women()
    {
        bool change = womenTogg.isOn;
        aC.women = change;
    }

    public void UpdateRightSide()
    {
        //public TextMeshProUGUI agentLvl;
        //public TMP_InputField wageChangeInput;
        //public TextMeshProUGUI iPgenTxt;
        //public TextMeshProUGUI healthTxt;
        //public TextMeshProUGUI refEducTxt;
        //public TextMeshProUGUI refAgeTxt;


        //public Toggle lowerTogg;
        //public Toggle middleTogg;
        //public Toggle eliteTogg;
        //public Toggle religiousTogg;
        //public Toggle RefwomenTogg;
        //public Toggle RefmenTogg;
        //public GameObject refAgentContent;


        if (aC.refA != null)
        {
            middleSide.SetActive(true);
            rightSide.SetActive(true);

            Agent agent = aC.refA;

            agentLvl.text = "Level " + agent.agentLvl.ToString();
            wageChangeInput.text = agent.wages.ToString();
            if(agent.citRef.gender == 0)
            {
                sexTxt.text = "Sex: Male";
            }
            else { sexTxt.text = "Sex: Female"; }
            iPGenTxt.text = Mathf.RoundToInt(agent.iPGen) + "/" + Mathf.RoundToInt(agent.expMax) + " Exp";

            Vector3 calc = new Vector3(0, 1, 1);

            if (agent.iPGen > 0)
            {
                calc = new Vector3(agent.iPGen / agent.expMax, 1, 1);
            }
            else
            {
                calc = new Vector3(0, 1, 1);
            }
            
            
            iPGenImg.transform.localScale = calc;
            healthTxt.text = "Health: " +agent.citRef.health.ToString();
            refEducTxt.text = "Education: " + agent.citRef.education.ToString();
            refAgeTxt.text = "Age: " + agent.citRef.age.ToString();
            orderTxt.text = "Orders: " + aP.orderList[agent.order];
            satisfactionTxt.text = "Motivation: " + aP.satisfactList[agent.satisf].satisLvl;


            lowerTogg.isOn = agent.lower;
            middleTogg.isOn = agent.middle;
            eliteTogg.isOn = agent.elite;
            religiousTogg.isOn = agent.religious;
            RefwomenTogg.isOn = agent.women;
            RefmenTogg.isOn = agent.men;
           


            if (contentList2.Count > 0)
            {
                foreach (GameObject go in contentList2)
                {
                    Destroy(go);
                }
            }

            contentList2.Clear();

            for (int i = 0; i < com.residentCultures.Count; i++)
            {
                Culture cult = com.residentCultures[i];

                GameObject cPrefab = Instantiate(prefab3, refAgentContent.transform);
                contentList2.Add(cPrefab);

                AgentPanelRightBtn aPBtn = cPrefab.GetComponent<AgentPanelRightBtn>();

                aPBtn.cult = cult;

                aPBtn.tog.isOn = false;
                aPBtn.img.sprite = cult.cIcon;
                aPBtn.aM = this;
                aPBtn.aC = aC;
                aPBtn.agent = agent;
                

                if (agent.targetCultures.Contains(cult))
                {
                    aPBtn.tog.isOn = true;
                }
            }
        }
        else
        {
            middleSide.SetActive(false);
            rightSide.SetActive(false);
        }
    }

    public void ChangeWages()
    {
        float offer = float.Parse(wageChangeInput.text);
        aWs.agent.wages = offer;
        aWs.agent.citRef.income = offer;
        aWs.wages.text = offer.ToString();
    }

    public void ChangeTargets(int index)
    {
        Agent agent = aWs.agent;
        if (index == 0)
        {
            agent.lower = lowerTogg.isOn;
        }
        else
        {
            if (index == 1)
            {
                agent.middle = middleTogg.isOn;
            }
            else
            {
                if (index == 2)
                {
                    agent.elite = eliteTogg.isOn;
                }
                else
                {
                    if (index == 3)
                    {
                        agent.religious = religiousTogg.isOn;
                    }
                    else
                    {
                        if (index == 4)
                        {
                            agent.women = womenTogg.isOn;
                        }
                        else
                        {
                            if (index == 5)
                            {
                                agent.men = menTogg.isOn;
                            }
                            
                        }
                    }
                }
            }
        }
    }

    public void GiveOrders(int index)
    {
        Agent agent = aWs.agent;

        if(index == 0)
        {

        }
        else
        {
            if (index == 1)
            {
                
            }
            else
            {
                if (index == 2)
                {
                    if(agent.order != 3)
                    {
                        agent.order = 3;
                        orderTxt.text = "Orders: " + aP.orderList[agent.order];
                    }
                    else
                    {
                        agent.order = 0;
                        orderTxt.text = "Orders: " + aP.orderList[agent.order];
                    }
                    
                }
                
            }
        }
    }
}
