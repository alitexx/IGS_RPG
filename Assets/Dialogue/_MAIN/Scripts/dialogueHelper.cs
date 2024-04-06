using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class dialogueHelper : MonoBehaviour
{
    public CanvasGroup pressSpaceTOConinute;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeinOutHelperText());
    }

    private IEnumerator fadeinOutHelperText()
    {
        yield return new WaitForSeconds(3);
        pressSpaceTOConinute.DOFade(1, 1);
        yield return new WaitForSeconds(2);
        pressSpaceTOConinute.DOFade(0, 1);
        yield return new WaitForSeconds(3);
        pressSpaceTOConinute.gameObject.SetActive(false);
    }
}
