using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class PickerController : MonoBehaviour
{
    //This class manage all picker specification and behaviors.
    [SerializeField] private PickerSpecs _pickerSpecs;
    [SerializeField] private GameObject _leftProp;
    [SerializeField] private GameObject _rightProp;
    [SerializeField] private TextMeshProUGUI startInformText;

    private float _moveDuration = 3f;
    private float _pickerSpeed = 15;
    private float _boundaries = 9;
    private float _propBoundaries = 7;
    private bool _gameStarted = false;
    private bool _sectionStarted = true;



    private void Start()
    {
        if (_pickerSpecs != null)
        {
            //Assigning picker specs at start function.
            _pickerSpeed = _pickerSpecs.GetPickerSpeed();
            transform.localScale = _pickerSpecs.GetPickerSize();
            for (int i = 0; i < 5; i++)
                transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = _pickerSpecs.GetPickerColor();
        }
    }


    private void OnMouseDrag()
    {
        if (!_gameStarted)
        {
            //This if trigger when player swipe the object.(It compile once.)
            transform.DOMoveX(_pickerSpeed, _moveDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            _gameStarted = true;
            startInformText.gameObject.SetActive(false);
        }
        if (_sectionStarted)
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

    public void TurnOnProps(bool on)
    {
        _leftProp.SetActive(on);
        _rightProp.SetActive(on);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Prop Collectible")
        {
            TurnOnProps(true);
            Destroy(other.gameObject);
        }
    }
}
