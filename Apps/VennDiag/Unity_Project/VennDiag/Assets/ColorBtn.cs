using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBtn : MonoBehaviour
{
    public void SetColor()
    {
        VennBtn.theColor = GetComponent<Image>().color;
    }
}
