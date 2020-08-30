using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float SpeedBoostValue;
    public float SpeedBoostTime;
    public int ScoreBoostValue;
    public float TimeBoostValue;
    public bool IsInstantiated;
    public GameObject EmptyPointDetectPrefab;
    public movement player;

    public static List<GameObject> BoosterObj;
    public static void GenerateBooster(movement _player)
    {
        int rnd = Random.Range(0, 3);
        GameObject booster = Instantiate(BoosterObj[rnd]);
        booster.GetComponent<Booster>().IsInstantiated = true;
        booster.GetComponent<Booster>().player = _player;

    }
    //// Start is called before the first frame update
    void Start()
    {
        if (BoosterObj == null) BoosterObj = new List<GameObject>();
        if (!IsInstantiated)
            BoosterObj.Add(this.gameObject);
        else
            StartCoroutine(FindEmptyPoint());
    }


    IEnumerator FindEmptyPoint()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("Start searching free point");
        bool Found = false;
        Vector2 Lefttop = new Vector2(-11.76f, 8.5f);
        Vector2 rightBottom = new Vector2(14.38f, -0.64f);
        Vector2 finalpoint = new Vector2();

        while (!Found)
        {
            //yield return null;
            float Xpos = Random.Range(Lefttop.x, rightBottom.x);
            float Ypos = Random.Range(Lefttop.y, rightBottom.y);
            Debug.Log("Xpos=" + Xpos + "   Ypos=" + Ypos);
            GameObject obj = Instantiate(EmptyPointDetectPrefab, new Vector2(Xpos, Ypos), Quaternion.identity);
            yield return new WaitForSeconds(1f);
            if (obj.GetComponent<DetctFreeSpawnPoint>().IsFree)
            {
                finalpoint = new Vector2(Xpos, Ypos);
                Debug.Log("Free point detected");
                Found = true;
                this.gameObject.transform.position = finalpoint;
                GetComponent<SpriteRenderer>().enabled = true;
                Destroy(obj);
                yield break;
            }
            Debug.Log("Free point try again");
            Destroy(obj);
        }

    }
    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Consts.TAG_Player)
        {
            if (player.ID == collision.gameObject.GetComponent<movement>().ID)
            {
                if (ScoreBoostValue > 0)
                {
                    player.GetComponent<movement>().AddScore(ScoreBoostValue);
                    Destroy(this.gameObject);
                }
                if (TimeBoostValue > 0)
                {
                    player.GetComponent<movement>().PlayerTime += TimeBoostValue;
                    Destroy(this.gameObject);
                }
                if (SpeedBoostValue > 0)
                {
                    StartCoroutine(SpeedBoosterCatched(SpeedBoostValue, SpeedBoostTime));
                }
            }
        }
    }

    IEnumerator SpeedBoosterCatched(float value, float time)
    {
        movement p = player.GetComponent<movement>();
        float initspeed = p.speed;
        float t = time;
        p.speed += value;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        p.speed = initspeed;
        Destroy(this.gameObject);
    }
}
