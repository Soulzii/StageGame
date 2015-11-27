using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {


    public GameObject MainGUI;
    public GameObject LevelGUI;


	// Use this for initialization
	void Start () {
        MainGUI.SetActive(true);
        LevelGUI.SetActive(false);
	}

    public void LevelSelect()
    {
        MainGUI.SetActive(false);
        LevelGUI.SetActive(true);
    }

    public void BacktoMenu()
    {
        LevelGUI.SetActive(false);
        MainGUI.SetActive(true);
    }

    public void StartLevel(float level)
    {
        Application.LoadLevel("Level" + level.ToString());
    }


    public void ExitGame()
    {
        Application.Quit();
    }
	

}
