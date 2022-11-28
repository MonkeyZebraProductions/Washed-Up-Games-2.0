using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TriggerUI : MonoBehaviour
{

    public PlayerInput saveInput;
    private InputAction Submit;

    public GameObject UICanvas;

    public UnityEvent TriggerEvent;

    private void Awake()
    {
        Submit = saveInput.actions["Submit"];
        UICanvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            saveInput.SwitchCurrentActionMap("Player/UI");
            UICanvas.SetActive(true);
            if (Submit.IsPressed())
            {
                Debug.Log("Hege");
                TriggerEvent.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            saveInput.SwitchCurrentActionMap("Player");
            UICanvas.SetActive(false);
        }
    }
}
