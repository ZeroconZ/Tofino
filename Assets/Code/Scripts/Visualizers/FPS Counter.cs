using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class FPS : MonoBehaviour
{
    private float fps;
    public TextMeshProUGUI FPScounter;
    void Start()
    {

        InvokeRepeating("GetFPS", 0, 1f);
        Application.targetFrameRate = 60;

    }

    void GetFPS()
    {

        fps = (int) (1f/Time.unscaledDeltaTime);
        FPScounter.text = "FPS: " + fps.ToString();
    }
}
