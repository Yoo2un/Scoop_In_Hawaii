using UnityEngine;
using System;
using System.IO;

// 실제 저장 파일을 읽고/쓰는 역할 담당 클래스
public class FileDataHandler
{
    // 저장 폴더 경로
    private string dataDirPath = "";

    // 저장 파일 이름
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    // 생성자
    // 저장 위치와 파일 이름을 받아서 변수에 저장
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    // =========================
    // 데이터 불러오기
    // =========================
    public GameData Load()
    {
        // 폴더 경로 + 파일 이름 합쳐서 전체 경로 만들기
        // 예:
        // C:/Users/.../saveData/game.save
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        // 불러온 데이터를 저장할 변수
        GameData loadedData = null;

        // 해당 파일이 실제 존재하는지 확인
        if (File.Exists(fullPath))
        {
            try
            {
                // 파일 안의 JSON 문자열 저장용 변수
                string dataToLoad = "";

                // 파일 열기
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // 파일 내용을 읽기 위한 Reader 생성
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // 파일 전체 내용 읽기
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if(useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // JSON 문자열 -> GameData 객체로 변환
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                // 에러 발생 시 콘솔 출력
                Debug.LogError("Error occured when trying to load data from file: "
                    + fullPath + "\n" + e);
            }
        }

        // 불러온 데이터 반환
        return loadedData;
    }

    // =========================
    // 데이터 저장하기
    // =========================
    public void Save(GameData data)
    {
        // 저장할 전체 파일 경로 만들기
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // 저장 폴더가 없으면 자동 생성
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // GameData 객체 -> JSON 문자열로 변환
            // true = 보기 좋게 줄바꿈 포함
            string dataToStore = JsonUtility.ToJson(data, true);

            if(useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // 파일 생성 모드로 열기
            // 기존 파일 있으면 덮어쓰기
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // 파일에 텍스트 쓰기 위한 Writer 생성
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    // JSON 문자열 파일에 저장
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            // 저장 중 에러 발생 시 콘솔 출력
            Debug.LogError("Error occured when trying to save data to file: "
                + fullPath + "\n" + e);
        }
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public bool HasSaveFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        return File.Exists(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}