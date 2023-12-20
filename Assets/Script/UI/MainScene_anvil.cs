using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene_anvil : MonoBehaviour
{
    public string otherSceneName;

    void OnMouseDown()
    {

        SceneManager.LoadScene(otherSceneName);

    }
}
