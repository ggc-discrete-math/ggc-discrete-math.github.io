using UnityEngine;
using System.Collections;

public class ButtonChecker : MonoBehaviour {
	private CmdBlockSpawner cmdSpawner;
	GameObject helpPanel;
	void Start () {
		cmdSpawner = GameObject.Find("BlockPad").GetComponent<CmdBlockSpawner>();
		helpPanel = GameObject.Find("HelpPanel");
	}
	
	void Update () {
		if(cmdSpawner.isExecutingCmds() || cmdSpawner.isMovingCmdBlocks())
			//Ignore all commands whie machine is running
			return;
		if(helpPanel.activeSelf)
			//Ignore commands while help screen is up.
			return;

		if ( Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)){
			RaycastHit hitInfo;
			Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (r, out hitInfo)){
				if(hitInfo.collider.name=="RoundButton"){
					ButtonAction currentBtn = hitInfo.collider.GetComponent<ButtonAction>();
					if(Input.GetMouseButtonDown(0))
						currentBtn.onClickDown();
					if(Input.GetMouseButtonUp(0))
						currentBtn.onClickUp();
				}
			}
		}
	}
}
