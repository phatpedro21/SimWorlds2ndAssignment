using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;



[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshBuildFromImage : MonoBehaviour {

    byte[] referenceByteArr;
    Texture2D referenceTex;
    int[,] edgeArr;
	Terrain currentTerrain;
	//Mesh newMesh;
	Color pixCol;
	public GameObject RedObject, GreenObject, BlueObject;
	float baseHeight = 2f;
	float wallHeight = 20f;

	List<GameObject> spawnedObjects = new List<GameObject> ();

	public ColourPrefabSetter currentColourPrefabSetter;

	MeshFilter meshFilter;
	Mesh mesh;

    //For bitwise operator refenrece
    //1000 : 8 : left
    //0100 : 4 : right
    //0010 : 2 : up
    //0001 : 1 : down

    // Use this for initialization
    void Start() 
	{

		currentColourPrefabSetter = FindObjectOfType<ColourPrefabSetter>();

		meshFilter = GetComponent<MeshFilter>();
		mesh = meshFilter.sharedMesh;
		//If there is not a mesh filter on current object, return error
		
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}
		
		//If there is not a mesh o nthe current object, assigns a mesh
		
		if (mesh == null)
		{
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
		}	
		//TRAIGNLE LIST ORDER REFERENCE
		/*new int[]{
			0,2,1,
			0,3,2,
			4,6,5,
			4,7,6,
			8,10,9,
			8,11,10,
			12,14,13,
			12,15,14
		};*/

		/*Mesh mesh = new Mesh();
		mesh.name = "testMesh";
		mesh.Clear();
		Vector3[] vertices = new Vector3[4];
		
		// No need to assign x, y and z separately!
		
		vertices[0] = new Vector3(-10.0f, 0.0f, 0.0f);
		vertices[1] = new Vector3(10.0f, 0.0f, 0.0f);
		vertices[2] = new Vector3(-10.0f, 50.0f, 0.0f);
		vertices[3] = new Vector3( 10.0f, 50.0f, 0.0f);
		
		mesh.vertices = vertices;*/

    }
	
	// Update is called once per frame
	void Update () {
	
	}


	public void BuildLevelMesh (string LevelBluePrint)
	{
		DeleteMesh ();
		if (Terrain.activeTerrain != null)
		{
			currentTerrain = Terrain.activeTerrain;
		}
		referenceByteArr = File.ReadAllBytes(LevelBluePrint);
		//referenceByteArr = File.ReadAllBytes ("Assets\\Resources\\MeshBuildTest.png");
		//textureHolder.GetComponent<Texture2D>();
		referenceTex = new Texture2D(1, 1);
		referenceTex.LoadImage(referenceByteArr);
		Debug.Log(referenceTex.height + " : " + referenceTex.width);
		edgeArr = new int[512, 512];
		
		for (int i = 0; i < referenceTex.height; i++)
		{
			for (int j = 0; j < referenceTex.width; j++)
			{
				edgeArr[i,j] = 0;
			}
		}
		
		//check across first, if white before then have vertex on left side, if white after vertex on right side.
		//then check up/down, if white above then have vertex on top side, if white below vertex on bottom side.
		
		//For all pixels in image
		for (int i = 0; i < referenceTex.height; i++)
		{
			for(int j = 0; j < referenceTex.width; j++)
			{
				
				float alphaHeight =  wallHeight - referenceTex.GetPixel(i, j).a * wallHeight;
				
				if(currentTerrain != null)
				{
					baseHeight = currentTerrain.SampleHeight(new Vector3 (i,0,j));
				}
				//If current pixel is black, check if it has any edges
				if (referenceTex.GetPixel(i, j) == Color.black)
				{
					//If pixel to left is white, current pixel has edge on left
					if (j != 0)
					{
						
						//Checks if edge on left
						if(referenceTex.GetPixel(i, j - 1) != Color.black)
						{
							edgeArr[i,j] |= 4 ;
						}
					}
					//If pixel to right is white, current pixel has edge on right
					if(j != referenceTex.width)
					{
						//Checks if edge on right
						if (referenceTex.GetPixel(i, j + 1) != Color.black)
						{
							edgeArr[i,j] |= 8 ;
						}
					}
					//If pixel above is white, current pixel has edge on top
					if (i != 0)
					{
						
						//Checks if edge on top
						if (referenceTex.GetPixel(i - 1, j) != Color.black)
						{
							edgeArr[i,j] |= 1 ;
						}
					}
					//If pixel below is white, current pixel has edge on bottom
					if (i != referenceTex.height)
					{
						//Checks if edge on bottom
						if (referenceTex.GetPixel(i + 1, j) != Color.black)
						{
							edgeArr[i,j] |= 2 ;
						}
					}
					
				}
				else if (referenceTex.GetPixel(i,j) != Color.white)
				{
					pixCol = referenceTex.GetPixel(i,j);
					for(int x = 0;  x < currentColourPrefabSetter.colourList.Count; x ++)
					{
						//if(referenceTex.GetPixel(i,j) == Color.red && RedObject != null)
						if(pixCol.r == currentColourPrefabSetter.colourList[x].r && pixCol.g == currentColourPrefabSetter.colourList[x].g
						   && pixCol.b == currentColourPrefabSetter.colourList[x].b && currentColourPrefabSetter.prefabs[x] != null)
						{				
								spawnedObjects.Add((GameObject)Instantiate(currentColourPrefabSetter.prefabs[x],
								                                           new Vector3(i,baseHeight + alphaHeight ,j),
							                                               Quaternion.identity));
						}
					}
				}
				

			}
		}
		
		//Creates new lists for building mesh
		List<Vector3> vertexArr = new List<Vector3>();
		List<int> triangleList = new List<int> ();
		List<Color> colorList = new List<Color> ();
		int faceNum = 0;
		
		
		
		for(int i = 0; i < 512; i++)
		{

			for (int j = 0; j < 512; j++)
			{	

				if(currentTerrain != null)
				{
					baseHeight = currentTerrain.SampleHeight(new Vector3 (i,0,j));
				}
				//For placing walls at height of terrain
				
				
				if(edgeArr[i,j] != 0)
				{

					//If current pixel had edge on left, create a left facing quad to left of corresponding point
					if((edgeArr[i,j] & 8) == 8)
					{
						vertexArr.Add(new Vector3(i+ 0.5f, baseHeight, j+0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i - 0.5f, baseHeight,j+ 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i - 0.5f, baseHeight + wallHeight,j+ 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i+0.5f, baseHeight + wallHeight,j+ 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 2);
						triangleList.Add (faceNum + 1);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 3); 
						triangleList.Add(faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 1);
						triangleList.Add (faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 2); 
						triangleList.Add(faceNum + 3);
						
						faceNum += 4;
					}
					//If current pixel had edge on right, create a left facing quad to right of corresponding point
					if((edgeArr[i,j] & 4) == 4)
					{
						
						vertexArr.Add(new Vector3( i - 0.5f, baseHeight, j  - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3( i + 0.5f, baseHeight, j  - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3( i + 0.5f, baseHeight + wallHeight,  j - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3( i - 0.5f, baseHeight + wallHeight,  j - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 2);
						triangleList.Add (faceNum + 1);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 3); 
						triangleList.Add(faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 1);
						triangleList.Add (faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 2); 
						triangleList.Add(faceNum + 3);
						
						faceNum += 4;
						
					}
					//If current pixel had edge on top, create an "upwards" facing quad above of corresponding point
					if((edgeArr[i,j] & 2) == 2)
					{
						vertexArr.Add(new Vector3(i+ 0.5f,  baseHeight, j  - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i+ 0.5f,  baseHeight, j+  0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i+ 0.5f, baseHeight + wallHeight,  j+ 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i+ 0.5f, baseHeight + wallHeight,  j - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 2);
						triangleList.Add (faceNum + 1);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 3); 
						triangleList.Add(faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 1);
						triangleList.Add (faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 2); 
						triangleList.Add(faceNum + 3);
						
						faceNum += 4;
					}
					//If current pixel had edge below, create a 'downwards' facing quad below corresponding point
					if((edgeArr[i,j] & 1) == 1)
					{
						vertexArr.Add(new Vector3(i  - 0.5f,  baseHeight, j+  0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i  - 0.5f,  baseHeight, j  - 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i  - 0.5f, baseHeight + wallHeight,  j -0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						vertexArr.Add(new Vector3(i  - 0.5f, baseHeight + wallHeight,  j+ 0.5f));
						colorList.Add(referenceTex.GetPixel(i,j));
						
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 2);
						triangleList.Add (faceNum + 1);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 3); 
						triangleList.Add(faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add (faceNum + 1);
						triangleList.Add (faceNum + 2);
						triangleList.Add(faceNum);
						triangleList.Add(faceNum + 2); 
						triangleList.Add(faceNum + 3);
						faceNum += 4;
					}
				}
			}
			
		}		

		
		//Sets mesh
		mesh.Clear();
		mesh.vertices = vertexArr.ToArray ();
		mesh.triangles = triangleList.ToArray ();
		
		//Sends mesh to mesh collider
		MeshCollider _collider = GetComponent<MeshCollider> ();
		_collider.sharedMesh = mesh;
		mesh.colors = colorList.ToArray ();
	}

	public void DeleteMesh()
	{
		mesh.Clear ();
		for (int i = 0; i < spawnedObjects.Count; i++) 
		{
			Destroy(spawnedObjects[i]);
		}
		spawnedObjects.Clear ();
	}

	public void SetWallHeight(int newWallHeight)
	{
		wallHeight = newWallHeight;
	}
}
