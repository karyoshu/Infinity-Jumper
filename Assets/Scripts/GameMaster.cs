using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

//Enumeration to define different game states
public enum GameState
{
	NotPlaying,
	Playing,
}

public class GameMaster : MonoBehaviour {
	//setting game state to not playing
	static GameState gameState = GameState.NotPlaying;	
	static int score = 0;						//setting score to 0
	int tempScore;								//variable to store intermediate score
	public Rigidbody2D player;					//reference to rigidbody of player
	float startTime;							//variable to store start time
	Pauser pauser;								//reference to pauser script
	Camera cam;									//reference to main camera
	public Transform blockParentPrefab;			//reference to block parent prefab
	public Transform blockParentPlatformPrefab;	//reference to block parent prefab
    public Transform blockParent;				//reference to block parent transform
    public Transform blockParentPlatform;		//reference to block parent transform
	float soundLevel;							//variable to store sound level
	string controlType;							//variable to store control type
	public GameObject tiltControlGameObject;	//reference to tilt control
    public GameObject buttonControlGameObject;	//reference to button control
	public static GameMaster gm;				//reference to self object
    public Text scoreGUI;						//reference to score's Text component
    public Text hiScoreGUI;						//reference to hi-score's Text component
    public GameObject controlReversed;			//reference to controlReversed label gameObject
    public Text controlReversedCount;			//reference to controlReversed label's count Text component

    public bool greenOrBlueBallTaken = false;	//if green or blue ball has been taken or not
    public int numRedBallsTaken = 0;			//no of red balls taken since game started

    ScreenManager screenManager;				//reference to screen manager
    public Animator mainMenuCanvas;				//reference to mainMenuCanvas animator
    public Slider musicSlider;					//reference to music slider
	
	void Awake()
	{
		//to create a singleton class
		if(gm == null)
			gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameMaster> ();
		pauser = gameObject.GetComponent<Pauser> ();
		cam = Camera.main;
		soundLevel = PlayerPrefs.GetFloat ("SoundLevel");		//getting sound level from player preference
		controlType = PlayerPrefs.GetString ("ControlType");	//getting control type from player preference
		Screen.sleepTimeout = SleepTimeout.NeverSleep;			//stop screen from timing out
		HighScore.getHighScore ();								//getting highScore into HighScore 
        //PlayGamesPlatform.Activate();
        screenManager = gameObject.GetComponent<ScreenManager>();
	}
	
	void Start () {
		if (string.IsNullOrEmpty(controlType)) {
			controlType = "Tilt";		//if no control is set, set it to tilt by default
		}
		//setting Active property both type of controls to false as they will be activated in Update() method
		tiltControlGameObject.SetActive (false);
		buttonControlGameObject.SetActive (false);
		
        musicSlider.value = soundLevel;
		//Authenticating user
        Social.localUser.Authenticate((bool success) =>
        {
            screenManager.OpenPanel(mainMenuCanvas);
        });
	}
	
	void Update () {
		switch (gameState)
		{
			case GameState.Playing:
			//if Gamestate is playing
				pauser.paused = false;	//unpause the game
				tempScore = (int)((Time.time - startTime) * 10) + (int)(player.position.y * 1.5);	//update the tempScore
				if(tempScore > score)
					score = tempScore;		//assign tempScore to score
				if(controlType == "Button")
					buttonControlGameObject.SetActive(true);	//activate controls
				else
					tiltControlGameObject.SetActive(true);
				break;
			default:
				pauser.paused = true;
				buttonControlGameObject.SetActive(false);
				tiltControlGameObject.SetActive(false);
				break;
		}
		PlayerPrefs.SetFloat("SoundLevel", soundLevel);		//set sound level
        if (scoreGUI != null && hiScoreGUI != null)
        {
            if (score > HighScore.highScore)
                HighScore.highScore = score;				//set highScore
            scoreGUI.text = score.ToString();
            hiScoreGUI.text = "HISCORE: " + HighScore.highScore.ToString();            
        }
        
        //unlock achievements
        unlockAchievements(score);			//unlock achievements
    }

    public void SetControlType(int control)
    {
        if (control == 1)
            controlType = "Button";
        else
            controlType = "Tilt";
        PlayerPrefs.SetString("ControlType", controlType);
    }
	
