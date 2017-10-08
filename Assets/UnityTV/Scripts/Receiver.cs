using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Nerdtron.TV
{
    public class Receiver : MonoBehaviour
    {
        public Logger logger;
        public RawImage screen;
        public AudioSource speaker;
        public bool isRandomPlay = false;
        public string[] videoURLS = new string[] { };

        private MovieTexture loadedMovieTexture;
        private WWW[] m_VideoBuffer = new WWW[] { };

        void Start()
        {
            StartCoroutine("Buffer");
        }

        private IEnumerator Buffer()
        {
            int key = 0;
            System.Array.Resize(ref m_VideoBuffer, videoURLS.Length);

            while (true)
            { 
                // Download the video.
                WWW www = new WWW(this.videoURLS[key]);
                while (www.isDone == false)
                {
                    this.logger.Log("The video is loading!");
                    yield return 0;
                }

                m_VideoBuffer[key] = www;

                ++key;
                Debug.Log(key.ToString());

                if (key == videoURLS.Length - 2)
                {
                    StartCoroutine("Play");
                }

                if (key == videoURLS.Length)
                {
                    Debug.Log("Break!");
                    yield break;
                }
            }
        }

        private int RandomizeKey()
        {
            int key = Random.Range(0, m_VideoBuffer.Length - 1);
            Debug.Log("Video Buffer size = " + m_VideoBuffer.Length.ToString());
            return key;
        }

        private IEnumerator Play()
        {
            int key = 0;
            WWW video;

            while (true)
            {
                if (this.isRandomPlay == true)
                {
                    key = RandomizeKey();
                    Debug.Log("Random key = " + key);
                }
                
                video = m_VideoBuffer[key];

                // Assigned the downloaded video to a movie texture.
                if (video != null)
                {
                    this.loadedMovieTexture = video.GetMovieTexture();
                    while (this.loadedMovieTexture.isReadyToPlay == false)
                    {
                        logger.Log("The video is loading!");
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
                        this.logger.Log("The video is playing!");
                        yield return 0;
                    }

                    this.logger.Log("The video is finished!");
                }

                if (key < this.videoURLS.Length -1)
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