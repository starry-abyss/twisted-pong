using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseControllerScript : MonoBehaviour {

	public bool left = false;
	
	public void OnValueChanged()
	{
		int value = Mathf.FloorToInt(GetComponent<Slider>().value);
		
		PlayerControlScript.ControllerType type = (PlayerControlScript.ControllerType) value;
		if (left)
			PlayerControlScript.leftController = type;
		else
			PlayerControlScript.rightController = type;
		
		Text[] comp = GetComponentsInChildren<Text>();
		for (int i = 0; i < comp.Length; ++i)
		{
			if (comp[i].gameObject.name == "Value")
			{
				comp[i].text = type.ToString();
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
