using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PolyMgr : MonoBehaviour
{
    public float spacing;
    public int target;
    int maxBase = 36;
    TMP_InputField totalInput;
    TMP_InputField baseInput;

    // Start is called before the first frame update
    void Start()
    {
        totalInput = GameObject.Find("TotalInput").GetComponent<TMP_InputField>();
        baseInput = GameObject.Find("BaseInput").GetComponent<TMP_InputField>();

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

    public void setTarget()
    {
        try
        {
            target = Mathf.Clamp(int.Parse(totalInput.text), 0,
                (int) Mathf.Pow(PolyRotate.numSides, 8) - 1);
        }
        catch
        {
            target = 0;
        }
        totalInput.text = "" + target;
    }

    public void setBase()
    {
        try
        {
            PolyRotate.numSides = Mathf.Clamp(int.Parse(baseInput.text), 2, maxBase);
        }
        catch
        {
            PolyRotate.numSides = 2;
        }
        baseInput.text = "" + PolyRotate.numSides;
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
            p.reset();

        setTarget();
    }

    public void increase()
    {
        target = Mathf.Min(255, target+1);
        totalInput.text = "" + target;
    }

    public void decrease()
    {
        target = Mathf.Max(0, target - 1);
        totalInput.text = "" + target;
    }

    int getTotal()
    {
        int total = 0;
        int n = 0;
        foreach (PolyRotate p in transform.GetComponentsInChildren<PolyRotate>())
        {
            total += (p.getValue() * (int)Mathf.Pow(PolyRotate.numSides, n));
            n++;
        }

        return total;
    }
}
