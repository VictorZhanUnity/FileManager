using System.Collections;
using UnityEngine;
using Managers.AndroidHandler;
using System;

namespace Managers.FileHandler
{
    /// <summary>
    /// �t�d�ɮ�IO�����B�z
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

        #region {========== �̫�@���s���ɮɪ����ɸ��|: LastImageURL ==========}
        private string m_LastImageURL = "";
        /// <summary>
        /// �̫�@���s���ɮɪ����ɸ��|
        /// </summary>
        public string LastImageURL
        {
            get
            {
                return m_LastImageURL;
            }
        }
        #endregion

        #region {========== �NTexture2D�s��PNG��b��ï: SaveImageToGallery ==========}
        private const string MediaStoreImagesMediaClass = "android.provider.MediaStore$Images$Media";
        /// <summary>
        /// �NTexture2D�s��JPG��b�����ï
        /// </summary>
        /// <param name="texture2D">�s(Texture2D)RawImage</param>
        /// <param name="title">�ɦW</param>
        /// <param name="description">�ɮ״y�z</param>
        /// <returns>�^���ɮ׮|</returns>
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

        #region {========== �^���ù��e��, �ݭn��StartCoroutine�I�s: SaveScreenShot ==========}
        /// <summary>
        /// �^���ù��e��, �ݭn��StartCoroutine�I�s
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
        /// �H����ɶ����Ϥ��ɦW
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
