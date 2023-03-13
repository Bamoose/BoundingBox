using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestBoundBox : MonoBehaviour
{
    public GameObject target;
    public Transform pool;

    public GameObject uiStartInfo;
    public TextMeshProUGUI uiVolumeTxt;
    public TMP_InputField uiNumOf;
    public TMP_InputField uiMaxX;
    public TMP_InputField uiMaxY;
    public TMP_InputField uiMaxZ;
    

    public Transform spawnPref;
    private List<Transform> _pool = new List<Transform>();
    private int _numToSpawn = 10;
    private float _maxX = 5f;
    private float _maxY = 5f;
    private float _maxZ = 5f;
    private Bounds _mBounds;

    private Renderer[] _cRenders;
    private bool _needsRenderers = true;

    private void Start()
    {
        _mBounds = GetComponent<Renderer>().bounds;

    }

    private Transform MakeNewObj(Transform pref, Vector3 pos, Quaternion rot, Transform parent)
    {
        Transform newObj = GameObject.Instantiate(pref, pos, rot, parent);
        return newObj;
    }

    
    public void RecalculateBounds()
    {
        if (target == null)
            return;

        if (uiStartInfo.activeSelf)
        {
            uiStartInfo.SetActive(false);
        }

        CheckInputFields();

        if(_pool.Count != _numToSpawn)
        {
            ResetRenders();

            //increase pool if needed
            if (_pool.Count < _numToSpawn)
            {
                int numToInstantiate = _numToSpawn - _pool.Count;

                for (int i = 0; i < numToInstantiate; i++)
                {
                    var newSpawn = MakeNewObj(spawnPref, Vector3.zero, Quaternion.identity, pool);                    
                    _pool.Add(newSpawn);
                    newSpawn.name += " "+_pool.Count;
                }
            }
            else
            {       
                for (int i = _pool.Count-1; i >= _numToSpawn ; i--) 
                {
                    _pool[i].gameObject.SetActive(false);
                    _pool[i].parent = pool;
                }
            }
        }
                
        for (int i = 0; i < _numToSpawn; i++)
        {
            _pool[i].gameObject.SetActive(true);
            _pool[i].parent = target.transform;
            _pool[i].position = new Vector3(GetRand(eAxis.x), GetRand(eAxis.y), GetRand(eAxis.z));
            _pool[i].rotation = Quaternion.Euler(new Vector3(GetRand(eAxis.x), GetRand(eAxis.y), GetRand(eAxis.z)));
        }

        ResetBounds();

        if (_needsRenderers)
            _cRenders = target.GetComponentsInChildren<Renderer>();

        _needsRenderers = false;

        

        foreach (Renderer r in _cRenders)
        {
            _mBounds.Encapsulate(r.bounds);
        }

        transform.position = _mBounds.center;
        transform.localScale = _mBounds.size;

        CalculateVolume();
    }

    private void CalculateVolume()
    {
        float vol = transform.localScale.x*transform.localScale.y*transform.localScale.z;
        uiVolumeTxt.text = string.Format("Volume:  {0:F2}", vol);

    }

    private float GetRand(eAxis axis)
    {
        float r = 0;
        switch (axis)
        {
            case eAxis.x:
                r = Random.Range(-_maxX, _maxX);
                break;
            case eAxis.y:
                r = Random.Range(-_maxY, _maxY);
                break;
            case eAxis.z:
                r = Random.Range(-_maxZ, _maxZ);
                break;
        }
        return r;
    }

    private void CheckInputFields()
    {
        if (uiNumOf.text == string.Empty)
            uiNumOf.text = "10";


        if (!int.TryParse(uiNumOf.text, out _numToSpawn))
        {
            _numToSpawn = 1;
        }

        if (!float.TryParse(uiMaxX.text, out _maxX))
        {
            _maxX = 5f;
        }

        if (!float.TryParse(uiMaxY.text, out _maxY))
        {
            _maxY = 5f;
        }

        if (!float.TryParse(uiMaxZ.text, out _maxZ))
        {
            _maxZ = 5f;
        }
    }

    private void ResetBounds()
    {
        Bounds emptyBounds = new Bounds(Vector3.zero, Vector3.one);
        _mBounds = emptyBounds;
    }

    public void ResetRenders()
    {
        _needsRenderers = true;
    }
}

public enum eAxis
{
    x,
    y,
    z
}
