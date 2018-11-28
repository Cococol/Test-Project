using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
        public InputField SaveFileName;
        public GameObject Register;

        [Header("Overig")]
        public Toggle RememberUsername;
        public Toggle Showpassword;
        public Text RespondText;
        public bool RememberUsernameBool = false;
        public bool ShowPasswordBool = false;
        #endregion

        #region variables
        string json;
        string LoadFileName;

        DirectoryInfo saveFile;

        FileInfo[] saveInfo;

        public Dictionary<string, string> accountHolder = new Dictionary<string, string>();

        public SaveGameData Data;

        SaveGameDataInfo DataInfo = new SaveGameDataInfo();
        #endregion

        private void Start()
        {
            Data = FindObjectOfType<SaveGameData>();
            saveFile = new DirectoryInfo(Application.streamingAssetsPath);
            saveInfo = saveFile.GetFiles("*.json");
            Data.SetAccMan();
            if (File.Exists(Application.dataPath + "/StreamingAssets/SaveUsernameInfo.json"))
            {
                Data.LoadUsername();
            }
            else
            {
                return;
            }
        }

        private void Update()
        {
            if(loginUsername.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                loginPassword.Select();
            }

            if (registerUsername.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                registerPassword.Select();
            }
            else if(registerPassword.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                SaveFileName.Select();
            }
        }

        public void RegisterAccount()
        {
            //test with respondtext, respond text isn't shown when the project has been build. 
            if (saveFile.GetFiles().Length > 0)
            {
                //test with respondtext, respond text isn't shown when the project has been build.
                foreach (FileInfo Fi in saveInfo)
                {
                    //test with respondtext, respond text isn't shown when the project has been build.
                    json = File.ReadAllText(Fi.ToString());

                    DataInfo = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
                    accountHolder = DataInfo.Accounts;

                    if (registerUsername.text == "" || registerPassword.text == "" || SaveFileName.text == "")
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
                if (registerUsername.text == "" || registerPassword.text == "" || SaveFileName.text == "")
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
                    RespondText.text = "<color=red>No save files found. Please register your account first.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
                else if (!accountHolder.ContainsKey(loginUsername.text) || !accountHolder.ContainsValue(loginPassword.text))
                {
                    RespondText.text = "<color=red>No save files found. Please register your account first.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
            }
        }

        public void RememberUsernameToggle()
        {
            if (RememberUsernameBool)
            {
                RememberUsernameBool = false;
                loginUsername.text = "";
            }
            else if (!RememberUsernameBool)
            {
                RememberUsernameBool = true;
            }
            Data.SaveUsername();
        }

        public void ShowPasswordToggle()
        {
            if (ShowPasswordBool)
            {
                ShowPasswordBool = false;
                loginPassword.contentType = InputField.ContentType.Password;
                loginPassword.ActivateInputField();
            }
            else if (!ShowPasswordBool)
            {
                ShowPasswordBool = true;
                loginPassword.contentType = InputField.ContentType.Standard;
                loginPassword.ActivateInputField();
            }
        }

        private void ResetValues()
        {
            if (!RememberUsernameBool)
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
