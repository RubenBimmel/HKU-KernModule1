using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Products : MonoBehaviour {

	public enum Ingredients {Meat, Fish, Corn, Soy, Dairy, Vegetables, Discharge, _Length};
	private enum Nutritions {Fats, Proteins, Carbohydrates, Vitamins};
	public ShakeBehaviour activeCup;
	public ShakeBehaviour cupPrefab;
	public Transform cupsParent;
	public UIArrowBehaviour shopComputerButton;

	//Singleton
	private static Products _instance;
	public static Products Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	//Gets called when a player picks up a new cup
	public void AddCup() {
		ShakeBehaviour Cup = (ShakeBehaviour)Instantiate (cupPrefab);
		Cup.transform.parent = cupsParent;
		Cup.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		
		activeCup = Cup;
		SetactiveCupActive (true);
	}

	//Gets called when the player picks up or releases a cup
	public void SetactiveCupActive(bool b) {
		activeCup.active = b;
		shopComputerButton.SetActive (!b);
	}

	//Gets asked by the character when a shake is given
	public int[] CalculateNutritions(int LactoseMultiplier, int SojaMultiplier) {
		int[] nutritionCupValues = new int[4];
		nutritionCupValues [(int)Nutritions.Fats] = activeCup.Values [(int)Ingredients.Fish] * 10 + activeCup.Values [(int)Ingredients.Dairy] * 10 * LactoseMultiplier;
		nutritionCupValues [(int)Nutritions.Proteins] = activeCup.Values [(int)Ingredients.Meat] * 10 + activeCup.Values [(int)Ingredients.Fish] * 10 + activeCup.Values [(int)Ingredients.Soy] * 10 * SojaMultiplier;
		nutritionCupValues [(int)Nutritions.Carbohydrates] = activeCup.Values [(int)Ingredients.Corn] * 10 + activeCup.Values [(int)Ingredients.Soy] * 10 * SojaMultiplier;
		nutritionCupValues [(int)Nutritions.Vitamins] = activeCup.Values [(int)Ingredients.Vegetables] * 20;
		
		for (int i = 0; i < 4; i++) {
			nutritionCupValues [i] -= 20;
			nutritionCupValues [i] -= activeCup.Values [(int)Ingredients.Discharge] * 10;
		}
		
		return nutritionCupValues;
	}
		
}
