using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour {

	public void ExitGame ()
    {
        Application.Quit();
    }

    public void StartGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void ToSettings ()
    {
        SceneManager.LoadScene(2);
    }

    public void ToMenu ()
    {
        SceneManager.LoadScene(0);
    }
}
