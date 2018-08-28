using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Effect_Script : Photon.MonoBehaviour {

    public GameObject source;
    public int teamInt;
    public float damage;
    public float ImpactUp;
    public float ImpactBack;
    public float speed;
    public float birthTimer = 0.4f;

    public float lifetime = 1f;
    private float tempFloat;
    private float tempFloat2;

    public List<GameObject> hitList = new List<GameObject>();
    public GameObject responderObj;
    public GameObject DeathEffect;
    public float particleTimer;

    private void Start()
    {
        if (!photonView.isMine) return;
        responderObj.GetComponent<AOE_Responder_Script>().speed = speed;
    }

    private void Update()
    {
        if (!photonView.isMine) return;

        if(birthTimer <= 0)
        {
            if(lifetime > 0)
            {
                lifetime -= 1 * Time.deltaTime;

            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            birthTimer -= 1 * Time.deltaTime;
        }
    }

    public void FoundColl(GameObject colObj, GameObject sourceCol)
    {
        if (!photonView.isMine) return;

        GameObject somebody = colObj.gameObject;

        if (somebody.layer == LayerMask.NameToLayer("Character"))
        {
            
            if (somebody != source && somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt)
            {

                if (somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt)
                {
                    if(hitList.Contains(somebody) == false)
                    {
                        hitList.Add(somebody);

                        Fighter_Stats_Script fSS = somebody.GetComponent<Fighter_Stats_Script>();

                        Vector3 fromPosition = source.transform.position;
                        Vector3 toPosition = somebody.transform.position;
                        Vector3 direction = toPosition - fromPosition;


                        Ray landingRay = new Ray(fromPosition, direction);

                        if (DeathEffect != null)
                        {
                            GameObject ParticlePrefab = PhotonNetwork.Instantiate(DeathEffect.transform.name, sourceCol.transform.position, Quaternion.Euler(0, 0, 0), 0);

                            ParticlePrefab.GetComponent<SubEmitterScript>().photonView.RPC("KillObject", PhotonTargets.All, particleTimer);
                        }

                        fSS.DamageTarget(damage, direction, ImpactUp, ImpactBack, source, responderObj, null);

                    }
                }
            }
        }
    }
}
