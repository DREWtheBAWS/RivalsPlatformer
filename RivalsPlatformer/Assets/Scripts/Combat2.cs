using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Combat2 : NetworkBehaviour {

    public CircleCollider2D AttackBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack(AttackBox);
        }
	}
    private void Attack(CircleCollider2D col)
    {
       
        var cols = Physics2D.OverlapCircle(col.bounds.center, col.radius, LayerMask.GetMask("Hitbox"));
        Debug.Log(cols.GetInstanceID()+" : " + AttackBox.GetInstanceID());
    }
}
