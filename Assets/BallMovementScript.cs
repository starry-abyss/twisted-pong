using UnityEngine;
using System.Collections;

public class BallMovementScript : MonoBehaviour {

	public float velocity;
	public Vector2 velocityDirection;
	Rigidbody2D body;
	
	float velocity0;
	
	public AudioClip Sound_Hit;
	public AudioClip Sound_Round;
	
	ModScript modScript;
	GameObject game;
	
	bool waitForStart;
	
	//bool leftBegins;
	
	public void resetPos()
	{
		//gameObject.SetActive(false);
		body.position = new Vector2(0,0);
		body.velocity = new Vector2(0,0);
		velocityDirection = new Vector2(0,0);
	}
	
	IEnumerator NewRoundCoroutine(bool leftBegins)
	{
		yield return new WaitForSeconds(1.5f);
		
		Random.seed = (int)System.DateTime.Now.Ticks;
		float startAngle = Random.Range(-Mathf.PI / 4, Mathf.PI / 4);
		
		velocityDirection = new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle));
		if (leftBegins)
			velocityDirection.x = -velocityDirection.x;
		//velocityDirection.Normalize();
		
		waitForStart = false;
	}
	
	public void NewRound(bool leftBegins)
	{
		if (waitForStart) return;
		waitForStart = true;
		velocity = velocity0;
		resetPos();
		game.GetComponent<ScoreScript>().showMessage("Ball spawned");
		game.GetComponent<AudioSource>().PlayOneShot(Sound_Round);
		
		//gameObject.SetActive(true);
		
		StartCoroutine(NewRoundCoroutine(leftBegins));
	}

	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game");
		modScript = game.GetComponent<ModScript>();
		body = GetComponent<Rigidbody2D>();
		
		//leftBegins = true;
		velocity0 = velocity;
		
		//NewRound(true);
		
		//modScript.SetModActivated(ModScript.Mods.Invisible, true);
	}
	
	// Update is called once per frame
	void Update () {
		//if (!game.GetComponent<ScoreScript>().gameStarted) return;
	
		if (modScript.modActivated(ModScript.Mods.Invisible))
		{
			/*bool visible = (Mathf.FloorToInt((Time.time
			    - modScript.modStartTime(ModScript.Mods.Invisible)) * 2) % 6) < 3;
			GetComponent<Renderer>().enabled = visible;
			GetComponentInChildren<Light>().enabled = visible;*/
			
			float t = Time.time - modScript.modStartTime(ModScript.Mods.Invisible);
			const float T = 2.0f;
			const float phi = Mathf.PI / 2.0f;
			
			float visibility = Mathf.Cos(t * 2 * Mathf.PI / T + phi) + 1.0f;
			if (visibility > 1.0f) visibility = 1.0f;
			GetComponent<Renderer>().material.SetFloat("_Cutoff", visibility);
			//GetComponentInChildren<Light>().enabled = (visibility >= 0.5f);
			GetComponentInChildren<Light>().enabled = false;
		}
		else
		{
			//GetComponent<Renderer>().enabled = true;
			GetComponentInChildren<Light>().enabled = true;
			GetComponent<Renderer>().material.SetFloat("_Cutoff", 0.0f);
		}
			
		/*if (Input.GetKeyDown(KeyCode.Return))
		{
			NewRound(true);
		}*/
	}
	
	void FixedUpdate () {
		if (!game.GetComponent<ScoreScript>().gameStarted) return;
		
		if (modScript.modActivated(ModScript.Mods.Control))
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			bool left = (velocityDirection.x > 0.0f);
			for (int i = 0; i < players.Length; ++i)
			{
				PlayerControlScript playerScript = players[i].GetComponent<PlayerControlScript>();
				
				if (left == playerScript.left)
				{
					if (Mathf.Abs(playerScript.getPlayerInput()) < 0.1f) break;
					
					float a = Mathf.Atan2(velocityDirection.y, (left ? 1.0f : -1.0f) * velocityDirection.x);
					
					//if (!left) a = Mathf.PI-a;
					
					a += playerScript.getPlayerInput() * Mathf.PI / 50.0f;
					//if (a > ) ;
					
					const float maxAngle = Mathf.PI * 3.0f / 8.0f;
					
					if (a < -maxAngle) a = -maxAngle;
					if (a > maxAngle) a = maxAngle;
					
					//if (!left) a = Mathf.PI-a;
					
					velocityDirection.x = (left ? 1.0f : -1.0f) * Mathf.Cos(a);
					velocityDirection.y = Mathf.Sin(a);
					break;
				}
			}
			
			
		}
		
		//transform.Translate(velocity);
		body.velocity = velocityDirection * velocity;
		//MovePosition(velocity);
		//body.AddForce(velocity * 10);
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!game.GetComponent<ScoreScript>().gameStarted) return;
		//Collider2D collider = collision.collider;
		
		for (int i = 0; i < collision.contacts.Length; ++i)
		{
			ContactPoint2D contact = collision.contacts[i];
			//Vector2 pos = contact.point;
			Vector2 n = contact.normal;
			
			if ((Mathf.Abs(n.x) > 0.1f) && (Mathf.Abs(n.y) > 0.1f))
			{
				velocity -= 2.0f;
			}
			else
			{
				velocity += modScript.modActivated(ModScript.Mods.Turbo) ? 4.0f : 1.0f;
			}
			if (velocity > 15) velocity = 15;
			if (velocity < velocity0) velocity = velocity0;
			
			PlayerControlScript player = collision.collider.gameObject.GetComponent<PlayerControlScript>();
			/*if (player != null)
			{
				float a = Mathf.Atan2(n.y, n.x);
				a += (player.left ? 1.0f : -1.0f) * player.getVelocityDirectionY() * Mathf.PI / 4;
				//if (a > ) ;
				n.x = Mathf.Cos(a);
				n.y = Mathf.Sin(a);
				//n.Normalize();
			}*/
			
			velocityDirection = Vector3.Reflect(velocityDirection, n);
			//if (player != null)
			if (Mathf.Abs(velocityDirection.x) < 0.1f)
			{
				velocityDirection.x *= 3;
				velocityDirection.Normalize();
				
				//velocityDirection.y += player.getVelocityDirectionY();
				//velocityDirection.Normalize();
				
				/*float a = Mathf.Atan2(velocityDirection.y, velocityDirection.x);
				a = (player.left ? 1.0f : -1.0f) * a;
				if (a < -Mathf.PI / 4) a = -Mathf.PI / 4;
				if (a > Mathf.PI / 4) a = Mathf.PI / 4;
				//if (a > ) ;
				velocityDirection.x = Mathf.Cos(a);
				velocityDirection.y = Mathf.Sin(a);*/
				
				/*if (Mathf.Abs(velocityDirection.y) > Mathf.Abs(velocityDirection.x))
				{
					velocityDirection = new Vector2(1,1);
					velocityDirection.Normalize();
				}*/
			}
			
			
		
			/*if (Mathf.Abs(n.y) > 0.1f)
				velocityDirection.y = -velocityDirection.y;
			if (Mathf.Abs(n.x) > 0.1f)
				velocityDirection.x = -velocityDirection.x;*/
				
			// if player not wall
			//velocity *= 1.1f;
			
			
			game.GetComponent<AudioSource>().PlayOneShot(Sound_Hit);
			
			//velocityDirection = -velocityDirection;
			//velocityDirection.Normalize();
		}
	}
}
