using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Consts
{
    //Tags
    public const string TAG_VEGETABLE = "Vegetable";
    public const string TAG_ChopBoard = "ChopBoard";
    public const string TAG_Client = "Client";
    public const string TAG_Trash = "Trash";
    public const string TAG_Booster = "Booster";
    public const string TAG_Extraplate = "Extraplate";
    public const string TAG_Player = "Player";


}
public class movement : MonoBehaviour
{
    public int ID;
    public float speed = 1.2f;
    public Vector2 direction;
    private Rigidbody2D rb2d;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;
    public KeyCode pick;
    public List<Vegetable> Basket;
    public Salad salad;
    private int BasketSize = 2;
    public GameObject TouchObj;
    public bool AllowMove;
    public int Score;
    public float PlayerTime;
    public Text ScoreUI;
    public Text TimeUI;
    public bool EndOfGame = false;


    //private Vector2 direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        rb2d = GetComponent<Rigidbody2D>();
        AddScore(0);
    }

    public void AddScore(int score)
    {
        Score += score;
        ScoreUI.text = string.Format("Score : {0}", score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (EndOfGame)
            return;

        if (PlayerTime - Time.deltaTime < 0)
        {
            EndOfGame = true;
            GameManager.instance.PlayerFinish();
            PlayerTime = 0;
            return;
        }


        PlayerTime -= Time.deltaTime;
        TimeUI.text = string.Format("Time : {0}", PlayerTime.ToString("f2"));
        if (Input.GetKeyUp(pick) && AllowMove)
        {
            if (TouchObj != null)
            {
                if ((Basket.Count < BasketSize) && (TouchObj.tag == Consts.TAG_VEGETABLE))
                {
                    GameObject instVeg = Instantiate(TouchObj, Vector3.zero, Quaternion.identity);
                    instVeg.transform.SetParent(this.transform);
                    instVeg.transform.localPosition = Vector3.zero + VegSpawnPoint();
                    instVeg.transform.localScale = 0.5f * Vector3.one;
                    Destroy(instVeg.GetComponent<BoxCollider2D>());
                    Basket.Add(instVeg.GetComponent<Vegetable>());
                    Debug.Log("Add vegtype=" + instVeg.GetComponent<Vegetable>().Type + "player basket");
                }
                if (TouchObj.tag == Consts.TAG_ChopBoard)
                {
                    if (Basket.Count > 0)// move vegetable to chop
                    {
                        Debug.Log("remove " + Basket[0].gameObject + "from player basket");
                        Vector3 oldpos = Basket[0].gameObject.transform.position;
                        if (TouchObj.GetComponent<ChopBoard>().
                            ChopAddVegToSalad(Basket[0].gameObject, this))
                        {

                            if (Basket.Count > 1)
                                Basket[1].gameObject.transform.position = oldpos;
                            Basket.RemoveAt(0);
                        }
                        else
                            Debug.Log("Error. reached to maximum chopboard capity.");
                    }
                    else//pickup Salad
                    {
                        salad = TouchObj.GetComponent<ChopBoard>().salad;
                        salad.transform.SetParent(this.gameObject.transform);
                        TouchObj.GetComponent<ChopBoard>().salad = null;
                        salad.transform.localPosition = Vector3.zero;
                    }
                }

                if (TouchObj.tag == Consts.TAG_Client)
                {
                    TouchObj.GetComponent<Client>().OfferSalad(salad, this);
                }

                if (TouchObj.tag == Consts.TAG_Trash)
                {
                    if (salad != null)
                        TouchObj.GetComponent<Trash>().MoveToTrash(salad, this);
                    else if (Basket.Count > 0)
                    {
                        TouchObj.GetComponent<Trash>().MoveToTrash(Basket[0].gameObject, this);
                        Basket.RemoveAt(0);
                    }
                }

                if (TouchObj.tag == Consts.TAG_Extraplate)
                {
                    Extraplate extraplate = TouchObj.GetComponent<Extraplate>();

                    if (extraplate.ReadyToAdd())
                    {
                        Debug.Log("extraplate.ReadyToAdd");
                        Vector3 oldpos = Basket[0].gameObject.transform.position;
                        if (extraplate.AddVegetable(Basket[0]))
                        {
                            if (Basket.Count > 1)
                                Basket[1].gameObject.transform.position = oldpos;
                            Basket.RemoveAt(0);
                        }
                    }
                    else//remove veg from plate
                    {
                        Debug.Log("extraplate.RemoveVegetable");
                        Vegetable veg = extraplate.veg;
                        veg.transform.SetParent(this.transform);
                        veg.transform.localPosition = Vector3.zero + VegSpawnPoint();
                        Basket.Add(veg);
                        extraplate.RemoveVegetable();
                    }
                }
            }
        }
    }



    public Vector3 VegSpawnPoint()
    {
        int id = -1;
        if (Basket.Count == 0) id = -1;
        else
            id = 1;

        return new Vector3(-1 * id, 1, 0);
    }

    void FixedUpdate()
    {
        if (EndOfGame)
            return;
        direction = ((Vector2.right * (IsKeyPressed(Right) - IsKeyPressed(Left))) +
            (Vector2.up * (IsKeyPressed(Up) - IsKeyPressed(Down))))
            * speed * Time.fixedDeltaTime;
        //Move my Rigidbody 2D position according to the new direction
        if (AllowMove)
            rb2d.MovePosition(rb2d.position + direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Game Over - Back to menu
        //SceneManager.LoadScene("Menu");
        //Debug.Log("OnCollisionEnter2D " + collision.gameObject.name);
        //if (collision.collider.gameObject.tag == Consts.TAG_VEGETABLE)
        TouchObj = collision.collider.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Game Over - Back to menu
        //SceneManager.LoadScene("Menu");
        //Debug.Log("OnCollisionExit2D " + collision.gameObject.name);
        TouchObj = null;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionStay2D " + collision.gameObject.name);
        TouchObj = collision.collider.gameObject;
    }

    public int IsKeyPressed(KeyCode k)

    {
        return (Input.GetKey(k)) ? 1 : 0;
    }
}
