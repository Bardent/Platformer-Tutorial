﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration, knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque;
    [SerializeField]
    private bool applyKnockback;
    [SerializeField]
    private GameObject hitParticle;

    private float currentHealth, knockbackStart;

    private int playerFacingDirection;

    private bool playerOnLeft, knockback;

    private PlayerController pc;
    private GameObject aliveGO, brokenTopGO, brokenBotGO;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator aliveAnim;

    private void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerController>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBotGO = transform.Find("Broken Bottom").gameObject;

        aliveAnim = aliveGO.GetComponent<Animator>();
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);
    }

    private void Update()
    {
        CheckKnockback();
    }    

    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBotGO.transform.position = aliveGO.transform.position;

        rbBrokenBot.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
        rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedX * playerFacingDirection, knockbackDeathSpeedY);
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
