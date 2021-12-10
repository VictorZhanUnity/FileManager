using System.Collections;
using UnityEngine;
using Managers.AndroidHandler;
using System;

namespace Managers.FileHandler
{
    /// <summary>
    /// 負責檔案IO相關處理
    /// </summary>
    public class FileManager
    {
        #region {========== Singleton: Instance ==========}
        private static FileManager _instance;
        public static FileManager Instance
        {
            get
            {
                if (_instance == null) _instance = new FileManager();
                return _instance;
            }
        }
        #endregion

        #region {========== 最後一次存圖檔時的圖檔路徑: LastImageURL ==========}
        private string m_LastImageURL = "";
        /// <summary>
        /// 最後一次存圖檔時的圖檔路徑
        /// </summary>
        public string LastImageURL
        {
            get
            {
                return m_LastImageURL;
            }
        }
        #endregion

        #region {========== 將Texture2D存成PNG放在相簿: SaveImageToGallery ==========}
        private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
        /// <summary>
        /// 將Texture2D存成JPG放在手機相簿
        /// </summary>
        /// <param name="texture2D">存(Texture2D)RawImage</param>
        /// <param name="title">檔名</param>
        /// <param name="description">檔案描述</param>
        /// <returns>回傳檔案徑</returns>
        public string SaveImageToGallery(Texture2D texture2D, string title="", string description="")
        {
            Debug.Log("SaveImageToGallery");
            using (var mediaClass = new AndroidJavaClass(MediaStoreImagesMediaClass))
            {
                using (var cr = AndroidManager.Instance.CurrentActivity.Call<AndroidJavaObject>("getContentResolver"))
                {
                    
                    var image = Texture2DToAndroidBitmap(texture2D);
                    if (title.Length == 0) title = DateTimeFileName;
                    m_LastImageURL = mediaClass.CallStatic<string>("insertImage", cr, image, title, description);
                    return m_LastImageURL;
                }
            }
        }
        private AndroidJavaObject Texture2DToAndroidBitmap(Texture2D texture2D)
        {
            byte[] encoded = texture2D.EncodeToPNG();
            using (var bf = new AndroidJavaClass("android.graphics.BitmapFactory"))
            {
                return bf.CallStatic<AndroidJavaObject>("decodeByteArray", encoded, 0, encoded.Length);
            }
        }
        #endregion

        #region {========== 擷取螢幕畫面, 需要用StartCoroutine呼叫: SaveScreenShot ==========}
        /// <summary>
        /// 擷取螢幕畫面, 需要用StartCoroutine呼叫
        /// </summary>
        public IEnumerator SaveScreenShot(string title = "", string description = "")
        {
            yield return new WaitForEndOfFrame();

            int width = Screen.width;
            int height = Screen.height;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            SaveImageToGallery(texture, title, description);
        } 
        #endregion

        /// <summary>
        /// 以日期時間為圖片檔名
        /// </summary>
        private String DateTimeFileName
        {
            get
            {
                return DateTime.Now.ToString("F");
            }
        }
    }
}
