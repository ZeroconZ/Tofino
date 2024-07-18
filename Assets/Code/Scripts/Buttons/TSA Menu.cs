using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class TSAMenu : MonoBehaviour
{

    public GameObject EV;
    public GameObject EN;

    void Start()
    {

        EV.gameObject.SetActive(false);
        EN.gameObject.SetActive(false);

    }

    public void OnMouseDown()
    {

        EV.gameObject.SetActive(!EV.gameObject.activeSelf);
        EN.gameObject.SetActive(!EN.gameObject.activeSelf);

    }


}
