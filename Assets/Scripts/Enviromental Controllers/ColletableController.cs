using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColletableController : MonoBehaviour
{
    //This class controls the behavior of collectible.
    [SerializeField] private CollectiblesSpecs _collectiblesSpecs;
    private Color _collectibleColor = new Color(0, 0, 0);

    private void Start()
    {

        if (_collectibleColor != null && _collectibleColor != Color.black)
        {
            ChangeColor(_collectibleColor);
        }
        else if (_collectiblesSpecs != null)
        {
            ChangeColor(_collectiblesSpecs.GetCollectibleColor());
        }
    }

    public void ChangeColor(Color clr)
    {
        _collectibleColor = gameObject.GetComponent<Renderer>().material.color = clr;
    }

}
