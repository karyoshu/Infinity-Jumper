using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
    public ScreenManager screenManager;
    public Animator mainMenu;
	// Use this for initialization
	void Start () {
        Invoke("ChangeToMenu", 4);
	}
	
    void ChangeToMenu()
    {
        screenManager.OpenPanel(mainMenu);
    }
}
