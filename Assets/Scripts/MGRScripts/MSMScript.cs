using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSMScript : MonoBehaviour {

    private static MSMScript instance; //The active main scene manager object

    private LinkedList<GameObject> players; //A list of references to all players in the game

    void Awake() {
        //Make sure the player list is created,
        //as player objects register with the MSM in Start()
        players = new LinkedList<GameObject>();
        //Make this scene's MSM the active MSM instance
        instance = this;
    }

    public static void RegisterPlayer(GameObject player) {
        //Adds a reference to a player object to the list of players
        //in this scene
        instance.players.AddLast(player);
    }

    //Returns a reference to nearest player object to a given object
    public static GameObject NearestPlayer(GameObject obj) {
        //Get the passed object's position
        Vector3 oPos = obj.transform.position;

        GameObject closestPlayer = null; //Initially assume there are no players

        //Set the distance of the closest player to + infty,
        //as anything will be closer than that
        float closestMag = float.PositiveInfinity;
        
        //For every player we know of in the scene
        foreach (GameObject player in instance.players) {
            //calculate the distance to that player
            float mag = (player.transform.position - oPos).magnitude;
            //if this player is closer than all of those so far, and they aren't dead
            if (mag < closestMag && !player.GetComponent<PlayerController>().IsDead()) {
                //remember them as the closest player
                closestMag = mag;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    //Gets the position of the nearest player to an object
    public static Vector2 NearestPlayerPosition(GameObject obj) {
        //Find the nearest player
        GameObject closestPlayer = NearestPlayer(obj);

        //If there weren't any players, send back a dummy position
        //so errors arent thrown
        if(closestPlayer == null) {
            return new Vector2(0, 0);
        }

        //otherwise, return the closest player's position
        return closestPlayer.transform.position;
    }

    public LinkedList<GameObject> GetPlayers()
    {
        return players;
    }
}
