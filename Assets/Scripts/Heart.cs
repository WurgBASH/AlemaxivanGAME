using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player character = collider.GetComponent<Player>();
        
        if (character)
        {
            character.Lives++;
            Destroy(gameObject);
        }
    }
}
