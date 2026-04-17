using UnityEngine.SceneManagement;
using TMPro; 
using UnityEngine;

public class MenuManagers : MonoBehaviour
{
    public TMP_Text NamaPanel;

    public void SetNamaPanel(string nama)
    {
        NamaPanel.text = nama;
    }

    public void LoadScene(string namascene)
    {
        SceneManager.LoadScene(namascene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}