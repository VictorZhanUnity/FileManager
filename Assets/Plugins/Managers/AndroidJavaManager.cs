using UnityEngine;

namespace Managers.AndroidHandler
{
    /// <summary>
    /// �t�dAndroid�������q�B�z
    /// </summary>
    public class AndroidManager
    {
        #region {========== Singleton: Instance ==========}
        private static AndroidManager _instance;
        public static AndroidManager Instance
        {
            get
            {
                if (_instance == null) _instance = new AndroidManager();
                return _instance;
            }
        }
        #endregion

        #region {========== �^���ثe��Activity : CurrentActivity ==========}
        private AndroidJavaObject m_CurrentActivity;
        public AndroidJavaObject CurrentActivity
        {
            get
            {
                if (m_CurrentActivity == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    m_CurrentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                return m_CurrentActivity;
            }
        }
        #endregion
    }
}
