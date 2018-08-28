using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgentClasses : MonoBehaviour {

    public string aName;
    public Agent aClass;
    public Community com;
    public string occName;
    public Sprite occIcon;

    // Recruitment

    public int recruInt;
    public float wageOffer;
    public float minEduc;
    public float minAge;
    public float maxAge;

    public bool women;
    public bool men;

    public List<Culture> recruCultures = new List<Culture>();

    // WorkerList

    public List<Citizen> workerList = new List<Citizen>();
    public List<Citizen> potentialList = new List<Citizen>();

    // Basics

    public Agent refA;
    public Character character;

    private void Start()
    {
        aName = aClass.transform.name;

        MainMenu.EndRoundTick2 += UpdateAP;
        MainMenu.EndRoundTick7 += ClearThis;
    }

    public void UpdateAP()
    {
        if(recruInt > 0 && wageOffer > 0)
        {
            SearchWorker();
        }
        
    }

    public void SearchWorker()
    {
        

        potentialList.Clear();

        for(int i = 0; i < com.SearchingForJobList.Count; i++)
        {
            Citizen c = com.SearchingForJobList[i];

            potentialList.Add(c);

            if(workerList.Contains(c))
            {
                potentialList.Remove(c);
            }
            else
            {
                if(women == false && c.gender == 1)
                {
                    potentialList.Remove(c);
                }
                else
                {
                    if (men == true && c.gender == 0)
                    {
                        potentialList.Remove(c);
                    }
                    else
                    {
                        if (c.education < minEduc)
                        {
                            potentialList.Remove(c);
                        }
                        else
                        {
                            if (c.age < minAge || c.age > maxAge)
                            {
                                potentialList.Remove(c);
                            }
                            else
                            {
                                if (c.income > wageOffer)
                                {
                                    potentialList.Remove(c);
                                }
                                else
                                {
                                    if(recruCultures.Contains(c.culture) == false)
                                    {
                                        potentialList.Remove(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            

        }



        if (potentialList.Count > 0)
        {
            

            recruInt -= 1;
            
            Citizen c = potentialList[Random.Range(0,potentialList.Count-1)];

            if(c.occupation != null)
            {
                
            }

            c.occupation = gameObject;

            if (c.occupationScript != null)
            {
                
                Destroy(c.occupationScript);
            }

            c.gameObject.AddComponent<Agent>();

            Agent agent = c.gameObject.GetComponent<Agent>();
            agent.original = aClass;
            agent.wages = wageOffer;
            agent.aP = this;
            agent.agentPool = GameObject.Find("AgentPool").gameObject.GetComponent<AgentPool>();
            agent.satisf = 0;
            agent.StartSetUp();
            

            c.income = wageOffer;

            workerList.Add(c);
            com.SearchingForJobList.Remove(c);

            if (recruInt > 0)
            {
                SearchWorker();
            }

        }
    }
    
    public void ClearThis()
    {
        int a = workerList.Count;

        for (int i = 0; i < a; i++)
        {
            Citizen c = workerList[i];
            if (c.occupation != gameObject && c.health == 0)
            {
                workerList.Remove(workerList[i]);
                Destroy(c.gameObject.GetComponent<Agent>());
            }
        }
    }

    
}
