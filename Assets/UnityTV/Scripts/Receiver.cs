using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Nerdtron.TV
{
    [System.Serializable]
    public class Playlist
    {
        public string[] urls;
    }

    public class Receiver : MonoBehaviour
    {
        public Logger m_Logger;
        public RawImage m_Screen;
        public AudioSource m_Speaker;
        public bool isRandomPlay = false;

        private Playlist m_Playlist;
        private MovieTexture m_LoadedMovieTexture;
        private WWW[] m_VideoBuffer = new WWW[] { };
        private string m_PlaylistFileName = "Playlist.json";

        void Start()
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, m_PlaylistFileName);
            if (System.IO.File.Exists(filePath))
            {
                string dataAsJson = System.IO.File.ReadAllText(filePath);
                m_Playlist = JsonUtility.FromJson<Playlist>(dataAsJson);
            }

            StartCoroutine("Buffer");
        }

        private IEnumerator Buffer()
        {
            int key = 0;
            System.Array.Resize(ref m_VideoBuffer, m_Playlist.urls.Length);

            while (true)
            { 
                // Download the video.
                WWW www = new WWW(m_Playlist.urls[key]);
                while (www.isDone == false)
                {
                    m_Logger.Log("The video is loading!");
                    yield return 0;
                }

                m_VideoBuffer[key] = www;

                ++key;
                Debug.Log(key.ToString());

                if (key == m_Playlist.urls.Length - 2)
                {
                    StartCoroutine("Play");
                }

                if (key == m_Playlist.urls.Length)
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
                    m_LoadedMovieTexture = video.GetMovieTexture();
                    while (m_LoadedMovieTexture.isReadyToPlay == false)
                    {
                        m_Logger.Log("The video is loading!");
                        yield return 0;
                    }

                    m_Screen.texture = m_LoadedMovieTexture as MovieTexture;
                    m_Speaker.clip = m_LoadedMovieTexture.audioClip;

                    // Play the video.
                    m_LoadedMovieTexture.Play();
                    m_Speaker.Play();

                    // Check if the video has finished playing.
                    while (m_LoadedMovieTexture.isPlaying == true)
                    {
                        m_Logger.Log("Video playing!");
                        m_Logger.Log("Duration = " + m_LoadedMovieTexture.duration.ToString() + " secs");
                        yield return 0;
                    }

                    m_Logger.Log("The video is finished!");
                }

                if (key < m_Playlist.urls.Length -1)
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