using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int _energyGoal = 13;
    [SerializeField] private GameObject _winPanel = null;
    [SerializeField] private GameObject _pausePanel = null;
    [SerializeField] private GameObject _pauseText = null;
    [SerializeField] private GameObject _creditsPanel = null;
    [SerializeField] private int _startingSceneIndex = 0;
    [SerializeField] private int _firstLevelSceneIndex = 1;
    [SerializeField] private GameObject _titlePanel = null;

    private int _energy = 0;
    private TextMeshProUGUI _goalDisplay;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (!_energyDisplay)
            Debug.LogError(name + ": Energy TextMeshProUGUI not found!");

        if (!_creditsPanel)
            Debug.LogError(name + ": Credits panel object not found!");

        if (!_titlePanel)
            Debug.LogError(name + ": Title panel object not found!");
    }

    private void Start()
    {
        _energyDisplay.text = "";
    }

    public void GetGoalDisplay(TextMeshProUGUI goalTMP)
    {
        _goalDisplay = goalTMP;
        UpdateDisplay();
    }

    private void Update()
    {
        if (!_titlePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && (_pausePanel.activeSelf || _creditsPanel.activeSelf || _titlePanel.activeSelf || _winPanel.activeSelf))
        {
            Quit();
        }

        if (Input.GetKeyDown(KeyCode.C) && (_pausePanel.activeSelf || _winPanel.activeSelf))
        {
            Credits();
        }

        if (Input.GetKeyDown(KeyCode.P) && _winPanel.activeSelf)
        {
            ReloadGame();
        }
    }

    public void TogglePause()
    {
        if (_winPanel.activeSelf)
        {
            _creditsPanel.SetActive(false);
        }
        else
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

    public void ReloadGame()
    {
        _winPanel.SetActive(false);
        SceneManager.LoadScene(_startingSceneIndex);
    }

    public void StartGame()
    {
        _titlePanel.SetActive(false);
        _pauseText.SetActive(true);

        SceneManager.LoadScene(_firstLevelSceneIndex);
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
            _goalDisplay.enabled = false;
            _energyDisplay.enabled = false;
            _winPanel.SetActive(true);

            player.Dance();
        }
    }
}
