using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactersManager : MonoBehaviour {

	public CharacterBehaviour[] characters;
	[HideInInspector]
	public int activeCharacter;
	[HideInInspector]
	public bool charactersAreActive;

	//Singleton
	private static charactersManager _instance;
	public static charactersManager Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	
	// Initialisation
	void Start(){
		activeCharacter = 0;
		charactersAreActive = false;
		
		for (int i = 0; i < characters.Length; i++) {
			characters [i].gameObject.SetActive (false);
		}
	}
	
	
	// Gets called when the player opens the shop
	public void LoadFirstCharacter () {
		activeCharacter = 0;
		charactersAreActive = true;
		
		if (characters [activeCharacter].alive) {
			characters [activeCharacter].gameObject.SetActive (true);
		}
		else {
			LoadNextCharacter ();
		}
	}

	//When a character leaves, the next character enters the shop
	public void LoadNextCharacter(){
		if (activeCharacter < characters.Length - 1) {
			characters [activeCharacter].gameObject.SetActive (false);
			activeCharacter += 1;
			
			if (characters [activeCharacter].alive) {
				characters [activeCharacter].gameObject.SetActive (true);
			}
			else {
				LoadNextCharacter ();
			}
		} else {
			characters [activeCharacter].gameObject.SetActive (false);
			charactersAreActive = false;
			GameManager.Instance.charactersFinished = true;
		}
	}

	//Return if any character is alive
	public bool IsAnyoneAlive () {
		foreach (CharacterBehaviour Person in characters) {
			if (Person.alive) {
				return true;
			}
		}
		return false;
	}
}
