using UnityEngine;

//싱글톤 클래스
//인스턴스를 단 1개만 생성해서 어디서든 접근 가능한 생성 디자인 패턴
public class DataPersistenceManager : MonoBehaviour
{

   private GameData gameData;
   
   public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
            instance = this;
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //저장된 데이터 불러오기(로드할 데이터가 있는 경우)

        //로드할 데이터가 없는 경우
        if(this.gameData == null)
        {
            Debug.Log("No data was found.");
            NewGame();
        }

        //로드된 데이터를 필요한 다른 모든 스크립트로 전달하는 경우(?)
    }

    public void SaveGame()
    {
        //다른 스크립트에서 데이터를 업데이트할 수 있도록 데이터 전달하기

        //파일 데이터 핸들러를 사용하여 해당 데이터를 파일에 저장하기
    }
}
