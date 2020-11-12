using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Krivodeling.UI.Effects;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public static GameOverController Instance;

    [SerializeField] private UIBlur blur;
    [SerializeField] private RectTransform image;

    public void GameOver()
    {
        blur.intensity = 0;
        DOTween.To(() => blur.intensity, (x) => blur.intensity = x, 0.3f, 0.5f);
        image.DOAnchorPosY(0f, 0.5f)
            .SetEase(Ease.OutBack);
        AudioManager.Instance.GameOver();
    }

    private void Awake() => Instance = this;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameOver();
        }
    }
}
