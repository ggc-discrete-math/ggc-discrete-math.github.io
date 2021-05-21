using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GUIMenu : MonoBehaviour {

	public HelpPanel helpPanel;

	public GUIStyle btnStyle;
	public GUIStyle boxStyle;
	public GUIStyle labelStyle;

	private float buttonToScreenWidthPercent = (1.0f/7);
	private float buttonHeighttoWidthPercent = (1.0f/4);
	private int btnW;
	private int btnH;
	private int bigSpace;
	private int space;

	private Rect menuArea;
	private List<Rect> resetMenuBtnRects;

	//private Rect cmdMenuArea;
	private List<Rect> cmdMenuBtnRects;

	public int[] resetValues;

	private float defSpeed = 1;
	private float maxSpeed = 10;
	private float speed;
	private bool offGUI = false;
	private bool resetting = false;
	private bool reset_filling = false;
	private float timer = -1;
	private CmdBlockSpawner cmdSpawner;
	private ArmController robotArm;


	void Start () {
		resetValues = new int[4];
		speed = defSpeed;
		Time.timeScale = speed;
		cmdSpawner = GameObject.Find("BlockPad").GetComponent<CmdBlockSpawner>();
		robotArm = GameObject.Find("prop_robotArm").GetComponent<ArmController>();
		
		//repeat initCoords every 1/2 sec for window resizing.
		InvokeRepeating("initCoords", 0, 0.5f);
		
		//Debug FPS
		//StartCoroutine(calcFPS());
	}

	private void initCoords(){
		float x, y;

		//calc stnd btn dimensions.
		btnW = (int) (Screen.width*buttonToScreenWidthPercent);
		btnH = (int) (btnW * buttonHeighttoWidthPercent);
		bigSpace = Screen.width/100;
		space=bigSpace/2;

		//dynamic btnStyle properties. (overflow for sqaure btnTexture transparent area)
		btnStyle.overflow = new RectOffset(0,0,(btnW-btnH)/2, (btnW-btnH)/2);
		btnStyle.fontSize=(int)(0.75*btnH);
		labelStyle.fontSize=(int)(0.75*btnH);

		int numRows = 4;
		float menuAreaW = 2*btnW+5*bigSpace;
		float menuAreaH = bigSpace+numRows*(bigSpace+btnH);
		menuArea = new Rect(Screen.width - bigSpace - menuAreaW, Screen.height-bigSpace-menuAreaH, menuAreaW, menuAreaH);

		resetMenuBtnRects = new List<Rect>();
		cmdMenuBtnRects = new List<Rect>();
		x = menuArea.x+2*bigSpace;
		y = menuArea.y+2*bigSpace;
		for(int i = 0; i<numRows; i++){
			resetMenuBtnRects.Add(new Rect(x,y,btnW,btnH));
			if(i==3)//slider specific placement
				cmdMenuBtnRects.Add(new Rect(x+bigSpace+btnW,y+bigSpace,btnW,btnH));
			else
				cmdMenuBtnRects.Add(new Rect(x+bigSpace+btnW,y,btnW,btnH));
			y+=btnH+space;
		}
	}

	void OnGUI(){
		//Draw FPS label
		//GUI.Label(new Rect(Screen.width - 100,10,100,100), "FPS: " + fps);
		//Take screenshot btn.
		/*
		if(GUI.Button(new Rect(Screen.width-50, 0, 50,50), "Pic"))
			Application.CaptureScreenshot("SB_Screenshot.png", 3);
			*/

		//Menu Box
		GUI.Box(menuArea, GUIContent.none, boxStyle);

		GUI.enabled = !resetting;
		//RESET BUTTON
		if(GUI.Button(resetMenuBtnRects[3], "Reset", btnStyle)){
			Time.timeScale = maxSpeed;
			//stop the current execution of commands.
			if(cmdSpawner.isExecutingCmds())
				cmdSpawner.forceReset();
			resetting = true;
		}
		if(resetting){
			if(cmdSpawner.isExecutingCmds())
				return;
			if(reset_filling)
				return;
			//GameObject.Find("prop_robotArm").GetComponent<ArmController>().emptyAllPads();
			//fill buckets up to resetValues
			reset_filling=true;
			StartCoroutine(preFillBucketsForReset());
		}

		GUI.enabled = true;


		//RESET VALUE BUTTONS
		int numRecs = 4;
		for(int i = 0; i<numRecs; i++){
			Rect rArea = resetMenuBtnRects[1];
			float w = (rArea.width - space*(numRecs+1))/4;
			
			Rect r = new Rect(space+rArea.x + i*(w+space), rArea.y, w,btnH);
			if(GUI.Button(r, robotArm.padNames[i], btnStyle)){
				resetValues[i]++;
				if(resetValues[i]>5)
					resetValues[i]=0;
			}
			
			rArea = resetMenuBtnRects[2];
			r = new Rect(space+rArea.x + i*(w+space), rArea.y, w,btnH);
			GUI.Label(r, ""+resetValues[i], labelStyle);
		}

		//HELP BTN
		if(GUI.Button(resetMenuBtnRects[0], "Help", btnStyle)){
			//enabled = false;
			//GetComponent<HelpScreen>().enabled = true;
			helpPanel.show();
		}

		//SPEED SLIDER
		float newSpeed = GUI.HorizontalSlider (cmdMenuBtnRects[3], speed, 1.0f, maxSpeed);
		if(newSpeed!=speed){
			speed = newSpeed;
			Time.timeScale = speed;
		}

		//waiting for action to complete before resetting input back to on
		if(offGUI){
			GUI.enabled = false;
			//must wait on coroutine to begin before checking its status.
			if(checkTimer(0.5f))
				offGUI = false;
		}else{
			//wait till coroutine to complete, before turning on GUI
			GUI.enabled = (!cmdSpawner.isExecutingCmds() && 
			               !cmdSpawner.isMovingCmdBlocks());
		}

		//RUN CMDS
		if(GUI.Button(cmdMenuBtnRects[0], "Run", btnStyle)){
			cmdSpawner.executeCmds();
			offGUI = true;
		}
		
		//ADD CMD
		if(GUI.Button(cmdMenuBtnRects[1], "New Cmd", btnStyle)){
			StartCoroutine(cmdSpawner.spawnNewCmdBlock());
			offGUI = true;
		}

		//REMOVE CMD
		//Don't remove last command.
		if(GUI.Button(cmdMenuBtnRects[2], "Del Cmd", btnStyle) && cmdSpawner.getCmdCount()>1){
			StartCoroutine(cmdSpawner.removeCmdBlock());
			offGUI = true;
		}
	}

	public IEnumerator preFillBucketsForReset(){
		BotContainer[] pads = robotArm.getPads();
		for(int i = 0; i<resetValues.Length; i++){
			while(pads[i].botCount()!=resetValues[i]){
				if(pads[i].botCount()>resetValues[i]){
					yield return StartCoroutine(robotArm.removeFromPad(i));
				}
				else{
					yield return StartCoroutine(robotArm.addToPad(i));
				}
			}
		}
		Time.timeScale = speed;
		resetting = false;
		reset_filling=false;
	}

	/*
	private float sum = 0;
	private float fps;
	private int count = 0;
	IEnumerator calcFPS() {
		while(true){
			//reset avg every 5 sec.
			if(count>=5){
				count = 0;
				sum = 0;
			}
			count++;
			sum += (1.0f/Time.deltaTime);
			fps = sum/count;
			
			yield return new WaitForSeconds(1.0f);
		}
	}
	*/
	private bool checkTimer(float seconds){
		if(timer==-1){
			timer=Time.time+seconds;
			return false;
		}
		if(Time.time>timer){
			timer = -1;
			return true;
		}
		return false;
	}


}
