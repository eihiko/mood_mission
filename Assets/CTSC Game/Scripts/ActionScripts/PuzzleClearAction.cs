using UnityEngine;
using System.Collections;

public class PuzzleClearAction : MissionAction {
	private PuzzleMiniGame.CreatePuzzleGrid grid;
	private PuzzleMiniGame.CreatePuzzlePieces pieceGenerator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){
		//ArrayList panels = new ArrayList ();
		//ArrayList pieces = new ArrayList ();
		foreach (Transform child in grid.transform) {
			GameObject.Destroy(child.gameObject);
			//panels.Add (child.gameObject);
		}
		foreach (Transform child in pieceGenerator.transform) {
			GameObject.Destroy(child.gameObject);
			//pieces.Add (child.gameObject);
		}
		/*GameObject[] panelArray = panels.ToArray (System.Type.GetType("GameObject"));
		GameObject[] pieceArray = pieces.ToArray (System.Type.GetType("GameObject"));
		for(int i=0;i<panelArray.Length;i++){
			GameObject.Destroy(panelArray[i]);
		}
		for (int j=0; j<pieceArray.Length; j++) {
			GameObject.Destroy (pieceArray [j]);
		}*/
		return true;
	}

	public PuzzleClearAction(PuzzleMiniGame.CreatePuzzleGrid toClear){
		grid = toClear;
		pieceGenerator = grid.puzzlePieceGenerator;
	}
}
