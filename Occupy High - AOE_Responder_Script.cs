using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Responder_Script : MonoBehaviour {

    public GameObject respondTarget;
    public Collider Thiscollider;
    public float speed;
    public AOE_Effect_Script AOES;

    public Transform respondTransform;
    

    private void Start()
    {
        respondTarget = transform.parent.gameObject;
        AOES = respondTarget.GetComponent<AOE_Effect_Script>();
        respondTransform = respondTarget.transform;

        Thiscollider = GetComponent<CapsuleCollider>();
        
        
    }

    private void Update()
    {
        transform.RotateAround(respondTransform.position, Vector3.up, speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != respondTarget)
        {
            
            AOES.FoundColl(other.gameObject, gameObject);
        }
    }

    
}
