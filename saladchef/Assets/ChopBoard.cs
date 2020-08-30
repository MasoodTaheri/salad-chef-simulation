using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class Salad
//{
//    public List<Vegetable> Items;
//}

public class ChopBoard : MonoBehaviour
{
    public List<Vegetable> Contain;
    public movement Player;
    public float ChoppingTime;
    public Salad saladPrefab;
    public Salad salad;
    public SpriteRenderer ProgressBar;
    public int ChoppingBoardCapacity = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //public Salad ChopCointainers()
    //{
    //    return new Salad();

    //}

    public bool ChopAddVegToSalad(GameObject veg, movement player)
    {

        if (salad == null)
        {
            salad = Instantiate(saladPrefab);
            salad.transform.SetParent(this.transform);
            salad.transform.localPosition = Vector3.zero;
        }
        if (salad.Items.Count >= ChoppingBoardCapacity)
            return false;


        Debug.Log("Add " + veg.GetComponent<Vegetable>().Type + "chopboard salad");
        salad.AddToSalad(veg.GetComponent<Vegetable>());
        veg.transform.SetParent(salad.transform);
        veg.gameObject.transform.localPosition = Vector3.zero;
        veg.gameObject.SetActive(false);
        StartCoroutine(Chop(veg, player));
        return true;
    }

    IEnumerator Chop(GameObject veg, movement player)
    {
        salad.InChoping();
        //ProgressBar.transform.localScale = Vector3.one;
        player.AllowMove = false;
        float t = veg.GetComponent<Vegetable>().ChoppingTime;
        //float factor = Time.deltaTime / t;
        Vector3 scalefactor = new Vector3(Time.deltaTime / t, 0, 0);
        while (t > 0)
        {
            t -= Time.deltaTime;
            if (ProgressBar.gameObject.transform.localScale.x > 0)
                ProgressBar.transform.localScale -= scalefactor;
            else
                ProgressBar.transform.localScale = Vector3.zero;
            yield return null;
        }
        player.AllowMove = true;
        salad.EndOfChopping();

        yield return new WaitForSeconds(1);
        ProgressBar.transform.localScale = Vector3.one;
    }
}
