using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesPool<T> where T : Behaviour, IInPool
{
    private readonly AssetReference _assetReference = null;
    private readonly List<T> _members = new List<T>();
    private readonly Transform _defaultParent = null;

    public AddressablesPool(AssetReference prefab, int number, Transform defaultParent = null, bool createNumber = false)
    {
        this._assetReference = prefab;
        this._defaultParent = defaultParent;
        Create();
        if (createNumber)
        {
            for (int i = 0; i < number; i++)
            {
                Create();
            }
        }
    }

    private T Create()
    {
        T clone = null;
        if (_defaultParent != null)
        {
            clone = AddressablesSystem.InstantiateAsync(_assetReference, Vector3.zero, Quaternion.identity, _defaultParent).Result.GetComponent<T>();
        }
        else
        {
            clone = AddressablesSystem.InstantiateAsync(_assetReference).Result.GetComponent<T>();
        }
        clone.gameObject.SetActive(false);
        _members.Add(clone);
        return clone;
    }

    public T GetObject(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        T result = TakeOrCreate();

        result.transform.position = position;
        result.transform.rotation = rotation;
        if (parent != null)
        {
            result.transform.SetParent(parent, true);
        }
        result.OnSpawn();

        return result;
    }

    public List<T> GetObjects(int amount, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            T obj = GetObject(position, rotation, parent);
            list.Add(obj);
        }
        return list;
    }

    public List<T> GetObjects(int amount, Transform parent = null)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            T obj = GetObject(parent);
            list.Add(obj);
        }
        return list;
    }

    public List<T> GetObjectsAndActive(int amount, Transform parent = null)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < amount; i++)
        {
            T obj = GetObject(parent);
            obj.gameObject.SetActive(true);
            list.Add(obj);
        }
        return list;
    }

    public T GetObject(Transform parent = null)
    {
        T result = TakeOrCreate();
        if (parent != null)
        {
            result.transform.SetParent(parent, true);
        }
        result.OnSpawn();
        return result;
    }

    public T GetObjectAndActive(Transform parent = null)
    {
        T result = TakeOrCreate();
        if (parent != null)
        {
            result.transform.SetParent(parent, true);
        }
        result.gameObject.SetActive(true);
        result.OnSpawn();
        return result;
    }

    public T GetObjectAndActive(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        T result = TakeOrCreate();
        result.transform.position = position;
        result.transform.rotation = rotation;

        if (parent != null)
        {
            result.transform.SetParent(parent, true);
        }
        result.gameObject.SetActive(true);
        result.OnSpawn();
        return result;
    }


    private T TakeOrCreate()
    {
        for (int i = _members.Count - 1; i >= 0; i--)
        {
            if (_members[i] == null)
            {
                _members[i] = Create();
                return _members[i];
            }
            else if (_members[i].gameObject.activeSelf == false)
            {
                return _members[i];
            }
        }

        T clone = Create();
        _members.Add(clone);
        return clone;
    }

    public bool CheckCanCreate()
    {
        if (_defaultParent != null && _assetReference != null) return false;
        else return true;
    }

    public static bool CheckCanCreate(AddressablesPool<T> thisPool)
    {
        if (thisPool == null) return true;

        if (thisPool._defaultParent != null && thisPool._assetReference != null) return false;
        else return true;
    }

    public List<T> GetAllMembers()
    {
        return _members;
    }

    public List<T> GetActiveObjects()
    {
        List<T> list = new List<T>();

        foreach (var member in _members)
        {
            if (member.gameObject.activeSelf)
            {
                list.Add(member);
            }
        }

        return list;
    }

    public int ActiveSelfCount()
    {
        int count = 0;
        for (int i = 0; i < _members.Count; i++)
        {
            _members[i].TryGetComponent(out GameObject member);
            if ((object)member != null)
                count += 1;
        }

        return count;
    }

    public void UnActiveAll()
    {
        for (int i = 0; i < _members.Count; i++)
        {
            _members[i].OnDead();
            _members[i].gameObject.SetActive(false);
        }
    }
}