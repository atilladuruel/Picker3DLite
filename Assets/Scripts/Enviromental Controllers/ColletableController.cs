using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColletableController : MonoBehaviour
{
    //This class controls the behavior of collectible.
    [SerializeField] private CollectiblesSpecs _collectiblesSpecs;

    private void Start()
    {
        if (_collectiblesSpecs != null)
        {
            gameObject.GetComponent<Renderer>().material.color = _collectiblesSpecs.GetCollectibleColor();
        }
    }
}
