using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Piece : MonoBehaviour, Interactable {

	public ChessBoard.Pieces id;

	public void SetPointer(){

		PointerManager.Instance.SetHand ();
	}

	public void Interact(){
		ChessBoard.instance.Move (id);
	}      


} 
