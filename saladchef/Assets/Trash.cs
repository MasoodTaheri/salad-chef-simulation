using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{

    public List<GameObject> items;
    public int SaladMinusPoint;
    public int vegetableMinusPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToTrash(GameObject Veg,movement player)
    {
        Veg.transform.SetParent(this.transform);
        Veg.transform.localPosition = Vector3.zero;
        items.Add(Veg);
        player.AddScore(vegetableMinusPoint);
        StartCoroutine(Deleteall());
    }

    public void MoveToTrash(Salad salad,movement player)
    {
        salad.gameObject.transform.SetParent(this.transform);
        //salad.gameObject.transform.position = Vector3.zero;
        salad.gameObject.transform.localPosition = Vector3.zero;
        items.Add(salad.gameObject);
        player.AddScore(SaladMinusPoint);
        StartCoroutine(Deleteall());
    }

    IEnumerator Deleteall()
    {
        yield return new WaitForSeconds(1);
        while (items.Count > 0)
        {
            Destroy(items[0].gameObject);
            items.RemoveAt(0);
            yield return null;
        }
    }
}
