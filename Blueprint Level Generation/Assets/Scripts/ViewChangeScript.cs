using UnityEngine;
using System.Collections;

public class ViewChangeScript : MonoBehaviour {

    bool FPS;
    GameObject player;
	// Use this for initialization
	void Start () {
        FPS = false;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeView()
    {
        if(FPS)
        {
            player.GetComponent<Move>().gravity = 0;
            player.transform.position = new Vector3(401f, 375f, -595f);
            player.transform.LookAt(FindObjectOfType<Terrain>().gameObject.transform);
            FPS = false;
        }
        else
        {
           
            player.transform.position = new Vector3(100f, 100f, 100f);
            player.GetComponent<Move>().gravity = 1;
            FPS = true;
        }
    }
}
