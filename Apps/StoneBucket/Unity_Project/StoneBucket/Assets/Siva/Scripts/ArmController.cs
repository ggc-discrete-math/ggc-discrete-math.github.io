using UnityEngine;
using System.Collections;

public class ArmController : MonoBehaviour {

	private ArmRota body;
	private ArmRota forearm;
	private ArmRota hand;
	private ArmRota topFinger;
	private ArmRota bottomFinger;
	
	private SpareBots spareBots;
	private BombBot bombBot;
	private TractorBeam tBeam;
	
	private BotContainer[] pads;
	public string[] padNames = {"A", "B", "C", "D"};
	
	private static int bodyIndex = 0;
	private static int forearmIndex = 1;
	private static int handIndex = 2;

	//pad index 4 is spare bots.
	int sparePadIndex = 4;
	private float[,] padAngles = new float[,] {
		{110,90,137},{155,95,127},{205,83,150},{250,70,171},{317, 100, 110}};
	
	
	void Start(){
		body = GameObject.Find("prop_robotArm_body").GetComponent<ArmRota>();
		forearm = GameObject.Find("prop_robotArm_arm").GetComponent<ArmRota>();
		hand = GameObject.Find("prop_robotArm_hand").GetComponent<ArmRota>();
		topFinger = GameObject.Find("prop_robotArm_clawTop_base").GetComponent<ArmRota>();
		bottomFinger = GameObject.Find("prop_robotArm_clawLow_base").GetComponent<ArmRota>();

		tBeam = GameObject.Find("TractorBeam").GetComponent<TractorBeam>();
		spareBots = GameObject.Find("SpareBotContainer").GetComponent<SpareBots>();//??
		pads = new BotContainer[padNames.Length];
		for(int i = 0; i<pads.Length; i++){
			GameObject hPad = GameObject.Find("prop_hoverPad" + padNames[i]);
			pads[i] = hPad.transform.Find("hoverPadLight (point)/BotContainer").
				GetComponent<BotContainer>();
		}

		//testing requires IEnumerator return
		//yield return null;
		//Application.CaptureScreenshot("SB_Screenshot.png", 3);
		//yield return null;
		//yield return StartCoroutine(toPad(sparePadIndex));
	}

	/*
	public void emptyAllPads(){
		foreach(BotContainer bc in pads){
			bc.removeAllBots();
		}
	}
	*/
	
	public IEnumerator addToPad(int padIndex){
		//remove bomb from spare pad		
		yield return StartCoroutine(toPad(sparePadIndex));
		bombBot = spareBots.removeBot();
		if(bombBot==null)
			yield break;
		tBeam.lift(bombBot.transform, spareBots.getDropPoint());
		while(tBeam.isLifting())
			yield return null;
		yield return StartCoroutine(pinch());

		yield return StartCoroutine(toPad(padIndex));
		pads[padIndex].addBot(bombBot);
		yield return StartCoroutine(release());

		//wait for bombot to fall
		yield return new WaitForSeconds(0.5f);

		yield return StartCoroutine(toUpHigh());
	}

	public IEnumerator removeFromPad(int padIndex){
		//remove bomb from pad		
		yield return StartCoroutine(toPad(padIndex));
		bombBot = pads[padIndex].removeBot();
		tBeam.lift(bombBot.transform, pads[padIndex].getDropPoint());
		while(tBeam.isLifting())
			yield return null;
		yield return StartCoroutine(pinch());
		
		//drop at spawn
		yield return StartCoroutine(toUpHigh());
		
		yield return StartCoroutine(toPad(sparePadIndex));
		yield return StartCoroutine(release());
		spareBots.addBot(bombBot);
		//wait for bombot to fall
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(toUpHigh());
	}

	public BotContainer[] getPads(){
		return pads;
	}
	
	

	IEnumerator toUpHigh(){
		forearm.rotateTo(100);		
		hand.rotateTo(110);
		while(hand.inMotion() || forearm.inMotion())
			yield return null;
	}
	
	IEnumerator toPad(int padIndex){
		body.rotateTo(padAngles[padIndex,bodyIndex]);
		while(body.inMotion())
			yield return null;

		hand.rotateTo(padAngles[padIndex,handIndex]);
		forearm.rotateTo(padAngles[padIndex,forearmIndex]);		
		while(hand.inMotion() || forearm.inMotion())
			yield return null;
	}
	
	
	IEnumerator pinch(){
		topFinger.rotateTo(5);
		bottomFinger.rotateTo(355);
		while(topFinger.inMotion() || bottomFinger.inMotion())
			yield return null;
		bombBot.transform.parent = hand.transform;
		//put bomb squarely inside fingers.
		bombBot.transform.localPosition = 
			new Vector3(3.000407f, -0.4857899f, 0.07097399f);
		bombBot.turnOff();//??
	}

	IEnumerator release(){
		topFinger.rotateTo(10);
		bottomFinger.rotateTo(350);
		while(topFinger.inMotion() || bottomFinger.inMotion())
			yield return null;
		bombBot.transform.parent = null;
	}
	
}