	public void ChangeState(int newState)
	{
		gameState = (GameState)newState;
	}

	IEnumerator CountDown()
	{
		//this method is used to display control reversed label when red ball is taken by player
        controlReversed.SetActive(true);
        controlReversedCount = controlReversed.GetComponentInChildren<Text>();
        for (int i = 5; i > 0; i--)
        {
            controlReversedCount.text = i.ToString();
            yield return new WaitForSeconds(1);
        } 
        controlReversed.SetActive(false);
	}

	public float getStartTime()
	{
		return startTime;
	}

    public void setStartTime()
    {
        startTime = Time.time;
    }

	public void Reset()
	{
		//this method resets all the variables, player position, camera position, destroy and recreate background and platforms at origin
		score = 0;
        greenOrBlueBallTaken = false;
        numRedBallsTaken = 0;
		player.transform.position = new Vector2(0, 0);
		cam.transform.position = new Vector3(0, 0, -10);
		MoveUp.cameraSpeed = 1f;
		gameState = GameState.NotPlaying;
		Destroy (blockParent.gameObject);
		Destroy (blockParentPlatform.gameObject);
		tiltControlGameObject.SetActive (false);
		buttonControlGameObject.SetActive (false);
		blockParent = Instantiate (blockParentPrefab, new Vector3 (0, 0, 0), transform.rotation) as Transform;
		blockParentPlatform = Instantiate (blockParentPlatformPrefab, new Vector3 (0, 0, 0), transform.rotation) as Transform;
		pauser.timeScale = 1;
        player.GetComponent<PlayerControl>().moveSpeed = 200;
        player.velocity = new Vector2(0, 0);
		HighScore.setHighScore ();
        controlReversed.SetActive(false);
	}

    public void SetSoundLevel()
    {
        this.soundLevel = musicSlider.value;
    }

    public void ToggleSound()
    {
        GetComponent<AudioSource>().enabled = !GetComponent<AudioSource>().enabled;
    }
	
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void OpenLeaderboard()
    {
        //PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIhLabw8YREAIQAQ");
    }

    public void unlockAchievements(int highScore)
    {
		//this function unlocks achievements based on score and other scenarios
        if ((highScore >= 20000 
            && PlayerPrefs.GetInt("ach01") == 1 
            && PlayerPrefs.GetInt("ach02") == 1 
            && PlayerPrefs.GetInt("ach03") == 1 
            && PlayerPrefs.GetInt("ach04") == 1 
            && PlayerPrefs.GetInt("ach05") == 1 
            && PlayerPrefs.GetInt("ach06") == 1 
            && PlayerPrefs.GetInt("ach07") == 1) 
            || PlayerPrefs.GetInt("ach08") == 1)
		//if highScore is greater than 20000 and all other achievements are unlocked, then unlock achievement 8
        {
            PlayerPrefs.SetInt("ach08", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQCQ", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if ((highScore >= 7000 && GameMaster.gm.greenOrBlueBallTaken == false) || PlayerPrefs.GetInt("ach07") == 1)
        {
            PlayerPrefs.SetInt("ach07", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQBw", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if ((highScore >= 8000 && GameMaster.gm.numRedBallsTaken > 5) || PlayerPrefs.GetInt("ach06") == 1)
        {
            PlayerPrefs.SetInt("ach06", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQBA", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if ((highScore >= 5000 && GameMaster.gm.greenOrBlueBallTaken == false) || PlayerPrefs.GetInt("ach05") == 1)
        {
            PlayerPrefs.SetInt("ach05", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQCA", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if (highScore >= 15000 || PlayerPrefs.GetInt("ach04") == 1)
        {
            PlayerPrefs.SetInt("ach04", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQBg", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if (highScore >= 10000 || PlayerPrefs.GetInt("ach03") == 1)
        {
            PlayerPrefs.SetInt("ach03", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQBQ", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if (highScore >= 6000 || PlayerPrefs.GetInt("ach02") == 1)
        {
            PlayerPrefs.SetInt("ach02", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQAw", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        if (highScore >= 3000 || PlayerPrefs.GetInt("ach01") == 1)
        {
            PlayerPrefs.SetInt("ach01", 1);
            Social.ReportProgress("CgkIhLabw8YREAIQAg", 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
    }
}
