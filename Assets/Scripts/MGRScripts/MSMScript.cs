using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSMScript : MonoBehaviour {

    private static MSMScript instance;

    private LinkedList<GameObject> players;
    private static GameObject p1, p2;
    private static PlayerInfo p1Info, p2Info;

    public static readonly int LEVEL_1_1 = 0;
    private static string[] LEVEL_NAMES = {
        "Level1Scene"
    };
    public static readonly int SUBLEVEL_TRANSITION = 0;
    public static readonly int LEVEL_TRANSITION = 1;

    void Awake() {
        players = new LinkedList<GameObject>();
        instance = this;
    }

    void Update() {
        
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

    public static void LoadLevel(int levelID, int transitionType) {
        if(transitionType == LEVEL_TRANSITION) {
            //reset important player info for next level
            if(p1Info != null) { p1Info.ClearForNextLevel(); }
            if(p2Info != null) { p2Info.ClearForNextLevel(); }
        }
        SceneManager.LoadScene(LEVEL_NAMES[levelID]);
    }

    public static void SetupSinglePlayerGame(int p1Type) {
        p1Info = new PlayerInfo(p1Type);
        p2Info = null;
    }

    public static void SetupCoOpGame(int p1Type, int p2Type) {
        p1Info = new PlayerInfo(p1Type);
        p2Info = new PlayerInfo(p2Type);
    }

    public static void QuitGame() {
        Application.Quit();
    }

}
