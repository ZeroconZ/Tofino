using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEN : MonoBehaviour
{
    public GameObject EventNotif;

    
    public void Start()
    {

        EventNotif.gameObject.SetActive(true);

    }

    public void Switch()
    {

        if(EventNotif.activeSelf == false)
            EventNotif.SetActive(true);
        
        else 
            EventNotif.SetActive(false);

    }

}