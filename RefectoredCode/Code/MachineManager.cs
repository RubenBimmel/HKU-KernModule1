using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour {

	public Transform[] taps;
	public int[] tapValues;

	//Singleton
	private static MachineManager _instance;
	public static MachineManager Instance { get { return _instance; } }
	
	//Singleton
	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		tapValues = new int[taps.Length];
	}
	
	//Gets called each new day to update the machines values
	public void Updatetaps () {
		for (int i = 0; i < taps.Length; i++) {
			taps [i].GetComponent<MachineBehaviour> ().value = tapValues [i];
		}
	}
}
