using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static int countUnlockedLevel = 1;

    [SerializeField]
    Sprite unlockedIcon;

    [SerializeField]
    Sprite lockedIcon;

    [SerializeField]
    Sprite lockIcon;
    // Use this for initialization
    void Start () {

        for (int i = 0; i < 4; i++)
        {
            int numLvl = i + 1;
            transform.GetChild(i).gameObject.name = numLvl.ToString();
            transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = numLvl.ToString();

            if (i < countUnlockedLevel)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = unlockedIcon;
                transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().sprite = lockedIcon;
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }	
        for(int i = 4; i < transform.childCount; i++)
        {
           transform.GetChild(i).GetComponentInChildren<Text>().text = "";
           transform.GetChild(i).GetComponent<Image>().sprite = lockIcon;
           transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
	}
}
