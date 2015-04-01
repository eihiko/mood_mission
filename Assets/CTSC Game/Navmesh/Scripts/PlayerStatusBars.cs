using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatusBars : MonoBehaviour {


	public RectTransform healthTransform;
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	private int currentHealth;


	private int CurrentHealth
	{
		get 
		{ 
			return currentHealth; 
		}
		set 
		{
			currentHealth = value;
			HandleHealth();
		}
	}
	public float maxHealth;
	public Text healthText;
	public Image visualHealth; //This field is for setting the color of the healthbar
	public float damageCooldown;
	private bool onCD;
	// Use this for initialization
	void Start () {
		cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = (int)maxHealth;
		onCD = false;

	}

	
	// Update is called once per frame
	void Update () {

	}

	public void TakeDamage (int damage) 
	{

		int counter = 0;

			if (!onCD && currentHealth > 0)
			{
				StartCoroutine(CoolDownDmg(damage, true));
			}

	}

	public void GainHealth (int health) 
	{
		int counter = 0;
		if (!onCD && currentHealth < maxHealth)
		{
				StartCoroutine(CoolDownDmg(health, false));
		}

	}
	IEnumerator CoolDownDmg(int counter, bool isDamage) 
	{
		while(counter > 0) {
			onCD = true;
			if(isDamage && currentHealth > 0) {

				CurrentHealth -= 1;
			}
			else if(!isDamage && currentHealth < maxHealth) {
				CurrentHealth += 1;
			}
			yield return new WaitForSeconds (damageCooldown);
			onCD = false;
			counter--;
		}
	}

	/*void OnTriggerStay(Collider other) 
	{
		if(other.gameObject.tag == "Player")
		{
			//Debug.Log(onCD + "  " + currentHealth);
			if (!onCD && currentHealth > 0)
			{

				StartCoroutine(CoolDownDmg(0));
				CurrentHealth -= 1;
			}
		}
	}*/


	void HandleHealth()
	{
		healthText.text = "Health: " + currentHealth;

		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY);
	}

	float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
