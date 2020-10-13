using System.Collections;
using System.Collections.Generic;
using Krivodeling.UI.Effects;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UIBlur blur;
    [SerializeField] private Image soundToggle;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;

    private bool isSoundOn;

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
        isSoundOn = !isSoundOn;
        soundToggle.sprite = isSoundOn ? soundOn : soundOff;
        //TODO Управление звуком
    }

    public void Restart()
    {
        //TODO RESTART
    }

    public void Like()
    {
        //TODO LIKE
    }
}
