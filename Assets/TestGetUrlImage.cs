using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestGetUrlImage : MonoBehaviour
{
    public Image image;
    public string path = "Assets/LimDongYoon/Sprites/Test";
    public string url = "https://pbs.twimg.com/profile_images/798463233774350336/KlHqUNgL_400x400.jpg";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadImage(url));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DownloadImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            GetComponent<Image>().sprite = sprite;
            byte[] imageBytes = texture.EncodeToPNG();

            // 파일 시스템에 이미지를 저장합니다.
            string directoryPath = Path.Combine(Application.dataPath, "LimDongYoon/Sprites/Test");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, "DownloadedImage.png");
            File.WriteAllBytes(filePath, imageBytes);
        }
    }
}
