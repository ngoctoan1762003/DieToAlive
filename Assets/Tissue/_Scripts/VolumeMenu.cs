using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeMenu : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstSelected;

    private void Start() 
    {
        //menu.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            ToggleVolumeMenu();
        }
    }

    private void ToggleVolumeMenu() 
    {
        // toggle menu OFF
        if (menu.gameObject.activeInHierarchy)
        {
            menu.gameObject.SetActive(false);
            //characterController.EnableMovement();
        }
        // toggle menu ON
        else 
        {
            menu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelected);
            //characterController.DisableMovement();
        }
    }
}
