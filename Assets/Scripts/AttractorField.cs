using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorField : MusicListener {

    public MusicManager.Note m_Note;
    [Range(1f,100f)]
    public float attractionStrength = 1.4f;

    private Rigidbody2D player_rb;
    private PlayerManager playerManager;
    private bool isAttracting;

    private SpriteRenderer spriteRenderer;
    private Color attractingColor;
    private Color neutralColor;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        neutralColor = spriteRenderer.color;
        attractingColor = neutralColor;
        attractingColor.a *= attractionStrength;

        var player = GameObject.FindGameObjectWithTag("Player");
        player_rb = player.GetComponent<Rigidbody2D>();
        playerManager = player.GetComponent<PlayerManager>();
    }

    private void FixedUpdate()
    {
        if (isAttracting && !playerManager.isFinished())
        {
            Vector3 distance = transform.position - player_rb.transform.position;
            /*
            player_rb.MovePosition(
                Vector2.MoveTowards(
                    player_rb.transform.position, 
                    transform.position, 
                    (1 / distance.sqrMagnitude) * attractionStrength * baseAttractionModifier * Time.deltaTime
                )
            );
            */
            playerManager.AddAttractor(transform.position, (1f / distance.sqrMagnitude) * attractionStrength);
        }
    }

    public override void OnNotePlayed(int note)
    {
        if (note == (int)m_Note)
        {
            isAttracting = true;
            spriteRenderer.color = attractingColor;
        }
    }

    public override void OnNoteStopped(int note)
    {
        if (note == (int)m_Note)
        {
            isAttracting = false;
            spriteRenderer.color = neutralColor;
        }
    }

}
