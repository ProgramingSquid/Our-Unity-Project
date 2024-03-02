using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickToExspand : MonoBehaviour
{

    public float animationDuration;
    [Header("Exspanding Object")]
    public Vector2 scale;
    public Vector2 expandedScale;
    [Header("Button")]
    public RectTransform buttonTransform;
    public TMP_Text buttonText;
    public Vector2 exspandedButtonPos;
    public Vector2 buttonPos;
    RectTransform rectTransform;
    [Header("Backround Panel")]
    public RectTransform backroundTransform;
    public Vector2 exspandedBackroundScale;
    public Vector2 BackroundScale;
    public Vector2 exspandedBackroundPos;
    public Vector2 BackroundPos;
    
    bool isExspanded = false;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
    public void Expand()
    {
        
        if(isExspanded == true)
        {
            rectTransform.LeanScale(scale, animationDuration);
            buttonTransform.gameObject.LeanMoveLocal(exspandedButtonPos, animationDuration).setEase(LeanTweenType.easeInCubic);
            backroundTransform.gameObject.LeanScale(exspandedBackroundScale, animationDuration).setEase(LeanTweenType.easeInCubic);
            backroundTransform.LeanMoveLocal(exspandedBackroundPos, animationDuration).setEase(LeanTweenType.easeInCubic);
            buttonText.text = "<";
            
            isExspanded = false;
            
        }
        else
        {
            rectTransform.LeanScale(expandedScale, animationDuration).setEase(LeanTweenType.easeInCubic);
            buttonTransform.gameObject.LeanMoveLocal(buttonPos, animationDuration).setEase(LeanTweenType.easeInCubic);
            backroundTransform.gameObject.LeanScale(BackroundScale, animationDuration).setEase(LeanTweenType.easeInCubic);
            backroundTransform.LeanMoveLocal(BackroundPos, animationDuration).setEase(LeanTweenType.easeInCubic);
            buttonText.text = ">";

            isExspanded = true;
        }
        
    }

}
