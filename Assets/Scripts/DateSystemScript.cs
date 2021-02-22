using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TigerForge;

public class DateSystemScript : MonoBehaviour
{
    //Accessing the input field of the date,year and month to get hold of everyinput typed.
    public TMP_InputField DayDateInput, YearDateInput, MonthDateInput;

    //List of Gameobjects DateCanvas to search through if a date is been added.
    public List<GameObject> CreatedDateCanvas;

    //Getting refrence to the calenderIcon, GotoDate button and the AddDate button.
    public GameObject AddDate, Go_ToDate, CalenderIcon;

    //The name of the canvas we are serching for in CreatedDateCanvas.
    public string SearchedCanvasName;
    //The current date displayed at the bottom, it is displayed with the help of searchedCanvasName.
    public TMP_Text DisplayDate;

    //The index/the location of the searched calenderDate in the list CreatedDateCanvas.
    public int SearchedIndex;

    //A NullCanvas is a canvas yet to be named.
    public GameObject NullCanvas;

    //The present displaying canvas.
    public GameObject currentCanvas;

    //This helps us to get the searched date and carrry it on to the next instantiated canvas to display.
    public string DayDate, MonthDate, YearDate;

    //Using easysavefile to create a saving file which can be saved.
    private EasyFileSave dateSystemFile;
    //We must store the names of the createdCanvases.
    public List<string> createdDateCanvasNames;

    //The instance of the save system that will help us name the lists.
    public SaveAndLoad saveAndLoad;
    public ContentManager contentManamger;

