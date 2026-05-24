using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//싱글톤 클래스
//인스턴스를 단 1개만 생성해서 어디서든 접근 가능한 생성 디자인 패턴
public class DataPersistenceManager : MonoBehaviour
{

   [Header("File Storage Config")]
   [SerializeField] private string fileName;

   private FileDataHandler dataHandler;
   private GameData gameData;
   private List<IDataPersistence> dataPersistenceObjects;
   
   public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        LoadGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //저장된 데이터 불러오기(로드할 데이터가 있는 경우)
        this.gameData = dataHandler.Load();

        //로드할 데이터가 없는 경우
        if(this.gameData == null)
        {
            Debug.Log("No data was found.");
            NewGame();
        }

        //로드된 데이터를 필요한 다른 모든 스크립트로 전달하는 경우(?)
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        
    }

    public void SaveGame()
    {
        //다른 스크립트에서 데이터를 업데이트할 수 있도록 데이터 전달하기
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log("Saving from: " + dataPersistenceObj);
            dataPersistenceObj.SaveData(ref gameData);
        }

        //파일 데이터 핸들러를 사용하여 해당 데이터를 파일에 저장하기
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
