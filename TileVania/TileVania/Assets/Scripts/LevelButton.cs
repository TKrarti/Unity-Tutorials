using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] int level = 1;

    Button levelButton;
    
    void Start()
    {
        levelText.text = level.ToString();
        levelButton = GetComponent<Button>();
        levelButton.onClick.AddListener(() => SceneManager.LoadScene(level));
    }


}
