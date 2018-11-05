using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // A GameManager for my strategy game "Monster Combat" which I created for
    // a game jam at the SAE Cologne. The script sets up the battle field 
    // organises the turn-based combat, the monster selection phase before the combat
    // and the positioning phase before the game starts.

    public GameObject prefab; // Prefab for the field-space-tiles
    public int yTiles; // number of tiles on the y-Axis
    public int xTiles; // number of tiles on the x-Axis (It is y and x from the top perspective onto the 2D-field
    public float setHeight; 
    public int maxMonster; // maximum amount of monsters per team

    [System.Serializable]
	public class tileSet 
    {
        // each tile gets a surface (grass, water, mountain, city, forest) which gets instantiated through a random factor

        public GameObject prefab; 
        public int propability; 
        public int minProp;
        public int maxProp;
        public string type;
    }

    public List<tileSet> tileSetList = new List<tileSet>();
    public List<Tile> tileList = new List<Tile>();
    public List<GameObject> prefabList = new List<GameObject>();
    public List<AudioSource> audioList = new List<AudioSource>();

    // Game
    public List<Player> playerList = new List<Player>();
    public Character playChar;
    public List<Character> charList = new List<Character>();

    public bool Game;
    public bool GameOver;
    public int gamePhase;
    public bool Turn;
    public GameObject graveyard;
    public TextMeshProUGUI warning;
    public float warningCounter;
    public int positionCounter;
    public GameObject confirmBtn;
    public GameObject RestartBtn;

 
    private void Awake()
    {      
        SetUpTiles();       
        for(int i = 0; i < tileSetList.Count; i++)
        {
            tileSet tS = tileSetList[i];

            if(i > 0)
            {
                tS.minProp = tileSetList[i - 1].maxProp;
                tS.maxProp = tS.minProp + tS.propability;
            }
            else
            {
                tS.minProp = 0;
                tS.maxProp = tS.minProp + tS.propability;
            }
        }
    }

    private void Start()
    {
        gamePhase = 0;
        Game = false;
        confirmBtn.SetActive(true);

        foreach(Player p in playerList)
        {
            p.pP.selectPanel.SetActive(true);
            p.pP.UpdatePanel();
            
        }
        WarningText("Choose your Monsters!", 3);
    }

    public void SetUpTiles()
    {
        Vector3 point = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        tileList.Clear();

        for(int y = 0; y < yTiles; y++)
        {

            for(int x = 0; x < xTiles; x++)
            {
                point.x = x * 2;
                point.z = y * 2;
                GameObject obj = Instantiate(prefab, point, rot);
                obj.transform.parent = transform;
                Tile t = obj.GetComponent<Tile>();
                t.xPos = x;
                t.yPos = y;
                tileList.Add(t);
            }
        }
    }

    public void Update()
    {
        if(Game)
        {
            if(charList.Count > 0)
            {
                

                SpeedContest();
            }
        }

        if(warningCounter > 0)
        {
            
            warningCounter -= Time.deltaTime;
            if(warningCounter < 1)
            {
                Color col = warning.color;
                col.a = warningCounter;
                warning.color = col;
            }
        }
        else
        {
            warning.gameObject.SetActive(false);
        }
    }

    public void WarningText(string s, float t)
    {
        Color col = warning.color;
        col.a = 1;
        warning.color = col;
        warning.gameObject.SetActive(true);
        warning.text = s;
        warningCounter = t;
    }

    public void SpeedContest()
    {
        if (GameOver == true) return;

        if(Turn == false)
        {
            foreach(Character c in charList)
            {
                Color col = c.cP.panelImg.color;
                col.a = 0.5f;
                c.cP.panelImg.color = col;

                col = c.cP.icon.color;
                col.a = 0.5f;
                c.cP.icon.color = col;
            }

            foreach(Player p in playerList)
            {                
                foreach(Character c in p.charList)                
                {
                    if (playChar != null) return;
                                                         
                    if (c.health > 0)
                    {
                        c.speedCounter += Random.Range(1, c.speed) * (Time.deltaTime * 20f);

                        Tile t = c.tilePos.GetComponent<Tile>();

                        if (c.bonusTile == "water" && t.tSurf.type == c.bonusTile)
                        {
                            c.speedCounter += Random.Range(1, 6) * (Time.deltaTime * 4);
                        }

                        p.pP.UpdatePanel();
                        if (c.speedCounter >= 100)
                        {
                            Turn = true;
                            c.speedCounter = 100;
                            playChar = c;
                            GameObject obj = Instantiate(prefabList[1]);
                            Vector3 pos = playChar.transform.position;
                            obj.transform.position = pos;
                            Destroy(obj, 2f);

                            Color col = c.cP.panelImg.color;

                            col = c.cP.panelImg.color;
                            col.a = 1;
                            c.cP.panelImg.color = col;

                            col = c.cP.icon.color;
                            col.a = 1;
                            c.cP.icon.color = col;

                            audioList[4].Play();

                            playChar.action += playChar.maxAction;
                            if (playChar.action > playChar.maxAction * 2)
                            {
                                playChar.action = playChar.maxAction * 2;
                            }
                            Bonus();
                            p.pP.UpdatePanel();
                            return;
                        }
                        
                    }
                    else
                    {
                        c.speedCounter = 0;
                    }
                }               
            }           
        }
    }

    public void EndHighlight()
    {
        foreach (Tile t in tileList)
        {
            t.EndHighlight();
        }
    }

    public void EndTurn()
    {
        audioList[1].Play();
        Creature c = playChar.creature;
        c.phase = 0;
        c.order = 0;
        playChar.speedCounter = 0;
        EndHighlight();
        Turn = false;
        foreach(Player p in playerList)
        {
            p.pP.UpdatePanel();
        }
        playChar = null;
    }

    public void Confirm()
    {
        if(gamePhase == 0 && Game == false)
        {
            bool success = false;
            foreach(Player p in playerList)
            {
                if(p.charList.Count != maxMonster)
                {
                    success = true;
                }
            }

            if(success == false)
            {
                audioList[1].Play();
                gamePhase = 1;
                WarningText("Position your Monsters on the battlefield!", 3);
                foreach (Player p in playerList)
                {
                    p.pP.selectPanel.SetActive(false);
                    p.pP.UpdatePanel();
                }

                charList.Clear();
                for (int a = 0; a < maxMonster; a++)
                {
                    foreach(Player p in playerList)
                    {                       
                        charList.Add(p.charList[a]);
                    }
                }

                confirmBtn.SetActive(false);
                positionCounter = 0;
                PositionPhase(positionCounter);
            }
            else
            {
                WarningText("Select "+maxMonster+" Monsters for each Player!", 2);
            }
        }
    }

    public void PositionPhase(int choice)
    {
        int i = choice;
        Character c = charList[i];

        EndHighlight();

        playChar = c;
        Color col = new Color(0,0,0,0);

        foreach (Character a in charList)
        {
            col = a.cP.panelImg.color;
            col.a = 0.5f;
            a.cP.panelImg.color = col;

            col = a.cP.icon.color;
            col.a = 0.5f;
            a.cP.icon.color = col;
        }


        col = c.cP.panelImg.color;
        col.a = 1f;
        c.cP.panelImg.color = col;

        col = c.cP.icon.color;
        col.a = 1f;
        c.cP.icon.color = col;


        playChar.creature.phase = 0;
        playChar.creature.Placement();
    }

    public void CheckState(Character c)
    {
        int t = c.team;
        Player p = c.owner;
        bool defeated = true;
        Player winner;
        foreach(Character b in p.charList)
        {
            if(b.health > 0)
            {
                defeated = false;
            }                       
        }

        if(defeated == true)
        {
            winner = playerList.Where(x => x != p).SingleOrDefault();

            GameOver = true;
            WarningText(winner.pName + " wins!", 3f);
            warning.color = winner.pColor;
            EndHighlight();
            
            RestartBtn.SetActive(true);
            EndTurn();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Bonus()
    {
        Tile t = playChar.tilePos.GetComponent<Tile>();

        
        if(playChar.bonusTile == "forest" && t.tSurf.type == playChar.bonusTile)
        {
            int tempInt = Random.Range(1, 3);

            playChar.action += tempInt;
            if (playChar.action > playChar.maxAction * 2)
            {
                playChar.action = playChar.maxAction * 2;
            }
            GameObject obj = Instantiate(prefabList[0]);
            Vector3 pos = playChar.transform.position;
            obj.transform.position = pos;
            DamageText dT = obj.GetComponent<DamageText>();
            dT.txt.text = tempInt.ToString() + " Bonus AP!";
            dT.txt.color = Color.yellow;
            dT.lifeTime = 1.5f;

        }
        else if(playChar.bonusTile == "city" && t.tSurf.type == playChar.bonusTile)
        {
            GameObject effect = playChar.creature.prefabList[3];
            playChar.Heal(Random.Range(3, 12), effect, playChar);
        }
    }
}
