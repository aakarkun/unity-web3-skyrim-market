using System;
using TMPro;
using UnityEngine;

public class ShopBase : MonoBehaviour
{
    [Header("Base UI")]
    [SerializeField] protected TextMeshProUGUI title;
    [SerializeField] protected TextMeshProUGUI statusLabel;

    protected virtual void OnEnable()
    {
        statusLabel.text = string.Empty;
    }
}
