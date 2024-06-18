using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleEventVis : MonoBehaviour
{
    public GameObject EventVis;

    
    public void Start()
    {

        EventVis.gameObject.SetActive(false);

    }

    public void Switch()
    {

        if(EventVis.activeSelf == false)
            EventVis.SetActive(true);
        
        else 
            EventVis.SetActive(false);

    }

}