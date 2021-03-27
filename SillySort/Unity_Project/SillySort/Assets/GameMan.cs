using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static List<int> numList;
    public static int numRevealed;
    public const int maxRevealed = 2;
    void Awake()
    {
        numList = new List<int>();
        for (int i = 0; i < 100; i++)
            numList.Add(i);

        numList.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
