using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public AudioClip Sound_EndGame;

	int leftScore;
	int rightScore;
	
	float messageTime;
	string message;
	
	MenuScript menu;
	
	public int pointsToWin = 3;
	
	public bool gameStarted;
	
	public void showMessage(string msg)
	{
		messageTime = Time.time;
		message = msg;
		updateUI();
	}
	
	public void incrementScore(bool left)
	{
		if (left)
			++leftScore;
		else
			++rightScore;
			
		if (leftScore >= pointsToWin)
		{
			gameStarted = false;
			GameObject.Find("LeftPlayerWon").GetComponent<Text>().enabled = true;
		}
		else if (rightScore >= pointsToWin)
		{
			gameStarted = false;
			GameObject.Find("RightPlayerWon").GetComponent<Text>().enabled = true;
		}
		
		if (!gameStarted)
		{
			GetComponent<AudioSource>().PlayOneShot(Sound_EndGame);
			GameObject.Find("Ball").GetComponent<BallMovementScript>().resetPos();
			menu.Show();
		}
		
		updateUI();
	}

	public void resetScores()
	{
		leftScore = 0;
		rightScore = 0;
		//gameStarted = true;
		GameObject.Find("LeftPlayerWon").GetComponent<Text>().enabled = false;
		GameObject.Find("RightPlayerWon").GetComponent<Text>().enabled = false;
		//menu.Hide();
		
		messageTime = 0.0f;
		
		message = "";
		updateUI();
	}
	
	void updateUI()
	{
		GameObject.Find("LeftPlayerScore").GetComponent<Text>().text = leftScore.ToString();
		GameObject.Find("RightPlayerScore").GetComponent<Text>().text = rightScore.ToString();
		GameObject.Find("Message").GetComponent<Text>().text = message;
	}

	// Use this for initialization
	void Start () {
		menu = GameObject.Find("Menu").GetComponent<MenuScript>();
		
		resetScores();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - messageTime > 3.0f)
		{
			message = "";
			updateUI();
		}
		
		if (Input.GetButtonDown("Pause"))
		{
			//print(1);
			if (!menu.Hidden())
			{
				if (gameStarted)
				{
					menu.Hide();
				}
			}
			else
				menu.Show();
		}
	}
}
