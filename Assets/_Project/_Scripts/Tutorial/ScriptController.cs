using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScriptController : MonoBehaviour
{
    private static ScriptController instance;
    public static ScriptController Instance => instance;

    private ScriptStep _curStepScript;
    private List<ScriptStep> _listScriptStep;

    public bool isScriptRun { get; set; }

    private List<ScriptChooseCard> _listScriptChooseCard;
    private List<ScriptCloseLibrary> _listScriptCloseLibrary;
    private List<ScriptOpenLibrary> _listScriptOpenLibrary;
    private List<ScriptOpenUnitInformation> _listScriptOpenUnitInformation;
    private List<ScriptUnlockInformation> _listScriptUnlockInformation;
    private List<ScriptUseCard> _listScriptUseCard;
    private List<ScriptReadyCard> _listScriptReadyCard;
    private List<ScriptDrawCard> _listScriptDrawCard;
   
    private TutorialID currentTutorialID;
    public TutorialID CurrentTutorialID => currentTutorialID;

    private int flowScriptIndex
    {
        get
        {
            if (_curStepScript != null)
                return _curStepScript.stepIndex;
            else
                return 0;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _listScriptChooseCard = new List<ScriptChooseCard>();
        _listScriptCloseLibrary = new List<ScriptCloseLibrary>();
        _listScriptUnlockInformation = new List<ScriptUnlockInformation>();
        _listScriptOpenLibrary = new List<ScriptOpenLibrary>();
        _listScriptOpenUnitInformation = new List<ScriptOpenUnitInformation>();
        _listScriptUseCard = new List<ScriptUseCard>();
        _listScriptReadyCard = new List<ScriptReadyCard>();
        _listScriptDrawCard = new List<ScriptDrawCard>();
        
        AddListener();
    }

    public void CheckScriptLobby()
    {
        
    }

    public void CheckScriptMainGame()
    {
        
    }

    private void AddListener()
    {
        ObserverManager.Subscribe(GameEventID.OnChooseCard, OnChooseCard);
        ObserverManager.Subscribe(GameEventID.OnOpenLibrary, OnOpenLibrary);
        ObserverManager.Subscribe(GameEventID.OnOpenUnitInformation, OnOpenUnitInformation);
        ObserverManager.Subscribe(GameEventID.OnOpenUnitInformation, OnOpenUnitInformation);
        ObserverManager.Subscribe(GameEventID.OnUnlockInformation, OnUnlockInformation);
        ObserverManager.Subscribe(GameEventID.OnCloseLibrary, OnCloseLibrary);
        ObserverManager.Subscribe(GameEventID.OnUseCard, OnUseCard);
        ObserverManager.Subscribe(GameEventID.OnReadyCard, OnReadyCard);
        ObserverManager.Subscribe(GameEventID.OnDrawCard, OnDrawCard);
    }

    private void OnChooseCard(object data)
    {
        foreach (var scriptStep in _listScriptChooseCard)
        {
            scriptStep.OnChooseCard();
        }
    }
    
    private void OnUseCard(object data)
    {
        foreach (var scriptStep in _listScriptUseCard)
        {
            scriptStep.OnUseCard();
        }
    }
            
    private void OnDrawCard(object data)
    {
        foreach (var scriptStep in _listScriptDrawCard)
        {
            scriptStep.OnDrawCard();
        }
    }
        
    private void OnReadyCard(object data)
    {
        foreach (var scriptStep in _listScriptReadyCard)
        {
            scriptStep.OnReadyCard();
        }
    }
    
    private void OnCloseLibrary(object data)
    {
        foreach (var scriptStep in _listScriptCloseLibrary)
        {
            scriptStep.OnCloseLibrary();
        }
    }
    
    private void OnUnlockInformation(object data)
    {
        foreach (var scriptStep in _listScriptUnlockInformation)
        {
            scriptStep.OnUnlockUnitInformation();
        }
    }
    
    private void OnOpenUnitInformation(object data)
    {
        foreach (var scriptStep in _listScriptOpenUnitInformation)
        {
            scriptStep.OnOpenUnitInformation();
        }
    }
    
    private void OnOpenLibrary(object data)
    {
        foreach (var scriptStep in _listScriptOpenLibrary)
        {
            scriptStep.OnOpenLibrary();
        }
    }

    #region Script Process

    private void Update()
    {
        if (isScriptRun)
        {
            if (_curStepScript.IsStartStep == false) _curStepScript.OnStepStart();
            else if (_curStepScript.CheckCompletion()) _curStepScript.OnStepComplete();
            else if (_curStepScript.IsCanNextStep) NextStep();
        }
    }

    public void StartRunScript()
    {
        _curStepScript = _listScriptStep[0];
        isScriptRun = true;
    }

    private void NextStep()
    {
        if (_listScriptStep == null || _listScriptStep.Count == 0)
            ScriptEnd();
        if (flowScriptIndex >= _listScriptStep.Count - 1) ScriptEnd();
        else
        {
            _curStepScript = _listScriptStep[flowScriptIndex + 1];
        }
    }

    private void SetCanNextStep()
    {
        _curStepScript.SetCanNextStep(true);
    }

    private void ScriptEnd()
    {
        isScriptRun = false;

        _listScriptStep?.Clear();
    }

    #endregion

    private ScriptStep AddScriptStep(ScriptStep scriptStep)
    {
        _listScriptStep.Add(scriptStep);
        
        if (scriptStep.GetType() == typeof(ScriptChooseCard))
        {
            _listScriptChooseCard.Add((ScriptChooseCard)scriptStep);
        }            
        if (scriptStep.GetType() == typeof(ScriptUseCard))
        {
            _listScriptUseCard.Add((ScriptUseCard)scriptStep);
        }      
        if (scriptStep.GetType() == typeof(ScriptCloseLibrary))
        {
            _listScriptCloseLibrary.Add((ScriptCloseLibrary)scriptStep);
        }      
        if (scriptStep.GetType() == typeof(ScriptUnlockInformation))
        {
            _listScriptUnlockInformation.Add((ScriptUnlockInformation)scriptStep);
        }        
        if (scriptStep.GetType() == typeof(ScriptOpenLibrary))
        {
            _listScriptOpenLibrary.Add((ScriptOpenLibrary)scriptStep);
        }
        if (scriptStep.GetType() == typeof(ScriptOpenUnitInformation))
        {
            _listScriptOpenUnitInformation.Add((ScriptOpenUnitInformation)scriptStep);
        }
        if (scriptStep.GetType() == typeof(ScriptReadyCard))
        {
            _listScriptReadyCard.Add((ScriptReadyCard)scriptStep);
        }
        if (scriptStep.GetType() == typeof(ScriptDrawCard))
        {
            _listScriptDrawCard.Add((ScriptDrawCard)scriptStep);
        }

        return scriptStep;
    }

    #region Gameplay

    public void RunProcessGuideStage()
    {
        _listScriptStep = new List<ScriptStep>();
        currentTutorialID = TutorialID.First;

        PanelHighlight.Instance.SetActive(true);

        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(0);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));

        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(1);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(2);
                GameSystem.Instance.Enemies[0].Highlight();
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(3);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(4);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
                GameSystem.Instance.Enemies[0].DeHighlight();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(5);
                PanelHighlight.Instance.AddHightLightObj(GameSystem.Instance.Enemies[0].DiceTarget.transform.parent);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.PopupDescription.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(6);
                PanelHighlight.Instance.AddHightLightObj(GameSystem.Instance.Enemies[0].DiceTarget.transform.parent);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.PopupDescription.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(7);
                PanelHighlight.Instance.AddHightLightObj(GameSystem.Instance.Enemies[0].DiceTarget.transform.parent);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.PopupDescription.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(8);
                PanelHighlight.Instance.AddHightLightObj(GameSystem.Instance.Enemies[0].DiceTarget.transform.parent);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.PopupDescription.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
               
        AddScriptStep(new ScriptOpenLibrary(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(9);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.LibraryButton.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                       
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(10);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.LibraryCanvas.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                               
        AddScriptStep(new ScriptOpenUnitInformation(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(11);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.LibraryUnitContainer.GetChild(0).transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(12);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.SkillAndPassivePanel);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(13);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.SkillAndPassivePanel);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                
        AddScriptStep(new ScriptUnlockInformation(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(14);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.LibraryCardContainer.GetChild(0).GetComponent<CardInfo>().LockGO.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(15);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(16);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(17);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                        
        AddScriptStep(new ScriptCloseLibrary(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(18);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.LibraryBackButton);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(19);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(20);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.CardContainer.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                
        AddScriptStep(new ScriptChooseCard(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(21);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.handCardsTransform.GetChild(0).transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(22);
                GameSystem.Instance.Enemies[0].Highlight();
                UIManager.Instance.SetLockReadyCard(true);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                
        AddScriptStep(new ScriptUseCard(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(23);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                UIManager.Instance.SetLockReadyCard(false);
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(24);
                DOVirtual.DelayedCall(1, () =>
                {
                    Time.timeScale = 0.05f;
                });
            },
            () =>
            {
                Time.timeScale = 1f;
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(25);
                Time.timeScale = 0.05f;
            },
            () =>
            {
                Time.timeScale = 1;
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(26);
                Time.timeScale = 1;
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(27);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                       
        AddScriptStep(new ScriptChooseCard(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(28);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.handCardsTransform.GetChild(0).transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                               
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(29);
                GameSystem.Instance.Enemies[0].Highlight();
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                       
        AddScriptStep(new ScriptReadyCard(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(30);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
                GameSystem.Instance.Enemies[0].DeHighlight();
            }
        ));
                                                                                                                              
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(31);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.readyCardsTransform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                      
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(32);
                GameSystem.Instance.Enemies[0].Highlight();
                GameSystem.Instance.Player.Highlight();
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                              
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(33);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                      
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(34);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                GameSystem.Instance.Enemies[0].DeHighlight();
                GameSystem.Instance.Player.DeHighlight();
                SetCanNextStep();
            }
        ));
                                                                                                                                                   
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(35);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(36);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                
        AddScriptStep(new ScriptDrawCard(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(37);
                PanelHighlight.Instance.AddHightLightObj(UIManager.Instance.DrawButton.transform);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(38);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(39);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                GameSystem.Instance.Enemies[0].DeHighlight();
                GameSystem.Instance.Player.DeHighlight();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
                                                                                                                                                        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(40);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHighlight.Instance.SetActive(false);
                PanelHint.Instance.HideAll();
                SaveLoadManager.Instance.SaveDoneTutorial("Tutorial");
                SetCanNextStep();
            }
        ));
        
        StartRunScript();
        GameSystem.Instance.StartCombat(MapManager.Instance.GetNodeConfigs("Tutorial").node as CombatNodeConfig);
    }

    public void RunProcessGuideStage2()
    {
        _listScriptStep = new List<ScriptStep>();
        currentTutorialID = TutorialID.Second;

        PanelHighlight.Instance.SetActive(true);

        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(0);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(1);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHighlight.Instance.SetActive(false);
                PanelHint.Instance.HideAll();
                InventoryManager.Instance.RestoreSnapshot();
                SaveLoadManager.Instance.SaveDoneTutorial("Tutorial2");
                GameSystem.Instance.StartCombat(MapManager.Instance.GetNodeConfigs("Tutorial2").node as CombatNodeConfig);
                SetCanNextStep();
            }
        ));
        
        StartRunScript();
    }
    
    public void RunProcessGuideStage3()
    {
        _listScriptStep = new List<ScriptStep>();
        currentTutorialID = TutorialID.Third;

        PanelHighlight.Instance.SetActive(true);

        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(0);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(1);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(2);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHint.Instance.HideAll();
                SetCanNextStep();
            }
        ));
        
        AddScriptStep(new ScriptAnyActionNext(_listScriptStep.Count, 0.25f, 0.1f,
            () =>
            {
                PanelHint.Instance.ShowHint(3);
            },
            () =>
            {
                PanelHighlight.Instance.AllComeToOldParent();
                PanelHighlight.Instance.SetActive(false);
                PanelHint.Instance.HideAll();
                SaveLoadManager.Instance.SaveDoneTutorial("Tutorial3");
                SetCanNextStep();
                MapManager.Instance.ResetRun();
            }
        ));
        
        StartRunScript();
    }
    
    #endregion
}

public enum TutorialID
{
    First,
    Second,
    Third
}