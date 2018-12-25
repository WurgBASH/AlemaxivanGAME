using System.Collections;
using UnityEngine;

public class winLevel : MonoBehaviour
{
    private Player character;
    private void Awake()
    {
        character = FindObjectOfType<Player>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           Debug.Log("sa");
           character.Win();
        }
    }
}
