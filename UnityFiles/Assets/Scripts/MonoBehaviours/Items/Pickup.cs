using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private Resource item;

    public Resource PickUp()
    {
        return item;
    }
}