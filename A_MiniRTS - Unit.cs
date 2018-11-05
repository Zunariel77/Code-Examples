using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    // This script controls the Units in my game MiniRTS
    // It sets up its "AI" with 1 sense which is a collider
    // It finds a target, tries to get closer and then attacks
    // it based on its unit type

    public NavMeshAgent agent;
    public float actionTime = 1;
    public float timer;
    public int team;
    public Unit target;
    public int type; // 1 = Legionaire, 2 = Speer Soldier, 3 = Archer
    public float damage;
    public float range = 3;
    public Transform fortress;
    public float health;

    public Color baseColor;
    public Color damageColor;
    public float dmgTimer;
    public Material mat;
    public int attackRate;
    public GameObject arrow;
    public float shootPower = 1;
	
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        mat = GetComponent<Renderer>().material;
	}
		
	void Update () {
        if (dmgTimer > 0)
        {
            dmgTimer -= 1 * Time.deltaTime;
            if (dmgTimer <= 0)
            {
                mat.color = baseColor;
            }
        }

        if (type == 0) return;

		if(timer > 0)
        {
            timer -= 1 * Time.deltaTime;
        }
        else
        {
            timer = actionTime;
            Action();
        }
        
        if(target != null)
        {
            if (target.health <= 0)
            {
                target = null;
                return;
            }

            Vector3 thisPos = transform.position;
            Vector3 point = target.transform.position;
            float dist = Vector3.Distance(thisPos, point);
            
            if(dist > range && type == 3)
            {
                target = null;
            }
        }       
    }

    public void Action()
    {
        if(fortress != null)
        {
            if(target == null)
            {
                agent.isStopped = false;
                agent.SetDestination(fortress.position);
            }
            else if(target != null)
		    {
                    Vector3 thisPos = transform.position;
                    Vector3 point = target.transform.position;
                    float dist = Vector3.Distance(thisPos, point);
                    
                    if(dist >= range)
                    {
                        agent.SetDestination(point);
                    }
                    else
                    {
                        agent.isStopped = true;
                        transform.LookAt(target.transform);

                        if(type != 3)
                        {
                            target.Damage(damage, this);                            
                        }
                        else if(attackRate <= 0)
                            {
                                attackRate = 3;
                                Shoot();
                            }
                            else
                            {
                                attackRate -= 1;
                            }       
                                              
                    }                   
                }          
                      
        }
    }

    public void Damage(float damage, Unit source)
    {       
        health -= damage;
        
        if(type == 1 && source.type == 2) // Speer vs Legionaire
        {
            damage *= 2f;
        }
        else if (type == 3 && source.type == 2) // Speer vs Archer
        {
            damage *= 0.5f;
        }
        else if (type == 1 && source.type == 3) // Archer vs Legionaire
        {
            damage *= 0.15f;
        }
        else if (type == 2 && source.type == 3) // Archer vs Speer
        {
            damage *= 2f;
        }
        else if (type == 2 && source.type == 1) // Legionaire vs Speer
        {
            damage *= 0.5f;
        }
        else if (type == 3 && source.type == 1) // Legionaire vs Archer
        {
            damage *= 3f;
        }
        else if (type == 0 && source.type == 1) // Legionaire vs Fortress
        {
            damage *= 1f;
        }
        else if (type == 0 && source.type == 2) // Speer Soldier vs Fortress
        {
            damage *= 1f;
        }
        else if (type == 0 && source.type == 3) // Archer vs Fortress
        {
            damage *= 0.25f;
        }
        
        dmgTimer = 0.35f;
        mat.color = damageColor;

        if(health <= 0)
        {
            if(type == 0)
            {
                GetComponent<Fortress>().Defeat(this);
            }
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        Vector3 thisPos = transform.position;
        thisPos.z += 0.1f;
        Vector3 point = target.transform.position;

        float dist = Vector3.Distance(thisPos, point);

        GameObject obj = Instantiate(arrow, thisPos, Quaternion.identity);

        Arrow arr = obj.GetComponent<Arrow>();
        arr.team = team;
        arr.shootPower = shootPower;
        arr.damage = damage;
        arr.source = this;

        arr.Shoot(dist, transform.rotation);       
    }
}
