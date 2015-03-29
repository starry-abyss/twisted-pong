using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModScript : MonoBehaviour {

	public enum Mods { Invisible = 0, Confuse, Turbo, Control, Pick,
				MOD_COUNT };
	
	readonly string[] modLogosNames = { "invisible", "confuse", "turbo", "control", "pick" };
	Sprite[] modLogos;
	
	List<GameObject> modItems;
	
	GameObject game;
				
	public AudioClip Sound_Mod;
	public AudioClip Sound_Mod_Remove;
	public AudioClip Sound_Pick;
	
	GameObject pickMod;
	PickScript[] pickItems;
	
	float lastItemSpawnTime;
				
	struct ModInfo
	{
		public float startTime;
		public bool activated;
	}

	ModInfo[] mods;
	
	public void clearModItems()
	{
		for (int i = 0; i < modItems.Count; ++i)
		{
			Destroy(modItems[i]);
		}
		modItems.Clear();
	}
	
	public void resetMods()
	{
		lastItemSpawnTime = Time.time;
		for (int i = 0; i < (int)Mods.MOD_COUNT; ++i)
		{
			ModInfo mod;
			mod.activated = false;
			mod.startTime = 0.0f;
			
			mods[i] = mod;
		}
	}
	
	public float modStartTime(Mods mod)
	{
		return mods[(int)mod].startTime;
	}
	
	public bool modActivated(Mods mod)
	{
		return mods[(int)mod].activated;
	}
	
	public void ToggleModActivated(Mods mod)
	{
		if (modActivated(mod))
			SetModActivated(mod, false);
		else
			SetModActivated(mod, true);
	}
	
	public void SetModActivated(Mods mod, bool state)
	{
		if (state)
		{
			mods[(int)mod].startTime = Time.time;
			mods[(int)mod].activated = true;
			GetComponent<ScoreScript>().showMessage(mod.ToString() + " MOD added");
			GetComponent<AudioSource>().PlayOneShot(Sound_Mod);
			
			if (mod == Mods.Pick)
			{
				pickMod.SetActive(true);
				PickScript.resetPoints();
				//PickScript[] pickItems = pickMod.GetComponentsInChildren<PickScript>();
				for (int i = 0; i < pickItems.Length; ++i)
				{
					pickItems[i].gameObject.SetActive(true);
				}
			}
		}
		else
		{
			mods[(int)mod].activated = false;
			GetComponent<ScoreScript>().showMessage(mod.ToString() + " MOD removed");
			GetComponent<AudioSource>().PlayOneShot(Sound_Mod_Remove);
			
			if (mod == Mods.Pick)
				pickMod.SetActive(false);
		}
	}
	
	/*public bool modActivated(Mods mod)
	{
		return (mods & (1L << mod)) == (1L << mod);
	}
	
	public void SetModActivated(Mods mod, bool state)
	{
		if (state)
		{
			mods |= (1L << mod);
			
		}
		else
			mods &= ~(1L << mod);
	}*/

	// Use this for initialization
	void Start () {
		mods = new ModInfo[(int)Mods.MOD_COUNT];
		modItems = new List<GameObject>();
		modLogos = new Sprite[(int)Mods.MOD_COUNT];
		
		for (int i = 0; i < (int)Mods.MOD_COUNT; ++i)
		{
			modLogos[i] = Resources.Load<Sprite>(modLogosNames[i]);
		}
		
		//game = GameObject.Find("Game");
		resetMods();
		pickMod = GameObject.Find("PickMod");
		pickItems = pickMod.GetComponentsInChildren<PickScript>();
		pickMod.SetActive(false);
	}
	
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.F2))
		{
			ToggleModActivated(Mods.Invisible);
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ToggleModActivated(Mods.MOD_COUNT - 1);
		}
		
		if (!GetComponent<ScoreScript>().gameStarted)
			return;
		
		List<float> itemY = new List<float>();
		itemY.Add(-2.8f);
		itemY.Add(2.8f);
		itemY.Add(-1.4f);
		itemY.Add(1.4f);
		
		for (int i = 0; i < modItems.Count;)
		{
			if (modItems[i].GetComponent<ItemScript>().taken)
			{
				Destroy(modItems[i]);
				modItems.RemoveAt(i);
			}
			else
			{
				for (int j = 0; j < itemY.Count; ++j)
				{
					if (Mathf.Abs(modItems[i].GetComponent<Transform>().position.y - itemY[j]) < 0.1f)
					{
						itemY.RemoveAt(j);
						break;
					}
				}
				
				++i;
			}
		}
		
		if ((modItems.Count < 4) && (Time.time - lastItemSpawnTime > 5.0f))
		{
			lastItemSpawnTime = Time.time;
			
			Random.seed = (int)System.DateTime.Now.Ticks;
			float y = itemY[Random.Range(0, itemY.Count)];
			Vector3 pos = new Vector3(0, y, 1.0f);
			GameObject item = Instantiate(Resources.Load("Mod")) as GameObject;
			item.GetComponent<Transform>().position = pos;
			int mod = Random.Range(0, (int) Mods.MOD_COUNT);
			item.GetComponent<ItemScript>().mod = (Mods) mod;
			item.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Mod" + mod);
			item.GetComponentInChildren<SpriteRenderer>().sprite = modLogos[mod];
			modItems.Add(item);
		}
	}
}
