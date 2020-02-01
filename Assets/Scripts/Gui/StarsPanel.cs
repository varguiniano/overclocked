using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StarsPanel : MonoBehaviour
{
    public Image[] stars;
    public GameManager GameManager;

    private int _starsNumber;
    // Update is called once per frame
    void Update()
    {
        _starsNumber = GameManager.Stars;
        if (_starsNumber >= 10)
        {
            _starsNumber = 10;
        }
        if (_starsNumber <= 0)
        {
            _starsNumber = 0;
        }
        
        _starsNumber = _starsNumber / 2;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < _starsNumber; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }
}