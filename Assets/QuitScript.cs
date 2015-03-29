using UnityEngine;
using System.Collections;

public class QuitScript : MonoBehaviour {

	public void OnClick()
	{
		Application.Quit();
	}

	// Use this for initialization
	void Start () {
		if (Application.isEditor || Application.isWebPlayer)
			gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
