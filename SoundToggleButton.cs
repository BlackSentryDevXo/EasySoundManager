using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public enum ButtonType
    {
        BackGroundMusic, 
        SoundFx, 
    };

    public Image switchImage;
    public ButtonType type;
    public Sprite OnImage;
    public Sprite OffImage;



    void Start()
    {
        // _image = GetComponent<Image>();
        ToggleButton();
    }

    public void ToggleButton()
    {
        var muted = false;

        if (type == ButtonType.BackGroundMusic)
        {
            muted = SoundManager.instance.IsBackgroundMusicMuted();
        }
        else if (type == ButtonType.SoundFx)
        {
            muted = SoundManager.instance.IsSoundFXMuted();
        }

        if (muted)
        {
            switchImage.sprite = OffImage;
        }
        else
        {
            switchImage.sprite = OnImage;
        }
    }
}
