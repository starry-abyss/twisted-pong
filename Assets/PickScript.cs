using UnityEngine;
using System.Collections;

public class PickScript : MonoBehaviour {

	static int pointsLeft;
	static int pointsRight;
	
	public bool left = false;
	GameObject game;
	GameObject pickMod;
	
	static public void resetPoints()
	{
		pointsLeft = 0;
		pointsRight = 0;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Ball")
		{
			if (left)
				++pointsLeft;
			else
				++pointsRight;
				
			game.GetComponent<ModScript>().GetComponent<AudioSource>().PlayOneShot(game.GetComponent<ModScript>().Sound_Pick);
			
			if (pointsLeft >= 3)
			{
				game.GetComponent<ScoreScript>().incrementScore(true);
				game.GetComponent<ModScript>().resetMods();
				if (game.GetComponent<ScoreScript>().gameStarted)
					other.gameObject.GetComponent<BallMovementScript>().NewRound(false);
				game.GetComponent<ModScript>().SetModActivated(ModScript.Mods.Pick, false);
			}
			else if (pointsRight >= 3)
			{
				game.GetComponent<ScoreScript>().incrementScore(false);
				game.GetComponent<ModScript>().resetMods();
				if (game.GetComponent<ScoreScript>().gameStarted)
					other.gameObject.GetComponent<BallMovementScript>().NewRound(true);
				game.GetComponent<ModScript>().SetModActivated(ModScript.Mods.Pick, false);
			}
			
			gameObject.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game");
		pickMod = GameObject.Find("PickMod");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
