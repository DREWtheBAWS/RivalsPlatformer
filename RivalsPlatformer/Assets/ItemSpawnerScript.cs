using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawnerScript : MonoBehaviour {

    public GameObject[] Items;
    public NetworkManager networkManager;
    float randX;
    Vector2 whereToSpawn;
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (networkManager.numPlayers > 0)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                randX = Random.Range(-5, 5);
                whereToSpawn = new Vector2(randX, transform.position.y);
                Instantiate(Items[0], whereToSpawn, Quaternion.identity);
            }
        }
        
	}
}
