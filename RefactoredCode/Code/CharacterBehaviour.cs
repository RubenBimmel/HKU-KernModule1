using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour {

	public List<ShakeBehaviour> cups;
	public int[] nutritionValues;
	public int lactoseMultiplier;
	public int sojaMultiplier;
	public bool alive;
	public Text characterMessage;
	public GameObject characterStats;

	public int startingAge;
	private int age;
	public Text ageText;

	public string[] messages;

	// Initialisation
	void Start() {
		nutritionValues = new int[] {80, 80, 80, 80};
	}
	
	//All characters get activated once a day (if alive). All character information is updated each day:
	void OnEnable() {
		if (GameManager.Instance.day < messages.Length)
			characterMessage.text = messages [GameManager.Instance.day];
		else {
			Debug.LogError (string.Concat (gameObject.name, " - Character message missing on day ", GameManager.Instance.day.ToString ()));
		}

		age = startingAge + (GameManager.Instance.period * 3);
		ageText.text = age.ToString ();
	}
	
	//Triggers mouseover character information
	void OnMouseEnter() {
		characterStats.SetActive (true);
	}

	//Ends mouseover character information
	void OnMouseExit() {
		characterStats.SetActive (false);
	}
	
	//Clicking on a character allows the player to give objects (Shakes)
	void OnMouseDown() {

		//When person is given a shake
		if (Products.Instance.ActiveCup.active && Products.Instance.ActiveCup.capped) {
			//Remove active shake
			Products.Instance.SetActiveCupActive(false);
			Products.Instance.ActiveCup.gameObject.SetActive (false);
			
			AudioManager.Instance.PlaySoundEffect (1);

			//Add cup to library
			cups.Add (Products.Instance.ActiveCup);

			//Caclculate nutrition change and death
			int[] nutritionChange = Products.Instance.CalculateNutritions (lactoseMultiplier, sojaMultiplier);

			for (int i = 0; i < nutritionValues.Length; i++) {
				nutritionValues [i] += nutritionChange [i];
				if (nutritionValues [i] > 100)
					nutritionValues [i] = 100;
				else if (nutritionValues [i] <= 0)
					alive = false;
			}
			
			Invoke ("LoadNextCharacter", 1f);
		}
	}

	//Simple void function so this can be delayed using invoke
	private void LoadNextCharacter (){
		CharactersManager.Instance.LoadNextCharacter ();
	}
}
