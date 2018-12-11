using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace consoleCommand
{
    public class ConsoleCommandScript : MonoBehaviour
    {
        #region Objects for console
        //inputfield where you can use commands
        public InputField inputFieldText;
        //text field which saves the commands you've used
        public Text consoleText { get; set; }
        //gameobject to active the console
        public GameObject console;
        #endregion

        #region needed variables
        //string which saves the current console command
        string textCommand;
        //float which will check for the number amount from the command
        float numberAmount;
        //array of strings which will save the parts of the commands
        //more explained at the split part
        string[] tokens = new string[2];
        PlayerController playerController;
        #endregion

        private void Start()
        {
            //makes sure the current gameobject stays alive through all scenes
            DontDestroyOnLoad(gameObject);
            playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            #region set console active
            //checks if the console is active yes or no, and depending on that will set it to active or false
            if (Input.GetKeyDown(KeyCode.BackQuote) && !console.activeInHierarchy)
            {
                ClearInputFields();
                console.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.BackQuote) && console.activeInHierarchy)
            {
                ClearInputFields();
                console.SetActive(false);
            }
            #endregion

            #region check command
            //the moment we press enter we will take the command that was given, split the command name and the value given.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //first we will save the string we got from our inputfield in an string variable so we can use it later on.
                textCommand = inputFieldText.text;

                /* now we will take the string we just saved from our inputfield and split it in 2 strings.
                the code splits the string the moment a space has been recognized and saves it in an array of strings.
                the array will only take 2 strings in it and will see the first string as the command and the second as the value. */
                tokens = textCommand.Split(' ');

                //will do the check to see if the command exist.
                CheckInputField();
            }
            #endregion
        }

        //checks for existing commands.
        public void CheckInputField()
        {
            //first it will do a check to see if there was anything put into the inputfield, if not it will stop the function and give an error.
            if (textCommand == "")
            {
                consoleText.text = consoleText.text + "<color=red>There exists no such command. Try the command <b>help</b> to see all the available commands.</color>" + "\n";
                return;
            }
            else
            {
                #region Commands
                //the switch will check the first token in the array, which should be the command
                //it will check if the string of the token is the same as the string to start an command
                switch (tokens[0])
                {
                    //should give all viable commands at the moment.
                    case ("help"):
                        ClearInputFields();
                        break;
                    //gives exp to the character
                    case ("exp"):
                        //it will take the value of the string and put it to a float.
                        numberAmount = float.Parse(tokens[1]);
                        //puts the command you used into the text box to see what commands you used in the past play session.
                        consoleText.text = consoleText.text + tokens[0] + " " + tokens[1] + "\n";
                        playerController.Exp += numberAmount;
                        playerController.expSlider.value = playerController.expSliderCalculator();
                        ClearInputFields();
                        break;
                    default:
                        consoleText.text = consoleText.text + "<color=red>There exists no such command. Try the command <b>help</b> to see all the available commands.</color>" + "\n";
                        ClearInputFields();
                        break;
                }
                #endregion
            }
        }

        //clears the inputfield and all variables used so it doesn't interfere with the next commands.
        void ClearInputFields()
        {     
            inputFieldText.text = "";
            numberAmount = 0;
            textCommand = "";
        }
    }
}
