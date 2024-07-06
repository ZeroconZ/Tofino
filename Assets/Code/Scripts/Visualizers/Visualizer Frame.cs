using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class VFrame : MonoBehaviour, IDragHandler
{
    
    public Canvas canvas;
    private RectTransform rectTransform;

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();


    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

}
