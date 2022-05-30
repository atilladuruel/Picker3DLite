using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Paths Specs", menuName = "Path")]

public class PathsSpecs : ScriptableObject
{
    //All path specifications stored in this scriptable object class.
    [SerializeField] private string pathName;
    [SerializeField] private int pathNumber;
    [SerializeField] private List<string> target;
    [SerializeField] private int targetNumber;
    [SerializeField] private bool firstPath;

    [SerializeField] private Color groundColor;
    [SerializeField] private Color barrierColor;
    [SerializeField] private Color basketColor;

    public string GetPathName() { return pathName; }
    public int GetPathNumber() { return pathNumber; }
    public List<string> GetTarget() { return target; }
    public int GetTargetNumber() { return targetNumber; }
    public bool GetFirsPathInfo() { return firstPath; }
    public Color GetGroundColor() { return groundColor; }
    public Color GetBarrierColor() { return barrierColor; }
    public Color GetBasketColor() { return basketColor; }
}
