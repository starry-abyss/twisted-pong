using UnityEngine;
using System.Collections;

public class OutZoneScript : MonoBehaviour {

	public bool left;
	GameObject game;

	void OnTriggerEnter2D(Collider2D other)
	{
		//print(other.gameObject.name);
		if (other.gameObject.name == "Ball")
		{
			game.GetComponent<ScoreScript>().incrementScore(!left);
			game.GetComponent<ModScript>().resetMods();
			if (game.GetComponent<ScoreScript>().gameStarted)
				other.gameObject.GetComponent<BallMovementScript>().NewRound(left);
		}
	}

	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
