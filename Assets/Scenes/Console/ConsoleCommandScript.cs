using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace consoleCommand
{
    public class ConsoleCommandScript : MonoBehaviour
    {
        PlayerController playerController;

        public InputField inputFieldText;
        public Text consoleText;
        public GameObject console;

        string textCommand;

        float numberAmount;

        bool consoleActive = false;

        string[] tokens = new string[2];

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            textCommand = inputFieldText.text;

            if (Input.GetKeyDown(KeyCode.BackQuote) && !consoleActive)
            {
                console.SetActive(true);
                consoleActive = true;
            }
            else if (Input.GetKeyDown(KeyCode.BackQuote) && consoleActive)
            {
                console.SetActive(false);
                consoleActive = false;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                tokens = textCommand.Split(' ');
                CheckInputField();
            }
        }

        public void CheckInputField()
        {
            if(textCommand == "")
            {
            }
            else
            {
                numberAmount = float.Parse(tokens[1]);
            }
            #region Commands
            switch (tokens[0])
            {
                case ("help"):

                    break;
                case ("exp"):
                    consoleText.text = consoleText.text + tokens[0] + " " + tokens[1] + "\n";
                    playerController.Exp += numberAmount;
                    playerController.expSlider.value = playerController.expSliderCalculator();
                    ClearInputFields();
                    break;
                default:
                    consoleText.text = consoleText.text + "<color=red>There exists no such command. Try the command <b>Help</b> to see all the available commands.</color>" + "\n";
                    ClearInputFields();
                    break;
            }
            #endregion
        }

        void ClearInputFields()
        {      
            inputFieldText.text = "";
            numberAmount = 0;
            textCommand = "";
        }
    }
}
