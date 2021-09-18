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
    [SerializeField] private TextMeshProUGUI _goalDisplay = null;
    [SerializeField] private int _energyGoal = 13;
    [SerializeField] private GameObject _winPanel = null;
    [SerializeField] private GameObject _pausePanel = null;
    [SerializeField] private GameObject _pauseText = null;
    [SerializeField] private GameObject _creditsPanel = null;

    private int _energy = 0;

    private void Awake()
    {
        _instance = this;

        if (!_energyDisplay)
            Debug.LogError(name + ": Energy TextMeshProUGUI not found!");

        if (!_creditsPanel)
            Debug.LogError(name + ": Credits panel object not found!");
    }

    private void Start()
    {
        if (!_goalDisplay)
            Debug.LogError(name + ": Goal TextMeshProUGUI not found!");

        UpdateDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Q) && _pausePanel.activeSelf)
        {
            Quit();
        }
    }

    public void TogglePause()
    {
        _creditsPanel.SetActive(false);
        _pauseText.SetActive(!_pauseText.activeSelf);
        _pausePanel.SetActive(!_pausePanel.activeSelf);

        if (_pausePanel.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
		Application.Quit();
        #endif
    }

    public void Credits()
    {
        _creditsPanel.SetActive(true);
    }

    private void UpdateDisplay()
    {
        _energyDisplay.text = "Energy: " + _energy;
        _goalDisplay.text = _energy + " / " + _energyGoal + " Energy\nrequired to open";
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

    public void CheckWin(Player player)
    {
        if (_energy >= _energyGoal)
        {
            _energyDisplay.enabled = false;
            _winPanel.SetActive(true);

            player.Dance();
        }
    }
}
