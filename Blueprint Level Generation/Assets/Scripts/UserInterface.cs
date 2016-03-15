using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

public class UserInterface : MonoBehaviour {

	MeshBuildFromImage currentMeshEditor;
	TerrainEditor currentTerrainEditor;
	ColourPrefabSetter currentColourPrefabSetter;
	public Canvas settings, editor, colourAssign; 
	public InputField terrainHeightIF, wallHeightIF, prefabAdd1, prefabAdd2, prefabAdd3,prefabAdd4, prefabAdd5, prefabAdd6;

	// Use this for initialization
	void Start () {

		settings.enabled = false;
		editor.enabled = true;
		colourAssign.enabled = false;

		currentMeshEditor = FindObjectOfType<MeshBuildFromImage> ();
		currentTerrainEditor = FindObjectOfType<TerrainEditor> ();
		currentColourPrefabSetter = FindObjectOfType<ColourPrefabSetter> ();


	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (terrainHeightIF != null) 
		{
			InputField.SubmitEvent _tHeightChangeEvent = new InputField.SubmitEvent();
			_tHeightChangeEvent.AddListener(tHeightChanged);
			terrainHeightIF.onEndEdit = _tHeightChangeEvent;
		}
		if (wallHeightIF != null) 
		{			
			InputField.SubmitEvent  _wHeightChangeEvent = new InputField.SubmitEvent();
			_wHeightChangeEvent.AddListener(wHeightChanged);
			wallHeightIF.onEndEdit = _wHeightChangeEvent;
		}


		InputField.SubmitEvent _newPrefab1 = new InputField.SubmitEvent();
		_newPrefab1.AddListener(_prefab1Added);
		prefabAdd1.onEndEdit = _newPrefab1;

		InputField.SubmitEvent _newPrefab2 = new InputField.SubmitEvent();
		_newPrefab2.AddListener(_prefab2Added);
		prefabAdd2.onEndEdit = _newPrefab2;

		InputField.SubmitEvent _newPrefab3 = new InputField.SubmitEvent();
		_newPrefab3.AddListener(_prefab3Added);
		prefabAdd3.onEndEdit = _newPrefab3;

		InputField.SubmitEvent _newPrefab4 = new InputField.SubmitEvent();
		_newPrefab4.AddListener(_prefab4Added);
		prefabAdd4.onEndEdit = _newPrefab4;

		InputField.SubmitEvent _newPrefab5 = new InputField.SubmitEvent();
		_newPrefab5.AddListener(_prefab5Added);
		prefabAdd5.onEndEdit = _newPrefab5;

		InputField.SubmitEvent _newPrefab6 = new InputField.SubmitEvent();
		_newPrefab6.AddListener(_prefab6Added);
		prefabAdd6.onEndEdit = _newPrefab6;

		
		
	}
	//User selects file and terrain is build
	public void LoadTerrainBlueprint()
	{
		string terrainBlueprintFilepath = "null";
		MeshBuildFromImage currentMeshEditor = FindObjectOfType<MeshBuildFromImage> ();
		//Clears current built level to allow new one to be built on new terrain
		currentMeshEditor.DeleteMesh ();
		while (!terrainBlueprintFilepath.EndsWith(".png")) 
		{
			terrainBlueprintFilepath = UnityEditor.EditorUtility.OpenFilePanel ("Pick Terrain Blueprint", "", "");
		}	

		Debug.Log ("thanks for selecting a png");


		currentTerrainEditor.buildImage1 (terrainBlueprintFilepath);

		//levelBlueprintFilepath = UnityEditor.EditorUtility.OpenFilePanel("Pick Level Blueprint" ,"","");
		//MeshBuildFromImage meshBuilder = FindObjectOfType<MeshBuildFromImage>();
		//meshBuilder.BuildLevelMesh(levelBlueprintFilepath);	
	}

	//
	public void LoadLevelBlueprint()
	{
		string levelBlueprintFilepath = "null";
		while (!levelBlueprintFilepath.EndsWith(".png")) 
		{
			levelBlueprintFilepath = UnityEditor.EditorUtility.OpenFilePanel ("Pick Level Blueprint", "", "");
		}	

		Debug.Log ("thanks for selecting a png");

		currentMeshEditor.BuildLevelMesh(levelBlueprintFilepath);
	}

	//MENU NAVIGATION

	public void EditorToSettings()
	{
		editor.enabled = false;
		settings.enabled = true;		
	}
	public void SettingsToEditor()
	{
		editor.enabled = true;
		settings.enabled = false;		
	}
	public void SettingsToColourAssign()
	{
		colourAssign.enabled = true;
		settings.enabled = false;		
	}
	public void ColourAssignToSettings()
	{
		colourAssign.enabled = false;
		settings.enabled = true;		
	}

	//HEIGHT SETTINGS

	private void tHeightChanged(string arg0)
	{
		int newTerrainHeight;
		int.TryParse(arg0, out newTerrainHeight);
		currentTerrainEditor.SetTerrainHeight(newTerrainHeight);
	}

	private void wHeightChanged(string arg0)
	{
		int newWallHeight;
		int.TryParse(arg0, out newWallHeight);
		currentMeshEditor.SetWallHeight(newWallHeight);
	}

	//PREFAB COLOUR ASSIGNMENT

	private void _prefab1Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (0, prefab);

	}
	private void _prefab2Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (1, prefab);		
	}
	private void _prefab3Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (2, prefab);	
	}
	private void _prefab4Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (3, prefab);
	}
	private void _prefab5Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (4, prefab);
		
	}
	private void _prefab6Added(string arg0)
	{
		GameObject prefab = (GameObject)Resources.Load (arg0);
		currentColourPrefabSetter.updateColourPair (5, prefab);
	}



}
