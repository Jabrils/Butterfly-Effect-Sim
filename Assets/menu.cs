using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour {
    public int seed;
    public int rWM;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (Application.loadedLevel == 0)
        {
            rWM = Mathf.Abs((int)GameObject.Find("Slider").GetComponent<Slider>().value - 42);
            GameObject.Find("TexT").GetComponent<Text>().text = "Relationship with Mom: " + rWM;
        }
    }

    // Use this for initialization
    public void MoveOn () {
        seed = int.Parse(GameObject.Find("InputField").GetComponent<InputField>().text);
        Application.LoadLevel(Application.loadedLevel + 1);
	}
}
