using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VennBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public static Color theColor = Color.white;
    Image myImg;
    Color oldColor;
    void Start()
    {
        myImg = GetComponent<Image>();
        oldColor = myImg.color;
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myImg.color = theColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myImg.color = oldColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Color c = theColor;
        c.a = 0.5f;
        myImg.color = c;
        oldColor = theColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        myImg.color = theColor;
        oldColor = theColor;
    }
}
