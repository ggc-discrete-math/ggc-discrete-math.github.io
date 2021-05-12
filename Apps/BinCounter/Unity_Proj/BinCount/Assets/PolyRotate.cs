using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolyRotate : MonoBehaviour
{
    public int numSides;
    public float speed;

    int currentSide;
    float targetRot;
    float currentRot;

    // Start is called before the first frame update
    void Start()
    {
        
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
        currentSide = (currentSide + 1) % numSides;
        targetRot = (targetRot + (360 / numSides));
    }

    public void decrease()
    {
        currentSide = (numSides + currentSide - 1) % numSides;
        targetRot = (targetRot - (360 / numSides));
    }

    public int getValue()
    {
        return currentSide;
    }

    private void OnMouseDown()
    {
        //increase();
    }
}
