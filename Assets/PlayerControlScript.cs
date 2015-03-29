using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour {

	public enum ControllerType { Keyboard, Gamepad1, Gamepad2, Computer };
	
	public static ControllerType leftController = ControllerType.Keyboard;
	public static ControllerType rightController = ControllerType.Keyboard;

	public float velocity;
	Vector2 velocityDirection;
	
	GameObject game;
	ModScript modScript;
	
	public bool left;
	
	public float getPlayerInput()
	{
		return m_input;
	}
	
	float m_input;
	
	//public float getVelocityDirectionY() { return velocityDirection.y; }
	//Rigidbody2D body;

	// Use this for initialization
	void Start () {
		//body = GetComponent<Rigidbody2D>();
		game = GameObject.Find("Game");
		modScript = GameObject.Find("Game").GetComponent<ModScript>();
	}
	
	void LateUpdate()
	{
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!game.GetComponent<ScoreScript>().gameStarted) return;
		
		//float input = Input.GetAxis("Vertical");
		
		float input = 0.0f;
		
		ControllerType thisController = left ? leftController : rightController;
		if (thisController == ControllerType.Keyboard)
		{
			ControllerType otherController = !left ? leftController : rightController;
			if (otherController == ControllerType.Keyboard)
			{
				input = Input.GetAxis(left ? "Keyboard_Left" : "Keyboard_Right");
			}
			else
			{
				input = Input.GetAxis("Keyboard_Both");
			}
		}
		else if (thisController == ControllerType.Gamepad1)
		{
			input = Input.GetAxis("Gamepad_1");
		}
		else if (thisController == ControllerType.Gamepad2)
		{
			input = Input.GetAxis("Gamepad_2");
		}
		
		/*if (Mathf.Abs(input) <= 0.2f)
			velocityDirection = new Vector2(0,0);
		else if (input > 0.0f)
			velocityDirection = new Vector2(0,1);
		else
			velocityDirection = new Vector2(0,-1);*/
			
		if (modScript.modActivated(ModScript.Mods.Confuse))
		{
			input = -input;
		}
		m_input = input;
		
		if (modScript.modActivated(ModScript.Mods.Control))
		{
			if (left == (GameObject.Find("Ball").GetComponent<BallMovementScript>().velocityDirection.x > 0.0f))
				return;
		}
		
		velocityDirection = new Vector2(0,input);
		
		Vector3 angles = new Vector3(10,0,input * (left ? 10.0f : -10.0f));
		
		Quaternion rotation = transform.rotation;
		rotation.eulerAngles = angles;
		transform.rotation = rotation;
		
		Vector2 pos = transform.position;
		pos += velocityDirection * velocity;
		//const float limit = 3.0f;
		const float limit = 3.3f;
		if (pos.y > limit) pos.y = limit;
		if (pos.y < -limit) pos.y = -limit;
		transform.position = pos;
		
	}
}
