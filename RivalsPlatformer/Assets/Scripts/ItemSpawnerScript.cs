using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawnerScript : NetworkBehaviour {

    public GameObject[] Items;
    public NetworkManager networkManager;
    float randX;
    Vector2 whereToSpawn;
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;

	
	
	// Update is called once per frame
	void Update () {
        
        if (networkManager.numPlayers > 0)
        {
            CmdSpawnObject();
        }
        
	}

    [Command]
    void CmdSpawnObject()
    {
        if (Time.time > nextSpawn)
        {
            int rdm = Random.Range(0, Items.Length);
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-5, 5);
            whereToSpawn = new Vector2(randX, transform.position.y);
            GameObject ob = Instantiate(Items[rdm], whereToSpawn, Quaternion.identity);
            NetworkServer.Spawn(ob);
        }
    }
}
