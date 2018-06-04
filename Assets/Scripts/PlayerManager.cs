using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public float hoverSpeed = 0.3f;
    public float hoverTime = 6f;

    private List<Vector2> attractors;
    private List<float> attractorsStrengths;
    private bool finished;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private float baseAttractionModifier = 1f;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        GoalManager goalManager = FindObjectOfType<GoalManager>();
        if (goalManager != null)
            goalManager.reachedGoalEvent += OnPlayerReachedGoal;
        attractors = new List<Vector2>();
        attractorsStrengths = new List<float>();
    }

    public void AddAttractor(Vector2 position, float strength)
    {
        attractors.Add(position);
        attractorsStrengths.Add(strength);
    }

    private void FixedUpdate()
    {
        if (!finished)
        {
            if (attractors.Count > 0)
            {
                Vector2 attractionCenter = Vector2.zero;
                float totalAttractionStrength = 0f;
                foreach (var attractionStrength in attractorsStrengths)
                    totalAttractionStrength += attractionStrength;
                for (int i = 0; i < attractors.Count; ++i)
                    attractionCenter += attractors[i] * attractorsStrengths[i];
                attractionCenter /= totalAttractionStrength;
                rb.MovePosition(
                    Vector2.MoveTowards(
                        transform.position,
                        attractionCenter,
                        baseAttractionModifier * Time.fixedDeltaTime
                    )
                );
                attractors.Clear();
                attractorsStrengths.Clear();
            }
        }
        else if (hoverTime > 0f)
        {
            if (hoverTime < 0.5f)
                SceneManager.LoadSceneAsync(0);
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, hoverSpeed * Time.deltaTime));
            hoverTime -= Time.fixedDeltaTime;
        }
    }

    public bool isFinished()
    {
        return finished;
    }

    private void OnPlayerReachedGoal()
    {
        finished = true;
        targetPosition = transform.position + new Vector3(0, 10);
    }
}
