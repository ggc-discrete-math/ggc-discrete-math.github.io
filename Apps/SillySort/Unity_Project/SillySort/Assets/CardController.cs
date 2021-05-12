using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Text num, reveal;
    bool isHidden;
    int value;
    // Start is called before the first frame update
    void Start()
    {
        isHidden = true;
        int i = transform.GetSiblingIndex();
        value = GameMan.numList[i];
    }

    // Update is called once per frame
    void Update()
    {
        if(isHidden)
            num.text = "?";
        else
            num.text = "" + value;

        reveal.GetComponentInParent<Button>().interactable = (
            GameMan.numRevealed < GameMan.maxRevealed || !isHidden);

        transform.Find("Swap").GetComponent<Button>().interactable =
            (GameMan.swapA != transform);
    }

    public void revealToggle()
    {
        if (isHidden)
        {
            if (GameMan.numRevealed < GameMan.maxRevealed)
            {
                GameMan.numRevealed++;
                isHidden = false;
                reveal.text = "HIDE";
            }
        }
        else
        {
            GameMan.numRevealed--;
            isHidden = true;
            reveal.text = "SHOW";
        }
    }

    public void swap()
    {
        if (GameMan.swapA == null)
        {
            GameMan.swapA = transform;
        }
        else
        {
            int a = GameMan.swapA.GetSiblingIndex();
            int b = transform.GetSiblingIndex();
            GameMan.swapA.SetSiblingIndex(b);
            transform.SetSiblingIndex(a);
            GameMan.swapA = null;
        }
    }


}
