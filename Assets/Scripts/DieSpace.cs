using System.Collections;
using UnityEngine;

public class DieSpace : MonoBehaviour {


    public Player player;
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag =="Player")
		{
            player.Lose();
        }
	}
}
