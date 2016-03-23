using UnityEngine;
using System.Collections;

public class BlacksmithPuzzleAction : MissionAction {

	public bool puzzleComplete=false;

	private PuzzleMiniGame.CreatePuzzleGrid Grid;
	private PuzzleMiniGame.CreatePuzzlePieces Pieces;
	private int dimensions;
	private bool generated;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute() {
		if (!generated) {
			Pieces.action = this;
			Grid.DIMENSIONS = dimensions;
			Grid.StartGeneratingGrid ();
			generated = true;
		}
		return puzzleComplete;
	}

	public BlacksmithPuzzleAction(PuzzleMiniGame.CreatePuzzleGrid grid, int dimensions, TakenImage picture){
		this.Grid = grid;
		this.Pieces = Grid.puzzlePieceGenerator;
		Pieces.image = picture;
		this.dimensions = dimensions;
		generated = false;
	}
}
