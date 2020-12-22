using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddField : MonoBehaviour
{
    public GameObject InputField;
    public int InputFieldNumber =default;

    public void AddNewInputField()
    {
        InputFieldNumber++;
        GameObject newInputField = Instantiate(InputField, transform);
        newInputField.name = "Input Field " + InputFieldNumber.ToString();
    }

}
