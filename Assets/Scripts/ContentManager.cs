using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TigerForge;
using System.Collections;
using UnityEngine.EventSystems;

public class ContentManager : MonoBehaviour
{
    //Creats a prefab of the InputField which is instantiated when the user clicks on "ADD".
    public GameObject InputField;
    //Puts all instantiated InputFields under a list for easier sorting and calculations.
    public List<GameObject> ListOfInstantiatedInputFields = new List<GameObject>();
    //Calculates the number of instantiated InputFields.
    public int InputFieldCalculator =default;

    //This inputfield get hold of the current inputfield and put it in a list.
    public TMP_InputField FuelInput, ArrivalInput, DepartureInput, MovementInput, A_TInput, OperatorInput;

    //Create a list and add all the instantiated InputFields for the calculator to total them.
    public List<TMP_InputField> FuelTotalList, ArrivalTotalList, DepartureTotalList,
        MovementTotalList, A_TInputTotalList, OperatorInputTotalList;

    //The total number of fuel when calculated.
    public int TotalFuel, TotalArrival, TotalDeparture, TotalMovement;
    //The total fuel input field
    public TMP_Text FuelTotalTextField, ArrivalTotalTextField, DepartureTotalTextField, MovementTotalTextField;

    //This script calls some functions and data from the following scripts.
    public DateSystemScript dateSystemScript;
    public SaveAndLoad saveAndLoad;

    /*
     * THE INSTANCE OF EASYSAVEFILE VARIABLES.
     
    //This instance saves the input fields.
    EasyFileSave InputFieldsFile;
    //The list of fuel,aircraftType,operator,arrival,departure and movemnt input obtained to save.
    public List<int> SaveFuelList;
    public List<string> SaveAircraftTypeList;
    public List<string> SaveOperatorList;
    public List<int> SaveArrivalList;
    public List<int> SaveDepartureList;
    public List<int> SaveMovemntList;
    //Bool to check if the load button have been clicked to prevent it from loading twice.
    public bool checkLoad = default;
    //
    */
    public bool CheckLoadOfInputField = default;

    private void Start()
    {
        /*       
        checkLoad = true;
        CheckLoadOfInputField = false;
        InputFieldsFile = new EasyFileSave("input_fields_file");
        */
        //Getting the script of save and load and also datesysscript
        saveAndLoad = GameObject.FindWithTag("SaveAndLoad").GetComponent<SaveAndLoad>();
        dateSystemScript = GameObject.FindWithTag("DateSystem").GetComponent<DateSystemScript>();
    }

