using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript : MonoBehaviour {

    private Collider2D tileCollider;
    GameObject[] swords;

    // Use this for initialization
    void Start () {
        // Doesn't work
        tileCollider = GetComponent<Collider2D>();

        swords = GameObject.FindGameObjectsWithTag("Sword");

        for (int i = 0; i < swords.Length; i++) {
            Physics2D.IgnoreCollision(swords[i].GetComponent<Collider2D>(),
                                         tileCollider);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
