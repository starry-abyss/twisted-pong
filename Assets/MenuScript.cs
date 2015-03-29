using UnityEngine;
//using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour {

	//public bool gameStarted = false;
	GameObject game;
	
	public bool Hidden()
	{
		return !gameObject.active;
	}

	public void Show()
	{
		bool gameStarted = game.GetComponent<ScoreScript>().gameStarted;
		Time.timeScale = 0.0f;
		gameObject.SetActive(true);
		GameObject.Find("Button_Resume").GetComponent<Button>().interactable = gameStarted;
		//EventSystem.current.
		GameObject.Find("EventSystem").GetComponent<EventSystem>()
			.SetSelectedGameObject (GameObject.Find(gameStarted ? "Button_Resume" : "Button_NewGame"));
	}
	
	public void Hide()
	{
		Time.timeScale = 1.0f;
		gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		game = GameObject.Find("Game");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
