using UnityEngine;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{

    private bool pause = false;





    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseMenu()
    {
        var sources = FindObjectsOfType<AudioSource>();

        pause = !pause;
        if (pause)
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.ToString().Equals("Music"))
                {

                    source.Pause();
                }
            }
        }
        else
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.ToString().Equals("Music"))
                {
                    source.UnPause();
                }
            }
        }
    }
}
