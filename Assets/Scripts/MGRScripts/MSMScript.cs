using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSMScript : MonoBehaviour {

    private static MSMScript instance;

    private LinkedList<GameObject> players;

    void Awake() {
        players = new LinkedList<GameObject>();
        instance = this;
    }

    public static void RegisterPlayer(GameObject player) {
        instance.players.AddLast(player);
    }

    public static GameObject NearestPlayer(GameObject obj) {
        Vector3 oPos = obj.transform.position;

        LinkedListNode<GameObject> cur = instance.players.First;
        GameObject closestPlayer = null;
        float closestMag = float.PositiveInfinity;

        while(cur != null) {
            float mag = (cur.Value.transform.position-oPos).magnitude;
            if (mag < closestMag) {
                closestMag = mag;
                closestPlayer = cur.Value;
            }
            cur = cur.Next;
        }

        return closestPlayer;
    }

    public static Vector2 NearestPlayerPosition(GameObject obj) {
        GameObject closestPlayer = NearestPlayer(obj);

        if(closestPlayer == null) {
            return new Vector2(0, 0);
        }

        return closestPlayer.transform.position;
    }

}
