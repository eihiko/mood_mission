using UnityEngine;
using System.Collections;

public class GiveAction : MissionAction {

	GameObject from, to;
	GrabMe.kind kind;
	public GiveAction(GameObject fromA, GameObject toA, GrabMe.kind kindA){
		from = fromA;
		to = toA;
		kind = kindA;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute(){

		bool decision = false;
		switch (kind) {

		case GrabMe.kind.AMULET:
			decision = giveToPlayer(kind);
			break;
//		case GrabMe.kind.HEALING_WATER:
//		case GrabMe.kind.HERB:
//			decision = giveToTorkana(kind);
//			break;
		case GrabMe.kind.LETTERS:
		case GrabMe.kind.TOOLS:
			if (to.GetComponent<CharacterOurs>() != null) {
				decision = giveToPlayer (kind);
			}
			else {
				decision = giveToNPC (kind);
			}
			break;
		}

		return decision;
	}

	bool giveToPlayer(GrabMe.kind theKind){
		from.GetComponent<NPC_Character> ().setGiveItem (true);
		return to.GetComponent<CharacterOurs>().has(theKind);
	}

	bool giveToNPC(GrabMe.kind thisKind){
		NPC_Character NPCto = to.GetComponent<NPC_Character> ();
		from.GetComponent<CharacterOurs> ().drop (thisKind, NPCto.placeToDrop);
		NPCto.setTakeItem (true);
		NPCto.grab (NPCto.item);
		return !from.GetComponent<CharacterOurs> ().has (thisKind);
	}

    bool giveToTorkana(GrabMe.kind theKind){

		return false;
	}
}
