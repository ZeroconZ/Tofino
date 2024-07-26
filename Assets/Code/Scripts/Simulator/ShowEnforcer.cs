using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ShowEnforcer : MonoBehaviour
{
    public RectTransform REContainer;
    public GameObject enforcerDisplay;
    public GameObject blockDisplay;

    void Start()
    {

        enforcerDisplay.SetActive(false);
        blockDisplay.SetActive(true);

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

}