    //
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Delete))
        {

            // Delete this file.
            // This method clears stored data as well.

            //InputFieldsFile.Delete();

            Debug.Log(">> The file has been deleted." + "\n");
        }
    }


    /*A function used to instatiate new InputField and also calculate the number
    of InputFields to the InputFieldCalculator.*/
    public void AddNewInputField()
    {
        InputFieldCalculator++;
        GameObject newInputField = Instantiate(InputField, transform);

        //Caching the gameobjects, so that the it can be accessed by the text fields.
        GameObject Fuel, Arrival, Departure, Movement,A_T,Operator,RemoveButton;
        Fuel = newInputField.transform.GetChild(2).gameObject;
        Arrival = newInputField.transform.GetChild(3).gameObject;
        Departure = newInputField.transform.GetChild(4).gameObject;
        Movement = newInputField.transform.GetChild(5).gameObject;
        A_T = newInputField.transform.GetChild(0).gameObject;
        Operator = newInputField.transform.GetChild(1).gameObject;
        RemoveButton = newInputField.transform.GetChild(6).gameObject;

        //Get the text field of the various gameobjects.
        FuelInput = Fuel.GetComponent<TMP_InputField>();
        ArrivalInput = Arrival.GetComponent<TMP_InputField>();
        DepartureInput = Departure.GetComponent<TMP_InputField>();
        MovementInput = Movement.GetComponent<TMP_InputField>();
        A_TInput = A_T.GetComponent<TMP_InputField>();
        OperatorInput = Operator.GetComponent<TMP_InputField>();

        //Add them to the list.
        FuelTotalList.Add(FuelInput);
        ArrivalTotalList.Add(ArrivalInput);
        DepartureTotalList.Add(DepartureInput);
        MovementTotalList.Add(MovementInput);
        A_TInputTotalList.Add(A_TInput);
        OperatorInputTotalList.Add(OperatorInput);

        //Add the gameobject instance to ListOfInstantiatedInputFields;
        ListOfInstantiatedInputFields.Add(newInputField);

        //Changing the name of the gameobject instance to something specific.
        newInputField.name = "inputField " + InputFieldCalculator.ToString();
        //Change the name of the RemoveButton to the name of the inputField.
        RemoveButton.name = "inputField " + InputFieldCalculator.ToString();
    }

    public void RemoveSpecificInputField()
    {
        if (InputFieldCalculator != 0)
        {
            //Get's the current selected the name of the UI, thus the removeButton.
            string nameOfInputField = EventSystem.current.currentSelectedGameObject.name;
            //Search in the names in list of instantiated inputLists.
            foreach(GameObject inputField in ListOfInstantiatedInputFields)
            {
                if (inputField.name == nameOfInputField)
                {
                    //Getting the position of specific inputField to remove them from the lists.
                    int positionOfInputField = ListOfInstantiatedInputFields.IndexOf(inputField);
                    ListOfInstantiatedInputFields.RemoveAt(positionOfInputField);
                    FuelTotalList.RemoveAt(positionOfInputField);
                    ArrivalTotalList.RemoveAt(positionOfInputField);
                    DepartureTotalList.RemoveAt(positionOfInputField);
                    MovementTotalList.RemoveAt(positionOfInputField);
                    //Recalculating the decrementor and the total values of each field...
                    InputCalculatorDecrementor();
                    FuelCalculator();
                    ArrivalCalculator();
                    DepartureCalculator();
                    MovementCalculator();
                    A_TCalculator();
                    OperatorCalculator();
                    //Destroy the selected inputField.
                    Destroy(inputField);
                    return;
                }
            }
        }
    }

    //Remove the last InputField from the content viewport,and update the InputFieldCalculator.
    public void RemoveLastInputField()
    {
        if (InputFieldCalculator != 0)
        {
            //Caching the last InputField into a game object and later destroying it.
            GameObject unwantedInputFIeld = ListOfInstantiatedInputFields[ListOfInstantiatedInputFields.Count - 1];
            Destroy(unwantedInputFIeld);

            //Removing the last item from the ListOfInstantiatedInputFields.
            ListOfInstantiatedInputFields.RemoveAt(InputFieldCalculator - 1);

            //Removing the last item in the list of TOTALS.
            FuelTotalList.RemoveAt(FuelTotalList.Count - 1);
            ArrivalTotalList.RemoveAt(ArrivalTotalList.Count - 1);
            DepartureTotalList.RemoveAt(DepartureTotalList.Count - 1);
            MovementTotalList.RemoveAt(MovementTotalList.Count - 1);

            //Recalculating the decrementor and the total values of each field...
            InputCalculatorDecrementor();
            FuelCalculator();
            ArrivalCalculator();
            DepartureCalculator();
            MovementCalculator();
            A_TCalculator();
            OperatorCalculator();
        }
    }

    //InputCalculatorDecrementor, this decreases the ImputFieldsCalculator to avoid bugs.
    private void InputCalculatorDecrementor()
    {
        InputFieldCalculator--;
        //
        saveAndLoad.SaveOfFuelCalc--;
        saveAndLoad.SaveOfArrivalCalc--;
        saveAndLoad.SaveOfDepartureCalc--;
        saveAndLoad.SaveOfMovembtCalc--;
        saveAndLoad.SaveOfAircraftTypeCalc--;
        saveAndLoad.SaveOfOperatorCalc--;
    }

    //Calculating the total number of fuel.
    public void FuelCalculator()
    {
        //The int of items saved in the saveFuelSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.FuelSaveDates.Count; i++)
        {
            if(saveAndLoad.FuelSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.FuelSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //TotalFuel is always 0 when this is called.
        TotalFuel = 0;
        //
        saveAndLoad.SaveOfFuelCalc = 0;
        //Setting the value of totalInputfields to 0 everytime this is called. But we will add it in the foreach loop.
        saveAndLoad.TotalNumberOfInputField[dateSystemScript.SearchedCanvasName ] = 0;

        //Checking all available fuelFields to add them to the TotalFuel.
        foreach (TMP_InputField fuelTotal in FuelTotalList)
        {
            int highlightedFuel;
            highlightedFuel = int.Parse(fuelTotal.text);

            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.FuelSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfFuelCalc] = highlightedFuel;
            TotalFuel += highlightedFuel;
            saveAndLoad.SaveOfFuelCalc += 1;
            //Incrementing the value of totalInputField of a specific date everytime an input field is calculated.
            //This will let us know the total number of InputFields is on a specific date.
            saveAndLoad.TotalNumberOfInputField[dateSystemScript.SearchedCanvasName]++;
        }
        //Input the correct answer to the TotalFuelTextField.
        FuelTotalTextField.text = TotalFuel.ToString();
    }

    //Calculating the total number of arrival.
    public void ArrivalCalculator()
    {
        //The int of items saved in the ArrivalSaveSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.ArrivalSaveDates.Count; i++)
        {
            if (saveAndLoad.ArrivalSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.ArrivalSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //
        saveAndLoad.SaveOfArrivalCalc = 0;

        //TotalArrival is always 0 when this is called.
        TotalArrival = 0;
        //Checking all available arrivalFields to add them to the TotalArrival.
        foreach (TMP_InputField arrivalTotal in ArrivalTotalList)
        {
            int highlightedArrival;
            highlightedArrival = int.Parse(arrivalTotal.text);
            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.ArrivalSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfArrivalCalc] = highlightedArrival;
            saveAndLoad.SaveOfArrivalCalc += 1;

            TotalArrival += highlightedArrival;
        }
        //Input the correct answer to the TotalArrivalTextField.
        ArrivalTotalTextField.text = TotalArrival.ToString();
    }

    //Calculating the total number of departure.
    public void DepartureCalculator()
    {
        //The int of items saved in the ArrivalSaveSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.DepartureSaveDates.Count; i++)
        {
            if (saveAndLoad.DepartureSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.DepartureSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //
        saveAndLoad.SaveOfDepartureCalc = 0;


        //TotalDeparture is always 0 when this is called.
        TotalDeparture = 0;
        //Checking all available departureFields to add them to the TotalDeparture.
        foreach (TMP_InputField departureTotal in DepartureTotalList)
        {
            int highlightedDeparture;
            highlightedDeparture = int.Parse(departureTotal.text);
            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.DepartureSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfDepartureCalc] = highlightedDeparture;
            saveAndLoad.SaveOfDepartureCalc += 1;

            TotalDeparture += highlightedDeparture;
        }
        //Input the correct answer to the TotalDepartureTextField.
        DepartureTotalTextField.text = TotalDeparture.ToString();
    }

    //Calculating the total number of movement.
    public void MovementCalculator()
    { 
        //The int of items saved in the ArrivalSaveSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.MovementSaveDates.Count; i++)
        {
            if (saveAndLoad.MovementSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.MovementSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //
        saveAndLoad.SaveOfMovembtCalc = 0;


        //TotalMovement is always 0 when this is called.
        TotalMovement = 0;
        //Checking all available movementFields to add them to the TotalMovement.
        foreach (TMP_InputField movementTotal in MovementTotalList)
        {
            int highlightedMovement;
            highlightedMovement = int.Parse(movementTotal.text);
            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.MovementSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfMovembtCalc] = highlightedMovement;
            saveAndLoad.SaveOfMovembtCalc += 1;

            TotalMovement += highlightedMovement;
        }
        //Input the correct answer to the TotalMovementTextField.
        MovementTotalTextField.text = TotalMovement.ToString();
    }

    public void A_TCalculator()
    {
        //The int of items saved in the ArrivalSaveSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.AircraftTypeSaveDates.Count; i++)
        {
            if (saveAndLoad.AircraftTypeSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.AircraftTypeSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //
        saveAndLoad.SaveOfAircraftTypeCalc = 0;


        foreach (TMP_InputField a_tTotal in A_TInputTotalList)
        {
            string highlightedA_T;
            highlightedA_T = a_tTotal.text;
            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.AircraftTypeSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfAircraftTypeCalc] = highlightedA_T;
            saveAndLoad.SaveOfAircraftTypeCalc += 1;

        }
    }

    public void OperatorCalculator()
    {

        //The int of items saved in the ArrivalSaveSys is always cleared accordig to a specific key, when this is called,and later populated.
        //NB: This always happens on the current date/canvas, because of the key.
        for (int i = 0; i <= saveAndLoad.OperatorSaveDates.Count; i++)
        {
            if (saveAndLoad.OperatorSaveDates.ContainsKey(dateSystemScript.SearchedCanvasName + "_" + i))
            {
                saveAndLoad.OperatorSaveDates.Remove(dateSystemScript.SearchedCanvasName + "_" + i);
            }
        }

        //
        saveAndLoad.SaveOfOperatorCalc = 0;


        foreach (TMP_InputField operatorTotal in OperatorInputTotalList)
        {
            string highlightedOperator;
            highlightedOperator = operatorTotal.text;
            //Add all highlightedFuel(all fueltotal.text) to the list of fuelSaveDates to save.
            //SaveFuelList.Add(highlightedFuel);
            saveAndLoad.OperatorSaveDates[dateSystemScript.SearchedCanvasName + "_" + saveAndLoad.SaveOfOperatorCalc] = highlightedOperator;
            saveAndLoad.SaveOfOperatorCalc += 1;

        }
    }


    /////
    ///
    public void PopulateInputField()
    {
        InputFieldCalculator++;
        GameObject newInputField = Instantiate(InputField, transform);

        //Caching the gameobjects, so that the it can be accessed by the text fields.
        GameObject Fuel, Arrival, Departure, Movement, A_T, Operator;
        Fuel = newInputField.transform.GetChild(2).gameObject;
        Arrival = newInputField.transform.GetChild(3).gameObject;
        Departure = newInputField.transform.GetChild(4).gameObject;
        Movement = newInputField.transform.GetChild(5).gameObject;
        A_T = newInputField.transform.GetChild(0).gameObject;
        Operator = newInputField.transform.GetChild(1).gameObject;


        //Get the text field of the various gameobjects.
        FuelInput = Fuel.GetComponent<TMP_InputField>();
        ArrivalInput = Arrival.GetComponent<TMP_InputField>();
        DepartureInput = Departure.GetComponent<TMP_InputField>();
        MovementInput = Movement.GetComponent<TMP_InputField>();
        A_TInput = A_T.GetComponent<TMP_InputField>();
        OperatorInput = Operator.GetComponent<TMP_InputField>();

        //Add them to the list.
        FuelTotalList.Add(FuelInput);
        ArrivalTotalList.Add(ArrivalInput);
        DepartureTotalList.Add(DepartureInput);
        MovementTotalList.Add(MovementInput);
        A_TInputTotalList.Add(A_TInput);
        OperatorInputTotalList.Add(OperatorInput);

        //Add the gameobject instance to ListOfInstantiatedInputFields;
        ListOfInstantiatedInputFields.Add(newInputField);

        //Changing the name of the gameobject instance to something specific.
        newInputField.name = "inputField " + InputFieldCalculator.ToString();

        /*
        if (SaveFuelList.Count == FuelTotalList.Count && CheckLoadOfInputField == false)
        {
            PopulateInputField();
        }*/
    }


    /*
    //This function fills the input field after createing them on load.
    public void FillInputField()
    {
        //This function takes the total number of inputs on a particular date,
        //and increment it gradually starting from 0. What is happening here is that,
        //When every input field is finished loading we take the first fueltotallist in the list,
        //and we equate its textfield to the one stored in the FuelsaveDates sys script. We are using the TotalNumberOfINFI
        //as its key.
        for(int i = 0; i < saveAndLoad.TotalNumberOfInputField[dateSystemScript.currentCanvas.name]; i++)
        {
            FuelTotalList[i].text = saveAndLoad.FuelSaveDates[dateSystemScript.SearchedCanvasName + "_" + i].ToString();
        }
    }*/


    /*
    public void SaveLists()
    {
        InputFieldsFile.Add("FuelSave", SaveFuelList);
        InputFieldsFile.Save();
        Debug.Log("Saaaavveee");
    }

    public void LoadLists()
    {
        if (checkLoad == true)
        {
            if (InputFieldsFile.Load())
            {
                SaveFuelList = InputFieldsFile.GetList<int>("FuelSave");
                InputFieldsFile.Dispose();
                Debug.Log("LLLooooaadddd");
                LoadInputs();
                checkLoad = false;
            }
        }
    }

    void LoadInputs()
    {
        foreach(int i in SaveFuelList)
        {
            AddNewInputField();
        }
    }

    public void PopulateInputField()
    {
        int u;
        u = 0;
        Debug.Log("PopulateInputField");
        foreach(int i in SaveFuelList)
        {
            FuelTotalList[u].text = i.ToString();
            u++;
        }
        CheckLoadOfInputField = true;
    }
    */
}
