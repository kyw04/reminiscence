using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempBtnAction : MonoBehaviour
{
    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void PopUp(GameObject openUI)
    {
        openUI.SetActive(true);
    }


    public void ClosePopUp(GameObject closeUI)
    {
        closeUI.SetActive(false);
    }
}