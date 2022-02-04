using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour {

    void Start() {
        GetComponent<MeshRenderer>().sortingLayerName = "Default";
    }

}
