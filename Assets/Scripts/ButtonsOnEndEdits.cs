using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsOnEndEdits : MonoBehaviour
{
    //
    public DateSystemScript dateSystemScript;

    // Start is called before the first frame update
    void Start()
    {
        //Finding the date system for the Add icon and Goto icon everytime a new canvas is instantiated. 
        dateSystemScript = GameObject.FindWithTag("DateSystem").GetComponent<DateSystemScript>();
        if (dateSystemScript == null)
        {
            Debug.LogError("Canvas script not found");
        }
    }


    //OnEndEdits calls for Search,AddDate and GOto icons.
    public void OnEndEditSearch()
    {
        dateSystemScript.SearchForDate();
    }

    public void AddDate()
    {
        dateSystemScript.AddDateFunctiion();
    }

    public void GoTo()
    {
        dateSystemScript.GoToDate();
    }

}
