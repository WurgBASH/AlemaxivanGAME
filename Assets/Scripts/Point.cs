using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player character = collider.GetComponent<Player>();

        if (character)
        {
            character.Points+=25;
            Destroy(gameObject,0.1F);
        }
    }
}

