using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	public static int highScore;
	
	public static void setHighScore()
	{
		PlayerPrefs.SetInt ("HighScore", highScore);
        Social.ReportScore(highScore, "CgkIhLabw8YREAIQAQ", (bool success) =>
        {
            // handle success or failure
        });
        
	}

	public static void getHighScore()
	{
		highScore = PlayerPrefs.GetInt ("HighScore");
        //report highscore to leaderboard
        setHighScore();
        //unlock achievements if login later
        GameMaster.gm.unlockAchievements(highScore);
	}
}
