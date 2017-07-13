using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Nerdtron.TV
{
    public class Receiver : MonoBehaviour
    {
        public bool isDebugMode;
        public bool isRandomPlay = false;

        public RawImage screen;
        public AudioSource speaker;

        public Text statusMessageLabel;
        public GameObject debugPanel;

        public string[] videoURLS = new string[] { };

        private MovieTexture loadedMovieTexture;

        void Start()
        {
            StartCoroutine("Stream");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                this.isDebugMode = !this.isDebugMode;
                this.debugPanel.SetActive(this.isDebugMode);
            }
        }

        private void LogStatus(string message)
        {
            this.statusMessageLabel.text = message;
        }

        private IEnumerator Stream()
        {
            int key = 0; // TODO - The key should be a parameter, user might want to start from a different position in the list.

            while (true)
            {
                // Check if we are streaming the playlist of videos randomly or sequentially.
                if (this.isRandomPlay == true)
                {
                    key = Random.Range(0, this.videoURLS.Length);
                }

                // Download the video.
                WWW www = new WWW(this.videoURLS[key]);
                while (www.isDone == false)
                {
                    if (this.isDebugMode == true) this.LogStatus("The video is loading!");
                    yield return 0;
                }

                // Assigned the downloaded video to a movie texture.
                this.loadedMovieTexture = www.movie;
                while (this.loadedMovieTexture.isReadyToPlay == false)
                {
                    yield return 0;
                }

                this.screen.texture = this.loadedMovieTexture as MovieTexture;
                this.speaker.clip = this.loadedMovieTexture.audioClip;

                // Play the video.
                this.loadedMovieTexture.Play();
                this.speaker.Play();

                // Check if the video has finished playing.
                while (this.loadedMovieTexture.isPlaying == true)
                {
                    if (this.isDebugMode == true) this.LogStatus("The video is playing!");
                    yield return 0;
                }

                if (this.isDebugMode == true) this.LogStatus("The video is finished!");

                if (this.isRandomPlay == false)
                {
                    if (key < this.videoURLS.Length - 1)
                    {
                        ++key;
                    }
                    else
                    {
                        key = 0;
                    }
                }
            }
        }
    }
}