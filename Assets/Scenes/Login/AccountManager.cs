using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;
using SaveData;

namespace AccountData
{
    public class AccountManager : MonoBehaviour
    {
        #region UI stuff
        [Header("Login UI")]
        public InputField loginUsername;
        public InputField loginPassword;
        public GameObject Login;

        [Header("Register UI")]
        public InputField registerUsername;
        public InputField registerPassword;
        public GameObject Register;

        [Header("Overig")]
        public InputField SaveFileName;
        public Toggle RememberUsername;
        public Text RespondText;
        public bool Remember = false;
        #endregion
        string json;
        string LoadFileName;

        DirectoryInfo saveFile;

        FileInfo[] saveInfo;

        public Dictionary<string, string> accountHolder = new Dictionary<string, string>();

        public SaveGameData Data;

        SaveGameDataInfo DataInfo = new SaveGameDataInfo();

        private void Start()
        {
            Data = FindObjectOfType<SaveGameData>();
            saveFile = new DirectoryInfo(Application.dataPath + "/StreamingAssets/");
            saveInfo = saveFile.GetFiles("*.json");
            if(File.Exists(Application.dataPath + "/StreamingAssets/SaveUsernameInfo.json"))
            {
                Data.LoadUsername();
            }
            else
            {
                return;
            }
        }

        public void RegisterAccount()
        {
            if (saveFile.GetFiles().Length > 0)
            {
                foreach (FileInfo Fi in saveInfo)
                {      
                    json = File.ReadAllText(Fi.ToString());

                    DataInfo = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
                    accountHolder = DataInfo.Accounts;

                    if (registerUsername.text == null || registerPassword.text == null || SaveFileName.text == null)
                    {
                        RespondText.text = "<color=red>Username, password or save file name has not been filled in.</color>";
                        StartCoroutine(RemoveText(3));
                    }
                    else if (accountHolder.ContainsKey(registerUsername.text) && accountHolder.ContainsValue(registerPassword.text))
                    {
                        RespondText.text = "<color=red> account already exists.</color>";
                        StartCoroutine(RemoveText(3));
                    }
                    else
                    {
                        accountHolder.Add(registerUsername.text, registerPassword.text);
                        RespondText.text = "<color=blue>Account has been registered.</color>";
                        Data.SaveData();
                        StartCoroutine(RemoveText(3));
                    }
                    accountHolder.Clear();
                }
                ResetValues();
            }
            else
            {
                Debug.Log("Path is not found");
                if (registerUsername.text == null || registerPassword.text == null || SaveFileName.text == null)
                {
                    RespondText.text = "<color=red>You got to put something in the inputfield, otherwise it wont work.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                else if (accountHolder.ContainsKey(registerUsername.text) && accountHolder.ContainsValue(registerPassword.text))
                {
                    RespondText.text = "<color=red> account already exists.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                else
                {
                    accountHolder.Add(registerUsername.text, registerPassword.text);
                    RespondText.text = "<color=blue>Account has been registered.</color>";
                    Data.SaveData();
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                accountHolder.Clear();
            }
        }

        public void AccountLogin()
        {
            if (saveFile.GetFiles().Length > 0)
            {
                foreach (FileInfo Fi in saveInfo)
                {
                    json = File.ReadAllText(Fi.ToString());

                    DataInfo = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
                    accountHolder = DataInfo.Accounts;
                    LoadFileName = DataInfo.saveFileName;

                    if (loginUsername.text == null || loginPassword.text == null)
                    {
                        RespondText.text = "<color=red>You got to put something in the inputfield, otherwise it wont work.</color>";
                        StartCoroutine(RemoveText(3));
                    }
                    else if (!accountHolder.ContainsKey(loginUsername.text) || !accountHolder.ContainsValue(loginPassword.text))
                    {
                        RespondText.text = "<color=red>Username or password is not correct.</color>";
                        StartCoroutine(RemoveText(3));
                    }
                    else if (accountHolder.ContainsKey(loginUsername.text) && accountHolder.ContainsValue(loginPassword.text))
                    {
                        RespondText.text = "<color=blue>Account information correct.</color>";
                        Data.LoadData(LoadFileName);
                        StartCoroutine(RemoveText(3));
                    }
                    accountHolder.Clear();
                }
                ResetValues();
            }
            else
            {
                if (loginUsername.text == null || loginPassword.text == null)
                {
                    RespondText.text = "<color=red>You got to put something in the inputfield, otherwise it wont work.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                else if (!accountHolder.ContainsKey(loginUsername.text) || !accountHolder.ContainsValue(loginPassword.text))
                {
                    RespondText.text = "<color=red>Username or password is not correct.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                else if (accountHolder.ContainsKey(loginUsername.text) && accountHolder.ContainsValue(loginPassword.text))
                {
                    RespondText.text = "<color=blue>Account information correct.</color>";
                    Data.LoadData();
                    ResetValues();
                    StartCoroutine(RemoveText(3));                
                }
                accountHolder.Clear();
            }
        }

        public void ToggleActivated()
        {
            if (Remember)
            {
                Remember = false;
                loginUsername.text = "";
            }
            else if (!Remember)
            {
                Remember = true;
            }
            Data.SaveUsername();
        }

        private void ResetValues()
        {
            if (!Remember)
            {
                loginUsername.text = "";
            }
            loginPassword.text = "";
            registerPassword.text = "";
            registerUsername.text = "";
        }

        #region UI toggle
        public void SwitchUIToLogin()
        {
            Login.SetActive(true);
            Register.SetActive(false);
            RespondText.text = "";
        }

        public void SwitchUIToRegister()
        {
            Login.SetActive(false);
            Register.SetActive(true);
            RespondText.text = "";
        }
        #endregion

        public IEnumerator RemoveText(float t)
        {
            yield return new WaitForSeconds(t);
            RespondText.text = "";
        }
    }
}
