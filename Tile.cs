using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;


public class Tile : MonoBehaviour {

    // This Script sets up the different Tiles which make up the playing field. 

    public GameObject prefab;
    private GameManager gM;
    public int xPos;
    public int yPos;
    public Character refChar;
    public int range;

    public tileSurface tSurf;

    [System.Serializable]
    public class tileSurface
    {
        public GameObject obj;
        public float baseHeight;
        public string type;
    }

    
    private void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();

        SetUpTile();
    }

    public void SetUpTile()
    {
        if (prefab == null)
        {
            int index = Random.Range(0, gM.tileSetList[gM.tileSetList.Count - 1].maxProp);

            GameManager.tileSet tS = gM.tileSetList.Where(x => x.minProp <= index && x.maxProp > index).SingleOrDefault();
            prefab = tS.prefab;

            Vector3 pos = transform.position;
            pos.y = 1.6f;
            Quaternion rot = transform.rotation;

            GameObject obj = Instantiate(prefab, pos, rot);
            obj.transform.parent = transform;

            //================
            tSurf.obj = obj;
            tSurf.baseHeight = obj.transform.position.y;
            tSurf.type = tS.type;
            obj.GetComponent<Surface>().mother = this;
        }
    }

    public void HighLight()
    {
        Vector3 pos = tSurf.obj.transform.position;
        pos.y += gM.setHeight;
        tSurf.obj.transform.position = pos;
        
        if(refChar != null)
        {
            pos = refChar.transform.position;
            pos.y += gM.setHeight;
            refChar.transform.position = pos;
        }
        
    }

    public void EndHighlight()
    {
        Vector3 pos = tSurf.obj.transform.position;
        pos.y = tSurf.baseHeight;
        tSurf.obj.transform.position = pos;
        range = 0;
        if (refChar != null)
        {
            pos = refChar.transform.position;
            pos.y = 2.5f;
            refChar.transform.position = pos;
        }
    }


    public void Clicked()
    {
        if(gM.playChar != null)
        {
            Creature c = gM.playChar.creature;
            if(c != null)
            {              
                    if (c.phase == 1 && range > 0 && this != gM.playChar.tilePos)
                    {
                        c.phase = 2;
                        c.targetTile = this;
                        c.DoChara(c.order);
                    }
                    else
                    {
                        gM.WarningText("Not a valid target!", 2);
                    }                               
            }
        }
    }

    public void DestroySurface()
    {
        Destroy(tSurf.obj);

        Vector3 pos = transform.position;
        pos.y = 1.6f;
        Quaternion rot = transform.rotation;

        GameObject obj = Instantiate(gM.tileSetList[0].prefab, pos, rot);

        tSurf.obj = obj;
        tSurf.type = "grass";
        prefab = obj;
        obj.transform.parent = transform;
        obj.GetComponent<Surface>().mother = this;
    }
}
