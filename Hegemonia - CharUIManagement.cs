using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharUIManagment : MonoBehaviour {

    public TextMeshProUGUI cName;
    public TextMeshProUGUI cTreasure;
    public TextMeshProUGUI cSecurity;
    public TextMeshProUGUI cBigSecurity;
    public Character character;
    public Image portrait;

    public GameObject window;

    // Security Window
    [Space]
    [Space]
    public List<GameObject> windowList = new List<GameObject>();
    [Space]
    [Space]
    public TextMeshProUGUI bodyguardNumber;
    public TMP_InputField wageChangeInput;

    public void Start()
    {
        MainMenu.EndRoundTick10 += UpdateThis;
    }

    public void UpdateThis()
    {
        cName.text = character.cName;
        cTreasure.text = character.treasure.ToString();
        cSecurity.text = character.security.ToString();
        cBigSecurity.text = character.security.ToString();
        portrait.sprite = character.icon;

        bodyguardNumber.text = character.bodyguards.Count + "/" + character.bodyguardMax;
        wageChangeInput.text = character.wageOffer.ToString();
    }

    public void OpenWindow(int index)
    {
        if (window.activeSelf == false)
        {
            window.SetActive(true);
            for (int i = 0; i < windowList.Count; i++)
            {
                windowList[i].SetActive(false);
            }
        }

        if(windowList[index].activeSelf == false)
        {
            for (int i = 0; i < windowList.Count; i++)
            {
                windowList[i].SetActive(false);
            }

            GameObject obj = windowList[index];

            obj.SetActive(true);
            UpdateThis();
        }
        else
        {
            for (int i = 0; i < windowList.Count; i++)
            {
                windowList[i].SetActive(false);
            }
        }
       
        
    }

    public void BodyguardDemand(int index)
    {
        if(index == 0)
        {
            if(character.bodyguardMax < 100)
            {
                character.bodyguardMax += 1;
            }
            
        }

        if (index == 1)
        {
            if (character.bodyguardMax > 0)
            {
                character.bodyguardMax -= 1;
            }

        }

        bodyguardNumber.text = character.bodyguards.Count + "/" + character.bodyguardMax;
    }

    public void ChangeWages()
    {
        float offer = float.Parse(wageChangeInput.text);
        wageChangeInput.text = offer.ToString();
        character.ChangeWages(offer);
    }
}
