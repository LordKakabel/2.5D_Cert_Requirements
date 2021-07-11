using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton Pattern
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UIManager not found!");
            return _instance;
        }
    }
    #endregion

    [SerializeField] private TextMeshProUGUI _energyDisplay = null;

    private int _energy = 0;

    private void Awake()
    {
        _instance = this;

        if (!_energyDisplay)
            Debug.LogError(name + ": Energy TextMeshProUGUI not found!");

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _energyDisplay.text = "Energy: " + _energy;
    }

    public void UpdateEnergy(int value)
    {
        _energy += value;
        UpdateDisplay();
    }

    public int GetEnergy()
    {
        return _energy;
    }
}