    // Start is called before the first frame update
    void Start()
    {
        //Every time this is started, we must search for all dependencies.
        SearchfForDependencies();

        //To make the createdDateCanvas searchable list this needed to be added to make it searcable.
        CreatedDateCanvas.Add(this.gameObject);
        //Add this gameobject to the names at startup.
        createdDateCanvasNames.Add(this.gameObject.name);
        
        //Creating a new instance of the save system to store createdCanvas names, also supressing warning.
        dateSystemFile = new EasyFileSave("SaveNamesOfCanvas");
        dateSystemFile.suppressWarning = false;
        //dateSystemFile.Delete();

        //Getting the code for save and load system.
        saveAndLoad = GameObject.FindWithTag("SaveAndLoad").GetComponent<SaveAndLoad>();
       //contentManamger = GameObject.FindWithTag("ContentManger").GetComponent<ContentManager>();
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Delete))
        {

            // Delete this file.
            // This method clears stored data as well.

            dateSystemFile.Delete();

            Debug.Log(">> The file has been deleted." + "\n");
        }
    }

    public void SearchForDate()
    {
        /*
         * Creating this, so that the DateSystem can output it's values to string for the function to search,
         * so basically everytime this function is called it puts the values of the DateSystem into a string
         * and later search for it.*/

        SearchedCanvasName = DayDateInput.text + "_" + MonthDateInput.text + "_" + YearDateInput.text;
        //Grabbing the text field of the searched dates so it can be displayed on the next canvas.
        DayDate = DayDateInput.text;
        MonthDate = MonthDateInput.text;
        YearDate = YearDateInput.text;

        /*Searching through the given CreatedDateCanvas strings to see if something matches the current 
        values of the date system thus "SearchedCanvasName".*/
        for (int i = 0; i < CreatedDateCanvas.Count; i++)
        {
            //When this function is called the callender icon needs to be disabled.
            CalenderIcon.SetActive(false);

            //Going through all available created scenes to search for a coresponding canvas with the name.
            if (CreatedDateCanvas[i].name.ToString() == SearchedCanvasName)
            {
                //If found show button GO-TO date.
                Go_ToDate.SetActive(true);
                AddDate.SetActive(false);
                SearchedIndex = i;
                break;
            }

            //Else show button AddDate.
            else
            {
                AddDate.SetActive(true);
                Go_ToDate.SetActive(false);
            }
        }
    }

    /*Instantiate a NullCanvas, add it to the list of createdDateCanvas, change the name to 
     *match the searchedCanvas name,turn on and turn on the AddButton and CalenderIcon respectively,
     *as it is in it's natural form(null canvas), the search dependencies will take care of switching it on and off,
     *lastly turn off this gameobject.*/
    public void AddDateFunctiion()
    {
        //Before a new canvas is instantiated, it will be better to turn on the add icon and the goto icon
        //and switch back on the calender icon.
        AddDate.SetActive(true);
        Go_ToDate.SetActive(true);
        CalenderIcon.SetActive(true);

        GameObject InstaceOfCanvas = Instantiate(NullCanvas);
        CreatedDateCanvas.Add(InstaceOfCanvas);
        currentCanvas.SetActive(false);

        //Searching for dependencies, like text and so on, on the new canvas
        SearchfForDependencies();

        //Everytime this is instantiated, it will be the current diaplaying canvas.
        currentCanvas = InstaceOfCanvas;
        InstaceOfCanvas.name = SearchedCanvasName;
        createdDateCanvasNames.Add(SearchedCanvasName);

        //Call this function to create a new int key to store data of the 
        //inputfields with the corresponding date as a key and this int as a supplement.
        saveAndLoad.AddDateToFuelSaveSys();
        saveAndLoad.AddDateToArrival();
        saveAndLoad.AddDateToDeparture();
        saveAndLoad.AddDateToMovemnt();
        saveAndLoad.AddDateToAircraftType();
        saveAndLoad.AddDateToOperator();
        //We are also creating an instance of a dictionary which will contain the total number of inputfield on a specific date.
        saveAndLoad.TotalNumberOfInputField.Add(currentCanvas.name, 0);
    }

    /*Turn the searched createdDateCanvas GameObject's on, GotoButton and CallenderIcon 
     *are turned on and on respectively, lastly turn off this gameobject.*/
    public void GoToDate()
    {
        if (SearchedCanvasName != currentCanvas.name)
        {
            //Instantiating a new canvas based on the index of the searched item in the list.
            CreatedDateCanvas[SearchedIndex].SetActive(true);

            //Before a new canvas is instantiated, it will be better to turn on the goto icon and add icon
            //and switch back on the calender icon.
            Go_ToDate.SetActive(true);
            AddDate.SetActive(true);
            CalenderIcon.SetActive(true);
            //Set current canvas.
            currentCanvas.SetActive(false);

            //Will need to search for dependencies again for the canvas.
            SearchfForDependencies();

            //Changing the current canvas.
            currentCanvas = CreatedDateCanvas[SearchedIndex];

            //Checking to see if the number of inputfield is not equal to the TotalNumberofInputFields
            //The input fields will be 0 when it starts afresh or loads, if so then check if the ListOfTotalInputFields
            //is equalto TotalNumberOfinputFields.
            if (contentManamger.ListOfInstantiatedInputFields.Count != saveAndLoad.TotalNumberOfInputField[SearchedCanvasName])
            {
                for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[SearchedCanvasName]; i++)
                {
                    contentManamger.PopulateInputField();
                }
                //Checking if it has finished creating the input fields, if so then fill the InputField with value.
                if(contentManamger.ListOfInstantiatedInputFields.Count == saveAndLoad.TotalNumberOfInputField[SearchedCanvasName])
                {
                    FillInputField();
                }
            }

        }
        else
        {
            CalenderIcon.SetActive(true);
            Go_ToDate.SetActive(false);
        }

    }

    //This function search for all the dependencies needed.
    public void SearchfForDependencies()
    {
        if (SearchedCanvasName != currentCanvas.name)
        {
            //Getting hold of the values of all parts of the DateInput including day,month and year.
            YearDateInput = GameObject.FindWithTag("YearInputField").GetComponent<TMP_InputField>();
            MonthDateInput = GameObject.FindWithTag("MonthInputField").GetComponent<TMP_InputField>();
            DayDateInput = GameObject.FindWithTag("DayInputField").GetComponent<TMP_InputField>();
            //Null checking if all was found.
            if (YearDateInput == null || MonthDateInput == null || DayDateInput == null)
            {
                Debug.LogError("Couldn't find one of the date inputs.");
            }

            //Naming the date system according to the searched date.
            DayDateInput.text = DayDate;
            MonthDateInput.text = MonthDate;
            YearDateInput.text = YearDate;

            //Displaying the current date at the bottom.
            DisplayDate.text = "Date: " + DayDate + "/" + MonthDate + "/" + YearDate;

            //Finding icons when a new canvas is instantiated or an old one is turned on.
            Go_ToDate = GameObject.FindWithTag("GoToDate");
            AddDate = GameObject.FindWithTag("AddDate");
            CalenderIcon = GameObject.FindWithTag("CalendarIcon");
            if (Go_ToDate != null || AddDate != null)
            {
                Go_ToDate.SetActive(false);
                AddDate.SetActive(false);
            }
            //Searching for the contentManager to check if we need to populate inputfield.
            contentManamger = GameObject.FindWithTag("ContentManager").GetComponent<ContentManager>();
            if (!contentManamger)
            {
                Debug.LogError("ContentManager not found.");
            }

        }

    }

    /// <summary>
    /// These are the save and load functions of the date system, what it basically does is that
    /// when the save button is clicked the names of the CreatedDateCanvas are stored in a list called
    /// CreatedCanvasNames. When it is time to load the load system check if the value of CreatedCanvas is the 
    /// same as CreatedCanvasNames, if it is not the same, proceed to instantiate null canvases  and name
    /// it according to the CreatedCanvasNames.
    /// </summary>

    public void SaveSystem()
    {
        //Adding the instantiated canvas names to the file system.
        dateSystemFile.Add("created_date_canvas", createdDateCanvasNames);
        //Saving the file.
        dateSystemFile.Save();
        Debug.Log(">> Data saved in: " + dateSystemFile.GetFileName() + "\n");
        Debug.Log("It have been saved");
    }

    public void LoadSystem()
    {
        //Check if dateSystem exits.
        if (dateSystemFile.Load())
        {
            createdDateCanvasNames = dateSystemFile.GetList<string>("created_date_canvas");
            dateSystemFile.Dispose();

            //As declared at start, this gameobject is the first element of CreatedDateCanvas,so in 
            //oder to avoid miscommunication when the i=0 thus with the name DateSystem, dont create an 
            //instance and name it datesystem, because we will be having one add itself at startup.
            if (CreatedDateCanvas.Count != createdDateCanvasNames.Count)
            {
                for (int i = 0; i < createdDateCanvasNames.Count; i++)
                {
                    if (i > 0)
                    {
                        //Instantiate a prefab and name it according to the name in createdCanvasName.
                        //Later add them to the CreatedDateCanvas.
                        GameObject InstanceOfSave = Instantiate(NullCanvas);
                        CreatedDateCanvas.Add(InstanceOfSave);
                        InstanceOfSave.SetActive(false);
                        InstanceOfSave.name = createdDateCanvasNames[i];

                        /*
                        //Populating input fields
                        if (CreatedDateCanvas[i].name == createdDateCanvasNames[i])
                        {
                            for (int u = 0; u > saveAndLoad.FuelSaveDates.Count; u++)
                            {

                            }
                        }

                        */
                    }
                }
            }

        }
        else
        {
            Debug.LogError("Null");
        }
    }

    public void FillInputField()
    {
        //This function takes the total number of inputs on a particular date,
        //and increment it gradually starting from 0. What is happening here is that,
        //When every input field is finished loading we take the first fueltotallist in the list,
        //and we equate its textfield to the one stored in the FuelsaveDates sys script. We are using the TotalNumberOfINFI
        //as its key.
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.FuelTotalList[i].text = saveAndLoad.FuelSaveDates[SearchedCanvasName + "_" + i].ToString();
        }
        //
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.ArrivalTotalList[i].text = saveAndLoad.ArrivalSaveDates[SearchedCanvasName + "_" + i].ToString();
        }
        //
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.DepartureTotalList[i].text = saveAndLoad.DepartureSaveDates[SearchedCanvasName + "_" + i].ToString();
        }
        //
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.MovementTotalList[i].text = saveAndLoad.MovementSaveDates[SearchedCanvasName + "_" + i].ToString();
        }
        //
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.A_TInputTotalList[i].text = saveAndLoad.AircraftTypeSaveDates[SearchedCanvasName + "_" + i].ToString();
        }
        //
        for (int i = 0; i < saveAndLoad.TotalNumberOfInputField[currentCanvas.name]; i++)
        {
            contentManamger.OperatorInputTotalList[i].text = saveAndLoad.OperatorSaveDates[SearchedCanvasName + "_" + i].ToString();
        }

        /*
        contentManamger.FuelCalculator();
        contentManamger.ArrivalCalculator();
        contentManamger.DepartureCalculator();
        contentManamger.MovementCalculator();
        */
    }

    public void RemoveInput()
    {
        contentManamger.RemoveSpecificInputField();
    }

}
