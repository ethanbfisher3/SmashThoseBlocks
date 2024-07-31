using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TitleUI : MonoBehaviour
{
    public static TitleUI Instance { get; private set; }

    public Button backButton;
    public Button volumeButton;
    public GameObject titlePage;
    public GameObject levelsPage;

    void Awake()
    {
        Instance = this;

        volumeButton.onClick.AddListener(AudioManager.Instance.ToggleMute);
        backButton.onClick.AddListener(() =>
        {

        });
    }
}
