using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationsController : MonoBehaviour
{
    public static AnimationsController Instance;

    [SerializeField] private Transform scorePlace;

    private void Awake() => Instance = this;

    public void AnimateLetter(Transform fieldCell)
    {
        var cellClone = Instantiate(fieldCell, transform);
        cellClone.position = fieldCell.transform.position;
        cellClone.GetComponent<ButtonDeleteWord>().enabled = false;
        //var text = Instantiate(fieldCell.GetChild(0), transform);
        var seq = DOTween.Sequence();
        seq.Append(cellClone.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .AppendCallback(() => Destroy(cellClone.gameObject));

        //cellClone.transform.DOScale(Vector3.zero, 0.5f);
        //text.DOMove(scorePlace.position, 0.5f);
        //fieldCell.GetComponent<Image>().sprite = ;
        //fieldCell.Find("Text").GetComponent<Text>().text = "";
    }
}
