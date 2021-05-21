using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotContainer : MonoBehaviour {
	
	private List<BombBot> bots;
	private Vector3 dropPoint;
	private TextMesh counter;

	void Start () {
		bots = new List<BombBot>();
		dropPoint = transform.Find("DropPoint").position;
		counter = transform.parent.parent.Find("Counter/Number").
			GetComponent<TextMesh>();
	}
			
	public void addBot(BombBot b){	
		b.turnOn();
		bots.Add(b);
		//set scale update in 1 second.
		Invoke("setCounter", 1);
	}
	
	public BombBot removeBot(){
		//should take top bot
		int index = Random.Range(0, bots.Count);
		BombBot b = bots[index];
		bots.RemoveAt(index);
		b.turnOff();
		setCounter();
		return b;
	}
	
	public void setCounter(){
		counter.text = ""+bots.Count;
	}
	
	public Vector3 getDropPoint(){
		return dropPoint;
	}
	
	public int botCount(){
		return bots.Count;
	}

	/*
	public void removeAllBots(){
		foreach(BombBot b in bots){
			Destroy(b.gameObject);
		}
		bots.Clear();
		setCounter();
	}
	*/
}
