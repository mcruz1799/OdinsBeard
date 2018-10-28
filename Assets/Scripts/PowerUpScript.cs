using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
/*Make sure that both the player and the powerup have colliders on. Also, make sure that either collider has "Is Trigger" enabled.
(Preferably the powerup). Make sure that the powerup has the correct tag.
*/
void OnTriggerEnter2D(Collider2D collision){
	if(collision.tag == "Player"){
		//Do Something
		Destroy(this.gameObject);
		}
    
	}
}
