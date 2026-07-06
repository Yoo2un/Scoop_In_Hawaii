using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void NewGameScene()
    {
        SceneManager.LoadScene("DayScene");
    }
}
