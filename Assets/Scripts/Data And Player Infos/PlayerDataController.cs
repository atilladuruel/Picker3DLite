using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public static int _ongoingGameHighScore;
    private PlayerData _playerData = new PlayerData();

    private void Start()
    {

    }

    private void Update()
    {
        if (_ongoingGameHighScore > _playerData.GetHighScore())
        {
            _playerData.SetHighScore(_ongoingGameHighScore);
        }
    }


    public int GetHighScore()
    {
        return _playerData.GetHighScore();
    }

}

public class PlayerData
{
    public void SetPlayerID(int value) { PlayerPrefs.SetInt("Player ID", value); }
    public int GetPlayerID() { return PlayerPrefs.GetInt("Player ID"); }

    public void SetHighScore(int value) { PlayerPrefs.SetInt("Player High Score", value); }
    public int GetHighScore() { return PlayerPrefs.GetInt("Player High Score"); }

    public void SetSelectedPickerOrder(int value) { PlayerPrefs.SetInt("Selected Picker Order", value); }
    public int GetSelectedPickerOrder() { return PlayerPrefs.GetInt("Selected Picker Order"); }

}