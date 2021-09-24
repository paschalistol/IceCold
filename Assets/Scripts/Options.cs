using UnityEngine;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{
    [SerializeField] private GameObject soundPanel, machineCanvas, screenPanel, pausePanel;
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
            pausePanel.SetActive(true);
            soundPanel.SetActive(true);
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.name.Equals("Music"))
                {
                    source.Pause();
                }
            }
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            soundPanel.SetActive(false);
            foreach (var source in sources)
            {
                if (!source.outputAudioMixerGroup.ToString().Equals("Music"))
                {
                    source.UnPause();
                }
            }
        }
    }

    private void FullScreen(bool fullScreen)
    {

    }
}
