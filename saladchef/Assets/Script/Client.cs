using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Client : MonoBehaviour
{
    public TextMeshPro Label;
    public SpriteRenderer TimeBar;
    public float timeofeach;
    public float TotalTime;
    public float RemainTime;
    private Vector3 scalefactor;
    public string SaladRecipe;
    public int scoreForeach;
    public int scoreForFinish;
    public int penalty;
    public List<movement> GuiltyPlayer;


    // Start is called before the first frame update
    void Start()
    {
        SaladRecipe = Salad.GenerateSaladOrder();
        Label.SetText(SaladRecipe);
        int count = (SaladRecipe.Length + 1) / 2;
        TotalTime = timeofeach * count;
        RemainTime = TotalTime;
        scalefactor = new Vector3(Time.deltaTime / TotalTime, 0, 0);
        scoreForFinish = scoreForeach * count;
    }

    // Update is called once per frame
    void Update()
    {
        RemainTime -= Time.deltaTime;
        if (TimeBar.transform.localScale.x > 0)
            TimeBar.transform.localScale -= scalefactor;
        else
        {//time has finished
            TimeBar.transform.localScale = Vector3.zero;
        }

        if (RemainTime <= 0)//time has finished
        {
            if (GuiltyPlayer.Count > 0)
            {
                foreach (movement item in GuiltyPlayer)
                    item.AddScore(penalty);
            }
            else
            {// penalty for All player
                movement[] players = GameObject.FindObjectsOfType<movement>();
                foreach (var item in players)
                {
                    item.AddScore(penalty);
                }
            }
            Destroy(this.gameObject);
        }
    }

    public void OfferSalad(Salad salad, movement player)
    {
        if (Salad.Compare(SaladRecipe, salad))
        {
            salad.gameObject.transform.SetParent(this.transform);
            salad.transform.position = Vector3.zero;
            player.salad = null;
            OfferAccepted(player);

        }
        else
        {
            Debug.Log("Salads are not matched");
            CustomerIsAngry(player);
        }
    }

    public void CustomerIsAngry(movement player)
    {
        scalefactor *= 3;//decrease value of time will be faster
        penalty *= 2;
        if (GuiltyPlayer.Count > 0)
        {
            for (int i = 0; i < GuiltyPlayer.Count; i++)
                if (GuiltyPlayer[i].ID == player.ID)
                    return;

        }
        GuiltyPlayer.Add(player);
    }

    public void OfferAccepted(movement player)
    {
        player.AddScore(scoreForFinish);
        if (RemainTime > (TotalTime * 70.0f / 100.0f))
        {
            Debug.Log("Booster Generated");
            Booster.GenerateBooster(player);
        }
        Destroy(this.gameObject);
    }
}
