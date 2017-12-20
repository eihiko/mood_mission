using UnityEngine;
using System.Collections;

public class AlterStatAction : MissionAction {
	
	private PlayerStatusBars bars;
	private string stat;
	private int amt;
	private bool increase;

	public AlterStatAction(string changedStat, int amount, bool increase){
		this.stat = changedStat;
		this.amt = amount;
		this.increase = increase;
		this.bars = GameObject.Find ("ControllerBody").GetComponent<PlayerStatusBars>();
	}

	public bool execute(){
		if (increase) {
			bars.IncreaseStat (amt, stat);
		} else {
			bars.DecreaseStat (amt, stat);
		}
		return true;
	}
}
