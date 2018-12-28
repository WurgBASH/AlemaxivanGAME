using UnityEngine;
using System.Collections;

public class StarBar : MonoBehaviour
{
    private Transform[] stars = new Transform[3];

    private Player character;


    private void Awake()
    {
        character = FindObjectOfType<Player>();

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i] = transform.GetChild(i);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < character.Stars) stars[i].gameObject.SetActive(true);
            else stars[i].gameObject.SetActive(false);
        }
    }
}
