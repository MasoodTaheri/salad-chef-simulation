using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public movement[] Players;
    private static GameManager _instance;
    public List<GameObject> ClientSpawnPoints;
    public List<GameObject> Clients;
    public GameObject ClientPrefab;
    public GameObject ClientRoot;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameManager>();
            return _instance;
        }
    }


    public bool IsGameFinished = false;
    public Text WinnerText;
    public GameObject WinnerPanel;
    // Start is called before the first frame update
    void Start()
    {
        Players = FindObjectsOfType<movement>();
        WinnerPanel.SetActive(false);
        //Clients = new List<GameObject>(5);
        StartCoroutine(GeneratClient());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GeneratClient()
    {
        yield return new WaitForSeconds(2);
        while (this.gameObject.activeInHierarchy)
        {

            //for (int i = 0; i < ClientSpawnPoints.Count; i++)
            //{
            //    if (Clients[i] == null)
            //    {
            //        Clients[i] = Instantiate(ClientPrefab, ClientSpawnPoints[i].transform.position, Quaternion.identity);
            //        Clients[i].transform.SetParent(ClientRoot.transform);

            //        break;
            //    }
            //}
            //yield return new WaitForSeconds(Random.Range(0, 8f));
            int id = Random.Range(0, ClientSpawnPoints.Count);
            if (Clients[id] == null)
            {
                Clients[id] = Instantiate(ClientPrefab, ClientSpawnPoints[id].transform.position, Quaternion.identity);
                Clients[id].transform.SetParent(ClientRoot.transform);
            }
            yield return new WaitForSeconds(Random.Range(0, 2.0f));
        }
    }

    public void PlayerFinish()
    {
        bool isfinished = true;
        foreach (var item in Players)
        {
            isfinished &= item.EndOfGame;
        }
        if (isfinished == true)
        {
            IsGameFinished = true;
            WinnerPanel.SetActive(true);
            if (Players[0].Score > Players[1].Score)
                WinnerText.text = "Winner is " + Players[0].gameObject.name;
            else
                WinnerText.text = "Winner is " + Players[1].gameObject.name;

        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
