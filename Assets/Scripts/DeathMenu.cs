using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("temple");
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
