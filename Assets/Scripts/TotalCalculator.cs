using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalCalculator : MonoBehaviour
{
    public ContentManager contentManager;

    private void Start()
    {
        contentManager = GameObject.FindWithTag("ContentManager").GetComponent<ContentManager>();
        if (!contentManager)
        {
            Debug.LogError("Couldn't find the contentmanager.");
        }
    }

    public void CalculateFuel()
    {
        contentManager.FuelCalculator();
    }

    public void CalculateArrival()
    {
        contentManager.ArrivalCalculator();
    }

    public void CalculateDeparture()
    {
        contentManager.DepartureCalculator();
    }

    public void CalculateMovement()
    {
        contentManager.MovementCalculator();
    }
    public void CalculateAircraftType()
    {
        contentManager.A_TCalculator();
    }
    public void CalculateOperator()
    {
        contentManager.OperatorCalculator();
    }

    public void RemoveInput()
    {
        contentManager.RemoveSpecificInputField();
    }
}
