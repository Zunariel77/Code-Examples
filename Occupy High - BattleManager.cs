using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Photon.MonoBehaviour {

    public List<GameObject> txtList = new List<GameObject>();
    public GameObject blockText;
    public GameObject brokenText;

    void Start ()
    {
		
	}
	
	public void HitText(float impactUp, float impactBack, float damage, GameObject bodyPart)
    {
        Vector3 bpSpot = bodyPart.transform.position;      
        GameObject txt = null;
        float impactVal = impactUp + impactBack;
       

        if (impactVal < 10)
        {
            txt = txtList[0];
        }
        else
        {
            if (impactVal < 15)
            {
                txt = txtList[1];
            }
            else
            {
                if (impactVal < 20)
                {
                    txt = txtList[2];
                }
                else
                {
                    if (impactVal < 25)
                    {
                        txt = txtList[3];
                    }
                    else
                    {
                        if (impactVal < 100)
                        {
                            txt = txtList[4];
                        }
                    }
                }
            }
        }

        if (txt != null)
        {
            
            PhotonNetwork.Instantiate(txt.transform.name, bpSpot += new Vector3(1, 1, 0), Quaternion.identity, 0);
        }
    }

    public void SpecialText(float impactUp, float impactBack, float damage, GameObject bodyPart)
    {
        Vector3 bpSpot = bodyPart.transform.position;
        GameObject txt = null;
        float impactVal = impactUp + impactBack;


        if (impactVal < 10)
        {
            txt = txtList[0];
        }
        else
        {
            if (impactVal < 15)
            {
                txt = txtList[1];
            }
            else
            {
                if (impactVal < 20)
                {
                    txt = txtList[2];
                }
                else
                {
                    if (impactVal < 25)
                    {
                        txt = txtList[3];
                    }
                    else
                    {
                        if (impactVal < 100)
                        {
                            txt = txtList[4];
                        }
                    }
                }
            }
        }

        if (txt != null)
        {

            PhotonNetwork.Instantiate(txt.transform.name, bpSpot += new Vector3(1, 1, 0), Quaternion.identity, 0);
        }
    }

    public void BlockText(GameObject bodypart)
    {
        if(bodypart == null){return;}

        Vector3 bpSpot = bodypart.transform.position;
        PhotonNetwork.Instantiate(blockText.transform.name, bpSpot += new Vector3(1, 1, 0), Quaternion.identity, 0);
    }

    public void BrokenText(GameObject bodypart)
    {
        if (bodypart == null) { return; }

        Vector3 bpSpot = bodypart.transform.position;
        PhotonNetwork.Instantiate(brokenText.transform.name, bpSpot += new Vector3(1,1,0), Quaternion.identity, 0);
    }
}
