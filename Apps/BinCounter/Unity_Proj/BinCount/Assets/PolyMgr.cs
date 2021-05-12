using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PolyMgr : MonoBehaviour
{
    public float spacing;
    public int target;
    TMP_Text totalText;
    // Start is called before the first frame update
    void Start()
    {
        totalText = GameObject.Find("TotalText").GetComponent<TMP_Text>();

        //Arrange Polys
        float spaces = (transform.childCount - 1) * spacing;
        float polys = transform.childCount;
        float x = (-0.50f * (spaces + polys)) + 0.5f;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).position = new Vector3(x,
                transform.position.y, transform.position.z);
            x += (1 + spacing);
        }

    }

    // Update is called once per frame
    void Update()
    {
        totalText.text = "Total: " + target;
        if (busySpinning())
            return;

        int total = getTotal();
        if (total < target)
        {
            incSpin();
        }
        else if (total > target)
        {
            decSpin();
        }
    }

    bool busySpinning()
    {
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
            if (p.spinning())
                return true;
        return false;
    }

    void incSpin()
    {
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
        {
            p.increase();
            if (p.getValue() > 0)
                break;
        }
    }

    void decSpin()
    {
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
        {
            if (p.getValue() > 0)
            {
                p.decrease();
                break;
            }
            else
            {
                p.decrease();
            }
        }
    }

    public void increase()
    {
        target = Mathf.Min(255, target+1);
    }

    public void decrease()
    {
        target = Mathf.Max(0, target - 1);
    }

    int getTotal()
    {
        int total = 0;
        int n = 0;
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
        {
            total += (p.getValue() * (int)Mathf.Pow(p.numSides, n));
            n++;
        }

        return total;
    }
}
