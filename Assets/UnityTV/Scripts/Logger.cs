using UnityEngine;

namespace Nerdtron.TV
{
    public class Logger : MonoBehaviour
    {
        private string m_Message;
        private bool m_ShowDebugScreen = false;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                m_ShowDebugScreen = !m_ShowDebugScreen;
            }
        }

        void OnGUI()
        {
            if (m_ShowDebugScreen == true)
            {
                GUI.Box(new Rect(Screen.width/2 - 100, Screen.height - 24, 200, 25), m_Message);
                GUI.Box(new Rect(Screen.width - 100, Screen.height - 24, 100, 25), "Version 1.0.0");
            }
        }

        public void Log(string message)
        {
            m_Message = message;
            Debug.Log(message);
        }
    }
}
