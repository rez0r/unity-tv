using UnityEngine;
using UnityEngine.UI;

public class LogManager : Singleton<LogManager>
{
    public RawImage screen;
    public bool isDebugMode;
    public GameObject debugPanel;
    public Text statusMessageLabel;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.isDebugMode = !this.isDebugMode;
            this.debugPanel.SetActive(this.isDebugMode);
        }
    }

    public void Log(string message)
    {
        Debug.Log(message);
        this.statusMessageLabel.text = message;
    }
}
