using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneToLoad;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Next();
        }
    }
    // Start is called before the first frame update
    public void Next()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
