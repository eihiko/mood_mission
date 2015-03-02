using UnityEngine;

public abstract class Conversation : MonoBehaviour {
	private int state;
	public class Option {
		public string message;
		public Dialogue to;
		public int newState;
		public Option(string message, Dialogue to, int newState) {

		}
	}
	public class Dialogue {
		private string message;
		private Option[] options;
		public Dialogue(string message, params Option[] options) {
			this.message = message;
			this.options = options;
		}
		public Dialogue Display(Rect p) {
			return this;
		}
	}
	public abstract void Begin(Transform focus);
}