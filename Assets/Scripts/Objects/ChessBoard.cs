using UnityEngine;
using System.Collections;

public class ChessBoard : MonoBehaviour {

	public enum Pieces {Queen,Rook,Bishop,Pawn};
	public static ChessBoard instance;

	void Awake(){
		instance = this;
	}

	public void Move(Pieces i){
		
		switch (i) {


			case Pieces.Queen:
				instance.GetComponent<Animator> ().Play ("Queen", 0);              
                break;

			case Pieces.Rook:
				instance.GetComponent<Animator> ().Play ("Rook", 0);
                break;

			case Pieces.Bishop:
				instance.GetComponent<Animator> ().Play ("Bishop", 0);
				break;

			case Pieces.Pawn:
				instance.GetComponent<Animator> ().Play ("Pawn", 0);
				break;
			}
        
    }

	public void DisablePieces(){
		for (int i = 0; i < 4; i++) {
			gameObject.transform.GetChild (i).gameObject.layer = LayerMask.NameToLayer("Default");
        }
	}

	public void EnablePieces(){
		for (int i = 0; i < 4; i++) {
			gameObject.transform.GetChild (i).gameObject.layer = LayerMask.NameToLayer("Interactive");
		}
	}


	public void StartDoorFocus(){
		gameObject.transform.GetChild (8).GetComponent<DoorChess> ().Open ();

	}

}
