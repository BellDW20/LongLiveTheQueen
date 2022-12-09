using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

    private GameObject _prefab;
    private LinkedList<GameObject> _unusedObjects;
    private bool _resizable;

    public ObjectPool(GameObject prefab, bool resizable, int count) {
        _prefab = prefab;
        _resizable = resizable;

        _unusedObjects = new LinkedList<GameObject>();

        //Only add exactly how many we need depending on how many are already in the game
        for (int i = 0; i < count; i++) {
            AddPoolMember();
        }
    }

    public void Return(GameObject loanedObj) {
        loanedObj.SetActive(false);
        _unusedObjects.AddLast(loanedObj);
    }

    public GameObject Loan() {
        if(_unusedObjects.Count == 0) {
            if(!_resizable) {
                return null;
            }
            AddPoolMember();
        }

        GameObject loanedObj = _unusedObjects.First.Value;
        loanedObj.SetActive(true);
        _unusedObjects.RemoveFirst();

        return loanedObj;
    }

    private void AddPoolMember() {
        Return(Object.Instantiate(_prefab));
    }

}