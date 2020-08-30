using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum VegetablesEnum { a,b,c,d,e,f};
public class Vegetable : MonoBehaviour
{
    public VegetablesEnum Type;
    public TextMeshPro Label;
    public float ChoppingTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        Label.text = Type.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
