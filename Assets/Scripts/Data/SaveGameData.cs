using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccountData;
using System.IO;
using Newtonsoft.Json;

namespace SaveData
{
    public class SaveGameData : MonoBehaviour
    {
        string json;
        public string SaveFileName;

        public AccountManager AccMan;
        SaveGameDataInfo Data = new SaveGameDataInfo();

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SetAccMan()
        {
            AccMan = FindObjectOfType<AccountManager>();
        }

        public void SaveUsername()
        {
            Data.rememberUsername = AccMan.RememberUsernameBool;
            Data.rememberToggle = AccMan.RememberUsername.isOn;
            Data.lastUsedUsername = AccMan.loginUsername.text;
            json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/SaveUsernameInfo.json", json);
        }

        public void LoadUsername()
        {
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/SaveUsernameInfo.json");
            Data = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
            AccMan.loginUsername.text = Data.lastUsedUsername;
            AccMan.RememberUsernameBool = Data.rememberUsername;
            AccMan.RememberUsername.isOn = Data.rememberToggle;
        }

        public void SaveData()
        {
            SaveFileName = AccMan.SaveFileName.text;
            Data.saveFileName = AccMan.SaveFileName.text;
            Data.Accounts = AccMan.accountHolder;
            json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/" + SaveFileName + ".json", json);
        }

        public void LoadData(string loadSaveData)
        {
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/" + loadSaveData + ".json");
            Data = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
            AccMan.accountHolder = Data.Accounts;
            SaveFileName = Data.saveFileName;
        }
    }
    [SerializeField]
    public class SaveGameDataInfo
    {
        public string saveFileName;
        public bool rememberToggle;
        public bool rememberUsername;
        public string lastUsedUsername;
        public Dictionary<string, string> Accounts = new Dictionary<string, string>();
    }
}
