using UnityEngine;
using UnityEngine.SceneManagement;
using Map;
public class MySceneManager : MonoBehaviour
{
    NodeElementalType nodeElementalType;
    // Function to load a scene by name
    public static MySceneManager Instance;
    public AudioClip battleSceneMusic;
    public AudioClip stageSceneMusic;
    public AudioClip gameOverSceneMusic;
    public bool sceneLocked = false;
  
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환시에도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }
    public void LoadElmentalType(NodeElementalType nodeElementalType)
    {
        AudioManager.instance.ChangeBackgroundMusic(battleSceneMusic);
        switch (nodeElementalType)
        {
            case NodeElementalType.Fire:
                SceneManager.LoadScene("fire");
                break;
            case NodeElementalType.Water:
                SceneManager.LoadScene("water");
                break;
            case NodeElementalType.Wind:
                SceneManager.LoadScene("wind");
                break;
            case NodeElementalType.Land:
                SceneManager.LoadScene("rock");
                break;

        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Function to reload the current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Function to load the next scene in the build settings
    public void LoadNextScene()
    {
        if (sceneLocked) { Debug.Log("SceneChange is Locked!"); return; }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log("Scene Move To NextScene");
        }
        else
        {
            Debug.Log("You are at the last scene. No next scene to load.");
        }
    }

    // Function to load the previous scene in the build settings
    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            Debug.Log("You are at the first scene. No previous scene to load.");
        }
    }

    // Function to quit the game (Note: This will only work in a built game, not in the Unity Editor)
    public void QuitGame()
    {
        Application.Quit();
    }
}