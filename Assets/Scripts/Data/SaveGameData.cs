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
        //the json string where all the data is getting stored
        string json;
        //string that saves the savefile name;
        string SaveFileName;

        //assigns classes which is being used in this script
        AccountManager AccMan;
        SaveGameDataInfo Data = new SaveGameDataInfo();
        SaveUsername UsernameData = new SaveUsername();

        void Start()
        {
            //makes sure this object will not be destroyed when switching scenes
            DontDestroyOnLoad(gameObject);
        }

        public void SetAccMan()
        {
            //sets the accountmanager so it can be used in this script
            AccMan = FindObjectOfType<AccountManager>();
        }

        public void SaveUsername()
        {
            //sets the variables that have to be saved in the SaveGameDataInfo class
            UsernameData.rememberUsername = AccMan.RememberUsernameBool;
            UsernameData.rememberToggle = AccMan.RememberUsername.isOn;
            UsernameData.lastUsedUsername = AccMan.loginUsername.text;
            //takes the data from the SaveGameDataInfo class, sets it over to a string and formats it so it can be stored in a json file.
            json = JsonConvert.SerializeObject(UsernameData, Formatting.Indented);
            //Writes all the data that is stored in the json string to a .json file and stores it into a file named "SaveUsernameInfo" into the StreamingAssets folder.
            File.WriteAllText(Application.dataPath + "/StreamingAssets/SaveUsername/SaveUsernameInfo.json", json);
        }

        public void LoadUsername()
        {
            //puts all the data saved in the "SavedUsernameInfo" file into the json string.
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/SaveUsername/SaveUsernameInfo.json");
            //reads out the information stored into the json string and puts it into variables.
            UsernameData = JsonConvert.DeserializeObject<SaveUsername>(json);
            //sets the data from the json string equal to the variables.
            AccMan.loginUsername.text = UsernameData.lastUsedUsername;
            AccMan.RememberUsernameBool = UsernameData.rememberUsername;
            AccMan.RememberUsername.isOn = UsernameData.rememberToggle;
        }

        public void SaveData()
        {
            //sets the variables that have to be saved in the SaveGameDataInfo class
            SaveFileName = AccMan.SaveFileName.text;
            Data.saveFileName = AccMan.SaveFileName.text;
            Data.Accounts = AccMan.accountHolder;
            //takes the data from the SaveGameDataInfo class, sets it over to a string and formats it so it can be stored in a json file.
            json = JsonConvert.SerializeObject(Data, Formatting.Indented);
            //Writes all the data that is stored in the json string to a .json file and stores it into a file named "SaveUsernameInfo" into the StreamingAssets folder.
            File.WriteAllText(Application.dataPath + "/StreamingAssets/" + SaveFileName + ".json", json);
        }

        public void LoadData(string loadSaveData)
        {
            //puts all the data saved in the "loadSaveData" file into the json string.
            json = File.ReadAllText(Application.dataPath + "/StreamingAssets/" + loadSaveData + ".json");
            //reads out the information stored into the json string and puts it into variables.
            Data = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
            //sets the data from the json string equal to the variables.
            AccMan.accountHolder = Data.Accounts;
            SaveFileName = Data.saveFileName;
        }
    }
    //the class where all the variables are in which you want to save
    [SerializeField]
    public class SaveGameDataInfo
    {
        public string saveFileName;
        public Dictionary<string, string> Accounts = new Dictionary<string, string>();
    }
    //the class where all the variables are in which you want to save
    [SerializeField]
    public class SaveUsername
    {
        public bool rememberToggle;
        public bool rememberUsername;
        public string lastUsedUsername;
    }
}
