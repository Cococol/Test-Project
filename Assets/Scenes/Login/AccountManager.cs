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
        //all the game objects and ui objects that are neccessary to get the account system to work.
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
        //specific variables we need thoughout the script.
        #region variables
        string json;
        string LoadFileName;

        //The directoryInfo checks for all the files in that specific directory.
        DirectoryInfo saveFile;

        //the FileInfo[] is an array that can hold multiple types of files.
        FileInfo[] saveInfo;

        //the dictionary is made so it can save the matching username and password.
        public Dictionary<string, string> accountHolder = new Dictionary<string, string>();

        //the class where i can call functions to save info in the json files.
        public SaveGameData Data;

        //The class where i can call the variables that have been saved.
        SaveGameDataInfo DataInfo = new SaveGameDataInfo();
        #endregion

        private void Start()
        {
            Data = FindObjectOfType<SaveGameData>();
            SearchNewSaveFiles();
            Data.SetAccMan();
            //checks if the streaming assets folder exists, if it exists it will check if a specific file exists.
            if (Directory.Exists(Application.dataPath + "/StreamingAssets/"))
            {
                if (File.Exists(Application.dataPath + "/StreamingAssets/SaveUsernameInfo.json"))
                {
                    Data.LoadUsername();
                }
                else
                {
                    return;
                }
            }
            //if it doesn't exist, we will make a streaming assets path.
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/");
            }

        }

        private void Update()
        {
            //part of code that will make sure you can navigate easely though the login/register part
            #region navigation
            //checks if the inputfield loginUsername is focused, if that is true and tab is pressed go to the next inputfield.
            if (loginUsername.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                loginPassword.Select();
            }

            //checks if the inputfield registerUsername is focused, if that is true and tab is pressed go to the next inputfield.
            if (registerUsername.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                registerPassword.Select();
            }
            //checks if the inputfield registerPassword is focused, if that is true and tab is pressed go to the next inputfield.
            else if (registerPassword.isFocused && Input.GetKeyDown(KeyCode.Tab))
            {
                SaveFileName.Select();
            }

            //Here we will check if you are currently trying to log in or currently trying to register.
            //Depending what you are trying to do, the enter(return) button will change what it does.
            //If you are currently trying to login and press enter(return) you will activate the AccountLogin function.
            if (Login.activeInHierarchy == true && Input.GetKeyDown(KeyCode.Return))
            {
                AccountLogin();
            }
            //If you are currently trying to register and press enter(return) you will activate the RegisterAccount function.
            else if (Register.activeInHierarchy == true && Input.GetKeyDown(KeyCode.Return))
            {
                RegisterAccount();
            }
            #endregion
        }

        //Use this function when you want to register your account.
        public void RegisterAccount()
        {
            //First it will check if the inputfields are empty, if they are empty it will immidiatly stop the function.
            if (registerUsername.text == "" || registerPassword.text == "" || SaveFileName.text == "")
            {
                RespondText.text = "<color=red>Username, password or save file name has not been filled in.</color>";
                StartCoroutine(RemoveText(3));
                return;
            }
            else
            {
                SearchNewSaveFiles();
                //checks if files exist in the current directory it's checking.
                if (saveInfo.Length > 0)
                {
                    //foreach file found it will run the code below.
                    foreach (FileInfo Fi in saveInfo)
                    {
                        //it will load the current file the foreach has taken.
                        json = File.ReadAllText(Fi.ToString());

                        //it will get the information saved in the file and put the information in the dictionary and a string.
                        DataInfo = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
                        //the dictionary
                        accountHolder = DataInfo.Accounts;
                        //the string
                        LoadFileName = DataInfo.saveFileName;

                        //checks if the save file name of the username used for the account already exists, if it does it will stop the loop.
                        if (accountHolder.ContainsKey(registerUsername.text) || LoadFileName == DataInfo.saveFileName)
                        {
                            RespondText.text = "<color=red> account already exists.</color>";
                            StartCoroutine(RemoveText(3));
                            return;
                        }
                        //if that isn't the case it will register the account and stop the loop.
                        else
                        {
                            accountHolder.Add(registerUsername.text, registerPassword.text);
                            RespondText.text = "<color=blue>Account has been registered.</color>";
                            Data.SaveData();
                            StartCoroutine(RemoveText(3));
                            return;
                        }
                    }
                    ResetValues();
                }
                //if there are no files in the directory it will run the code below.
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

        //Use this function when you want to login into an existing account.
        public void AccountLogin()
        {
            //First it will check if the inputfields are empty, if they are empty it will immidiatly stop the function.
            if (loginUsername.text == "" || loginPassword.text == "")
            {
                RespondText.text = "<color=red>You got to put something in the inputfield, otherwise it wont work.</color>";
                StartCoroutine(RemoveText(3));
                return;
            }
            else
            {
                SearchNewSaveFiles();
                //checks if files exist in the current directory it's checking.
                if (saveInfo.Length > 0)
                {
                    //foreach file found it will run the code below.
                    foreach (FileInfo Fi in saveInfo)
                    {
                        //it will load the current file the foreach has taken.
                        json = File.ReadAllText(Fi.ToString());

                        //it will get the information saved in the file and put the information in the dictionary and a string.
                        DataInfo = JsonConvert.DeserializeObject<SaveGameDataInfo>(json);
                        //the dictionary
                        accountHolder = DataInfo.Accounts;
                        //the string
                        LoadFileName = DataInfo.saveFileName;

                        //First it will check if the current information taken from the file equals the information from the inputfields.
                        //if it is true it will load the data from that account and end the loop.
                        if (accountHolder.ContainsKey(loginUsername.text) && accountHolder.ContainsValue(loginPassword.text))
                        {
                            RespondText.text = "<color=blue>Account information correct.</color>";
                            Data.LoadData(LoadFileName);
                            StartCoroutine(RemoveText(3));
                            Data.SaveUsername();
                            ResetValues();
                            return;
                        }
                        //if the information doesn't exist in the files it will give an error message that the password or username is incorrect.
                        else if (!accountHolder.ContainsKey(loginUsername.text) || !accountHolder.ContainsValue(loginPassword.text))
                        {
                            RespondText.text = "<color=red>Username or password is not correct.</color>";
                            StartCoroutine(RemoveText(3));
                        }
                        accountHolder.Clear();
                    }
                    ResetValues();
                }
                //if there are no files in the directory it will give an error that there are no files found.
                else
                {
                    RespondText.text = "<color=red>No save files found. Please register your account first.</color>";
                    ResetValues();
                    StartCoroutine(RemoveText(3));
                }
            }
        }

        //Checks if the user wants to save their username for the next time they open the application.
        //this is used every time the toggle changes.
        public void RememberUsernameToggle()
        {
            if (RememberUsernameBool)
            {
                RememberUsernameBool = false;
            }
            else if (!RememberUsernameBool)
            {
                RememberUsernameBool = true;
            }
            Data.SaveUsername();
        }

        //Changes when the toggle is used.
        //is used to check what password you put in.
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

        //clears the inputfields
        private void ResetValues()
        {
            if (!RememberUsernameBool)
            {
                loginUsername.text = "";
            }
            loginPassword.text = "";
            registerPassword.text = "";
            registerUsername.text = "";
            SaveFileName.text = "";
        }

        //Will check what files there are in the streamingassets folder.
        public void SearchNewSaveFiles()
        {
            //searches for the streamingassets folder.
            saveFile = new DirectoryInfo(Application.dataPath + "/StreamingAssets/");
            //looks for all the files in the streamingassets folder with the type .json
            //The * in the string means that it will check for files with 0 or more characters in the name.
            //You can also put a ? there, the ? will check if there is zero or one character in the name, if there are more it will skip those files.
            saveInfo = saveFile.GetFiles("*.json");
        }

        //these functions work to change the UI to be able to register an account or log into an existing account.
        #region UI toggle
        public void SwitchUIToLogin()
        {
            Login.SetActive(true);
            Register.SetActive(false);
            ShowPasswordBool = false;
            Showpassword.isOn = false;
            ShowPasswordToggle();
            RespondText.text = "";
        }

        public void SwitchUIToRegister()
        {
            Login.SetActive(false);
            Register.SetActive(true);
            RespondText.text = "";
        }
        #endregion

        //The IEnumerator is called to clear the text in the respond text UI part.
        //It will make sure that the text won't stand there for the rest of the time.
        public IEnumerator RemoveText(float t)
        {
            yield return new WaitForSeconds(t);
            RespondText.text = "";
        }
    }
}
