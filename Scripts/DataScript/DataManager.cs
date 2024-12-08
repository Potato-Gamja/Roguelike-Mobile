using System.IO;
using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.CharacterScripts;    //픽셀 판타지 에셋에 있는 캐릭터 빌드 스크립트

[System.Serializable]
    public class SaveData
    {
        public string hair;                                       //플레이어의 머리 종류   
        public string armor;                                      //플레이어의 방어구 종류
        public string helmet;                                     //플레이어의 투구 종류
        public string mask;                                       //플레이어의 가면 종류

        public int coin;                                          //플레이어의 재화
    }

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public CharacterBuilder builder;

    public SaveData saveData = new SaveData();

    int coin;
    public string path;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance.gameObject);                        //씬을 다시 돌아와 같은 오브젝트가 있을 경우 오브젝트를 파괴
        path = Application.persistentDataPath + "/Data.json";    //제이슨의 경로를 설정
        LoadData();
        
        DontDestroyOnLoad(this.gameObject);                      //씬 전환 시에도 정보를 가질 수 있게 파괴되지 않게 만들기


    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(saveData, true);        //세이브 데이터를 제이슨 형태로 포멧팅한 문자열을 생성

        if (File.Exists(path))                                   //새로 데이터를 저장할 경우 오류가 발생하지 않게 경로에 데이터가 있는지 체크
        {
            saveData.hair = builder.Hair;
            saveData.armor = builder.Armor;
            saveData.helmet = builder.Helmet;
            saveData.mask = builder.Mask;
            saveData.coin = coin;
        }

        File.WriteAllText(path, json);                           //파일을 생성하고 저장
    }

    public void LoadData()
    {
        if (!File.Exists(path))                                  //경로에 저장되어 있는 데이터가 없을 경우 아래의 값을 넣은 세이브 데이터를 실행
        {
            saveData.hair = "Hair2";
            saveData.armor = "";
            saveData.helmet = "";
            saveData.mask = "";
            saveData.coin = 1000;

            SaveData();
        }
        else                                                     //경로에 데이터가 저장되어있다면 해당 데이터를 읽기
        {
            string json = File.ReadAllText(path);                //파일의 텍스트를 문자열로 저장
            saveData = JsonUtility.FromJson<SaveData>(json);     //제이슨 데이터를 역직렬화하여 세이브 데이터에 넣기
        }
    }
}
