using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTargeting : Photon.MonoBehaviour {

    SphereCollider col;
    public float size;
    public int minMissile;
    public GameObject prefab;
    public Vector3 spawnPos;
    public int tempInt;
    public GameObject owner;

    public int teamInt;
    public float damage;
    public float impactUp;
    public float impactBack;

    public float speed;
    public float missSpeed;
    public float maxStr;

    public List<GameObject> targetList = new List<GameObject>();

    private void Start()
    {
        if (!photonView.isMine)
            return;

        col = GetComponent<SphereCollider>();
        spawnPos = transform.position;
        

    }

    private void Update()
    {
        if (!photonView.isMine) return;

        if(col.radius <= size)
        {
            col.radius += 0.3f;
            if(col.radius >= size)
            {
                ShootTest();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        GameObject somebody = other.gameObject;



        if (somebody.layer == LayerMask.NameToLayer("Character"))
        {
            if (somebody != owner && somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt)
            {


                if (somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt)
                {
                    if (targetList.Contains(other.gameObject) == false)
                    {
                        targetList.Add(other.gameObject);
                    }
                }

            }
            
        }
    }

    public void ShootTest()
    {
        if (!photonView.isMine) return;


        if (targetList.Count > 0)
        {
            

            minMissile += targetList.Count;

            float tempFloat = 0;
            

            for(int i = 0; i < minMissile; i++)
            {


                tempFloat += 15;
                GameObject missile = PhotonNetwork.Instantiate(prefab.transform.name, spawnPos, Quaternion.Euler(260, tempFloat, 0), 0);

                ProjectileScript mS = missile.GetComponent<ProjectileScript>();

                mS.speed = missSpeed;
                mS.currSpeed = Random.Range(7,15);

                tempInt = Random.Range(0, targetList.Count);

                mS.target = targetList[tempInt];
                mS.targetTrans = mS.target.transform;
                mS.str = 2;
                mS.maxStr = maxStr;

                mS.birthTimer = 0.2f;
                mS.lifetime = 25;
                mS.damage = damage;
                mS.source = owner;
                mS.teamInt = teamInt;
                mS.ImpactBack = impactBack;
                mS.ImpactUp = impactUp;
                mS.diesOnImpact = true;
                mS.homingMiss = true;
                

            }
        }
    }
}
