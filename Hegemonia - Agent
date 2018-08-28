using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
 
    public string agentName;
    public int agentLvl;
    public float expMax = 20;  
    public Citizen citRef;
    public float wages;
    public float iPGen;
    [Range(0, 5)]
    public int satisf;
    public AgentClasses aP;
    public AgentPool agentPool;
    

    public float classIssues;
    public float genderEquality;
    public float scienceFaith;
    public float multiculture;

    public bool lower;
    public bool middle;
    public bool elite;
    public bool religious;
    public bool women;
    public bool men;

    public int order;
    public float satisDemand = 1;
    

    public List<Culture> targetCultures = new List<Culture>();

    public Agent original;

    public List<Citizen> workList = new List<Citizen>();

    private void Start()
    {
        expMax = 20;

        agentPool = GameObject.Find("AgentPool").gameObject.GetComponent<AgentPool>();

        MainMenu.EndRoundTick1_5 += RoundTrans;
    }

    public void StartSetUp()
    {
        citRef = GetComponent<Citizen>();
        agentLvl = 1;
        agentName = original.agentName;

        classIssues = original.classIssues;
        genderEquality = original.genderEquality;
        scienceFaith = original.scienceFaith;
        multiculture = original.multiculture;
    }

    public void RoundTrans()
    {
        if (wages == 0)
        {
            Quits();
            return;
        }

            
        iPGen = 0;

        

        if (order == 3)
        {
            Quits();
            return;
        }
        else
        {
            if (order == 0)
            {
                
            }
        }

        float charTres = Character.character.treasure;

        if (charTres >= wages)
        {
            charTres -= Mathf.RoundToInt(wages);
            citRef.wealth += wages;
            Character.character.treasure = Mathf.RoundToInt(charTres);

        }
        else
        {
            citRef.wealth += charTres;

            charTres = 0;
            Character.character.treasure = Mathf.RoundToInt(charTres);

        }

        citRef.UpdateCitizen();

        
        if (citRef.power <= satisDemand)
        {           
            if(satisf >= 5)
            {
                satisf = 5;
                Quits();
                return;
            }
            satisf += 1;
            if (wages == 0)
            {
                satisf += 1;
                if (satisf > 5) satisf = 5;

            }
        }
        else
        {
            satisf -= 1;
            if (satisf < 0) satisf = 0;
        }

        Work();
        
    }

    public void Quits()
    {
        if(aP != null && aP.workerList.Contains(citRef))
        {
            aP.workerList.Remove(citRef);
            citRef.occupation = null;
            citRef.income = 0;
            citRef.occupationScript = null;
            Destroy(this);
        }
        
        
    }

    public void Work()
    {
        Debug.Log("Working!");
        workList.Clear();
        Community com = citRef.home.GetComponent<Community>();

        

        if (targetCultures.Count > 0)
        {
            for(int i = 0; i < com.citizens.Count; i++)
            {
                Citizen cit = com.citizens[i];
                bool take = true;

                if(cit.gender == 0 && men == false)
                {
                    take = false;
                }
                else
                {
                    if (cit.gender == 1 && women == false)
                    {
                        take = false;
                    }
                    else
                    {
                        if (cit.power <= 0.75f && lower == false)
                        {
                            take = false;
                        }
                        else
                        {
                            if (cit.power > 0.75f && cit.power <= 2.5f && middle == false)
                            {
                                take = false;
                            }
                            else
                            {
                                if (cit.power >= 2.5f && elite == false)
                                {
                                    take = false;
                                }
                                else
                                {
                                    if (cit.religiousity > 50 && religious == false)
                                    {
                                        take = false;
                                    }
                                    else
                                    {
                                        if (targetCultures.Contains(cit.culture) == false)
                                        {
                                            take = false;
                                        }
                                        else
                                        {
                                            if(cit.popularity >= 90)
                                            {
                                                take = false;
                                            }
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }

                if(take == true)
                {
                    workList.Add(cit);
                }
            }
        }

        //==============================

        // 20 * 1 * 100/100  * 1.2f = 24

        float skill = citRef.education * (1 + agentLvl * 0.33f) * 100f/citRef.health * citRef.power;

        for (int i = 0; i < workList.Count; i++)
        {
            if(skill <= 0)
            {
                return;
            }

            Citizen cit = workList[i];
            float chance = 500;
            // gender

            float genderChance = 0;  // 100 + gender * 90, 25 - 100 = -90
            if (cit.gender == citRef.gender)
            {
                genderChance = 0;
            }
            else
            {
                genderChance = 100 - genderEquality;
            }

            //education

            float educChance = cit.education - citRef.education; // 50 - 30 
            if (educChance < 0) educChance *= -1;

            if (educChance > scienceFaith) { educChance = scienceFaith - educChance; }
            else
            {
                educChance = 0;
            }
                // 20 - 50 = -30;
                

            // class
            float classChance = classIssues - (cit.power * 33.33f); // 25 - 0.5f * 33.33f = 16.67f ... 75 - 2.5f * 33.33 = 75 ... 50 - 1.5f * 33 = 45

            // culture

            float cultChance = 0;

            if (cit.culture == citRef.culture)
            {
                cultChance = 0;
            }
            else
            {                
                cultChance = 100-multiculture;
            }

            // Age

            float agediff = cit.age - citRef.age;
            if (agediff < 0) agediff*= -1;
            chance -= agediff;

            // result

            chance = (chance - cultChance - genderChance - educChance - classChance) * 0.20f; // 400 - 25 - 

            skill -= cit.power;

            cit.popularity += chance;

            if(cit.popularity >= 90)
            {
                iPGen += Mathf.Round(cit.power);
                if(iPGen >= expMax)
                {
                    iPGen = 0;
                    agentLvl += 1;
                    expMax *= 2;
                }
            }
           

            
        }

    }
}
