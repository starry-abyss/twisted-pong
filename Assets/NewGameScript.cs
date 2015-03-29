using UnityEngine;
using System.Collections;

public class NewGameScript : MonoBehaviour {

	GameObject ball;
	GameObject game;
	MenuScript menu;

	public void OnClick()
	{
		Random.seed = (int)System.DateTime.Now.Ticks;
		ball.GetComponent<BallMovementScript>().NewRound(Random.Range(0, 2) == 0);
		game.GetComponent<ScoreScript>().resetScores();
		game.GetComponent<ModScript>().clearModItems();
		game.GetComponent<ModScript>().resetMods();
		
		Vector2 pos;
		pos = GameObject.Find("Player_Left").transform.position;
		pos.y = 0;
		GameObject.Find("Player_Left").transform.position = pos;
		
		pos = GameObject.Find("Player_Right").transform.position;
		pos.y = 0;
		GameObject.Find("Player_Right").transform.position = pos;
		
		game.GetComponent<ScoreScript>().gameStarted = true;
		menu.Hide();
	}

	// Use this for initialization
	void Start () {
		ball = GameObject.Find("Ball");
		game = GameObject.Find("Game");
		menu = GameObject.Find("Menu").GetComponent<MenuScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
