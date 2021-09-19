using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goalDisplay = null;

    private void Start()
    {
        if (!_goalDisplay)
            Debug.LogError(name + ": Goal Display TMP missing!");

        UIManager.Instance.GetGoalDisplay(_goalDisplay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                UIManager.Instance.CheckWin(player);
            }
        }
    }
}
