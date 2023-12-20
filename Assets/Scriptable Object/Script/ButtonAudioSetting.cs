using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudioSetting : MonoBehaviour
{
    public AudioClip audioClip;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => AudioManager.instance.mainAudioSource.PlayOneShot(audioClip));
        Debug.Log("Add button click event");
    }
}
