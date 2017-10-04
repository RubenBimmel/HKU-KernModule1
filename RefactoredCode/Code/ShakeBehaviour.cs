using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehaviour : MonoBehaviour {

	[HideInInspector]
	public int[] values;
	public Transform cap;
	private ShakeLevelBar bar;

	private const int MAX_CONTENT = 5;
	private const float MAX_CUP_Y_POS = -.5f;
	private const float MOVE_SPEED = .2f;
	private Vector3 mousePosition;

	[HideInInspector]
	public bool active;
	[HideInInspector]
	public bool inUse;
	[HideInInspector]
	public bool capped;

	// Initialisation
	void Start() {
		bar = transform.FindChild ("CupBar").FindChild ("CupBarValue").GetComponent<ShakeLevelBar> ();
		values = new int[(int)Products.Ingredients._Length];
		cap.gameObject.SetActive (false);
		inUse = false;
		capped = false;
	}

	// Gets called each frame
	void Update() {
		if (active) {
			UpdatePosition();
		}
	}

	//When active the cup will follow the mouse, limited on the y axis by the MAX_CUP_Y_POS value
	void UpdatePosition() {
		if (!inUse) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
			mousePosition.y = Mathf.Min (mousePosition.y, MAX_CUP_Y_POS);
		}

		//The lerp creates a smooth motion. The Z position gets forced back because of clipping issues.
		Vector3 cupPosition = Vector2.Lerp(transform.position, mousePosition, MOVE_SPEED);
		cupPosition.z = -3;
		transform.position = cupPosition;
	}

	//Gets called when a machine is used
	public void AddProduct (Products.Ingredients p) {
		int product = (int)p;
		int content = GetContent();

		if (content < MAX_CONTENT) {
			//Add product to cup
			values [product]++;
			bar.targetValue = ((float)content + 1f) / (float)MAX_CONTENT;
		} else {
			//Waste product
		}
	}

	//Gets activated when player picks up a cap
	public void AddCap (){
		cap.gameObject.SetActive (true);
		capped = true;
	}

	//Sums the content from all ingredients
	public int GetContent () {
		int content = 0;
		for (int i = 0; i < (int)Products.Ingredients._Length; i++) {
			content += values [i];
		}
		return content;
	}
}
