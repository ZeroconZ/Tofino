using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowEnforcer : MonoBehaviour
{
    public RectTransform REContainer;
    public GameObject enforcerDisplay;
    public TMP_Dropdown enforcer;
    public GameObject blockDisplay;
    public GameObject advancedDisplay;

    bool ShowAdvanced = true;

    void Start()
    {

        enforcerDisplay.SetActive(false);
        blockDisplay.SetActive(true);

        enforcer.onValueChanged.AddListener(OnDropdownValueChanged);

    }

    private void ChangePanelSize(int height, int width)
    {

        REContainer.sizeDelta = new UnityEngine.Vector2(width, height);

    }

    public void Show(int val)
    {

        if(val == 2)
        {

            //ChangePanelSize(500, 460);
            enforcerDisplay.SetActive(true);
            blockDisplay.SetActive(false);
            RE.instance.ProtoDeactiv(true);

        }
        else
        { 

            //ChangePanelSize(465, 460);
            enforcerDisplay.SetActive(false);
            blockDisplay.SetActive(true);
            RE.instance.ProtoDeactiv(false);

        }
    }

    public void OnDropdownValueChanged(int value)
    {

        if (ShowAdvanced && value == 3) 
        {
            AddFunctions(new List<string> { "1: Read Coils", 
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
                                            "43: Read Device Identification"});

            ShowAdvanced = false;

        } 

    }

    private void AddFunctions(List<string> newFunctions)
    {

        enforcer.options.RemoveAt(3);
        enforcer.AddOptions(newFunctions);

    }

}
