using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
           character.Win();
        }
    }
    public void BackToLevels()
    {
        SceneManager.LoadScene("levels");
    }
}
