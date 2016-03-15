using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TerrainEditor : MonoBehaviour {

	Terrain _terrain;
	float[,] heights;
	float[,] resetHeights;
	public GameObject textureHolder;
	byte[] referenceByteArr;
	Texture2D referenceTex;
	public string terrainImageName;
	string string_terrainHeight;
	int int_terrainHeight;

	bool reset;
	//float tFloat;

	// Use this for initialization
	void Start () 
	{
		_terrain = Terrain.activeTerrain;
		resetHeights = _terrain.terrainData.GetHeights(0, 0, _terrain.terrainData.heightmapWidth, _terrain.terrainData.heightmapHeight);
		for (int i = 0; i < _terrain.terrainData.heightmapWidth; i++) 
		{
			for (int j = 0; j < _terrain.terrainData.heightmapHeight; j++) 
			{
				resetHeights[i,j] = 0;
			}
		}
        heights = resetHeights;
        reset = false;

		resetTerrain ();
	}	
	// Update is called once per frame
	void Update () 
	{      
		      
	}

	public void resetTerrain ()
	{
        heights = resetHeights;
        _terrain.terrainData.SetHeights(0, 0, heights);
    }

	public void buildImage1(string bluePrintFilePath)
    {

		referenceByteArr = File.ReadAllBytes(bluePrintFilePath);
        
		Debug.Log(referenceByteArr.Length);
		
		//textureHolder.GetComponent<Texture2D>();
		referenceTex = new Texture2D(1, 1);
		referenceTex.LoadImage(referenceByteArr);
		_terrain.terrainData.size = new Vector3(referenceTex.width, 40f, referenceTex.height);
		
		heights = _terrain.terrainData.GetHeights(0, 0, _terrain.terrainData.heightmapWidth, _terrain.terrainData.heightmapHeight);
		resetHeights = _terrain.terrainData.GetHeights(0, 0, _terrain.terrainData.heightmapWidth, _terrain.terrainData.heightmapHeight);
		Debug.Log(_terrain.terrainData.heightmapWidth);
		Debug.Log(_terrain.terrainData.heightmapHeight);
		
		for (int i = 0; i < _terrain.terrainData.heightmapWidth; i++)
		{
			for (int j = 0; j < _terrain.terrainData.heightmapHeight; j++)
			{                             
				heights[j, i] = (1 -referenceTex.GetPixel(i,j).r) ;
				resetHeights[i, j] = 0.5f;             
			}
		}		
		
		_terrain.terrainData.SetHeights(0, 0, heights);    


		string levelBlueprintFilepath = "null";


    }

	public void SetTerrainHeight(int height)
	{
		_terrain.terrainData.size = new Vector3 (_terrain.terrainData.heightmapWidth, height,
		                                         _terrain.terrainData.heightmapHeight);
	}
  
}
