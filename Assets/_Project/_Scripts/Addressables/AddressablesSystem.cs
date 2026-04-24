using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesSystem
{
    private static readonly Dictionary<AssetReference, List<GameObject>> SpawnReferences =
        new Dictionary<AssetReference, List<GameObject>>();
    public static readonly Dictionary<string, AsyncOperationHandle<GameObject>> AsyncOperationHandles =
        new Dictionary<string, AsyncOperationHandle<GameObject>>();
    public static readonly Dictionary<string, AsyncOperationHandle<GameObject>> AsyncArmyLoadHandles =
        new Dictionary<string, AsyncOperationHandle<GameObject>>();

    private static readonly Dictionary<string, AsyncOperationHandle<GameObject>> AsyncMainSceneObjHandle =
        new Dictionary<string, AsyncOperationHandle<GameObject>>();

    private static readonly Dictionary<AssetReference, AsyncOperationHandle<AudioClip>> AudioClipAsyncOperationHandles =
        new Dictionary<AssetReference, AsyncOperationHandle<AudioClip>>();
    private static readonly Dictionary<AssetReference, AsyncOperationHandle<AnimatorOverrideController>> AnimAsyncOperationHandles =
        new Dictionary<AssetReference, AsyncOperationHandle<AnimatorOverrideController>>();

    public static AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetReference)
    {
        AsyncOperationHandle loadOperationHandle = LoadAssetAsync(assetReference);
        loadOperationHandle.WaitForCompletion();
        return InstantiateAsyncHandle(assetReference);
    }

    public static AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetReference, Transform parent)
    {
        AsyncOperationHandle loadOperationHandle = LoadAssetAsync(assetReference);
        loadOperationHandle.WaitForCompletion();
        return InstantiateAsyncHandle(assetReference, parent);
    }

    public static AsyncOperationHandle<GameObject> InstantiateAsync(
        AssetReference assetReference,
        Vector3 position,
        Quaternion rotation,
        Transform parent
        )
    {
        AsyncOperationHandle loadOperationHandle = LoadAssetAsync(assetReference);
        loadOperationHandle.WaitForCompletion();
        return InstantiateAsyncHandle(assetReference, position, rotation, parent);
    }


    public static AsyncOperationHandle<GameObject> InstantiateArmyAsync(
        AssetReference assetReference,
        Vector3 position,
        Quaternion rotation,
        Transform parent
    )
    {
        AsyncOperationHandle loadOperationHandle = LoadAssetArmyAsync(assetReference);
        loadOperationHandle.WaitForCompletion();
        return InstantiateAsyncHandle(assetReference, position, rotation, parent);
    }

    #region Instantiate and load private handle methods


    public static AsyncOperationHandle<GameObject> LoadAssetMainSceneAsync(AssetReference assetReference)
    {
        string assetKey = assetReference.RuntimeKey.ToString(); // Use a unique identifier for the asset

        if (AsyncMainSceneObjHandle.ContainsKey(assetKey))
        {
            return AsyncMainSceneObjHandle[assetKey];
        }

        AsyncOperationHandle<GameObject> operationHandle =
            Addressables.LoadAssetAsync<GameObject>(assetReference);
        AsyncMainSceneObjHandle[assetKey] = operationHandle;

        return operationHandle;
    }

    public static AsyncOperationHandle<GameObject> InstantiateMainSceneAsync(
        AssetReference assetReference,
        Vector3 position,
        Quaternion rotation,
        Transform parent
    )
    {
        AsyncOperationHandle loadOperationHandle = LoadAssetMainSceneAsync(assetReference);
        loadOperationHandle.WaitForCompletion();
        return InstantiateAsyncHandle(assetReference, position, rotation, parent);
    }



    public static AsyncOperationHandle<GameObject> LoadAssetAsync(AssetReference assetReference)
    {
        string assetKey = assetReference.RuntimeKey.ToString(); // Use a unique identifier for the asset

        if (AsyncOperationHandles.ContainsKey(assetKey))
        {
            return AsyncOperationHandles[assetKey];
        }

        AsyncOperationHandle<GameObject> operationHandle =
            Addressables.LoadAssetAsync<GameObject>(assetReference);
        AsyncOperationHandles[assetKey] = operationHandle;

        return operationHandle;
    }



    public static AsyncOperationHandle<GameObject> LoadAssetArmyAsync(AssetReference assetReference)
    {
        string assetKey = assetReference.RuntimeKey.ToString(); // Use a unique identifier for the asset

        if (AsyncArmyLoadHandles.ContainsKey(assetKey))
        {
            return AsyncArmyLoadHandles[assetKey];
        }

        AsyncOperationHandle<GameObject> operationHandle =
            Addressables.LoadAssetAsync<GameObject>(assetReference);
        AsyncArmyLoadHandles[assetKey] = operationHandle;

        return operationHandle;
    }

    public static AsyncOperationHandle<AudioClip> LoadAudioAssetAsync(AssetReference assetReference)
    {
        if (AudioClipAsyncOperationHandles.ContainsKey(assetReference)) return AudioClipAsyncOperationHandles[assetReference];

        AsyncOperationHandle<AudioClip> operationHandle =
            Addressables.LoadAssetAsync<AudioClip>(assetReference);
        AudioClipAsyncOperationHandles[assetReference] = operationHandle;
        operationHandle.WaitForCompletion();

        return operationHandle;
    }

    public static AsyncOperationHandle<AnimatorOverrideController> LoadAnimAssetAsync(AssetReference assetReference)
    {
        if (AnimAsyncOperationHandles.ContainsKey(assetReference)) return AnimAsyncOperationHandles[assetReference];

        AsyncOperationHandle<AnimatorOverrideController> operationHandle =
            Addressables.LoadAssetAsync<AnimatorOverrideController>(assetReference);
        AnimAsyncOperationHandles[assetReference] = operationHandle;
        operationHandle.WaitForCompletion();

        return operationHandle;
    }

    private static AsyncOperationHandle<GameObject> InstantiateAsyncHandle(AssetReference assetReference)
    {
        AsyncOperationHandle<GameObject> instantiateOperationHandle = Addressables.InstantiateAsync(assetReference);
        instantiateOperationHandle.Completed += (operation) =>
        {
            OnInstantiateCompleted(assetReference, operation);
        };

        instantiateOperationHandle.WaitForCompletion();
        return instantiateOperationHandle;
    }

    public static AsyncOperationHandle<GameObject> InstantiateAsyncHandle(AssetReference assetReference, Transform parent)
    {
        AsyncOperationHandle<GameObject> instantiateOperationHandle = Addressables.InstantiateAsync(assetReference, parent);
        instantiateOperationHandle.Completed += (operation) =>
        {
            OnInstantiateCompleted(assetReference, operation);
        };

        instantiateOperationHandle.WaitForCompletion();
        return instantiateOperationHandle;
    }

    private static AsyncOperationHandle<GameObject> InstantiateAsyncHandle(
        AssetReference assetReference,
        Vector3 position,
        Quaternion rotation,
        Transform parent
    )
    {
        AsyncOperationHandle<GameObject> instantiateOperationHandle = Addressables.InstantiateAsync(assetReference, position, rotation, parent);

        instantiateOperationHandle.Completed += (operation) =>
        {
            OnInstantiateCompleted(assetReference, operation);
        };

        instantiateOperationHandle.WaitForCompletion();
        return instantiateOperationHandle;
    }

    #endregion

    #region Event handler

    private static void OnInstantiateCompleted(AssetReference assetReference, AsyncOperationHandle<GameObject> operation)
    {
        if (SpawnReferences.ContainsKey(assetReference) == false)
        {
            SpawnReferences[assetReference] = new List<GameObject>();
        }

        SpawnReferences[assetReference].Add(operation.Result);
        if (operation.Result == null) return;
        AssetReferenceControl controlledObject = operation.Result.AddComponent<AssetReferenceControl>();
        controlledObject.Destroyed += OnObjectDestroy;
        controlledObject.reference = assetReference;
    }

    private static void OnObjectDestroy(AssetReference reference, AssetReferenceControl controlComponent)
    {
        string assetKey = reference.RuntimeKey.ToString();
        Addressables.ReleaseInstance(controlComponent.gameObject);

        SpawnReferences[reference].Remove(controlComponent.gameObject);

        if (SpawnReferences[reference].Count == 0)
        {
            if (AsyncOperationHandles.ContainsKey(assetKey))
            {
                if (AsyncOperationHandles[assetKey].IsValid())
                    Addressables.Release(AsyncOperationHandles[assetKey]);
                AsyncOperationHandles.Remove(assetKey);
            }
        }
    }

    public static void UnloadAllAudio()
    {
        List<AssetReference> keys = AudioClipAsyncOperationHandles.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            if (AudioClipAsyncOperationHandles[keys[i]].IsValid())
            {
                Addressables.Release(AudioClipAsyncOperationHandles[keys[i]]);
                AudioClipAsyncOperationHandles.Remove(keys[i]);
            }
        }
    }


    public static void UnloadAllMainAssets()
    {

        List<string> keys = AsyncMainSceneObjHandle.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            if (AsyncMainSceneObjHandle[keys[i]].IsValid())
            {
                Addressables.Release(AsyncMainSceneObjHandle[keys[i]]);
                AsyncMainSceneObjHandle.Remove(keys[i]);
            }
        }


        if (AsyncArmyLoadHandles == null) return;
        List<string> keyArmys = AsyncArmyLoadHandles.Keys.ToList();
        for (int i = 0; i < keyArmys.Count; i++)
        {
            if (AsyncArmyLoadHandles[keyArmys[i]].IsValid())
            {
                Addressables.Release(AsyncArmyLoadHandles[keyArmys[i]]);
                AsyncArmyLoadHandles.Remove(keyArmys[i]);
            }
        }
    }

    public static void UnloadAllAnim()
    {
        List<AssetReference> keys = AnimAsyncOperationHandles.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            if (AnimAsyncOperationHandles[keys[i]].IsValid())
            {
                Addressables.Release(AnimAsyncOperationHandles[keys[i]]);
                AnimAsyncOperationHandles.Remove(keys[i]);
            }

        }
    }

    #endregion
}