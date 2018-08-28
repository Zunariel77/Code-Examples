using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommunityPanel : MonoBehaviour {

    public Image objImg;
    public TextMeshProUGUI objTitle;
    public TextMeshProUGUI objPop;
    public GameObject currentPan;
    public GameObject windowPan;
    public GameObject refCom;

    private Community com;

    public Slider ipSlide;
    public TextMeshProUGUI ipSlideTxt;
    public TextMeshProUGUI ipRegTxt;
    public TextMeshProUGUI trasureTxt;


    private void Start()
    {
        windowPan.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ActivatePan(GameObject pan)
    {
        if(currentPan != null)
        {
            currentPan.SetActive(false);
        }
        currentPan = pan;
        currentPan.SetActive(true);
    }

    public void ClosePan()
    {
        windowPan.SetActive(false);
    }

    public void UpdatePanel()
    {
        com = refCom.GetComponent<Community>();

        

        objTitle.text = com.objName;
        objImg.sprite = com.objSprite;
        objPop.text = "Pop: " + com.citizens.Count;
        ipRegTxt.text = "+" + com.ipReg.ToString();
        ipSlideTxt.text = com.ipCur.ToString() + "/" + com.ip.ToString();
        if(com.ip != 0 && com.ipCur != 0)
        {
            ipSlide.value = com.ipCur / com.ip;
        }
        else
        {
            ipSlide.value = 0;
        }

        trasureTxt.text = com.treasure.ToString();

    }
}
