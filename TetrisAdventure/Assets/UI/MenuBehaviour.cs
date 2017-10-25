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

    public void StartAdventure ()
    {
        GameManager.type = GameManager.GameType.Adventure;
        SceneManager.LoadScene(2);
    }

    public void StartClassic ()
    {
        GameManager.type = GameManager.GameType.Classic;
        SceneManager.LoadScene(3);
    }

    public void ChangeLevel (int value)
    {
        GameManager.levelAtStart += value;
        GameManager.levelAtStart = Mathf.Clamp(GameManager.levelAtStart, 0, 10);
    }

    public void ChangeSpeed(int value)
    {
        GameManager.gameSpeedAtStart += value;
        GameManager.gameSpeedAtStart = Mathf.Clamp(GameManager.gameSpeedAtStart, 0, 10);
    }

    public void ToMenu ()
    {
        SceneManager.LoadScene(0);
    }
}
