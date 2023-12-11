using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Image : MonoBehaviour
{
    public Image[] fadeImages;
    public float fadeDuration = 2.0f;
    public float delayBeforeSceneChange = 3.0f;

    private void Start()
    {
        StartCoroutine(FadeIn(fadeImages[0]));
        StartCoroutine(FadeIn(fadeImages[1]));
    }

    IEnumerator FadeIn(Image image)
    {
        float timer = fadeDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(delayBeforeSceneChange);

        SceneManager.LoadScene("Main UI");

    }
}
