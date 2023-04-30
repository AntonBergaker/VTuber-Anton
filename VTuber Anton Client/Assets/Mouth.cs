using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mouth : MonoBehaviour {
    public MeshRenderer Renderer;

    [Serializable]
    public struct VismesTexture {
        public string Name;
        public Texture2D Image;
    }

    public VismesTexture[] VismesTextures;

    private Dictionary<string, Texture2D> namedVismesTextures;

    private void Awake() {
        namedVismesTextures = VismesTextures.ToDictionary(x => x.Name, x => x.Image);
    }

    void Start() {
        Renderer.sortingLayerName = "Default";
    }

    public void SetMouthShape(string vismes) {
        if (!namedVismesTextures.TryGetValue(vismes, out var texture)) {
            Debug.Log($"No vismes for {vismes}");
            return;
        }

        Renderer.material.mainTexture = texture;
    }

}
