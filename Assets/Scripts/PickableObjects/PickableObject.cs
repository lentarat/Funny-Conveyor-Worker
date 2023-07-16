using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public ObjectsType ObjectType;

    public enum ObjectsType
    {
        Apple,
        Banana,
        Orange,
        NumberOfTypes
    }
}
