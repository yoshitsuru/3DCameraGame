using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookAtCameraForward : MonoBehaviour
{
    public TextMeshProUGUI TextMeshProUGUI;

    public string text;

    void Start()
    {
        TextMeshProUGUI.text = text;
    }

    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
