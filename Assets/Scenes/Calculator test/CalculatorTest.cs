using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorTest : MonoBehaviour
{
    [SerializeField]
    InputField Calculator;

    string calculation;

    [SerializeField]
    string[] Tokens;

    [SerializeField]
    float ToDoCalculation;

    char[] allowedcharacters = new char[4];

    void Start()
    {
        allowedcharacters[0] = '-';
        allowedcharacters[1] = '+';
        allowedcharacters[2] = '/';
        allowedcharacters[3] = '*';
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            calculation = Calculator.text;
            Calculate(calculation);
        }
    }

    public void Calculate(string calculation)
    {
        Tokens = calculation.Split(allowedcharacters);

        foreach(string t in Tokens)
        {
            ToDoCalculation = float.Parse(t);
        }
    }
}
