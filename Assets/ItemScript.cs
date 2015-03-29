using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	float velocity;
	Vector3 velocityDirection;
	
	public ModScript.Mods mod;
	bool floating;
	public bool taken;
	
	//Rigidbody2D body;
	
	GameObject game;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		/*if (!game.GetComponent<ScoreScript>().gameStarted)
			return;*/
		
		//print(other.gameObject.tag);
		if (!floating && (other.gameObject.name == "Ball"))
		{
			floating = true;
			if (other.gameObject.GetComponent<BallMovementScript>().velocityDirection.x > 0.0f)
			{
				velocityDirection = new Vector3(1, 0, 0);
			}
			else
			{
				velocityDirection = new Vector3(-1, 0, 0);
			}
		}
		else if (other.gameObject.tag == "Player")
		//else if ((other.gameObject.name == "Player_Left") || (other.gameObject.name == "Player_Right"))
		{
			game.GetComponent<ModScript>().ToggleModActivated(mod);
			taken = true;
			//GetComponent<Renderer>().enabled = false;
			gameObject.SetActive(false);
		}
		/*else if (other.gameObject.tag == "Respawn")
			//else if ((other.gameObject.name == "Player_Left") || (other.gameObject.name == "Player_Right"))
		{
			taken = true;
		}*/
	}

	// Use this for initialization
	void Start () {
		velocity = 0.03f;
		//body = GetComponent<Rigidbody2D>();
		velocityDirection = new Vector3(0,0,0);
		floating = false;
		taken = false;

		game = GameObject.Find("Game");
		
		GetComponent<Renderer>().sortingLayerName = "Game";
		GetComponent<Renderer>().sortingOrder = -2;
	}
	
	void FixedUpdate () {
		if (!game.GetComponent<ScoreScript>().gameStarted)
			return;
			
		Vector3 pos = transform.position;
		pos += velocityDirection * velocity;
		transform.position = pos;
		
		if (transform.position.x < GameObject.Find("OutZone_Left").GetComponent<Transform>().position.x)
			taken = true;
		else if (transform.position.x > GameObject.Find("OutZone_Right").GetComponent<Transform>().position.x)
			taken = true;
	}
}
