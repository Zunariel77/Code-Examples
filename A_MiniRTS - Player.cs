using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    public string pName;
    public Fortress fortress;
    public Transform enemy;
    public Color baseColor;
    public Unit playerUnit;
    public GameObject unit;
    public float timer;
    public int team;
    public float actionTime = 1;
    
    public bool gameOver;
    public float startFacing;
    public Slider healthBar;

    public List<GameObject> unitList = new List<GameObject>();
    public List<Image> imgList = new List<Image>();

    void Start()
    {
        SliderChange(1);
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= 1 * Time.deltaTime;
        }
        else if (gameOver == false)
        {
            timer = actionTime;
            Spawn();
        }

        healthBar.value = playerUnit.health;
    }

    public void Spawn()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(pos.x - 0.5f, pos.x + 0.5f);

        GameObject obj = Instantiate(unit, pos, Quaternion.identity);

        Unit spawn = obj.GetComponent<Unit>();

        spawn.agent = spawn.GetComponent<NavMeshAgent>();
        spawn.mat = spawn.GetComponent<Renderer>().material;
        spawn.baseColor = baseColor;
        spawn.mat.color = baseColor;
        spawn.fortress = enemy;
        spawn.team = team;
        spawn.transform.Rotate(0, startFacing, 0);
    }

    public void SliderChange(int sV)
    {
        foreach(Image img in imgList)      
        {            
            img.color = new Color(1, 1, 1, 0.5f);
        }

        imgList[sV].color = new Color(1, 1, 1, 1f);
        unit = unitList[sV];
        
    }
}
