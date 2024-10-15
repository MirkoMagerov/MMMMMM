using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadSceneAsync(Levels.FirstLevel.ToString());
            GameManager.Instance.StartNewGame();
        }
    }
}
