using UnityEngine;
using System.Collections;

public class DoorChess : Focusable {


	void Update () {
		CheckFocusTime ();
	}

	public void Open(){
		StartFocus ();
	}
}
