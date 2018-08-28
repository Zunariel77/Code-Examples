using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommunitySetUp : MonoBehaviour {

    public float startWealth;
    public int startPop;
    public GameObject prefab;
    public GameObject civPool;

    public float cultureDiscr;    
    public float religiousDiscr;

    public List<CommunityCulture> cList = new List<CommunityCulture>();

    [System.Serializable]
    public class CommunityCulture
    {
        public Culture cCult;
        public float cPop;
        public float cWealth;
        public float cWKey;
        public int cAge;
        public float cMarriage;
        public int education;
        public List<Citizen> citList = new List<Citizen>();
        public int faithMin;
        public int faithMax;
    }

    
    private Community com;

    private void Start()
    {
        com = GetComponent<Community>();

        com.totalWealth = startWealth;

        for(int i = 0; i < cList.Count; i++)
        {
            CommunityCulture cc = cList[i];
            Culture cult = cc.cCult;

            cc.cPop *= startPop;
            cc.cWealth *= startWealth;
            cc.cMarriage *= startPop;

            if(com.residentCultures.Contains(cult) == false)
            {
                com.residentCultures.Add(cult);
            }

            float cPop = cList[i].cPop;

            for (int a = 0; a < cPop; a++)
            {
                GameObject citizen = Instantiate(prefab);
                citizen.transform.SetParent(civPool.transform);
                Citizen c = citizen.GetComponent<Citizen>();
                c.culture = cult;

                c.home = gameObject;
                com.citizens.Add(c);

                c.education = Random.Range(1, cc.education);
                cc.citList.Add(c);

                c.cultureDiscr = cultureDiscr;
                c.religiousDiscr = religiousDiscr;

                c.religiousity = Random.Range(cc.faithMin, cc.faithMax);

                c.Basics();
                c.Randomizer();
            }

            float total = 0;
            float partnerSearch = cc.cMarriage;

            for(int b = 0; b < cc.citList.Count; b++)
            {
                Citizen c = cc.citList[b];

                c.wealthPart = Random.Range(0, cc.cWKey);
                total += c.wealthPart;

                c.age = Random.Range(18, cc.cAge);

                if (partnerSearch > 0)
                {
                    c.onSearch = true;
                    partnerSearch -= 1;
                }
                
            }

            for (int b = 0; b < cc.citList.Count; b++)
            {
                Citizen c = cc.citList[b];

                c.wealthPart /= total;
                c.wealth += c.wealthPart * cc.cWealth;
            }


        }

        for(int c = 0; c < com.citizens.Count; c++)
        {
            Citizen cit = com.citizens[c];

            if(cit.onSearch)
            {
                Debug.Log("Searching!");
                cit.SearchPartner();
            }

            cit.power = cit.wealthPart * com.citizens.Count;
            

        }
    }

   
}
