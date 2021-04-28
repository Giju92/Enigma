using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {

	public static Puzzle instance;
	private GameObject[] TileDisplay = new GameObject[9];
	int freeTile;

	// this puzzle texture.
	public Texture PuzzleImage;

	// the shader used to render the puzzle.
	public Shader PuzzleShader;

	void Awake(){
		//set the singleton
		instance = this;

		//fill the array and set the real value
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				int offset = i * 3 + j;

				TileDisplay [offset] = instance.gameObject.transform.GetChild (offset).gameObject;
				//set position
				if (offset != 8) {
					TileDisplay [offset].GetComponent<Tile> ().setRealPosition (offset);
				}

				// create a new material using the defined shader.
				Material thisTileMaterial = new Material (PuzzleShader);

				// apply the puzzle image to it.
				thisTileMaterial.mainTexture = PuzzleImage;

				if (i == 0) {
					// set the offset and tile values for this material.
					thisTileMaterial.mainTextureOffset = new Vector2 ((1.0f / 3) * j, ((1.0f / 3) * -1));
					thisTileMaterial.mainTextureScale = new Vector2 (1.0f / 3, 1.0f / 3);

				} else if (i == 2) {
					// set the offset and tile values for this material.
					thisTileMaterial.mainTextureOffset = new Vector2 ((1.0f / 3) * j, ((1.0f / 3) * 0));
					thisTileMaterial.mainTextureScale = new Vector2 (1.0f / 3, 1.0f / 3);
				
				} else {
					// set the offset and tile values for this material.
					thisTileMaterial.mainTextureOffset = new Vector2 ((1.0f / 3) * j, ((1.0f / 3) * i));
					thisTileMaterial.mainTextureScale = new Vector2 (1.0f / 3, 1.0f / 3);
				}
				// assign the new material to this tile for display.
				TileDisplay [offset].GetComponent<Renderer> ().material = thisTileMaterial;

				}
			}

		// find the free tile
		for (int i = 0; i < 9; i++) {
			int flag = 1;
			for (int j = 0; j < 8 && flag==1 ; j++) {
				if (i == (TileDisplay [j].GetComponent<Tile> ().getStartPosition ())) { 
					flag = 0;	
				}
			}
			if (flag == 1) {
				freeTile = i;
				i = 9; 
			}				
		}


	}

	public bool isMovable(int i){

		int free_col = freeTile % 3;
		int free_row = freeTile / 3;

		int start_col = i % 3;
		int start_row = i / 3;

		if (free_row == start_row) {
			if (start_col + 1 < 3 && start_col + 1 == free_col)
				return true;
			else if (start_col - 1 >= 0 && start_col - 1 == free_col)
				return true;
		}

		if (free_col == start_col) {
			
			if (start_row + 1 < 3 && start_row + 1 == free_row)
				return true;
			else if (start_row - 1 >= 0 && start_row - 1 == free_row)
				return true;
		}

		return false;
	}

	public void startMovement(int idTile, int startPosition ){

		TileDisplay [idTile].GetComponent<Tile> ().move (freeTile);
		freeTile = startPosition;

		checkComplete ();	
	}

	void checkComplete(){

		int flag = 1;

		for (int i = 0; i < 8; i++) {
			if (!TileDisplay [i].GetComponent<Tile> ().checkRightPosition ())
				flag = 0;
		}

		if (flag == 1) {		
			for (int i = 0; i < 8; i++) {
				TileDisplay [i].GetComponent<Tile>().complete();					
			}
		
			instance.gameObject.transform.GetChild (8).gameObject.GetComponent<LastTile> ().active ();
		
		}
			
	}

	
	// Update is called once per frame
	void Update () {


	
	}
}
