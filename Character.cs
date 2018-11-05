using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class Character : MonoBehaviour {

    // This script could also be called "Monster" for Monster Combat
    // Functions are the basic SetUp, Positioning, Clicking Behavior, Damage and Heal

    // Basics

    public GameObject tilePos;
    public int xPos;
    public int yPos;
    private GameManager gM;
    public Creature creature;
    private Material mat;

    // Skills

    public int range;
    public int attack;
    public int defense;

    [System.Serializable]
    public class skillStats
    {
        public Sprite sIcon;
        public string sName;
        [TextArea]
        public string sDescr;
        public int sIconIndex;
        public int cost;
        public int range;
        public List<string> targetList = new List<string>();
    }

    public List<skillStats> sSList = new List<skillStats>();

    // Stats
    
    public int speed;
    public float speedCounter;
    public int maxHealth;
    public int health;
    public int maxAction;
    public int action;

    // Character

    public Sprite cIcon;
    public string cName;
    public int team;
    public Player owner;
    public List<string> accessList = new List<string>();
    public CharPanel cP;
    public string bonusTile;

    private void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
        creature = GetComponent<Creature>();
        mat = transform.Find("ColorCircle").gameObject.GetComponent<Renderer>().material;

        SetUp();
    }

    public void SetUp()
    {
        health = maxHealth;
        action = 0;        
        gM.charList.Add(this);
        owner = gM.playerList[team - 1];
        owner.charList.Add(this);
        owner.pP.SetUpChar(this);

        mat.color = owner.pColor;
        
        
    }

    public void Positioning()
    {
        Tile t = gM.tileList.Where(x => x.xPos == xPos && x.yPos == yPos).SingleOrDefault();
        Tile start = null;
        if (tilePos != null)
        {
            start = tilePos.GetComponent<Tile>();
        }
        
        tilePos = t.gameObject;

        Vector3 pos = tilePos.transform.position;
        pos.y = 2.5f;

        transform.position = pos;
        if(tilePos.GetComponent<Tile>() != null)
        {
            tilePos.GetComponent<Tile>().refChar = null;
        }
        if(start != null)
        {
            start.refChar = null;
        }
        
        t.refChar = this;       
    }

    public void Damage(int dmg, GameObject eff, Character source)
    {
        if(gM.GameOver == true)
        {
            return;
        }
        health -= dmg;
        Vector3 pos = transform.position;
        pos.y = 3;
        GameObject obj = Instantiate(gM.prefabList[0]);
        obj.transform.position = pos;
        DamageText dT = obj.GetComponent<DamageText>();
        dT.txt.text = "-" + dmg;
        dT.lifeTime = 1.5f;

        obj = Instantiate(eff);
        obj.transform.position = pos;
        Destroy(obj, 1);

        if(health <= 0)
        {
            health = 0;
            pos = gM.graveyard.transform.position;
            transform.position = pos;
            Tile t = tilePos.GetComponent<Tile>();
            t.refChar = null;
           

            gM.CheckState(this);
        }

        foreach(Player p in gM.playerList)
        {
            p.pP.UpdatePanel();
        }
    }

    public void OnMouseDown()
    {
        if (gM.playChar != null)
        {
            Creature c = gM.playChar.creature;
            Tile t = tilePos.GetComponent<Tile>();
            t.Clicked();           
        }
    }

    public void Heal(int dmg, GameObject eff, Character source)
    {
        health += dmg;
        Vector3 pos = transform.position;
        pos.y = 3;
        GameObject obj = Instantiate(gM.prefabList[0]);
        obj.transform.position = pos;
        DamageText dT = obj.GetComponent<DamageText>();
        dT.txt.color = Color.green;
        dT.txt.text = "+" + dmg;
        dT.lifeTime = 1.5f;

        obj = Instantiate(eff);
        obj.transform.position = pos;
        Destroy(obj, 1);

        if (health > maxHealth)
        {
            health = maxHealth;
                      
        }

        foreach (Player p in gM.playerList)
        {
            p.pP.UpdatePanel();
        }
    }
}
