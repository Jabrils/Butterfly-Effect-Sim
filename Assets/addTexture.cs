using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using world;

public class addTexture : MonoBehaviour {

    World w = new World();

	// Use this for initialization
	void Start () {
        SetAppearance(gameObject, w.skinType[0], new Color32(98, 0, 0, 255), Color.yellow, Color.green, Vector3.forward * 80);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetAppearance(GameObject who, Color32 skin, Color32 hair, Color32 shirt, Color32 pants, Vector3 hairStyle)
    {
        Texture2D tex = new Texture2D(32, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.SetPixel(1, 0, skin);
        tex.SetPixel(2, 0, shirt);
        tex.SetPixel(3, 0, pants);
        tex.SetPixel(4, 0, hair);
        tex.Apply();

        Renderer[] allR = who.GetComponentsInChildren<Renderer>();
        allR[0].material.mainTexture = tex;
        allR[1].material.mainTexture = tex;

        SkinnedMeshRenderer sMR = who.GetComponentInChildren<SkinnedMeshRenderer>();
        sMR.SetBlendShapeWeight(0, hairStyle.x);
        sMR.SetBlendShapeWeight(1, hairStyle.y);
        sMR.SetBlendShapeWeight(2, hairStyle.z);
    }
}
