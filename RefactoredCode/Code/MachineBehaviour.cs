using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehaviour : MonoBehaviour {

	public int value;
	public Products.Ingredients product;

	//Triggers when the machine is used. If a shake can be filled the machine will become active
	void OnMouseDown() {
		if (value > 0 && !Products.Instance.ActiveCup.capped && !Products.Instance.ActiveCup.inUse) {
			value--;
			Products.Instance.ActiveCup.AddProduct (product);
			StartCoroutine ("DropAnimation");
		}
	}

	//Starts when machine is activated. During the animation the shakes movement is deactivated.
	IEnumerator DropAnimation() {
		AudioManager.Instance.PlaySoundEffect (2);
		
		transform.GetChild (0).gameObject.SetActive (true);
		Products.Instance.ActiveCup.inUse = true;
		yield return new WaitForSeconds (.5f);
		
		transform.GetChild (0).gameObject.SetActive (false);
		Products.Instance.ActiveCup.inUse = false;
		yield return null;
	}
}
