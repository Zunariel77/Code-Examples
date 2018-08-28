using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour {

    #region basics

    public Culture culture;
    [Range(0, 1)]
    public int gender;
    [Range(18, 120)]
    public float age;
    [Range(0, 200)]
    public int education;
    [Range(0, 100)]
    public int religiousity;
    [Range(0, 100)]
    public int fear;
    [Range(0, 100)]
    public int health;

#endregion

    #region economy

    public GameObject home;
    public GameObject occupation;
    public string occName;
    public Sprite occIcon;
    public float income;
    public float wealth;

#endregion

    #region family

    public GameObject partner;
    public GameObject father;
    public GameObject mother;
    public List<GameObject> children = new List<GameObject>();
    [HideInInspector]
    public List<Citizen> searchList = new List<Citizen>();

    #endregion

#region technics

    [HideInInspector]
    public static int genderCounter;
    public float wealthPart;
    public bool onSearch;
    public bool onReprod;
    [HideInInspector]
    public float cultureDiscr;
    [HideInInspector]
    public float religiousDiscr;


    #endregion

    #region needs

    [Range(0, 100)]
    public float hunger;
    [Range(0, 100)]
    public float basicNeed;
    [Range(0, 100)]
    public float luxuryNeed;


    

    [System.Serializable]
    public class ethicNeed
    {
        public string needName;
        [Range(-100, 100)]
        public float varValue;
        [Range(-100, 100)]
        public float constValue;

        [Range(0f, 1f)]
        public float randomValue;
        [Range(-100, 100)]
        public float totalValue;
    }

    public List<ethicNeed> ethicNeedList = new List<ethicNeed>();

    #endregion

    #region powersystem

    public float power;
    [Range(-100, 100)]
    public float popularity;

    #endregion

    #region job

    public int jobTimer;
    public MonoBehaviour occupationScript;

#endregion

    private void Start()
    {
        health = 100;

        MainMenu.EndRoundTick1 += JobSearch;
        MainMenu.EndRoundTick2 += UpdateCitizen;
        MainMenu.EndRoundTick3 += Round3;
        MainMenu.EndRoundTick5 += Round5;
    }


    public void Randomizer()
    {
        for (int i = 0; i < ethicNeedList.Count; i++)
        {
            ethicNeed eN = ethicNeedList[i];

            eN.constValue = Random.Range(-100f, 100f) * eN.randomValue;
        }

        basicNeed = 100;

        UpdateCitizen();
    }

    public void BeingBorn()
    {

    }

    public void Basics()
    {
        genderCounter += 1;
        if(genderCounter > 1)
        {
            genderCounter = 0;
            
        }
        gender = genderCounter;

        
    }

   

    public void SearchPartner()
    {
        if (gender == 1 && partner == null)
        {
            Community com = home.GetComponent<Community>();
            searchList.Clear();

            for (int i = 0; i < com.citizens.Count; i++)
            {
                searchList.Add(com.citizens[i]);
            }

            List<Citizen> removeList = new List<Citizen>();

            for (int i = 0; i < searchList.Count; i++)
            {
                Citizen c = searchList[i];

                if (c.gender == gender)
                {
                    removeList.Add(c);
                }
                else
                {
                    if (c.partner != null)
                    {
                        removeList.Add(c);
                    }
                    else
                    {
                        if (Random.Range(0, 1) < cultureDiscr && c.culture != culture)
                        {
                            removeList.Add(c);
                        }
                        else
                        {
                            if (religiousity > c.religiousity + (religiousDiscr * 100) && religiousity < c.religiousity - (religiousDiscr * 100))
                            {
                                removeList.Add(c);
                            }
                        }

                        
                    }
                }
            }

            for (int i = 0; i < removeList.Count; i++)
            {
                searchList.Remove(removeList[i]);

            }

            if(searchList.Count > 0)
            {
                int index = Random.Range(0, searchList.Count - 1);
                Citizen c = searchList[index];

                partner = c.gameObject;
                c.partner = gameObject;

            }
            

        }
    }

    public void UpdateCitizen()
    {
        #region ethical needs update

        // Ethical Needs

        Community com = home.GetComponent<Community>();

        wealthPart = wealth / com.totalWealth;
       

        float tempFloat =
            - (fear * 0.3f)
            + (education * 0.5f)
            + (basicNeed * 0.5f)
            - (religiousity * 0.35f)
            - (luxuryNeed * 0.35f);


        ethicNeed eN = ethicNeedList[0];
        tempFloat *= 1 - eN.randomValue;

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float SecurityLiberation;

        tempFloat =
            -(fear * 0.5f)
            + (education * 0.7f)
            + (basicNeed * 0.3f)
            - (religiousity * 0.5f)
            - (luxuryNeed * 0.0f);

        tempFloat *= 1 - eN.randomValue;

        eN = ethicNeedList[1];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float PrecautionGrowth;

        tempFloat =
            -(fear * 0.45f)
            + (education * 0.4f)
            + (basicNeed * 0.6f)
            - (religiousity * 0.45f)
            - (luxuryNeed * 0.1f);

        eN = ethicNeedList[2];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float SpiritualismScience;

        tempFloat =
            -(fear * 0.5f)
            + (education * 0.8f)
            + (basicNeed * 0.2f)
            - (religiousity * 0.5f)
            - (luxuryNeed * 0.0f);

        tempFloat *= 1 - eN.randomValue;

        eN = ethicNeedList[3];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float ConflictPeace;

        tempFloat =
            -(fear * 0.7f)
            + (education * 0.8f)
            + (basicNeed * 0.2f)
            - (religiousity * 0.3f)
            - (luxuryNeed * 0.0f);

        tempFloat *= 1 - eN.randomValue;

        eN = ethicNeedList[4];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float PrivilegeSocialjustice;

        tempFloat =
            -(fear * 0.4f)
            + (education * 2f)
            - (basicNeed * 0.6f)
            - (religiousity * 0.6f)
            - (luxuryNeed * 0.4f);

        tempFloat *= 1 - eN.randomValue;

        eN = ethicNeedList[5];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

        //public float IsolationInclusion;

        tempFloat =
            -(fear * 0.8f)
            + (education * 0.5f)
            + (basicNeed * 0.5f)
            - (religiousity * 0.2f)
            - (luxuryNeed * 0.0f);

        tempFloat *= 1 - eN.randomValue;

        eN = ethicNeedList[6];

        eN.varValue = tempFloat;

        eN.totalValue = eN.varValue + eN.constValue;

#endregion

        // Power System

        power = wealthPart * com.citizens.Count;

        // jobsearch

        

    }

    public void JobSearch()
    {
        Community com = home.GetComponent<Community>();

        
        if (com.SearchingForJobList.Contains(this) == false)
        {
            com.SearchingForJobList.Add(this);
        }     
        
    }



    public void Round3()
    {
        Community com = home.GetComponent<Community>();

        jobTimer -= 1;

        if (jobTimer == 0 || occupationScript == null)
        {
            jobTimer = Random.Range(2, 5);
            JobSearch();
        }

    }

    public void Round5()
    {
        if(popularity >= 90)
        {
            float donate = wealth * Random.Range(0.01f, 0.04f);
            Character.character.treasure += Mathf.RoundToInt(donate);
        }

        popularity -= 30;

        
    }

    public void Die()
    {
        health = 0;
        home.GetComponent<Community>().citizens.Remove(this);
        transform.parent = GameObject.Find("Graveyard").transform;
        occupation = null;
        occupationScript = null;
    }

}
