using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Salad : MonoBehaviour
{
    public List<Vegetable> Items;
    public TextMeshPro Label;
    public bool Isready;
    private int SaladCapacity = 3;
    public string Finaltext;


    // Start is called before the first frame update
    void Start()
    {
        Label.text = "";
        Isready = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToSalad(Vegetable veg)
    {
        StartCoroutine(AddToSaladIE(veg));

    }

    IEnumerator AddToSaladIE(Vegetable veg)
    {
        yield return null;
        if (Items.Count >= SaladCapacity)
            yield break;
        Finaltext = Label.text;
        InChoping();

        if (Items.Count > 0)
            Finaltext += ",";
        Items.Add(veg);
        string s = veg.Type.ToString();
        Debug.Log("Add to salad veg.Type=" + s);
        Finaltext += s;
        Label.SetText(Finaltext);
    }

    public void InChoping()
    {
        Isready = false;
    }
    public void EndOfChopping()
    {
        Isready = true;
    }

    public static string GenerateSaladOrder()
    {
        string s = "";
        int saladVegetableCount = UnityEngine.Random.Range(2, 4);
        for (int i = 0; i < saladVegetableCount; i++)
        {
            if (i > 0)
                s += ",";
            int id = UnityEngine.Random.Range(1, 6);
            s += ((VegetablesEnum)id).ToString();
        }
        return s;
    }

    public static bool Compare(string saladRecipe, Salad salad)
    {
        bool ret0 = true;
        for (int i = 0; i < saladRecipe.Length; i++)
        {
            bool ret = false;
            if (saladRecipe[i] == ',')
                continue;
            for (int j = 0; j < salad.Items.Count; j++)
            {
                if (salad.Items[j].Type.ToString() == saladRecipe[i].ToString())
                    ret = true;
            }
            ret0 &= ret;
        }
        return ret0;
    }

}
