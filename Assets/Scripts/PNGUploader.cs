// Saves screenshot as PNG file.
using UnityEngine;
using System.Collections;
using System.IO;

public class PNGUploader : MonoBehaviour
{
    private string folderURL;

    // Take a shot immediately
    IEnumerator Start()
    {
        Debug.Log("PNGUploader Start");
        folderURL = "/sdcard/DCIM/Camera/SavedScreen.png";

        yield return new WaitForSeconds(2);
        StartCoroutine( UploadPNG());
    }

    IEnumerator UploadPNG()
    {
        Debug.Log("UploadPNG");
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(folderURL, bytes);
        Debug.Log($"Save:{folderURL}");
    }
}