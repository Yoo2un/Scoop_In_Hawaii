using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//�̱��� Ŭ����
//�ν��Ͻ��� �� 1���� �����ؼ� ��𼭵� ���� ������ ���� ������ ����
public class DataPersistenceManager : MonoBehaviour
{

   [Header("File Storage Config")]
   [SerializeField] private string fileName;
   [SerializeField] private bool useEncryption;

   private FileDataHandler dataHandler;
   private GameData gameData;
   private List<IDataPersistence> dataPersistenceObjects;
   
   public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        if (scene.name == "MainMenu" || scene.name == "Result")
            return;

        LoadGame();
    }


    public void NewGame()
    {
        DeleteGame();

        gameData = new GameData();

        SaveGame();
    }

    public void LoadGame()
    {
        //����� ������ �ҷ�����(�ε��� �����Ͱ� �ִ� ���)
        this.gameData = dataHandler.Load();

        //�ε��� �����Ͱ� ���� ���
        if(this.gameData == null)
        {
            Debug.Log("No data was found.");
            NewGame();
        }

        //�ε�� �����͸� �ʿ��� �ٸ� ��� ��ũ��Ʈ�� �����ϴ� ���(?)
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log(dataPersistenceObj.GetType().Name);
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    public bool HasGameData()
    {
        return dataHandler.HasSaveFile();
    }
    public void DeleteGame()
    {
        dataHandler.Delete();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
