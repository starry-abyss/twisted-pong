using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointsToWinScript : MonoBehaviour {

	public void OnValueChanged()
	{
		int value = Mathf.FloorToInt(GetComponent<Slider>().value);
		GameObject.Find("Game").GetComponent<ScoreScript>().pointsToWin = value;
	
		Text[] comp = GetComponentsInChildren<Text>();
		for (int i = 0; i < comp.Length; ++i)
		{
			if (comp[i].gameObject.name == "Value")
			{
				comp[i].text = value.ToString();
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		OnValueChanged();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
