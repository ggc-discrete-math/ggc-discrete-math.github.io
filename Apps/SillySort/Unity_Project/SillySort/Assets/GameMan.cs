using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMan : MonoBehaviour
{
    public static List<int> numList;
    public static int numRevealed;
    public const int maxRevealed = 2;
    public static Transform swapA;
    public static bool gameOver;

    static int numSwaps;
    static GameObject winner;
    static float startTime;

    void Awake()
    {
        numList = new List<int>();
        for (int i = 0; i < 100; i++)
            numList.Add(i);

        numList.Shuffle();

        //make the game super easy for testing
        //int temp = numList[0];
        //numList[0] = numList[1];
        //numList[1] = temp;

        gameOver = false;
        numRevealed = 0;
        swapA = null;
        numSwaps = 0;
        startTime = Time.time;
    }

    private void Start()
    {
        winner = GameObject.Find("Winner");
        winner.SetActive(false);
    }

    public void reStart()
    {
        SceneManager.LoadScene(0);
    }

    public static void checkWin(Transform cardParent)
    {
        //TEST END GAME
        numSwaps++;
        bool sorted = true;
        CardController[] cards = cardParent.GetComponentsInChildren<CardController>();
        string temp = "";
        for (int i = 0; i < cards.Length - 1; i++)
        {
            temp += cards[i].value + " ";
            if (cards[i].value > cards[i + 1].value)
                sorted = false;
        }
        print(temp + " " + cards[cards.Length - 1].value);
        GameMan.gameOver = sorted;
        if (GameMan.gameOver)
        {
            winner.SetActive(true);
            winner.GetComponent<Text>().text = "Sorted in " + numSwaps +
                " swaps and " + ((int) (Time.time - startTime)) + " seconds!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
