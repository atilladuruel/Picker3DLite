using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CheckPointController : MonoBehaviour
{
    //This class controling if current path ends and player made it.
    List<GameObject> _collectibles = new List<GameObject>();

    [SerializeField] private TextMeshPro _targetText;
    [SerializeField] private GameObject _platform;

    private void Start()
    {
        //gameObject.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = false;
    }

    private void Update()
    {
        if (transform.parent.gameObject.GetComponent<PathController>().GetActivePathInfo())
        {
            _targetText.text = _collectibles.Count + " / " + transform.parent.gameObject.GetComponent<PathController>().GetPathSpecs().GetTargetNumber();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<PickerController>() != null)
        {
            //if collided item contains pickercontroller script it mens its picker and it should stop there.
            collision.gameObject.GetComponent<PickerController>().TurnOnProps(false);
            collision.gameObject.GetComponent<PickerController>().SectionEnd();
            StartCoroutine(CheckCollectibles(collision));
        }
        else if (collision.gameObject.GetComponent<ColletableController>() != null && !_collectibles.Contains(collision.gameObject))
        {
            //This if controls and store game object of collectibles if its not added already.
            _collectibles.Add(collision.gameObject);
        }
    }

    private IEnumerator CheckCollectibles(Collider collision)
    {
        yield return new WaitForSeconds(5.0f);
        if (_collectibles.Count >= transform.parent.gameObject.GetComponent<PathController>().GetPathSpecs().GetTargetNumber())
        {
            PlayerDataController._ongoingGameHighScore ++;
            transform.parent.gameObject.GetComponent<PathController>().ActivateNextPath();
            _collectibles.Clear();
            _platform.GetComponent<Renderer>().material.color = transform.parent.gameObject.GetComponent<PathController>().GetPathSpecs().GetGroundColor();
            _platform.transform.DOMoveY(transform.parent.position.y, 1).SetDelay(2).SetEase(Ease.OutQuad).OnComplete(() => OnPlatformRiseComplete(collision));
        }
        else
        {
            Debug.Log("Section Failed!");
            collision.gameObject.GetComponent<PickerController>().RestartGame();
        }
    }

    void OnPlatformRiseComplete(Collider collision)
    {
        collision.gameObject.GetComponent<PickerController>().SectionStart();
    }
}
