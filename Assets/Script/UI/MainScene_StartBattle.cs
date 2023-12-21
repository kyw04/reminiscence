using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene_StartBattle : MonoBehaviour
{
    public string otherSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.ChangeBackgroundMusic(MySceneManager.Instance.stageSceneMusic);
            MySceneManager.Instance.LoadScene(otherSceneName);
        }
    }
}
