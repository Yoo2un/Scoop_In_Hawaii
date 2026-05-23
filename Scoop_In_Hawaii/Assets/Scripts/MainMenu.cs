using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGameClicked()
    {
        SceneManager.LoadScene("DayScene");
        //DataPersistenceManager.instance.NewGame();
    }

    public void LoadGameClicked()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void SaveGameClicked()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
