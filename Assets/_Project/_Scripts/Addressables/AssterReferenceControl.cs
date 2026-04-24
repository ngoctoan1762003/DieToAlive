using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetReferenceControl : MonoBehaviour
{
    public event Action<AssetReference, AssetReferenceControl> Destroyed;
    public AssetReference reference;

    private void OnDestroy()
    {
        Destroyed?.Invoke(reference, this);
    }
}