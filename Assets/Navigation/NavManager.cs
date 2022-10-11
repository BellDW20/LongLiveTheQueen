using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavManager : MonoBehaviour {

    private static NavManager instance;

    void Start() {
        instance = this;
    }

    void Update() {
        
    }

    public static Vector2 nearestPlayerPosition(Vector2 origin) {
        return new Vector2(0, 0);
    }

}
