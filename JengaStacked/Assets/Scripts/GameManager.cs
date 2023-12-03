using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private BlocksGenerator _generator;
    [SerializeField] private UIManager _ui;
    [SerializeField] private CameraController _camera;
    private List<BlockData> _data = new List<BlockData>();
    private List<Stack> _stacks = new List<Stack>();
    private int _selectedIndex = 0;

    private IEnumerator Start()
    {
        yield return StartCoroutine(FetchBlocksData());

        if (_data != null)
        {
            _generator.Init(_data);
            _ui.Init(_stacks);
            ChooseStack(_stacks[_selectedIndex]._stackPos, _selectedIndex);
        }
        else
        {
            Debug.LogError("Could not fetch data");
        }
    }

    public IEnumerator FetchBlocksData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            _data = JsonConvert.DeserializeObject<List<BlockData>>(json);
        }
    }

    public void DestroyCurrentGlass()
    {
        foreach(GameObject go in _stacks[_selectedIndex]._blocks)
        {
            go.GetComponent<Block>().Test();
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Block selected = hit.transform.gameObject.GetComponentInParent<Block>();
                if (selected != null)
                {
                    _ui.ShowInfo(selected.GetData());
                }
            }
        }
    }

    public void ChooseStack(Transform stackPos, int index)
    {
        _selectedIndex = index;
        _camera.MoveToStack(stackPos);
    }

    public void AddObjectToStack(List<GameObject> blocks, string grade, Vector3 spawnPoint)
    {
        GameObject go = new GameObject();
        go.transform.position = spawnPoint;
        _stacks.Add(new Stack(blocks, grade, go.transform));
    }
}

public class Stack
{
    public List<GameObject> _blocks = new List<GameObject>();
    public string _grade;
    public Transform _stackPos;

    public Stack(List<GameObject> blocks, string grade, Transform stackPos)
    {
        _blocks = blocks;
        _grade = grade;
        _stackPos = stackPos;
    }
}
