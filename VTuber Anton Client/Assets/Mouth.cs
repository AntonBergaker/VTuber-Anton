using UnityEngine;

public class Mouth : MonoBehaviour {

    void Start() {
        GetComponent<MeshRenderer>().sortingLayerName = "Default";
    }
}
