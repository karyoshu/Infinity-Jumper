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
	public GUISkin skin;			//refrence to the GUI skin
	static GameState gameState = GameState.NotPlaying;		//setting game state to main menu
	static int score = 0;			//setting score to 0
	int tempScore;					//variable to store intermediate score
	MoveUp cameraMove;				//refrence to moveup script of camera
	public Rigidbody2D player;				//refrence to rigidbody of player
	float startTime;				//variable to store start time
	Pauser pauser;					//refrence to pauser script
	Camera cam;						//refrence to main camera
	public Transform blockParentPrefab;		//refrence to block parent prefab
	public Transform blockParentPlatformPrefab;		//refrence to block parent prefab
    public Transform blockParent;			//refrence to block parent transform
    public Transform blockParentPlatform;			//refrence to block parent transform
	float soundLevel;				//varable to store sound level
	string controlType;				//varaible to store control type
	public GameObject tiltControlGameObject;		//refrence to tilt control
    public GameObject buttonControlGameObject;		//refrence to button control
	public static GameMaster gm;		//reference to self object
    public Text scoreGUI;
    public Text hiScoreGUI;
    public GameObject controlReversed;
    public Text controlReversedCount;

    public bool greenOrBlueBallTaken = false;
    public int numRedBallsTaken = 0;

    ScreenManager scrnMngr;
    public Animator mainMenuCanvas;
    //public Canvas playingCanvas;
    public Slider musicSlider;
	void Awake()
	{
		cameraMove = Camera.main.GetComponent<MoveUp> ();
		pauser = gameObject.GetComponent<Pauser> ();
		cam = Camera.main;
		soundLevel = PlayerPrefs.GetFloat ("SoundLevel");		//getting sound level from player prefrence
		controlType = PlayerPrefs.GetString ("ControlType");	//getting control type from player prefrence
		Screen.sleepTimeout = SleepTimeout.NeverSleep;			//stop screen to timeout
		if(gm == null)
			gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameMaster> ();
		HighScore.getHighScore ();
        Application.targetFrameRate = 300;
        PlayGamesPlatform.Activate();
        scrnMngr = gameObject.GetComponent<ScreenManager>();
	}
	// Use this for initialization
	void Start () {
		if (string.IsNullOrEmpty(controlType)) {
			controlType = "Tilt";
		}
		tiltControlGameObject.SetActive (false);
		buttonControlGameObject.SetActive (false);
        musicSlider.value = soundLevel;

        Social.localUser.Authenticate((bool success) =>
        {
            scrnMngr.OpenPanel(mainMenuCanvas);
        });
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState)
		{
		case GameState.Playing:
			pauser.paused = false;
			tempScore = (int)((Time.time - startTime) * 10) + (int)(player.position.y * 1.5);
			if(tempScore > score)
				score = tempScore;
			if(controlType == "Button")
				buttonControlGameObject.SetActive(true);
			else
				tiltControlGameObject.SetActive(true);
			break;
		default:
			pauser.paused = true;
            buttonControlGameObject.SetActive(false);
            tiltControlGameObject.SetActive(false);
			break;
		}
		PlayerPrefs.SetFloat("SoundLevel", soundLevel);
        if (scoreGUI != null && hiScoreGUI != null)
        {
            if (score > HighScore.highScore)
                HighScore.highScore = score;
            scoreGUI.text = score.ToString();
            hiScoreGUI.text = HighScore.highScore.ToString();            
        }
        
        //unlock achievements
        unlockAchievements(score);
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
        audio.enabled = !audio.enabled;
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
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIhLabw8YREAIQAQ");
    }

    public void unlockAchievements(int highScore)
    {
        if ((highScore >= 20000 
            && PlayerPrefs.GetInt("ach01") == 1 
            && PlayerPrefs.GetInt("ach02") == 1 
            && PlayerPrefs.GetInt("ach03") == 1 
            && PlayerPrefs.GetInt("ach04") == 1 
            && PlayerPrefs.GetInt("ach05") == 1 
            && PlayerPrefs.GetInt("ach06") == 1 
            && PlayerPrefs.GetInt("ach07") == 1) 
            || PlayerPrefs.GetInt("ach08") == 1)
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
