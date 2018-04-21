using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : Interactable {
    [SerializeField] private List<Mesh> cropStages; 

    private bool hasCrop;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Start () {
            
    }

    public override void Interact () {

    }
}
