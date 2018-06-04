using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour {

    public delegate void OnPlayerReachedGoal();
    public OnPlayerReachedGoal reachedGoalEvent;

    bool alreadyReached = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alreadyReached && collision.CompareTag("Player"))
        {
            reachedGoalEvent.Invoke();
            alreadyReached = true;
        }
    }

}
