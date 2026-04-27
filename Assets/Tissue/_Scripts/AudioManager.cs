using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour{

    public StudioListener StudioListener;

    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    Bus masterBus;
    Bus musicBus;
    Bus ambienceBus;
    Bus sfxBus;

    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    EventInstance ambienceEventInstance;
    EventInstance musicEventInstance;

    public static AudioManager Instance { get; private set; }

    void Awake(){

        if (Instance != null)
            Debug.LogError("Found more than one Audio Manager in the scene.");
        Instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        //musicBus = RuntimeManager.GetBus("bus:/Music");
        //ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        //sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        //GamePlayController.OnLose += CleanUp;
        //GamePlayController.OnWin += CleanUp;
        InitializeAmbience(FMODEvents.Instance.Ambience);
        InitializeMusic(FMODEvents.Instance.Music);
    }

    void OnDestroy() {
        //GamePlayController.OnLose -= CleanUp;
        //GamePlayController.OnWin -= CleanUp;
        CleanUp();
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        //musicBus.setVolume(musicVolume);
        //ambienceBus.setVolume(ambienceVolume);
        //sfxBus.setVolume(SFXVolume);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    //public void SetMusicArea(MusicArea area)
    //{
    //    musicEventInstance.setParameterByName("area", (float) area);
    //}

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }
}
