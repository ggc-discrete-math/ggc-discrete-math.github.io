using UnityEngine;
using System.Collections;

public class CmdBtn : ButtonAction {
	
	public Material addMat;
	public Material subMat;
	public Color addColor;
	public Color subColor;
	public string addText;
	public string subText;
	
	
	private static int ADD_STATE = 0;
	private static int SUB_STATE = 1;
	private static int NUM_OF_STATE = 2;
	private int state = SUB_STATE;
	
	private TextMesh btnText;	
	private Transform cmdBlock;
	private StepArrow stepArrow;
	
	private PadBtn padBtn;
	private GotoBtn gotoBtn;
	private EmptyBtn emptyBtn;
	
	private ArmController robotArm;
	private CmdBlockSpawner cmdSpawner;
	
	void Start () {
		cmdBlock = transform.parent.parent;
		stepArrow = GameObject.Find("StepArrow").GetComponent<StepArrow>();
		btnText = transform.parent.Find("Text").GetComponent<TextMesh>();
		padBtn = cmdBlock.Find("PadBtn/RoundButton").GetComponent<PadBtn>();
		gotoBtn = cmdBlock.Find("GotoBtn/RoundButton").GetComponent<GotoBtn>();
		emptyBtn = cmdBlock.Find("EmptyBtn/RoundButton").GetComponent<EmptyBtn>();
		cmdSpawner = GameObject.Find("BlockPad").GetComponent<CmdBlockSpawner>();
		
		robotArm = GameObject.Find("prop_robotArm").GetComponent<ArmController>();
		
		onClick();//swap to add block
	}
	
	void Update () {
	}
	
	public override void onClick(){
		//toggle state
		state = (state+1)%(NUM_OF_STATE);
		
		if(state==ADD_STATE){
			emptyBtn.turnOff();
			btnText.text = addText;			
			changeColor(addMat, addColor);
		}
		else{//SUB_STATE
			emptyBtn.turnOn();
			btnText.text = subText;
			changeColor(subMat, subColor);			
		}
	}
	
	private void changeColor(Material m, Color c){
		cmdBlock.GetComponent<Renderer>().material = m;
		foreach(ButtonAction btn in cmdBlock.GetComponentsInChildren<ButtonAction>())
			btn.setColor(c);
	}
	
	public IEnumerator execute(){
		int padIndex = padBtn.getPadIndex();
		bool emptyPad = containerIsEmpty(padBtn.getPadName());
		int gotoStepNum;
		
		if(btnText.text == subText && emptyPad){
			gotoStepNum = emptyBtn.getStepNum();
			emptyBtn.highLight();
		}
		else{
			this.highLight();
			padBtn.highLight();
			if(btnText.text == addText)
				yield return StartCoroutine(robotArm.addToPad(padIndex));
			else
				yield return StartCoroutine(robotArm.removeFromPad(padIndex));
			this.unHighLight();
			padBtn.unHighLight();
			gotoStepNum = gotoBtn.getStepNum();
			gotoBtn.highLight();
		}
		
		//go to next step and execute.
		if(gotoStepNum!=0){
			yield return StartCoroutine(stepArrow.switchToStep(
				cmdSpawner.getCmdBlock(gotoStepNum)));
			gotoBtn.unHighLight();
			emptyBtn.unHighLight();
		}
		cmdSpawner.executeCmd(gotoStepNum);
	}
		
	private bool containerIsEmpty(string containerName){
		GameObject padObj = GameObject.Find("prop_hoverPad" + containerName);
		return padObj.GetComponentInChildren<BotContainer>().botCount()==0;
	}
	
}

