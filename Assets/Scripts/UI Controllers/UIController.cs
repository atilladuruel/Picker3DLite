using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _pickerGameObject;
    [SerializeField] private List<Sprite> _pickerSprite;
    [SerializeField] private GameObject _pickerSelectUIImage;
    [SerializeField] private PlayerData _playerData = new PlayerData();
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    public static bool _menuActive = false;

    private void Start()
    {
        _hudPanel.SetActive(true);
        _menuPanel.SetActive(false);
        ChangePickerSelectImage(_playerData.GetSelectedPickerOrder());
    }

    private void Update()
    {
        if (_highScoreText != null)
            _highScoreText.text = "High Score :\n" + _pickerGameObject.GetComponent<PlayerDataController>().GetHighScore();
        if (_currentScoreText != null)
            _currentScoreText.text = PlayerDataController._ongoingGameHighScore.ToString();
    }

    public void Pause()
    {
        Debug.Log("Game Pause");
        Time.timeScale = 0f;
        _hudPanel.SetActive(false);
        _menuPanel.SetActive(true);
        _menuActive = true;

        if (_pickerGameObject.GetComponent<PickerController>() != null)
        {
            _pickerGameObject.GetComponent<PickerController>().PauseGame();
        }
    }


    public void Resume()
    {
        Time.timeScale = 1f;
        _hudPanel.SetActive(true);
        _menuPanel.SetActive(false);
        _menuActive = false;

        if (_pickerGameObject.GetComponent<PickerController>() != null)
        {
            _pickerGameObject.GetComponent<PickerController>().ResumeGame();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        _hudPanel.SetActive(true);
        _menuPanel.SetActive(false);
        _menuActive = false;
        PlayerDataController._ongoingGameHighScore = 1;

        if (_pickerGameObject.GetComponent<PickerController>() != null)
        {
            _pickerGameObject.GetComponent<PickerController>().RestartGame();
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

    public void UpArrowPickerChange()
    {
        int temp = _playerData.GetSelectedPickerOrder() - 1;

        if (_playerData.GetSelectedPickerOrder() > 0 && _playerData.GetSelectedPickerOrder() < _pickerGameObject.GetComponent<PickerController>().GetPickerSpecs().Count)
        {

            _playerData.SetSelectedPickerOrder(temp);
            ChangePickerSelectImage(_playerData.GetSelectedPickerOrder());
            _pickerGameObject.GetComponent<PickerController>().AssignPickerSpecs(_playerData.GetSelectedPickerOrder());
        }
    }

    public void DownArrowPickerChange()
    {
        int temp = _playerData.GetSelectedPickerOrder() + 1;

        if (_playerData.GetSelectedPickerOrder() >= 0 && _playerData.GetSelectedPickerOrder() < _pickerGameObject.GetComponent<PickerController>().GetPickerSpecs().Count - 1)
        {

            _playerData.SetSelectedPickerOrder(temp);
            ChangePickerSelectImage(_playerData.GetSelectedPickerOrder());
            _pickerGameObject.GetComponent<PickerController>().AssignPickerSpecs(_playerData.GetSelectedPickerOrder());
        }
    }

    public void ChangePickerSelectImage(int pickerNumber)
    {
        if (pickerNumber >= 0 && pickerNumber < _pickerSprite.Count)
            _pickerSelectUIImage.GetComponent<Image>().sprite = _pickerSprite[pickerNumber];
        else
            _pickerSelectUIImage.GetComponent<Image>().sprite = _pickerSprite[0];
    }

}
