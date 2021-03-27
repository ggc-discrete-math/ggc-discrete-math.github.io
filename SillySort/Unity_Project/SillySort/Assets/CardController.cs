using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Text num, reveal, left, right;
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

        left.GetComponentInParent<Button>().interactable = (
            transform.GetSiblingIndex() > 0);

        right.GetComponentInParent<Button>().interactable = (
            transform.GetSiblingIndex() < (transform.parent.childCount -1));

    }

    public void revealToggle()
    {
        if (isHidden)
        {
            if (GameMan.numRevealed < GameMan.maxRevealed)
            {
                GameMan.numRevealed++;
                isHidden = false;
                reveal.text = "Hide";
            }
        }
        else
        {
            GameMan.numRevealed--;
            isHidden = true;
            reveal.text = "Reveal";
        }
    }

    public void move(int dir)
    {
        int i = transform.GetSiblingIndex();
        transform.SetSiblingIndex(i + dir);
    }


}
