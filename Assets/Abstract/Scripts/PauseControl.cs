using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseControl : MonoBehaviour
{
    [SerializeField] private List <GameObject> toggleObjects;
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        foreach (var toggleObject in toggleObjects)
        {
            toggleObject.SetActive(!toggleObject.activeSelf);
        }
    }
}
