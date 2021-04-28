using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour, Interactable {

	public int startPosition;
	int realPosition;
	int actualPosition;
	Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		
		actualPosition = startPosition;
		targetPosition = getVectorPosition (startPosition);
			
	}

	void Update()
	{
		while(targetPosition != this.transform.localPosition)
		{
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPosition, 10.0f * Time.deltaTime);
		}

	}

	public void SetPointer() {

		if (Puzzle.instance.isMovable (actualPosition)) {
			PointerManager.Instance.SetHand();
		}
		else
			PointerManager.Instance.SetPoint();
	}

	public void Interact() {
		
		if(Puzzle.instance.isMovable (actualPosition)){
			Puzzle.instance.startMovement (realPosition,actualPosition);
            GetComponent<AudioSource>().Play();
		}
		
	}

	public void setRealPosition(int i){
	
		realPosition = i;
	}

	public int getStartPosition(){

		return startPosition;
	}

	public void setPosition(int i){

		this.transform.localPosition = getVectorPosition (i);
	}


	public void move(int targetPoint){

		actualPosition = targetPoint;
		targetPosition = getVectorPosition (targetPoint);
	}

	public bool checkRightPosition(){
	
		if (realPosition == actualPosition)
			return true;
		else
			return false;	
	
	}

	public void complete(){
	
		gameObject.layer = LayerMask.NameToLayer("Default");
	
	}

	Vector3 getVectorPosition(int i){

		switch (i) {			
			case 0:
				return new Vector3(-1,1,0);
			case 1:
				return new Vector3 (-0,1,0);
			case 2:
				return new Vector3 (1,1,0);
			case 3:
				return new Vector3 (-1,0,0);
			case 4:
				return new Vector3 (0,0,0);
			case 5:
				return new Vector3 (1,0,0);
			case 6:
				return new Vector3 (-1,-1,0);
			case 7:
				return new Vector3 (0,-1,0);
			case 8:
				return new Vector3 (1,-1,0);
			default:
				return new Vector3 (0,0,0);
		}
	}

}
