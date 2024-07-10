using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class View : MonoBehaviour
{
    
    public GameObject ViewAll;
    public GameObject ViewModbus;
    public GameObject ViewICMP;

    void Start()
    {

        ViewAll.gameObject.SetActive(true);
        ViewModbus.gameObject.SetActive(false);
        ViewICMP.gameObject.SetActive(false);

    }

    public void ViewMode(int val)
    {

        if(val == 0)
        {

            ViewAll.gameObject.SetActive(true);
            ViewModbus.gameObject.SetActive(false);
            ViewICMP.gameObject.SetActive(false);

        }
        else if(val == 1)
        {

            ViewAll.gameObject.SetActive(false);
            ViewModbus.gameObject.SetActive(true);
            ViewICMP.gameObject.SetActive(false);            

        }
        else if(val == 2)
        {

            ViewAll.gameObject.SetActive(false);
            ViewModbus.gameObject.SetActive(false);
            ViewICMP.gameObject.SetActive(true); 

        }

    }

}
