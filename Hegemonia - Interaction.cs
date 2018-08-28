using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour {

    public static GameObject target;
    private bool testBool;

    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    public delegate void Interacted();
    public static Interacted Interact1;

    public static Community com;
    public GameObject charWindow;

    void Start()
    {
        MainMenu.EndRoundTick1 += Close;

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            testBool = false;

            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            

            
            if(results.Count > 0)
            {
                testBool = true;
            }

            if(testBool == false)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                    if (hit.collider != null)
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("interactable"))
                        {
                            target = hit.collider.gameObject;

                            if (target.CompareTag("Community"))
                            {

                                target.GetComponent<Community>().Interact();
                                com = target.GetComponent<Community>();
                                Interact1();
                            }
                        }
                        else
                        {
                            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                            {
                                Close();
                            }

                        }
            }

            
        }       
    }

    public void Close()
    {
        if (target != null)
        {
            if (target.CompareTag("Community"))
            {
                target.GetComponent<Community>().Close();
            }
        }

        target = null;

        
        if(charWindow.activeSelf == true)
        {
            charWindow.SetActive(false);
        }
    }


}
