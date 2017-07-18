using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public string game;

    public void NewGame()
    {
        Application.LoadLevel(game);
    }
	// Use this for initialization
	public void QuitGame()
    {
        Application.Quit();
    }
}
