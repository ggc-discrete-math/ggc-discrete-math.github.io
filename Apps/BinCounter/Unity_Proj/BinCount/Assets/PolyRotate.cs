using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PolyRotate : MonoBehaviour
{
    public static int numSides = 2;
    float speed;

    int currentValue;
    TMP_Text[] texts;
    int currentTextIndex;

    float targetRot;
    float currentRot;

    // Start is called before the first frame update
    void Start()
    {
        texts = transform.GetComponentsInChildren<TMP_Text>();
        reset();
    }

    public void reset()
    {
        speed = Mathf.Lerp(360, 4 * 360, numSides / 16);
        currentValue = 0;
        targetRot += 180;
        setNextText();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRot < targetRot)
        {
            float delta = targetRot - currentRot;
            if (delta > Time.deltaTime * speed)
                currentRot += Time.deltaTime * speed;
            else
                currentRot = targetRot;
            transform.eulerAngles = Vector3.left * currentRot;
        }
        else if (currentRot > targetRot)
        {
            float delta = currentRot - targetRot;
            if (delta > Time.deltaTime * speed)
                currentRot -= Time.deltaTime * speed;
            else
                currentRot = targetRot;
            transform.eulerAngles = Vector3.left * currentRot;
        }
    }

    public bool spinning()
    {
        return currentRot != targetRot;
    }

    public void increase()
    {
        if (spinning())
            return;
        currentValue = (currentValue + 1) % numSides;
        targetRot += 180;
        setNextText();
    }

    public void decrease()
    {
        if (spinning())
            return;
        currentValue = (numSides + currentValue - 1) % numSides;
        targetRot += -180;
        setNextText();
    }

    void setNextText()
    {
        currentTextIndex = (currentTextIndex + 1) % 2;
        if (currentValue < 10)
            texts[currentTextIndex].text = currentValue.ToString();
        else
        {
            texts[currentTextIndex].text =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[currentValue - 10].ToString();
        }
    }

    public int getValue()
    {
        return currentValue;
    }
}
