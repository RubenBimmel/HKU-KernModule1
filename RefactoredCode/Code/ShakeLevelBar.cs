using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeLevelBar : MonoBehaviour {

	private Material mat;
	public float targetValue;
	private float value;

	// Initialisation
	void Start () {
		mat = transform.GetComponent<MeshRenderer> ().material;
		value = 0f;
		targetValue = 0f;
	}

	// Update is called once per frame
	void Update () {
		value = Mathf.Lerp (value, targetValue, .1f);
		mat.SetFloat ("_Cutoff", 1 - (value / 2));
	}
}
