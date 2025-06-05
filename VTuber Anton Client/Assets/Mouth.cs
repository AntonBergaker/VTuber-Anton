using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mouth : MonoBehaviour {
    public SpriteRenderer Renderer;

    [Serializable]
    public struct VismesSprite {
        public string Name;
        public Sprite Image;
    }

    public VismesSprite[] VismesTextures;

    private Dictionary<string, Sprite> namedVismesSprites;

    private void Awake() {
        namedVismesSprites = VismesTextures.ToDictionary(x => x.Name, x => x.Image);
    }

    void Start() {
        Renderer.sortingLayerName = "Default";
    }

    public void SetMouthShape(string vismes) {
        if (!namedVismesSprites.TryGetValue(vismes, out var texture)) {
            Debug.Log($"No vismes for {vismes}");
            return;
        }

        Renderer.sprite = texture;
    }

}
