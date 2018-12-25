using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        #region CheckWin
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (SceneManager.GetActiveScene().buildIndex == LevelManager.countUnlockedLevel)
            {
                LevelManager.countUnlockedLevel++;
            }
            Debug.Log("YOU WIN!!!");
            SceneManager.LoadScene(0);
        }
        #endregion

        #region CheckLose
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("YOU LOSE :(");
            SceneManager.LoadScene(0);
        }
        #endregion
    }
}
