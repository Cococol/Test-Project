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

        AccountManager AccMan;
        SaveGameDataInfo Data;

        void Start()
        {
            AccMan = FindObjectOfType<AccountManager>();
            DontDestroyOnLoad(gameObject);
        }

        public void SaveData()
        {
            Data.rememberUsername = AccMan.Remember;
            Data.Accounts = AccMan.accountHolder;
            Data.rememberToggle = AccMan.RememberUsername.isOn;
            json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/StreamingAssets/saveFile.json", json);
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
