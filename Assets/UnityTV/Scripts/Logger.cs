using UnityEngine;
using UnityEngine.UI;

namespace Nerdtron.TV
{
    public class Logger : MonoBehaviour
    {
        public GameObject debugPanel;
        public Text statusMessageLabel;

        private bool isDebugScreenDisplayed;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                this.isDebugScreenDisplayed = !this.isDebugScreenDisplayed;
                this.debugPanel.SetActive(this.isDebugScreenDisplayed);
            }
        }

        public void Log(string message)
        {
            Debug.Log(message);
            this.statusMessageLabel.text = message;
        }
    }
}
