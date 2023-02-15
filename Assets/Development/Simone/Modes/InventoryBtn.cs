using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class InventoryBtn : MonoBehaviour
{
    [SerializeField] AccItemType type;

    public delegate void ItemSelectEv(AccItemType type);
    public static event ItemSelectEv OnItemSelect;

    public void SelectItem()
    {
        Debug.Log($"{GetType().Name}.cs > PRESSED item button ({type})");

        OnItemSelect?.Invoke(type);
    }
}
