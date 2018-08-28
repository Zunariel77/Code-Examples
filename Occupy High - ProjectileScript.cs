using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : Photon.MonoBehaviour {

    public Vector3 shootingdirection;
    public GameObject source;
    public int teamInt;
    public float damage;
    public float ImpactUp;
    public float ImpactBack;
    public float speed = 10;
    public float birthTimer = 0.4f;
    public GameObject DeathEffect;
    public float particleTimer;
    public bool diesOnImpact;
    public float lifepoints = 5;

    public float lifetime = 100f;
    private float tempFloat;
    private float tempFloat2;

    public bool homingMiss;
    public GameObject target;
    public float currSpeed;
    public Transform targetTrans;
    public bool isKajok;

    public Vector3 pos;
    public Quaternion newRot;
    public Quaternion orRot;

    public float str;
    public float maxStr;
    public GameObject colTrans;

    private float targetFloat;
    private Quaternion rot;


    void Start()
    {
        if (!photonView.isMine)
        {


            return;
        }


           
        targetFloat = 2f;
        currSpeed = 10;

    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(particleTimer);
            stream.SendNext(isKajok);
            stream.SendNext(lifepoints);
            stream.SendNext(rot);
            stream.SendNext(teamInt);
        }
        else
        {
            particleTimer = (float)stream.ReceiveNext();
            isKajok = (bool)stream.ReceiveNext();
            lifepoints = (float)stream.ReceiveNext();
            rot = (Quaternion)stream.ReceiveNext();
            teamInt = (int)stream.ReceiveNext();
        }
    }

    void Update ()
    {
        if (!photonView.isMine)
        {
            
            transform.rotation = rot;


            return;
        }
           

        if (currSpeed < speed)
        {
            currSpeed += 1f * Time.deltaTime;
            
        }
        else
        {
            if (currSpeed > speed)
            {
                currSpeed = speed;
            }
        }

        if (homingMiss == false)
        {
            if (birthTimer <= 0)
            {
                transform.position += shootingdirection * currSpeed * Time.deltaTime;

            }
            else
            {
                birthTimer -= 1 * Time.deltaTime;
            }

            lifetime -= 1 * Time.deltaTime;

            if (lifetime <= 0)
            {

                PhotonNetwork.Destroy(gameObject);
            }            

            if (shootingdirection != Vector3.zero)
            {
                newRot = Quaternion.LookRotation(shootingdirection);
            }


            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 10);
            rot = transform.rotation;
        }
        else
        {
            if (str < maxStr)
            {
                str += 2 * Time.deltaTime;
                if (str > maxStr)
                {
                    str = maxStr;
                }
            }
           

            if (targetFloat > -1)
            {
                targetFloat -= 0.5f * Time.deltaTime;
            }


            Vector3 targetVector = targetTrans.position - transform.position;
            targetVector.y += targetFloat;

            newRot = Quaternion.LookRotation(targetVector);


            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, str * Time.deltaTime);

            transform.Translate(Vector3.forward * currSpeed * Time.deltaTime);
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            rot = transform.rotation;
        }

        if(lifepoints <= 0)
        {
            photonView.RPC("Dies", PhotonTargets.All);
        }

        
        

        
    }

    [PunRPC]
    public void UpdateRotation(Quaternion q)
    {
        transform.rotation = q;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        GameObject somebody = other.gameObject;        

        if (somebody.layer == LayerMask.NameToLayer("Character") && teamInt >= 1)
        {
            if (somebody != source && somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt )
            {
                if (somebody.GetComponent<Fighter_Stats_Script>() != null && teamInt != somebody.GetComponent<Fighter_Stats_Script>().teamInt)
                {
                    Fighter_Stats_Script fSS = somebody.GetComponent<Fighter_Stats_Script>();

                    Vector3 fromPosition = transform.position;
                    Vector3 toPosition = somebody.transform.position;
                    Vector3 direction = toPosition - fromPosition;

                    
                    Vector3 pos = other.transform.position;


                    if (DeathEffect != null && (diesOnImpact == false || isKajok))
                    {
                        GameObject ParticlePrefab = PhotonNetwork.Instantiate(DeathEffect.transform.name, fromPosition, Quaternion.Euler(0, 0, 0), 0);

                        ParticlePrefab.GetComponent<SubEmitterScript>().photonView.RPC("KillObject", PhotonTargets.All, particleTimer);
                    }

                    Ray landingRay = new Ray(fromPosition, direction);


                    fSS.DamageTarget(damage, direction, ImpactUp, ImpactBack, source, gameObject, null);

                    if (diesOnImpact)
                    {
                        KillParticle(somebody);
                    }
                }
            }
        }
        else
        {
            if (somebody.tag != "Ignore" && teamInt >= 1)
            {
                if (somebody.GetComponent<ProjectileScript>() != null)
                {
                    if (somebody.GetComponent<ProjectileScript>().teamInt != teamInt)
                    {
                        KillParticle(somebody);
                    }
                }
                else
                {
                    KillParticle(somebody);
                }
            }
            else
            {
                photonView.RPC("Dies", PhotonTargets.All);
            }
                        
        }

    }

    public void KillParticle(GameObject col)
    {
        if (!photonView.isMine) return;

        if (col.GetComponent<ProjectileScript>() != null)
        {
            lifepoints -= col.GetComponent<ProjectileScript>().damage;
            
        }
        else
        {
            lifepoints -= damage;
        }

        if (currSpeed > speed * 0.5f)
        {
            currSpeed -= speed * 0.4f;
        }

        if ((lifepoints <= 0 || lifetime <= 0 || col.layer == LayerMask.NameToLayer("Battle Field")))
        {
            photonView.RPC("Dies", PhotonTargets.All);
            
        }               
    }

    [PunRPC]
    public void Dies()
    {
        if(DeathEffect != null && !isKajok)
        {
            GameObject ParticlePrefab = Instantiate(DeathEffect, transform.position, Quaternion.Euler(0, 0, 0));

            ParticlePrefab.transform.localScale = transform.localScale;

            Destroy(ParticlePrefab, particleTimer);
        }        

        if(photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        
    }

   
}
