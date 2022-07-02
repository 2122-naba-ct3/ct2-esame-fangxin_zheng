using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    private Text text;
    private bool isMusicPlay = true;
    private AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        text.text = "MusicOpen";
        music = FindObjectOfType<AudioSource>().GetComponent<AudioSource>();
    }

   public void OnBtnClick()
    {
        if (isMusicPlay)
        {
            text.text = "MusicClose";
            isMusicPlay = !isMusicPlay;
            music.volume = 0f;
        }
        else
        {
            text.text = "MusicOpen";
            isMusicPlay = !isMusicPlay;
            music.volume = 0.25f;
        }
    }
}
