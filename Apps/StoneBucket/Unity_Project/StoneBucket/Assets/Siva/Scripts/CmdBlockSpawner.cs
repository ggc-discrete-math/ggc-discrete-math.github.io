using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CmdBlockSpawner : MonoBehaviour {
	public GameObject cmdBlockPrefab;
	private List<GameObject> cmdBlocks;
	
	private StepArrow stepArrow;
	private Gap gap;
	
	private Vector3 initPos = new Vector3(0,1.25f,0);
	private float riseTime = 0.5f;
	private float riseHeight = 0.6f;
	private float riseSoFar;
	private bool rising;
	private bool dropping;
	
	private bool executingCmds = false;
	private bool reset = false;
	
	void Start(){
		gap = transform.Find("Gap").GetComponent<Gap>();
		stepArrow = GameObject.Find("StepArrow").GetComponent<StepArrow>();
		cmdBlocks = new List<GameObject>();
		rising = false;
		dropping = false;
		StartCoroutine(cmdBlock(true));
	}
	
	void Update(){
		if(rising || dropping){
			float y = riseHeight*Time.deltaTime/riseTime;
			if(riseSoFar+y>=riseHeight){
				//snap to riseHeight
				y = riseHeight - riseSoFar;
			}
			riseSoFar += y;
			
			y = dropping?-y:y;
			foreach(GameObject cb in cmdBlocks){
				cb.transform.Translate(0,y,0);
			}
			
			if(riseSoFar>=riseHeight){
				rising = false;
				dropping = false;
			}
		}
	}
		
	public IEnumerator spawnNewCmdBlock(){
		yield return StartCoroutine(cmdBlock(true));
		//zoom Camera out until cmd block 1 is visible.
		GameObject cmdOne = getCmdBlock(1);
		Vector3 topSpt = cmdOne.transform.position;
		topSpt.y += cmdOne.transform.localScale.y/2;
		yield return StartCoroutine(
			Camera.main.GetComponent<CameraMover>().
			zoomOutUntiInView(topSpt));	
	}
		
	public IEnumerator removeCmdBlock(){
		yield return StartCoroutine(cmdBlock(false));
		//zoom camera in if we are too far out.
		GameObject cmdOne = getCmdBlock(1);
		Vector3 topSpt = cmdOne.transform.position;
		topSpt.y += cmdOne.transform.localScale.y;
		yield return StartCoroutine(
			Camera.main.GetComponent<CameraMover>().
			zoomInUntilOutOfView(topSpt));	
	}
	
	private IEnumerator cmdBlock(bool create){
		GetComponent<AudioSource>().Play();
		//open gap
		gap.stretch();
		while(gap.isStretching())
			yield return null;
		
		if(create){
			GameObject cb = createCmdBlock();
			//rise all blocks.
			riseSoFar = 0;
			rising = true;
			while(rising)
				yield return null;
			if(getCmdCount()==1)
				yield return StartCoroutine(stepArrow.switchToStep(cb));
		}
		else if(cmdBlocks.Count>0){
			//move step arrow to top block if not already there.
			executingCmds = true;//tell GUI we are busy.
			if(stepArrow.getCurrentCmdBlock()!=cmdBlocks[0])
				yield return StartCoroutine(stepArrow.switchToStep(cmdBlocks[0]));
			executingCmds=false;
			//drop all blocks
			riseSoFar = 0;
			dropping = true;
			while(dropping)
				yield return null;
			//remove last cmd block
			GameObject cb = cmdBlocks[cmdBlocks.Count-1];
			cmdBlocks.RemoveAt(cmdBlocks.Count-1);
			Destroy(cb);
		}
		
		//close gap
		gap.shrink();
		while(gap.isShrinking())
			yield return null;
		GetComponent<AudioSource>().Stop();
	}
	
	private GameObject createCmdBlock(){
		GameObject cmdBlock = GameObject.Instantiate(cmdBlockPrefab) as GameObject;
		cmdBlocks.Add (cmdBlock);
		//set parent to CmdBlocks GameObject
		cmdBlock.transform.parent = transform.parent;
		cmdBlock.transform.localPosition = initPos;
		cmdBlock.transform.localRotation = Quaternion.identity;
		//set label
		cmdBlock.transform.Find("StepLbl/Text").
			GetComponent<TextMesh>().text = cmdBlocks.Count+".";
		return cmdBlock;
	}
	
	public void executeCmds(){
		if(cmdBlocks.Count==0)
			return;
		StartCoroutine(restart());
	}
	
	public IEnumerator restart(){
		//unhighlight previous stop btn.
		GameObject prevCmd = stepArrow.getCurrentCmdBlock();
		foreach(ButtonAction btn in prevCmd.GetComponentsInChildren<ButtonAction>())
			btn.unHighLight();
		
		//move arrow to cmd 1
		yield return StartCoroutine(stepArrow.switchToStep(getCmdBlock(1)));
		
		executeCmd(1);
	}
	
	public void forceReset(){
		reset = true;
	}
	
	public void executeCmd(int stepNum){
		executingCmds = (stepNum>0) && !reset;

		if(!executingCmds){
			reset = false;
			//Stop the machine!
			return;
		}
		StartCoroutine(getCmdBlock(stepNum).GetComponentInChildren<CmdBtn>().execute());
	}

	public GameObject getCmdBlock(int stepNum){
		return cmdBlocks[stepNum-1];
	}

	public int getCmdCount(){
		return cmdBlocks.Count;
	}
	
	public bool isExecutingCmds(){
		return executingCmds;
	}
	
	public bool isMovingCmdBlocks(){
		return rising || dropping;
	}
}
