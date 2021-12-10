using UnityEngine;
using UnityEngine.UI;
using Managers.FileHandler;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button btnSaveImage, btnSaveScreen;
    [SerializeField] private RawImage rawImage;

    void Start()
    {
        btnSaveImage.onClick.AddListener(delegate { OnClickBtn(btnSaveImage); });
        btnSaveScreen.onClick.AddListener(delegate { OnClickBtn(btnSaveScreen); });
    }

    private void OnClickBtn(Button target)
    {
        if (target == btnSaveImage)
        {
            FileManager.Instance.SaveImageToGallery((Texture2D)rawImage.texture);

        }
        else if (target == btnSaveScreen)
        {
            StartCoroutine(FileManager.Instance.SaveScreenShot());
        }
        Debug.Log($"LastImageURL: {FileManager.Instance.LastImageURL}");
    }

    [ContextMenu("- Find Parameters")]
    private void FindParameters()
    {
        btnSaveImage = GameObject.Find("ButtonSaveImage").GetComponent<Button>();
        btnSaveScreen = GameObject.Find("ButtonSaveScreen").GetComponent<Button>();
        rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();
    }
}
