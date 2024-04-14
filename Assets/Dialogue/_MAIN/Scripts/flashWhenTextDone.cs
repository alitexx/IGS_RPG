using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class flashWhenTextDone : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CanvasGroup fadeContinue;
    [SerializeField] private TextMeshProUGUI buttonToPress;


    private void OnEnable()
    {
        fadeContinue.DOComplete();
        fadeContinue.alpha = 0;
        buttonToPress.text = audioStatics.interractButton;
        fadeContinue.DOFade(1, 2).OnComplete(() => {
            continueFlashing();
        });
    }

    private void continueFlashing()
    {
        fadeContinue.DOFade(0.25f, 2).OnComplete(() => {
            fadeContinue.DOFade(1, 2).OnComplete(() => {
                if (gameObject.activeSelf)
                {
                    continueFlashing();
                }
            });
        });
        
    }
}
