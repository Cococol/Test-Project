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
        public GameObject Register;

        [Header("Overig")]
        public InputField SaveFileName;
        public Toggle RememberUsername;
        public Text RespondText;
        public bool Remember = false;
        #endregion
        string json;

        public Dictionary<string, string> accountHolder = new Dictionary<string, string>();

        public SaveGameData Data;

        SaveGameDataInfo DataInfo = new SaveGameDataInfo();

        private void Start()
        {
            Data = FindObjectOfType<SaveGameData>();
            Data.LoadStartingData();
        }

        public void RegisterAccount()
        {
            if (registerUsername.text == "" || registerPassword.text == "")
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
                ResetValues();
                StartCoroutine(RemoveText(3));
            }
        }

        public void AccountLogin()
        {
            if (loginUsername.text == "" || loginPassword.text == "")
            {
                RespondText.text = "<color=red>You got to put something in the inputfield, otherwise it wont work.</color>";
                ResetValues();
                StartCoroutine(RemoveText(3));
            }
            else if (accountHolder.ContainsKey(loginUsername.text) && accountHolder.ContainsValue(loginPassword.text))
            {
                RespondText.text = "<color=blue>Account information correct.</color>";
                ResetValues();
                StartCoroutine(RemoveText(3));
            }
            else if (!accountHolder.ContainsKey(loginUsername.text) || !accountHolder.ContainsValue(loginPassword.text))
            {
                RespondText.text = "<color=red>Username or password is not correct.</color>";
                ResetValues();
                StartCoroutine(RemoveText(3));
            }
        }

        public void SaveTest()
        {
            Data.SaveData();
        }

        public void LoadTest()
        {
            Data.LoadData();
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

            float f = 0;

            f += Time.deltaTime;
            if (f > 1f)
            {
                RespondText.text = "";
                f = 0;
            }

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
