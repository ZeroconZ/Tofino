using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class ShowAdvancedMdB : MonoBehaviour
{

    public RectTransform TGContainer;
    public RectTransform TSAM;
    public GameObject EnforcerOpt;
    public TMP_Dropdown EnforcerDp;

    private bool ShowAdvanced = true;

    void Start()
    {

        EnforcerOpt.SetActive(false);

    }

    private void ChangePanelSize(int height, int width)
    {

        TGContainer.sizeDelta = new UnityEngine.Vector2(width, height);

    }

    public void ShowAdvancedOpt(int val)
    {

        if(ShowAdvanced && val == 1)
        {

            ChangePanelSize(400, 460);
            EnforcerOpt.SetActive(true);
            TSAM.anchoredPosition = new UnityEngine.Vector2(0, -280);
            AddFunctions(new List<string> { "",
                                            "1: Read Coils", 
                                            "2: Read Discrete Inputs",
                                            "3: Read Holding Registers ",
                                            "4, Read Input Registers", 
                                            "5: Write Single Coil",
                                            "6: Write Single Register",
                                            "7: Read Exception Status",
                                            "15: Write Multiple Coils",
                                            "16: Write Multiple Registers",
                                            "20: Read File Record",
                                            "21: Write File Record",
                                            "23: R/W Multiple Registers",
                                            "24: Read FIFO Queue",
                                            "43: Read Device ID",
                                            "90: Unity Programming/OFS"});

            TrafficGen.instance.ShowAdvModbus(true);
            ShowAdvanced = false;

        }
        else
        {

            ChangePanelSize(340, 460);
            EnforcerOpt.SetActive(false);
            TSAM.anchoredPosition = new UnityEngine.Vector2(0, -220);
            TrafficGen.instance.ShowAdvModbus(false);
            ShowAdvanced = true;

        }        

    }

    private void AddFunctions(List<string> newFunctions)
    {

        EnforcerDp.ClearOptions();
        EnforcerDp.AddOptions(newFunctions);

    }

}
