using System.Collections;
using System.Collections.Generic;
using Krivodeling.UI.Effects;
using UnityEngine;
using DG.Tweening;
using SaveSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UIBlur blur;
    [SerializeField] private Image soundToggle;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    public void Pause()
    {
        blur.intensity = 0;
        DOTween.To(() => blur.intensity, (x) => blur.intensity = x, 0.3f, 0.5f);
        gameObject.SetActive(true);
    }

    public void Resume()
    {
        GetComponent<Animator>().Play("Out");
        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => blur.intensity, (x) => blur.intensity = x, 0, 0.5f))
            .AppendCallback(() => gameObject.SetActive(false))
            .Play();
    }

    public void ToggleSound()
    {
        var isAudioOn = AudioManager.Instance.ToggleAudio();
        soundToggle.sprite = isAudioOn ? soundOn : soundOff;
    }

    public void Restart()
    {
        EasySave.Delete<string>("Save");
        SceneManager.LoadScene(0);
    }

    public void Like()
    {
        //TODO LIKE
    }
}
