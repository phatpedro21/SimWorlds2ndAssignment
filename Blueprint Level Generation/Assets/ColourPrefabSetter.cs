using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColourPrefabSetter : MonoBehaviour {

	List<Button> buttonList = new List<Button>();
	public List<Color> colourList = new List<Color> ();
	public List<GameObject> prefabs = new List<GameObject>();
	public Button button1, button2, button3, button4, button5, button6;
	Color b1Colour,b2Colour,b3Colour,b4Colour,b5Colour,b6Colour;

	// Use this for initialization
	void Start () 
	{
		buttonList.Add (button1);
		buttonList.Add (button2);
		buttonList.Add (button3);
		buttonList.Add (button4);
		buttonList.Add (button5);
		buttonList.Add (button6);

		Transform _thisObject = this.transform;

		for (int i = 0; i < buttonList.Count; i++)
		{
			colourList.Add(buttonList[i].GetComponent<Image>().color);
			prefabs.Add (null);
		}


	}
	
	// Update is called once per frame
	void Update () 
	{

		for(int i = 0; i < buttonList.Count; i++)
		{
			colourList[i] = buttonList[i].GetComponent<Image>().color;
		}	
	}

	public void updateColourPair(int pairNo ,GameObject prefabFilepath)
	{
		prefabs [pairNo] = (prefabFilepath);
	}
}
