using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;
using UnityEngine.UI;
using TMPro;
using System;

[ExecuteInEditMode]
public class MenuButton : MonoBehaviour
{
    [SerializeField] private string label;
    [SerializeField] private TextMeshProUGUI txtArea;
    

    public Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        if (btn == null)
            Debug.LogWarning($"{GetType().Name}.cs > Unlinked Button");
    }

    private void Start()
    {
        txtArea.text = label;

        if (!btn.IsInteractable())
        {
            txtArea.color = Color.white;

            Debug.Log($"{GetType().Name}.cs > Button text color is {txtArea.color}");
        }
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            txtArea.text = label;
        }
    }

    private void OnMouseOver()
    {
        
    }

    private void OnMouseExit()
    {
        
    }
}