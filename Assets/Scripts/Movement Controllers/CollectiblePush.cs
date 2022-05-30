using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePush : MonoBehaviour
{
    [SerializeField] private float _forceVal;


    private void OnCollisionEnter(Collision collision)
    {
        //This function push the collectibles for avoiding its drag back and glitch through picker picker.
        Vector3 forceVector = collision.gameObject.transform.position - transform.position;
        forceVector = new Vector3(Mathf.Abs(forceVector.x), 0, 0);
        forceVector.Normalize();
        collision.collider.attachedRigidbody.AddForceAtPosition(forceVector * _forceVal, transform.position, ForceMode.Impulse);
    }
}
