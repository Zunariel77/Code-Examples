using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSBtnScript : MonoBehaviour {

    private Button btn;

    public int slotNumber;
    public GameObject teamBTN;
    public GameObject stateBTN;
    public Sprite baseSprite;

    public Character_Selection cS;
    public Character_Pool cP;
    public int poolInt;

    private string tempString;
    public Character_Stats charStats;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        teamBTN.GetComponent<Button>().onClick.AddListener(TaskOnClick2);

        stateBTN.GetComponent<Button>().onClick.AddListener(TaskOnClick3);

        
    }

    private void TaskOnClick()
    {
        poolInt += 1;
        charStats.charInt = poolInt;
        if (poolInt >= cP.charList.Count)
        {
            poolInt = -1;
            Clear();
        }
        UpdateBTN();

        
    }

    private void TaskOnClick2()
    {
        charStats.teamInt += 1;
        if(charStats.teamInt > 8)
        {
            charStats.teamInt = 1;
        }

        UpdateBTN();
    }

    private void TaskOnClick3()
    {
        charStats.playerState += 1;
        if(charStats.playerState > 10)
        {
            charStats.playerState = 0;
        }

        UpdateBTN();

    }

    public void Clear()
    {
        charStats.charInt = -1;
        charStats.teamInt = 1;
        charStats.playerState = 0;
        poolInt = -1;

        UpdateBTN();
    }

    public void UpdateBTN()
    {
        if(poolInt == -1)
        {
            if(baseSprite != null)
            {
                gameObject.GetComponent<Image>().sprite = baseSprite;
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = null;
            }

            
            gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "";
            teamBTN.SetActive(false);
            stateBTN.SetActive(false);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = cP.charList[poolInt].GetComponent<Character_Prefab>().picture;
            gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = cP.charList[poolInt].GetComponent<Character_Prefab>().name;
            teamBTN.SetActive(true);
            stateBTN.SetActive(true);

            teamBTN.transform.Find("Text").gameObject.GetComponent<Text>().text = "Team" + charStats.teamInt.ToString();

            int playerNumber = slotNumber + 1;

            if (charStats.playerState == 0)
            {
                tempString = "Player" + playerNumber;
            }
            else
            {
                tempString = charStats.playerState.ToString();
                
            }

            stateBTN.transform.Find("Text").gameObject.GetComponent<Text>().text = tempString;

            charStats.Save();
        }

        
    }

    private void Update()
    {
        int playerNumber = slotNumber + 1;
        if(playerNumber <= 4)
        {
            if (Input.GetButtonDown("AttackP" + playerNumber))
            {
                TaskOnClick();
            }
            else
            {
                if (Input.GetButtonDown("BlockP" + playerNumber))
                {
                    TaskOnClick2();
                }
                else
                {
                    if (Input.GetButtonDown("SAttackP" + playerNumber))
                    {
                        TaskOnClick3();
                    }
                }
            }
        }

        

        
    }


}
