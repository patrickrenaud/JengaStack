using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _stackButtonPrefab;
    [SerializeField] private GameObject _stackButtonParent;

    [SerializeField] private TextMeshProUGUI _grade;
    [SerializeField] private TextMeshProUGUI _cluster;
    [SerializeField] private TextMeshProUGUI _standartID;

    public void Init(List<Stack> stacks)
    {
        for(int i = 0; i < stacks.Count; i++)
        {
            GameObject button = Instantiate(_stackButtonPrefab, _stackButtonParent.transform);
            StackButton script = button.GetComponent<StackButton>();
            script.Init(stacks[i], i);
        }
    }

    public void ShowInfo(BlockData blockData)
    {
        _grade.SetText(blockData.grade);
        _cluster.SetText(blockData.cluster);
        _standartID.SetText(blockData.standardid);
    }

    public void DestroyGlass()
    {
        GameManager.Instance.DestroyCurrentGlass();
    }
}
