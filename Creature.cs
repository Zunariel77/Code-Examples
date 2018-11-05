using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Creature : MonoBehaviour {

    public Character refChar;
    public string cName;

    public int phase;
    public int order;

    public Tile targetTile;
    public List<Tile> highTileList = new List<Tile>();
    public List<Tile> utiliList = new List<Tile>();
    
    public List<string> accessList = new List<string>();
    private GameManager gM;

    private Vector3 startPoint;
    private Vector3 targetPoint;

    private float speed;
    private float startTime;
    private float journeyLength;

    public float ActionTime;
    public int actionIndex;
    public int steps;
    public Animator anim;
    private GameObject actor;
    private Tile theTarget;
    private int tempInt;

    public List<GameObject> prefabList = new List<GameObject>();

    public void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
        refChar = GetComponent<Character>();
        anim = GetComponent<Animator>();
    }

    public void DoChara(int o)
    {        
        if(order != o && phase == 1)
        {
            phase = 0;
            gM.EndHighlight();
        }

        order = o;

        if(gM.gamePhase == 1)
        {           
            Placement();
        }
        else if (cName == "Dracula")
        {
            if(order == 0)
            {
                accessList.Clear();
                foreach(string s in refChar.accessList)
                {
                    accessList.Add(s);
                }
                Movement(order);
                foreach(Tile t in highTileList)
                {
                    if(refChar.accessList.Contains(t.tSurf.type) == false)
                    {
                        t.range = 0;
                        t.EndHighlight();
                    }
                }
            }
            else if(order == 1)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 2;
                Attack(order);
            }
            else if (order == 2)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 8;
                Attack(order);
            }
            else if (order == 3)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 9;
                Attack(order);
                if(phase == 1)
                {
                    gM.EndHighlight();
                    foreach(Tile t in gM.tileList)
                    {
                        if(t.tSurf.type == "grass" && t.refChar == null)
                        {
                            t.HighLight();
                            t.range = 1;
                        }
                    }
                    refChar.tilePos.gameObject.GetComponent<Tile>().HighLight();
                }
            }
        }
        else if(cName == "Werewolf")
        {
            if(order == 0)
            {
                accessList.Clear();
                foreach (string s in refChar.accessList)
                {
                    accessList.Add(s);
                }
                Movement(order);
                foreach (Tile t in highTileList)
                {
                    if (refChar.accessList.Contains(t.tSurf.type) == false)
                    {
                        t.range = 0;
                        t.EndHighlight();
                    }
                }
            }
            else if(order == 1)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 2;
                Attack(order);
            }
            else if (order == 2)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 6;
                Attack(order);
            }
            else if (order == 3)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 7;
                Attack(order);
            }
        }
        else if (cName == "Cthulhu")
        {
            if (order == 0)
            {
                accessList.Clear();
                foreach (string s in refChar.accessList)
                {
                    accessList.Add(s);
                }
                Movement(order);
                foreach (Tile t in highTileList)
                {
                    if (refChar.accessList.Contains(t.tSurf.type) == false)
                    {
                        t.range = 0;
                        t.EndHighlight();
                    }
                }
            }
            else if (order == 1)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 3;
                Attack(order);
            }
            else if (order == 2)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 4;
                Attack(order);
            }

            else if (order == 3)
            {
                accessList.Clear();
                foreach (string s in refChar.sSList[o].targetList)
                {
                    accessList.Add(s);
                }
                actionIndex = 5;
                Attack(order);
            }
        }
        
        
    }

    public void Movement(int o)
    {


        if (phase == 0)
        {
            Player p = refChar.owner;
            p.pP.tt.gameObject.SetActive(true);
            p.pP.tt.title.text = refChar.sSList[o].sName;
            p.pP.tt.cost.text = "Cost: " + refChar.sSList[o].cost.ToString();
            p.pP.tt.descr.text = refChar.sSList[o].sDescr;
            phase = 1;

            targetTile = refChar.tilePos.GetComponent<Tile>();

            int r = refChar.sSList[o].range;
            if (r > refChar.action)
            {
                r = refChar.action;
            }

            HighLight(r);
        }
        else if (phase == 1)
        {
            Player p = refChar.owner;
            p.pP.tt.gameObject.SetActive(false);
            gM.EndHighlight();
            phase = 0;
        }
        else if (phase == 2)
        {
            if (refChar.action >= refChar.sSList[o].cost)
            {
                phase = 0;
                order = 0;
                refChar.action -= targetTile.range;
                refChar.xPos = targetTile.xPos;
                refChar.yPos = targetTile.yPos;
                Player p = refChar.owner;


                gM.EndHighlight();

                startPoint = transform.position;
                targetPoint = targetTile.transform.position;
                targetPoint.y = 2.5f;

                speed = 4;
                startTime = 0;
                journeyLength = 0;

                startTime = Time.time;
                journeyLength = Vector3.Distance(startPoint, targetPoint);

                ActionTime = 2;
                actionIndex = 1;

                p.pP.UpdatePanel();


            }
            else
            {
                gM.WarningText("Not enough Action Points!", 2);
                phase = 1;
            }


            
        }

    }

    public void Attack(int o)
    {
        
        if (phase == 0)
        {
            
            Player p = refChar.owner;
            p.pP.tt.gameObject.SetActive(true);
            p.pP.tt.title.text = refChar.sSList[o].sName;
            p.pP.tt.cost.text = "Cost: " + refChar.sSList[o].cost.ToString();
            p.pP.tt.descr.text = refChar.sSList[o].sDescr;
            phase = 1;

            targetTile = refChar.tilePos.GetComponent<Tile>();

            int r = refChar.sSList[o].range;
            if (r > refChar.action)
            {
                r = refChar.action;
            }
            
            HighLight(r);
            if (refChar.sSList[o].targetList.Contains("grass") != true)
            {
                for (int i = highTileList.Count - 1; i >= 0; i--)
                {
                    Tile t = highTileList[i];
                    if (t.refChar == null)
                    {
                        if (t.tSurf.type == "grass" || t.tSurf.type == "water")
                        {
                            highTileList.Remove(t);
                        }
                    }
                    else if (t.refChar.team == refChar.team && accessList.Contains("friend") == false)
                    {
                        highTileList.Remove(t);
                    }

                }
                gM.EndHighlight();
            }


            foreach (Tile t in highTileList)
            {
                t.range = 1;
                t.HighLight();
            }
        }
        else if (phase == 1)
        {
            Player p = refChar.owner;
            p.pP.tt.gameObject.SetActive(false);
            gM.EndHighlight();
            phase = 0;
        }
        else if (phase == 2)
        {
            if (refChar.action >= refChar.sSList[o].cost)
            {
                phase = 0;
                order = 0;
                refChar.action -= refChar.sSList[o].cost;
                refChar.xPos = targetTile.xPos;
                refChar.yPos = targetTile.yPos;
                Player p = refChar.owner;


                gM.EndHighlight();

                targetPoint = targetTile.transform.position;

                Vector3 lookDir = targetPoint - transform.position;
                lookDir.y = 0;

                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = rot;

                ActionTime = 5;


                steps = 5;

                p.pP.UpdatePanel();
            }
            else
            {
                gM.WarningText("Not enough Action Points!", 2);
                phase = 1;
                
            }

            
        }
        

    }

    public void Update()
    {
        if(ActionTime > 0)
        {
            ActionTime -= Time.deltaTime * 1;

            if(actionIndex == 1)
            {
                Moving();
            }
            else if(actionIndex == 2)
            {
                Attacking_Drac();
            }
            else if (actionIndex == 3)
            {
                Attacking_Cthu();
            }
            else if (actionIndex == 4)
            {
                DarkRain();
            }
            else if (actionIndex == 5)
            {
                Necronomicon();
            }
            else if (actionIndex == 6)
            {
                TotalFury();
            }
            else if(actionIndex == 7)
            {
                Guillotine();
            }
            else if (actionIndex == 8)
            {
                Bloodthirst();
            }
            else if (actionIndex == 9)
            {
                AlmightyEye();
            }

            if (actionIndex == 0)
            {
                
                Player p = refChar.owner;
                ActionTime = 0;
                p.pP.UpdatePanel();
                if (refChar.action == 0)
                {
                    gM.EndTurn();
                }
            }
        }
    }

    public void HighLight(int r)
    {

        
        
        if (targetTile == null) {
            
            return;
        }
        gM.audioList[0].Play();
        

        int x = targetTile.xPos;
        int y = targetTile.yPos;

        highTileList.Clear();

        highTileList.Add(targetTile);

        for(int i = 0; i < r; i++)
        {
            
            foreach (Tile t in highTileList)
            {
                int tX = t.xPos;
                int tY = t.yPos;
                bool access = false;
                
                

                Tile found = gM.tileList.Where(X => X.xPos == tX + 1 && X.yPos == tY).SingleOrDefault();
                if(found != null)
                {


                    if (gM.Game == true)
                    {
                        if ((accessList.Contains(found.tSurf.type) && found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                        else if ((accessList.Contains("enemy") && found.refChar != null))
                        {
                            if (found.refChar.team != refChar.team || accessList.Contains("friend") == true)
                            {
                                if (highTileList.Contains(found) == false)
                                {
                                    access = true;
                                }
                            }
                        }
                        else if (((found.tSurf.type == "grass" || found.tSurf.type == "water")) && (found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                    }
                    else
                    {
                        if (highTileList.Contains(found) == false)
                        {
                            access = true;
                        }
                    }

                    if (access == true)
                    {
                        utiliList.Add(found);
                    }
                }

                access = false;

                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if(gM.Game == true)
                    {
                        if ((accessList.Contains(found.tSurf.type) && found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                        else if ((accessList.Contains("enemy") && found.refChar != null))
                        {
                            if (found.refChar.team != refChar.team || accessList.Contains("friend") == true)
                            {
                                if (highTileList.Contains(found) == false)
                                {
                                    access = true;
                                }
                            }
                        }
                        else if (((found.tSurf.type == "grass" || found.tSurf.type == "water")) && (found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                    }
                    else
                    {
                        if (highTileList.Contains(found) == false)
                        {
                            access = true;
                        }
                    }

                    

                    if (access == true)
                    {
                        utiliList.Add(found);
                    }
                }

                access = false;

                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY +1).SingleOrDefault();
                if (found != null)
                {
                    if (gM.Game == true)
                    {
                        if ((accessList.Contains(found.tSurf.type) && found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                        else if ((accessList.Contains("enemy") && found.refChar != null))
                        {
                            if (found.refChar.team != refChar.team || accessList.Contains("friend") == true)
                            {
                                if (highTileList.Contains(found) == false)
                                {
                                    access = true;
                                }
                            }
                        }
                        else if (((found.tSurf.type == "grass" || found.tSurf.type == "water")) && (found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                    }
                    else
                    {
                        if (highTileList.Contains(found) == false)
                        {
                            access = true;
                        }
                    }

                    if (access == true)
                    {
                        utiliList.Add(found);
                    }
                }

                access = false;

                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY - 1).SingleOrDefault();
                if (found != null)
                {
                    if (gM.Game == true)
                    {
                        if ((accessList.Contains(found.tSurf.type) && found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                        else if ((accessList.Contains("enemy") && found.refChar != null))
                        {
                            if (found.refChar.team != refChar.team || accessList.Contains("friend") == true)
                            {
                                if (highTileList.Contains(found) == false)
                                {
                                    access = true;
                                }
                            }
                        }
                        else if (((found.tSurf.type == "grass" || found.tSurf.type == "water")) && (found.refChar == null))
                        {
                            if (highTileList.Contains(found) == false)
                            {
                                access = true;
                            }
                        }
                    }
                    else
                    {
                        if (highTileList.Contains(found) == false)
                        {
                            access = true;
                        }
                    }

                    if (access == true)
                    {
                        utiliList.Add(found);
                    }

                }

                

                
            }

            foreach(Tile u in utiliList)
            {
                
                if(order == 0 && gM.Game == true)
                {
                    if(refChar.accessList.Contains(u.tSurf.type))
                    {
                        if (highTileList.Contains(u) == false)
                        {
                            highTileList.Add(u);
                            u.range = i + 1;
                        }
                    }
                }
                else
                {
                    if (highTileList.Contains(u) == false)
                    {
                        highTileList.Add(u);
                        u.range = i + 1;
                    }
                }
            
                
                
                
            }
            utiliList.Clear();
        }

        

        foreach(Tile t in highTileList)
        {           
            t.HighLight();
        }
    }

    public void Moving()
    {
        ActionTime = 2;

        float distCovered = (Time.time - startTime) * speed;

        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPoint, targetPoint, fracJourney);

        float dist = Vector3.Distance(transform.position, targetPoint);

        if (dist < 0.01f)
        {
            ActionTime = 0;
            actionIndex = 0;
            refChar.Positioning();

        }

        Vector3 lookDir = targetPoint - transform.position;
        lookDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookDir);
        
        transform.rotation = rot;
        
    }

    public void Attacking_Drac()
    {
        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;
            
        }
        else if (ActionTime < 4 && steps == 4)
        {
            

            Vector3 pos = targetPoint;
            pos.y = 3f;

            GameObject obj = Instantiate(prefabList[0]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 3;
        }
        else if(ActionTime < 3.5f && steps == 3)
        {
            Vector3 pos = targetPoint;
            pos.y = 2.5f;

            
            steps = 2;

            if(targetTile.refChar != null)
            {
                int damage = refChar.attack;
                damage = Random.Range(damage - 3, damage + 3);


                Character target = targetTile.refChar;
                GameObject effect = prefabList[1];
                target.Damage(damage, effect, refChar);
            }
            else
            {
                pos = targetPoint;
                pos.y = 2.5f;

                GameObject effect = prefabList[1];
                GameObject obj = Instantiate(effect);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                


                targetTile.DestroySurface();
            }
        }
        else if(ActionTime < 1f && steps == 2)
        {
            ActionTime = 0;
            actionIndex = 0;
        }


    }

    public void Attacking_Cthu()
    {
        

        if (steps == 5)
        {
            steps = 4;
            Vector3 pos = transform.position;
            pos.y = 3.1f;
            startPoint = pos;
            targetPoint = targetTile.transform.position;
            targetPoint.y = 3.1f;

            Vector3 lookDir = targetPoint - transform.position;
            lookDir.y = 0;
            Quaternion rot = Quaternion.LookRotation(lookDir);

            actor = Instantiate(prefabList[0], pos, rot);
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPoint, targetPoint);
            speed = 5;
        }
        else if (steps == 4)
        {
            actor.transform.position += transform.forward * Time.deltaTime * speed;

            float dist = Vector3.Distance(actor.transform.position, targetPoint);

            if(dist <= 0.4f)
            {
                Destroy(actor);

                steps = 3;

                Vector3 pos = targetPoint;

                if (targetTile.refChar != null)
                {
                    int damage = refChar.attack - 8;
                    damage = Random.Range(damage - 6, damage + 6);


                    Character target = targetTile.refChar;
                    GameObject effect = prefabList[1];
                    target.Damage(damage, effect, refChar);
                }
                else
                {

                    pos = targetPoint;
                    pos.y = 2.5f;

                    GameObject effect = prefabList[1];
                    GameObject obj = Instantiate(effect);
                    obj.transform.position = pos;
                    Destroy(obj, 1f);
                    

                    targetTile.DestroySurface();
                }
            }
        }
        else if (steps == 3)
        {


            ActionTime = 0;
            actionIndex = 0;
        }


    }

    public void DarkRain()
    {
        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;

        }
        else if (ActionTime < 4 && steps == 4)
        {


            Vector3 pos = targetPoint;
            pos.y = 2.5f;

            GameObject obj = Instantiate(prefabList[2], pos, Quaternion.identity);
            Destroy(obj, 1f);
            steps = 3;
        }
        else if (ActionTime < 3.5f && steps == 3)
        {
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 2;

            if (1 == 1)
            {
                int damage = refChar.attack;
                
                Character target = null;
                GameObject effect = prefabList[1];
                utiliList.Clear();
                if(targetTile.refChar != null)
                {
                    target = targetTile.refChar;
                    utiliList.Add(targetTile);
                }
                

                Tile t = targetTile;
                int tX = t.xPos;
                int tY = t.yPos;
                
                Tile found = gM.tileList.Where(X => X.xPos == tX + 1 && X.yPos == tY).SingleOrDefault();
                if(found != null)
                {
                    if(found.refChar != null)
                    {
                        utiliList.Add(found);
                    }
                    

                }
                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY-1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY + 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX+1 && X.yPos == tY + 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY + 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY - 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX + 1 && X.yPos == tY - 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        utiliList.Add(found);
                    }

                }

                foreach (Tile c in utiliList)
                {
                    target = c.refChar;
                    effect = prefabList[1];

                    
                    damage = Random.Range(damage - 3, damage + 3);
                    target.Damage(damage, effect, refChar);
                }
                
            }
            
        }
        else if (ActionTime < 1f && steps == 2)
        {
            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void Necronomicon()
    {
        GameObject effect = null;
        Character target = null;
        int damage = 0;
        Tile thisPos = null;
        int xPos = 0;
        int yPos = 0;
        int xVal = 0;
        int yVal = 0;
        Vector3 pos = Vector3.zero;
        GameObject obj = null;
        

        if (ActionTime < 5 && steps == 5)
        {
            
            theTarget = targetTile;
            damage = refChar.attack;

            steps = 4;
            thisPos = refChar.tilePos.GetComponent<Tile>();

            
        }
        else if (ActionTime < 4.5 && steps == 4)
        {
            
            thisPos = refChar.tilePos.GetComponent<Tile>();

            
            xVal = 0;
            yVal = 0;
            xPos = theTarget.xPos + xVal;
            yPos = theTarget.yPos + yVal;

            theTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos).SingleOrDefault();

            targetPoint = theTarget.transform.position;
            pos = targetPoint;
            pos.y = 2.5f;

            obj = Instantiate(prefabList[3]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 3;

            if(theTarget.refChar != null)
            {
                target = theTarget.refChar;
                effect = prefabList[1];

                damage = refChar.attack-6;
                damage = Random.Range(damage - 5, damage + 3);
                target.Damage(damage, effect, refChar);
            }
            else if (theTarget.tSurf.type != "grass" && theTarget.tSurf.type != "water")
            {
                pos = targetPoint;
                pos.y = 2.5f;

                effect = prefabList[1];
                obj = Instantiate(effect);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                

                theTarget.DestroySurface();


            }

            xVal = targetTile.xPos - thisPos.xPos;
            yVal = targetTile.yPos - thisPos.yPos;
            xPos = theTarget.xPos + xVal;
            yPos = theTarget.yPos + yVal;

            theTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos).SingleOrDefault();
             // gM.tileList.Where(x => x.xPos == xPos && x.yPos == yPos).SingleOrDefault();


        }
        else if (ActionTime < 4f && steps == 3)
        {
            
            thisPos = refChar.tilePos.GetComponent<Tile>();

            if (theTarget == null)
            {
                ActionTime = 0;
                actionIndex = 0;
                return;
            }
            targetPoint = theTarget.transform.position;
            pos = targetPoint;
            pos.y = 2.5f;

            obj = Instantiate(prefabList[3]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 2;

            if (theTarget.refChar != null)
            {
                target = theTarget.refChar;
                effect = prefabList[1];

                damage = refChar.attack - 6;
                damage = Random.Range(damage - 5, damage + 3);
                target.Damage(damage, effect, refChar);
            }
            else if (theTarget.tSurf.type != "grass" && theTarget.tSurf.type != "water")
            {
                pos = targetPoint;
                pos.y = 2.5f;

                effect = prefabList[1];
                obj = Instantiate(effect);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                

                theTarget.DestroySurface();
            }


            xVal = targetTile.xPos - thisPos.xPos;
            yVal = targetTile.yPos - thisPos.yPos;
            xPos = theTarget.xPos + xVal;
            yPos = theTarget.yPos + yVal;

            theTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos).SingleOrDefault();

        }
        else if (ActionTime < 3.5f && steps == 2)
        {
            
            thisPos = refChar.tilePos.GetComponent<Tile>();

            if (theTarget == null)
            {
                ActionTime = 0;
                actionIndex = 0;
                return;
            }

            targetPoint = theTarget.transform.position;
            pos = targetPoint;
            pos.y = 2.5f;

            obj = Instantiate(prefabList[3]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 1;

            if (theTarget.refChar != null)
            {
                target = theTarget.refChar;
                effect = prefabList[1];

                damage = refChar.attack - 6;
                damage = Random.Range(damage - 5, damage + 3);
                target.Damage(damage, effect, refChar);
            }
            else if (theTarget.tSurf.type != "grass" && theTarget.tSurf.type != "water")
            {
                pos = targetPoint;
                pos.y = 2.5f;

                effect = prefabList[1];
                obj = Instantiate(effect);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                
                theTarget.DestroySurface();
            }

            xVal = targetTile.xPos - thisPos.xPos;
            yVal = targetTile.yPos - thisPos.yPos;
            xPos = theTarget.xPos + xVal;
            yPos = theTarget.yPos + yVal;
            theTarget = gM.tileList.Where(X => (X.xPos == xPos && X.yPos == yPos)).SingleOrDefault();
        }
        else if (ActionTime < 3f && steps == 1)
        {

            thisPos = refChar.tilePos.GetComponent<Tile>();

            if (theTarget == null)
            {
                ActionTime = 0;
                actionIndex = 0;
                return;
            }

            targetPoint = theTarget.transform.position;
            pos = targetPoint;
            pos.y = 2.5f;

            obj = Instantiate(prefabList[3]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 0;

            if (theTarget.refChar != null)
            {
                target = theTarget.refChar;
                effect = prefabList[1];

                damage = refChar.attack+3;
                damage = Random.Range(damage - 5, damage + 3);
                target.Damage(damage, effect, refChar);
            }
            else if (theTarget.tSurf.type != "grass" && theTarget.tSurf.type != "water")
            {
                pos = targetPoint;
                pos.y = 2.5f;

                effect = prefabList[1];
                obj = Instantiate(effect);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                

                theTarget.DestroySurface();
            }

            xVal = targetTile.xPos - thisPos.xPos;
            yVal = targetTile.yPos - thisPos.yPos;
            xPos = theTarget.xPos + xVal;
            yPos = theTarget.yPos + yVal;
            theTarget = gM.tileList.Where(X => (X.xPos == xPos && X.yPos == yPos)).SingleOrDefault();
        }
        else if(ActionTime < 1.5f && steps == 0)
        {
            

            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void TotalFury()
    {
        int xVal = 0;
        int yVal = 0;
        int xPos = 0;
        int yPos = 0;
        Tile thisPos = null;
        Tile sideTarget = null;

        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;

        }
        else if (ActionTime < 4 && steps == 4)
        {
            targetPoint = targetTile.transform.position;
            Vector3 pos = targetPoint;
            pos.y = 2.5f;

            GameObject obj = Instantiate(prefabList[0]);
            obj.transform.position = pos;
            Destroy(obj, 1f);
            steps = 3;

            xPos = targetTile.xPos;
            yPos = targetTile.yPos;
            thisPos = refChar.tilePos.GetComponent<Tile>();
            xVal = xPos - thisPos.xPos;
            yVal = yPos - thisPos.yPos;

            if(yVal == 0)
            {
                sideTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos-1).SingleOrDefault();
                targetPoint = sideTarget.transform.position;
                pos = targetPoint;
                pos.y = 2.5f;

                obj = Instantiate(prefabList[0]);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                steps = 3;

                sideTarget = null;
                sideTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos + 1).SingleOrDefault();
                if(sideTarget != null)
                {
                    targetPoint = sideTarget.transform.position;
                    pos = targetPoint;
                    pos.y = 2.5f;

                    obj = Instantiate(prefabList[0]);
                    obj.transform.position = pos;
                    Destroy(obj, 1f);
                    steps = 3;
                }
                
            }
            else if (xVal == 0)
            {
                sideTarget = gM.tileList.Where(X => X.xPos == xPos-1 && X.yPos == yPos).SingleOrDefault();
                targetPoint = sideTarget.transform.position;
                pos = targetPoint;
                pos.y = 2.5f;

                obj = Instantiate(prefabList[0]);
                obj.transform.position = pos;
                Destroy(obj, 1f);
                steps = 3;

                sideTarget = null;
                sideTarget = gM.tileList.Where(X => X.xPos == xPos + 1 && X.yPos == yPos).SingleOrDefault();
                if(sideTarget != null)
                {
                    targetPoint = sideTarget.transform.position;
                    pos = targetPoint;
                    pos.y = 2.5f;

                    obj = Instantiate(prefabList[0]);
                    obj.transform.position = pos;
                    Destroy(obj, 1f);
                    steps = 3;
                }
                
            }
        }
        else if (ActionTime < 3.5f && steps == 3)
        {
            GameObject obj = null;
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 2;

            if (targetTile.refChar != null)
            {
                int damage = refChar.attack+5;
                damage = Random.Range(damage - 3, damage + 3);


                Character target = targetTile.refChar;
                GameObject effect = prefabList[1];
                target.Damage(damage, effect, refChar);


            }

            xPos = targetTile.xPos;
            yPos = targetTile.yPos;
            thisPos = refChar.tilePos.GetComponent<Tile>();
            xVal = xPos - thisPos.xPos;
            yVal = yPos - thisPos.yPos;

            if (yVal == 0)
            {
                sideTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos - 1).SingleOrDefault();
                if (sideTarget.refChar != null)
                {
                    int damage = refChar.attack + 5;
                    damage = Random.Range(damage - 3, damage + 3);

                    Character target = sideTarget.refChar;
                    GameObject effect = prefabList[1];
                    target.Damage(damage, effect, refChar);



                }

                sideTarget = gM.tileList.Where(X => X.xPos == xPos && X.yPos == yPos + 1).SingleOrDefault();
                if (sideTarget.refChar != null)
                {
                    int damage = refChar.attack + 5;
                    damage = Random.Range(damage - 3, damage + 3);

                    Character target = sideTarget.refChar;
                    GameObject effect = prefabList[1];
                    target.Damage(damage, effect, refChar);

                }
            }
            else if(xVal == 0)
            {
                sideTarget = gM.tileList.Where(X => X.xPos == xPos - 1 && X.yPos == yPos).SingleOrDefault();
                if (sideTarget.refChar != null)
                {
                    int damage = refChar.attack + 5;
                    damage = Random.Range(damage - 3, damage + 3);


                    Character target = sideTarget.refChar;
                    GameObject effect = prefabList[1];
                    target.Damage(damage, effect, refChar);
                }

                sideTarget = gM.tileList.Where(X => X.xPos == xPos + 1 && X.yPos == yPos).SingleOrDefault();
                if (sideTarget.refChar != null)
                {
                    int damage = refChar.attack + 5;
                    damage = Random.Range(damage - 3, damage + 3);


                    Character target = sideTarget.refChar;
                    GameObject effect = prefabList[1];
                    target.Damage(damage, effect, refChar);
                }
            }

        }
        else if (ActionTime < 1f && steps == 2)
        {
            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void Guillotine()
    {
        

        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;

        }
        else if (ActionTime < 5 && steps == 4)
        {
            
            steps = 3;
                       
        }
        else if (ActionTime < 4.5f && steps == 3)
        {
            GameObject obj = null;
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 2;

            if (targetTile.refChar != null)
            {
                int damage = refChar.attack;
                damage = Random.Range(damage - 6, damage + 2);


                Character target = targetTile.refChar;
                GameObject effect = prefabList[2];
                target.Damage(damage, effect, refChar);

                if (target.health <= 0)
                {
                    ActionTime = 1;
                    steps = 0;
                }
            }

            

        }
        else if (ActionTime < 4f && steps == 2)
        {
            GameObject obj = null;
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 1;

            if (targetTile.refChar != null)
            {
                int damage = refChar.attack;
                damage = Random.Range(damage - 6, damage + 2);


                Character target = targetTile.refChar;
                GameObject effect = prefabList[2];
                target.Damage(damage, effect, refChar);

                if (target.health <= 0)
                {
                    ActionTime = 1;
                    steps = 0;
                }
            }



        }
        else if (ActionTime < 3.5f && steps == 1)
        {
            GameObject obj = null;
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 0;

            if (targetTile.refChar != null)
            {
                int damage = refChar.attack;
                damage = Random.Range(damage - 6, damage + 2);


                Character target = targetTile.refChar;
                GameObject effect = prefabList[2];
                target.Damage(damage, effect, refChar);

                if(target.health <= 0)
                {
                    ActionTime = 1;
                    steps = 0;
                }
            }



        }
        else if (ActionTime < 5f && steps == 0)
        {
            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void Bloodthirst()
    {
        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;
            tempInt = 0;

        }
        else if (ActionTime < 4.5f && steps == 4)
        {


            Vector3 pos = targetPoint;
            pos.y = 2.5f;

            GameObject obj = Instantiate(prefabList[2]);
            obj.transform.position = pos;
            Destroy(obj, 2f);
            steps = 3;
        }
        else if (ActionTime < 3.5f && steps == 3)
        {
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 2;

            if (1 == 1)
            {
                int damage = refChar.attack;

                Character target = null;
                GameObject effect = prefabList[1];
                utiliList.Clear();
                if (targetTile.refChar != null)
                {
                    target = targetTile.refChar;                   
                    if (targetTile.refChar.team != refChar.team)
                    {
                        utiliList.Add(targetTile);
                    }
                }


                Tile t = targetTile;
                int tX = t.xPos;
                int tY = t.yPos;

                Tile found = gM.tileList.Where(X => X.xPos == tX + 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if(found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }
                        
                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY - 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY + 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }

                

                foreach (Tile c in utiliList)
                {
                    target = c.refChar;
                    effect = prefabList[1];

                    damage = refChar.attack +3;
                    damage = Random.Range(damage - 3, damage + 3);
                    if (target.team != refChar.team)
                    {
                        tempInt += damage;
                    }                 
                    target.Damage(damage, effect, refChar);
                }

            }

        }
        else if (ActionTime < 1f && steps == 2)
        {
            
            if (tempInt > 0)
            {
                tempInt /= gM.maxMonster;
                int baseHeal = tempInt;
                foreach (Character c in refChar.owner.charList)
                {
                    if (c.health > 0)
                    {
                        tempInt = baseHeal;
                        tempInt = Random.Range(tempInt - 6, tempInt + 6);
                        if (tempInt < 1)
                        {
                            tempInt = 1;
                        }
                        c.Heal(tempInt, prefabList[3], refChar);
                    }

                }
            }

            

            steps = 0;
            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void AlmightyEye()
    {
        if (ActionTime < 5 && steps == 5)
        {
            steps = 4;
            tempInt = 0;

            Vector3 pos = transform.position;
            pos.y = 2.5f;

            GameObject obj = Instantiate(prefabList[4]);
            obj.transform.position = pos;
            Destroy(obj, 2f);
            

        }
        else if (ActionTime < 4 && steps == 4)
        {

            Vector3 pos = targetPoint;
            pos.y = 2.5f;

            refChar.xPos = targetTile.xPos;
            refChar.yPos = targetTile.yPos;
            refChar.Positioning();
            GameObject obj = Instantiate(prefabList[5]);
            obj.transform.position = pos;
            Destroy(obj, 2f);
            steps = 3;
        }
        else if (ActionTime < 3.5f && steps == 3)
        {
            Vector3 pos = targetPoint;
            pos.y = 2.5f;


            steps = 2;

            if (1 == 1)
            {
                int damage = refChar.attack;

                Character target = null;
                GameObject effect = prefabList[1];
                utiliList.Clear();
                if (targetTile.refChar != null)
                {
                    target = targetTile.refChar;
                    if (targetTile.refChar.team != refChar.team)
                    {
                        utiliList.Add(targetTile);
                    }
                }


                Tile t = targetTile;
                int tX = t.xPos;
                int tY = t.yPos;

                Tile found = gM.tileList.Where(X => X.xPos == tX + 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX - 1 && X.yPos == tY).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY - 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }
                found = gM.tileList.Where(X => X.xPos == tX && X.yPos == tY + 1).SingleOrDefault();
                if (found != null)
                {
                    if (found.refChar != null)
                    {
                        if (found.refChar.team != refChar.team)
                        {
                            utiliList.Add(found);
                        }

                    }

                }

                foreach (Tile c in utiliList)
                {
                    target = c.refChar;
                    effect = prefabList[1];

                    damage += 3;
                    damage = Random.Range(damage - 6, damage + 6);
                    tempInt += damage;
                    target.Damage(damage, effect, refChar);
                }

            }

        }
        else if (ActionTime < 1f && steps == 2)
        {
           

            steps = 0;
            ActionTime = 0;
            actionIndex = 0;
        }
    }

    public void Placement()
    {


        if(phase == 0)
        {
            
            targetTile = gM.tileList.Where(X => X.xPos == refChar.owner.StartPosX && X.yPos == refChar.owner.StartPosY).SingleOrDefault();

            accessList.Clear();
            accessList.Add("grass");
            accessList.Add("water");
            accessList.Add("city");
            accessList.Add("forest");
            accessList.Add("enemy");
            accessList.Add("mountain");

            HighLight(5);

            utiliList.Clear();

            foreach(Tile t in highTileList)
            {
                if(refChar.accessList.Contains(t.tSurf.type) == false || t.refChar != null)
                {
                    t.range = 0;
                    t.EndHighlight();
                }
                else
                {
                    t.range = 1;
                }
            }

            phase = 1;
        }
        else if(phase == 2)
        {
            gM.audioList[0].Play();
            Character p = gM.playChar;

            p.xPos = targetTile.xPos;
            p.yPos = targetTile.yPos;
            p.tilePos = null;
            p.Positioning();

            gM.positionCounter += 1;
            if (gM.positionCounter < gM.charList.Count)
            {
                
                gM.PositionPhase(gM.positionCounter);
            }
            else
            {
                foreach(Character c in gM.charList)
                {
                    c.creature.phase = 0;
                    c.creature.order = 0;
                }

                gM.EndHighlight();
                gM.gamePhase = 2;
                gM.playChar = null;
                gM.Turn = false;
                gM.Game = true;
            }
            
            
        }

        
    }
}
