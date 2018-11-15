using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccountData;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace SaveData
{
    public class SaveGameData : MonoBehaviour
    {
        string json;
        string activeScene;
        string saveName;

        public AccountManager AccMan;
        SaveGameDataInfo Data = new SaveGameDataInfo();

        void Start()
        {
            AccMan = FindObjectOfType<AccountManager>();
            DontDestroyOnLoad(gameObject);
        }

        public void LoadStartingData()
        {
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/saveFile.json");
            Data = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
            AccMan.accountHolder = Data.Accounts;
            AccMan.Remember = Data.rememberUsername;
            AccMan.RememberUsername.isOn = Data.rememberToggle;
        }

        public void SaveData()
        {
            saveName = AccMan.SaveFileName.text;
            Data.rememberUsername = AccMan.Remember;
            Data.Accounts = AccMan.accountHolder;
            Data.rememberToggle = AccMan.RememberUsername.isOn;
            json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/" + saveName + ".json", json);
        }

        public void LoadData()
        {
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/saveFile.json");
            Data = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
            AccMan.accountHolder = Data.Accounts;
            AccMan.Remember = Data.rememberUsername;
            AccMan.RememberUsername.isOn = Data.rememberToggle;
        }
    }

    public class SaveGameDataInfo
    {
        public bool rememberToggle;
        public bool rememberUsername;
        public Dictionary<string, string> Accounts = new Dictionary<string, string>();
    }
}
