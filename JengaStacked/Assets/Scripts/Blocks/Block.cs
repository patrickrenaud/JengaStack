using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum RessourceType
    {
        Glass = 0,
        Wood = 1,
        Steal = 2,
    }

    [Serializable]
    public struct BlockTypes
    {
        public RessourceType _type;
        public Color _color;
        public int _weight;
    }

    [SerializeField] private List<BlockTypes> _blockTypes = new List<BlockTypes>();

    private BlockData _data;
    private BlockTypes _type;
    public BlockData GetData() { return _data; }
    private Renderer _renderer;
    private Rigidbody _rb;


    public void Init(BlockData data)
    {
        _data = data;
        _renderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>();
        _renderer.material.color = GetColorForRessourceType((RessourceType)data.mastery);
    }

    public Color GetColorForRessourceType(RessourceType ressourceType)
    {
        _type = _blockTypes.FirstOrDefault(bt => bt._type == ressourceType);
        return _type._color;
    }

    public void Test()
    {
        if (_data.mastery == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            _rb.useGravity = true;
            _rb.mass = _type._weight;
        }
    }
}
