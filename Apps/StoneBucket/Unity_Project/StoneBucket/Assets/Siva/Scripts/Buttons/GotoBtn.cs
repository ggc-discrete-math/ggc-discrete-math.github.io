using UnityEngine;
using System.Collections;

public class GotoBtn : ButtonAction {
	protected TextMesh btnText;
	private int stepNum;
	private CmdBlockSpawner cmdSpawner;
	
	public void Start () {
		cmdSpawner = GameObject.Find("BlockPad").GetComponent<CmdBlockSpawner>();
		btnText = transform.parent.Find("Text").GetComponent<TextMesh>();
		btnText.text = "Stop";
		stepNum = 0;
	}

	public override void onClick(){
		int max = cmdSpawner.getCmdCount();
		stepNum++;
		if(stepNum>=max+1){
			stepNum=0;
			btnText.text = "Stop";
		}
		else{
			btnText.text = "Go " + stepNum;
		}
		
	}
	
	public int getStepNum(){
		//goto stepNum may be too big if commands have been removed.
		if(stepNum>cmdSpawner.getCmdCount()){
			btnText.text = "Stop";
			stepNum = 0;
			//stops machine
		}
		return stepNum;
	}
}
