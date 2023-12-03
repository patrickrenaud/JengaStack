using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackButton : MonoBehaviour
{
    private TextMeshProUGUI _tmPro;

    public void Init(Stack stack, int index)
    {
        _tmPro = GetComponentInChildren<TextMeshProUGUI>();
        _tmPro.SetText(stack._grade);

        GetComponent<Button>().onClick.AddListener(()=> GameManager.Instance.ChooseStack(stack._stackPos, index));
    }


}
