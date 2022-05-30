using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible Specs", menuName = "Collectible")]

public class CollectiblesSpecs : ScriptableObject
{
    //All collectible specifications stored in this scriptable object class.
    [SerializeField] private string collectibleName;
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private string shape;
    [SerializeField] private Color color;
    private string cubeShape = "Cube Shape", sphereShape = "Sphere Shape", capsuleShape = " Shape";

    public string GetCollectibleName() { return collectibleName; }
    public Vector3 GetCollectibleSize() { return size; }
    public string GetCollectibleSpeed() { return shape; }
    public Color GetCollectibleColor() { return color; }
}
