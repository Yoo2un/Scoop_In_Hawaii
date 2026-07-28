using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    private void Start()
    {
        continueButton.interactable = DataPersistenceManager.instance.HasGameData();
    }


    public void NewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadScene("DayScene");
    }

    public void LoadGameClicked()
    {
        SceneManager.LoadScene("DayScene");
    }

}
