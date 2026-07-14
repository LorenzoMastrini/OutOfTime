using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private List<GameObject> _items = new List<GameObject>();
    private int _maxItems = 3;
    private GameObject _equippedObject;
    private int _equippedIndex = -1;
    private InputAction _useItemAction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        _useItemAction = InputSystem.actions.FindAction("UseItem");
        _useItemAction.performed += _ => UseItem();
    }



    public bool IsEmpty() => _items.Count == 0;



    public bool AddItem(GameObject item)
    {
        if (_items.Count >= _maxItems)
        {
            Debug.Log("Inventario pieno");
            return false;
        }

        _items.Add(item);

        Debug.Log("Oggetto aggiunto nello slot " + (_items.Count - 1));
        return true;
    }



    public void RemoveItem(GameObject item)
    {
        int index = _items.IndexOf(item);

        if (index != -1)
        {
            _items.RemoveAt(index);

            if (_equippedIndex == index)
            {
                UnequipCurrentItem();
            }

            // Update equipped index
            if (_equippedIndex > index)
            {
                _equippedIndex--;
            }
        }
    }



    public void EquipItem(GameObject item)
    {
        int index = _items.IndexOf(item);

        if (index == -1)
        {
            Debug.Log("Oggetto non presente nell'inventario");
            return;
        }

        UnequipCurrentItem();

        GameObject newObject = Instantiate(
            item,
            _spawnPoint.position,
            Quaternion.identity,
            _spawnPoint
        );

        _equippedObject = newObject;
        _equippedIndex = index;
    }



    public bool AlreadyInInventory(GameObject item)
    {
        foreach (GameObject obj in _items)
        {
            if (obj != null && obj.name == item.name)
            {
                return true;
            }
        }

        return false;
    }



    private void UnequipCurrentItem()
    {
        if (_equippedObject != null)
        {
            Destroy(_equippedObject);
            _equippedObject = null;
        }

        _equippedIndex = -1;
    }



    public void UseItem()
    {
        if (_equippedObject == null)
        {
            Debug.Log("Nessun oggetto equipaggiato");
            return;
        }

        Item itemScript = _equippedObject.GetComponent<Item>();

        if (itemScript != null)
        {
            itemScript.Use();
        }
    }


    public void RemoveEquippedItem()
    {
        if (_equippedIndex < 0)
            return;

        int indexToRemove = _equippedIndex;

        UnequipCurrentItem();

        _items.RemoveAt(indexToRemove);
    }



    public void ClearInventory()
    {
        UnequipCurrentItem();

        _items.Clear();

        _equippedObject = null;
        _equippedIndex = -1;
    }
}