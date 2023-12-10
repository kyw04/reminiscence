using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene_anvil : MonoBehaviour
{
    public string otherSceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnMouseDown()
    {

        SceneManager.LoadScene(otherSceneName);
    }
}
