using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraplate : MonoBehaviour
{
    public Vegetable veg;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public bool ReadyToAdd()
    {
        return (veg == null);
    }

    public bool AddVegetable(Vegetable _veg)
    {
        if (veg != null) return false;
        veg = _veg;
        veg.transform.SetParent(this.transform);
        veg.transform.localPosition = Vector3.zero;
        return true;
    }


    public void RemoveVegetable()
    {
        if (veg == null) return;

        veg = null;
    }
}
