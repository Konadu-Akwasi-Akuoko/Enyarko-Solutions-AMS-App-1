using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;


public class SaveAndLoad : MonoBehaviour
{
    //This int will contain the values and to help 
    //iddentify better, thier corresponding keys
    //will be the dates created. For example "1_1_1"+SaveOfFuelCalc
    public Dictionary<string, int> FuelSaveDates = new Dictionary<string, int>();
    public Dictionary<string, int> ArrivalSaveDates = new Dictionary<string, int>();
    public Dictionary<string, int> DepartureSaveDates = new Dictionary<string, int>();
    public Dictionary<string, int> MovementSaveDates = new Dictionary<string, int>();
    public Dictionary<string, string> AircraftTypeSaveDates = new Dictionary<string, string>();
    public Dictionary<string, string> OperatorSaveDates = new Dictionary<string, string>();

    //The key which distinguish the various input fields, eg. 1_1_1_1, 1_1_1_2, 1_1_1_3, this value is the changing one, the key.
    public int SaveOfFuelCalc;
    public int SaveOfArrivalCalc;
    public int SaveOfDepartureCalc;
    public int SaveOfMovembtCalc;
    public int SaveOfAircraftTypeCalc;
    public int SaveOfOperatorCalc;

    //This dictionary contains all the total number of  input fields instance on a specific date.
    public Dictionary<string, int> TotalNumberOfInputField = new Dictionary<string, int>();

    //The instance of the date system.
    public DateSystemScript dateSysScript;

    //The name of the save instances of aircraftType,operator,fuel,departure,arrival and movement we will be using.
    EasyFileSave SaveInstaceFuel;
    EasyFileSave SaveInstanceAircraftType;
    EasyFileSave SaveInstanceOperator;
    EasyFileSave SaveInstanceDeparture;
    EasyFileSave SaveIsntanceArrival;
    EasyFileSave SaveInstanceMovement;

    void Start()
    {
        //Initiating a new EasyFileSave instance.
        SaveInstaceFuel = new EasyFileSave("my_save_fuel_int");
        SaveInstanceAircraftType = new EasyFileSave("my_save_aircrafttype_list");
        SaveInstanceOperator = new EasyFileSave("my_save_operator_list");
        SaveInstanceDeparture = new EasyFileSave("my_save_departure_list");
        SaveIsntanceArrival = new EasyFileSave("my_save_arrival_list");
        SaveInstanceMovement = new EasyFileSave("my_save_movement_list");
        
        dateSysScript = GameObject.FindWithTag("DateSystem").GetComponent<DateSystemScript>();


    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Delete))
        {
            // Delete this file.
            // This method clears stored data as well.
            SaveInstaceFuel.Delete();
            Debug.Log(">> The file has been deleted." + "\n");
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            Debug.Log("L is pressed");
            foreach (KeyValuePair<string, string> kvp in OperatorSaveDates)
            {
                Debug.LogFormat("Key ={0},Value={1}", kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<string, int> kvp in TotalNumberOfInputField)
            {
                Debug.LogFormat("Key={0},Value={1}", kvp.Key, kvp.Value);
            }
        }

    }

    //This function and its subsequent functions add the list of things to save to 
    //a dictionary with its given key as the date of the system.
    public void AddDateToFuelSaveSys()
    {
        SaveOfFuelCalc = 0;
    }

    public void AddDateToAircraftType()
    {
        SaveOfAircraftTypeCalc = 0;
    }
    public void AddDateToOperator()
    {
        SaveOfOperatorCalc = 0;
    }
    public void AddDateToDeparture()
    {
        SaveOfDepartureCalc = 0;
    }
    public void AddDateToArrival()
    {
        SaveOfArrivalCalc = 0;
    }
    public void AddDateToMovemnt()
    {
        SaveOfMovembtCalc = 0;
    }


    //Saving the fuel list.
    public void SaveFuelDictionry()
    {
        SaveInstaceFuel.Add("TotalNumberOfInputField", TotalNumberOfInputField);
        //
        SaveInstaceFuel.Add("FuelSavedDictInt", FuelSaveDates);
        SaveInstanceAircraftType.Add("AircraftTypeDictString", AircraftTypeSaveDates);
        SaveInstanceOperator.Add("OperatorDictString", OperatorSaveDates);
        SaveInstanceMovement.Add("MovementDictInt", MovementSaveDates);
        SaveInstanceDeparture.Add("DepartureDictInt", DepartureSaveDates);
        SaveIsntanceArrival.Add("ArrivalDictInt", ArrivalSaveDates);
        //
        SaveInstaceFuel.Save();
        SaveInstanceAircraftType.Save();
        SaveInstanceOperator.Save();
        SaveInstanceDeparture.Save();
        SaveIsntanceArrival.Save();
        SaveInstanceMovement.Save();
        
    }

    //Loading the data in SaveInstaceFuel
    public void LoadFuelDictionary()
    {
        if (SaveInstaceFuel.Load())
        {
            FuelSaveDates = SaveInstaceFuel.GetDictionary<string, int>("FuelSavedDictInt");
            TotalNumberOfInputField = SaveInstaceFuel.GetDictionary<string, int>("TotalNumberOfInputField");
            SaveInstaceFuel.Dispose();
            Debug.Log("Succesfully loaded");
        }

        if (SaveInstanceAircraftType.Load())
        {
            AircraftTypeSaveDates = SaveInstanceAircraftType.GetDictionary<string, string>("AircraftTypeDictString");
            SaveInstanceAircraftType.Dispose();
        }

        if (SaveInstanceOperator.Load())
        {
            OperatorSaveDates = SaveInstanceOperator.GetDictionary<string, string>("OperatorDictString");
            SaveInstanceOperator.Dispose();
        }

        if (SaveInstanceDeparture.Load())
        {
            DepartureSaveDates = SaveInstanceDeparture.GetDictionary<string, int>("DepartureDictInt");
            SaveInstanceDeparture.Dispose();
        }

        if (SaveIsntanceArrival.Load())
        {
            ArrivalSaveDates = SaveIsntanceArrival.GetDictionary<string, int>("ArrivalDictInt");
            SaveIsntanceArrival.Dispose();
        }

        if (SaveInstanceMovement.Load())
        {
            MovementSaveDates = SaveInstanceMovement.GetDictionary<string, int>("MovementDictInt");
            SaveInstanceMovement.Dispose();
        }
    }

    public void Quit()
    {
        StartCoroutine(QuitInstnce());
    }

    IEnumerator QuitInstnce()
    {
        yield return new WaitForSeconds(3);
        Application.Quit();
    }
}