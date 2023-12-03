using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BlocksGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private float _rowGap = 0.5f;
    [SerializeField] private float _heightGap = 0.5f;
    [SerializeField] private float _distanceBetweenGrades = 10f;
    [SerializeField] private GameObject _gradeText;

    private List<BlockData> _blocksInfo = new List<BlockData>();
    private Dictionary<string, List<BlockData>> _listByGrade = new Dictionary<string, List<BlockData>>();
    public Dictionary<string, List<BlockData>> GetListByGrade() { return _listByGrade; }

    private Vector3 _spawnPoint = Vector3.zero;
    private int _rowCount = 3;

    public void Init(List<BlockData> data)
    {
        _blocksInfo = data;
        CreateBlocks();
    }

    private void CreateBlocks()
    {
        foreach (BlockData block in _blocksInfo)
        {
            if (!_listByGrade.ContainsKey(block.grade))
            {
                _listByGrade[block.grade] = new List<BlockData>();
            }

            _listByGrade[block.grade].Add(block);
        }

        foreach (var kvp in _listByGrade)
        {
            string grade = kvp.Key;
            List<BlockData> gradeList = kvp.Value.OrderBy(b => b.domain)
                                      .ThenBy(b => b.cluster)
                                      .ThenBy(b => b.standardid)
                                      .ToList();

            CreateJengaTower(grade, gradeList);

            _spawnPoint += new Vector3(_distanceBetweenGrades, 0, 0);
            _spawnPoint.y = 0;
        }
    }

    private void CreateJengaTower(string grade, List<BlockData> gradeList)
    {
        int count = 0;
        bool ltr = false;
        List<GameObject> totalBlocks = new List<GameObject>();
        List<GameObject> toRotate = new List<GameObject>();

        foreach (BlockData blockData in gradeList)
        {
            if (count < _rowCount)
            {
                GameObject go = Instantiate(_blockPrefab, _spawnPoint, Quaternion.identity, null);
                Block block = go.GetComponent<Block>();
                block.Init(blockData);

                _spawnPoint += new Vector3(_rowGap, 0f, 0);
                toRotate.Add(go);
                totalBlocks.Add(go);
                count++;
            }
            else
            {
                _spawnPoint += new Vector3(-3 * _rowGap, _heightGap, 0);
                if (ltr)
                {
                    ApplyRotationOffset(toRotate);
                }
                toRotate.Clear();
                ltr = !ltr;
                count = 0;
            }
        }

        Vector3 stackPos = GetAveragePos(totalBlocks);
        GameManager.Instance.AddObjectToStack(totalBlocks, grade, stackPos);

        GameObject text = Instantiate(_gradeText, stackPos, Quaternion.identity, null);
        text.GetComponentInChildren<TextMeshPro>().SetText(grade);
        text.transform.position = new Vector3(text.transform.position.x, 0, -3);
    }

    private void ApplyRotationOffset(List<GameObject> toRotate)
    {
        GameObject temp = new GameObject();
        temp.transform.position = GetAveragePos(toRotate);

        foreach (GameObject obj in toRotate)
        {
            obj.transform.parent = temp.transform;
        }

        temp.transform.localEulerAngles += new Vector3(0, 90, 0);

        foreach (GameObject obj in toRotate)
        {
            obj.transform.parent = null;
        }

        Destroy(temp);
    }

    private Vector3 GetAveragePos(List<GameObject> toRotate)
    {
        Vector3 sum = Vector3.zero;
        foreach (GameObject obj in toRotate)
        {
            sum += obj.transform.position;
        }
        return sum / toRotate.Count;
    }
}
