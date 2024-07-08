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

        ViewAll.SetActive(true);
        ViewModbus.SetActive(false);
        ViewICMP.SetActive(false);

    }

    public void ViewMode(int val)
    {

        if(val == 0)
        {

            ViewAll.SetActive(true);
            ViewModbus.SetActive(false);
            ViewICMP.SetActive(false);

        }
        else if(val == 1)
        {

            ViewAll.SetActive(false);
            ViewModbus.SetActive(true);
            ViewICMP.SetActive(false);            

        }
        else if(val == 2)
        {

            ViewAll.SetActive(false);
            ViewModbus.SetActive(false);
            ViewICMP.SetActive(true); 

        }

    }

}
