using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Picker Specs", menuName ="Picker")]

public class PickerSpecs : ScriptableObject
{
    //All Picker specifications stored in this scriptable object class.
    [SerializeField] private string pickerName = "Picker";
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private float speed = 10;
    [SerializeField] private Color color;

    public string GetPickerName()  { return pickerName; }
    public Vector3 GetPickerSize() { return size; }
    public float GetPickerSpeed() { return speed; }
    public Color GetPickerColor() { return color; }
}
