using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI thiefText;
    public List<GameObject> thiefs = new List<GameObject>();
    public GameObject thief;
    public Transform startPlatform;
    public GameObject winPanel;
    public GameObject failPanel;
    private Vector3 startScaleText;

    [Range(1,20)]
    public int startThief;
    [Range(1,20)]
    public int wantedThief; 
    [HideInInspector] public int arrivedThief; 
    [HideInInspector] public bool isGameOver = false;

    private void Awake()
    {
        instance = this;        
    }

    private void Start()
    {
        for (int i = 0; i < startThief; i++)
        {
            thiefs.Add(Instantiate(thief, startPlatform.GetChild(i).transform.position, Quaternion.identity));
        }

        startScaleText = thiefText.transform.localScale;
        UpdateText();
    }

    public void UpdateText()
    {
        thiefText.text = arrivedThief.ToString() + "/" + wantedThief.ToString();
        thiefText.transform.DOScale(startScaleText*1.25f,0.25f).OnComplete(()=> thiefText.transform.DOScale(startScaleText, 0.25f));

        if (arrivedThief >= wantedThief)
        {
            isGameOver = true;
            winPanel.SetActive(true);
        }
    }

    public void CheckThiefCount()
    {
        if (thiefs.Count < wantedThief)
        {
            isGameOver = true;
            failPanel.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
