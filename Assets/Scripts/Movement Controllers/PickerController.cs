using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class PickerController : MonoBehaviour
{
    //This class manage all picker specification and behaviors.
    [SerializeField] private List<PickerSpecs> _pickerSpecs;
    [SerializeField] private GameObject _leftProp;
    [SerializeField] private GameObject _rightProp;
    [SerializeField] private TextMeshProUGUI startInformText;
    [SerializeField] private GameObject _restartPointGameObject;
    [SerializeField] private PlayerData _playerData = new PlayerData();
    [SerializeField] private List<GameObject> _pathGameObjects = new List<GameObject>();

    private float _moveDuration = 3f;
    private float _pickerSpeed = 15;
    private float _boundaries = 9;
    private float _propBoundaries = 7;
    private bool _gameStarted = false;
    private bool _sectionStarted = true;
    private bool _gamePaused = false;



    private void Start()
    {
        if (_pickerSpecs != null)
        {
            //Assigning picker specs at start function.
            AssignPickerSpecs(_playerData.GetSelectedPickerOrder());
        }

    }

    public void AssignPickerSpecs(int pickerSpecsNumber)
    {
        //Assigning picker specs


        if (DOTween.IsTweening(transform))
        {
            DOTween.Kill(transform);
            _gameStarted = false;
        }

        int PSN = (pickerSpecsNumber >= 0 && pickerSpecsNumber < _pickerSpecs.Count) ? pickerSpecsNumber : 0;


        _pickerSpeed = _pickerSpecs[PSN].GetPickerSpeed();
        transform.localScale = _pickerSpecs[PSN].GetPickerSize();
        for (int i = 0; i < 5; i++)
            transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = _pickerSpecs[PSN].GetPickerColor();


        _playerData.SetSelectedPickerOrder(PSN);

    }

    private void OnMouseDrag()
    {
        if (!UIController._menuActive)
        {
            if (!_gameStarted)
            {
                //This if trigger when player swipe the object.(It compile once.)
                if (!DOTween.IsTweening(transform))
                {
                    transform.DOMoveX(transform.position.x + _pickerSpeed, _moveDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                }
                else
                {
                    DOTween.Play(transform);
                }

                if (PlayerDataController._ongoingGameHighScore == 0)
                    PlayerDataController._ongoingGameHighScore = 1;

                _gameStarted = true;
                startInformText.gameObject.SetActive(false);
            }


            if (_sectionStarted && !_gamePaused)
            {
                //This if section drag the picker object and keep it in boundaries.
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                float u = -ray.origin.y / ray.direction.y;
                float boundaries = transform.GetChild(5).gameObject.activeSelf ? _propBoundaries : _boundaries;

                Debug.DrawRay(ray.origin, ray.direction * 20);

                if (ray.direction.z * u + ray.origin.z < boundaries && ray.direction.z * u + ray.origin.z > -boundaries)
                    transform.position = new Vector3(transform.position.x, transform.position.y, ray.direction.z * u + ray.origin.z);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //This function push the collectibles to lower path at the end of the path and along the path.
        Vector3 forceVector = collision.gameObject.transform.position - transform.position;
        forceVector = new Vector3(Mathf.Abs(forceVector.x), 0, 0);
        forceVector.Normalize();

        if (!_sectionStarted)
        {
            collision.collider.attachedRigidbody.AddForceAtPosition(forceVector * (_pathGameObjects[1].GetComponent<PathController>().GetActivePathInfo() ? 3 : 10), transform.position, ForceMode.Impulse);
        }
        else
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.x < (_pickerSpeed / _moveDuration))
            {
                collision.collider.attachedRigidbody.AddForceAtPosition(forceVector * ((_pickerSpeed / _moveDuration) - collision.gameObject.GetComponent<Rigidbody>().velocity.x), transform.position, ForceMode.Impulse);
            }
        }
    }


    public void SectionEnd()
    {
        //This function stops when picker reach the end of the path.
        _sectionStarted = false;
        DOTween.Pause(transform);

    }

    public void SectionStart()
    {
        //This function starts motion of the picker when its pass the last path.
        _sectionStarted = true;
        DOTween.Play(transform);
    }

    public void PauseGame()
    {
        //This function stops when picker reach the end of the path.
        _gamePaused = true;
        DOTween.Pause(transform);

    }

    public void ResumeGame()
    {
        //This function starts motion of the picker when its pass the last path.
        _gamePaused = false;
        if (_sectionStarted)
            DOTween.Play(transform);
    }

    public void RestartGame()
    {
        SetGameStartedInfo(false);
        transform.position = _restartPointGameObject.transform.position;
        _sectionStarted = true;
        _gamePaused = false;
        DOTween.Kill(transform);
        startInformText.gameObject.SetActive(true);
        foreach (GameObject path in _pathGameObjects)
        {
            if (path.GetComponent<PathController>() != null)
            {
                path.GetComponent<PathController>().DestroyCollectibles();
                path.GetComponent<PathController>().SetActivePathInfo(false);
            }
        }

        _pathGameObjects[0].GetComponent<PathController>().SetActivePathInfo(true);
    }

    public void TurnOnProps(bool on)
    {
        //Turn it on when collect the props icon
        _leftProp.SetActive(on);
        _rightProp.SetActive(on);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Detecting the collecting props icon
        if (other.name == "Prop Collectible")
        {
            TurnOnProps(true);
            Destroy(other.gameObject);
        }
    }

    public void SetGameStartedInfo(bool on)
    {
        _gameStarted = on;
    }

    public List<PickerSpecs> GetPickerSpecs()
    {
        return _pickerSpecs;
    }
}
