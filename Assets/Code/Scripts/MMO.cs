using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class MMO : MonoBehaviour
{
    
    public static MMO instance;
    public TextMeshProUGUI ModeDisplay;

    private const float updInterv = 0.5f;
    private float lastUpd = 0f;
    private string tofinoMode;

    void Awake()
    {

        if(MMO.instance == null)
            MMO.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Update()
    {

        lastUpd += Time.deltaTime;

        if(lastUpd >= updInterv)
        {
            
            updMode();
            lastUpd = 0f;

        }

    }

    public void newMode(string mode)
    {

        tofinoMode = mode;

    }

    public void updMode()
    {

        ModeDisplay.text = "Estado: " + tofinoMode;

    }
}
