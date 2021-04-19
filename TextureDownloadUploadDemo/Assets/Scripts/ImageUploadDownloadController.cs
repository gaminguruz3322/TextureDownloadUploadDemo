using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageUploadDownloadController : MonoBehaviour
{

    [SerializeField] Image textureImage;
    [SerializeField] string imageURL;
    [SerializeField] string fileName;
    //

    /// <summary>
    /// click event - load image from web URL 
    /// </summary>
    public void OnLoadImageFromWebButtonClick()
    {
        StartCoroutine(LoadTextureFromWeb());
    }

    // enumerator to load texture from web URL
    IEnumerator LoadTextureFromWeb()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
            textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
            textureImage.SetNativeSize();
        }
    }

    /// <summary>
    /// click event - load image from disk 
    /// </summary>
    public void OnLoadImageFromDiskButtonClick()
    {
        if(!File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.LogError("File Not Exist!");
            return;
        }

        LoadImageFromDisk();
    }

    // load texture bytes from disk and compose sprite from bytes
    private void LoadImageFromDisk()
    {
        byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + fileName);
        Texture2D loadedTexture = new Texture2D(0, 0);
        loadedTexture.LoadImage(textureBytes);
        textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
        textureImage.SetNativeSize();

    }

    /// <summary>
    /// click event - save image to disk 
    /// </summary>
    public void OnSaveImageButtonClick()
    {
        if (textureImage.sprite == null)
        {
            Debug.LogError("No Image to Save!");
            return;
        }

        WriteImageOnDisk();
    }

    // generate texture bytes and save to disk
    private void WriteImageOnDisk()
    {
        byte[] textureBytes = textureImage.sprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + fileName, textureBytes);
        Debug.Log("File Written On Disk!");
    }

    /// <summary>
    /// click event - make image blank 
    /// </summary>
    public void OnBlankImageButtonClick()
    {
        textureImage.sprite = null;
    }

}
