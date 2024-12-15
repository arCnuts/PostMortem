
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Settings Components")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    //public AudioSource backgroundMusic;   -- tutaj audio

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1.0f);
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("temple");
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

    
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
