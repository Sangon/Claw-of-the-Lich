﻿using UnityEngine;
using System.Collections;

public class GameHUD : MonoBehaviour
{
    private CameraScripts cameraScripts;
    private PartySystem partySystem;
    private TargetedAbilityIndicator targetedAbilityIndicator;

    private bool[] targeting = new bool[8];

    public bool isTargeting()
    {
        for (int i = 0; i < 8; i++)
            if (targeting[i])
                return true;
        return false;
    }

    // Use this for initialization
    void Start()
    {
        cameraScripts = Camera.main.GetComponent<CameraScripts>();
        partySystem = GameObject.Find("PartySystem").GetComponent<PartySystem>();
        targetedAbilityIndicator = GameObject.Find("HUD").GetComponent<TargetedAbilityIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            cameraScripts.toggleLock();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            HealthBar[] healthBars = FindObjectsOfType(typeof(HealthBar)) as HealthBar[];
            foreach (HealthBar bar in healthBars)
            {
                bar.toggleVisibility();
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (GameObject c in partySystem.aliveCharacters)
            {
                if (c != null && c.GetComponent<UnitCombat>() != null)
                    c.GetComponent<UnitCombat>().takeDamage(-c.GetComponent<UnitCombat>().getMaxHealth(), null);
            }
        }

        //////////////////////////////////////
        /// SPELLIT
        /////////////////////////////////////
        GameObject character = null;
        if (Input.GetKeyDown(KeyCode.Q) && partySystem.getCharacter(1).GetComponent<UnitCombat>().isAlive())
        {
            targeting[0] = true;
        }
        if (Input.GetKeyUp(KeyCode.Q) && targeting[0])
        {
            character = partySystem.getCharacter(1);
            character.GetComponent<UnitCombat>().castSpellInSlot(0);
            character.GetComponent<UnitMovement>().stop();
            targeting[0] = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && partySystem.getCharacter(1).GetComponent<UnitCombat>().isAlive())
        {
            targeting[1] = true;
        }
        if (Input.GetKeyUp(KeyCode.W) && targeting[1])
        {
            character = partySystem.getCharacter(1);
            character.GetComponent<UnitCombat>().castSpellInSlot(1);
            character.GetComponent<UnitMovement>().stop();
            targeting[1] = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && partySystem.getCharacter(2).GetComponent<UnitCombat>().isAlive())
        {
            targeting[2] = true;
        }
        if (Input.GetKeyUp(KeyCode.E) && targeting[2])
        {
            character = partySystem.getCharacter(2);
            character.GetComponent<UnitCombat>().castSpellInSlot(0);
            character.GetComponent<UnitMovement>().stop();
            targeting[2] = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && partySystem.getCharacter(2).GetComponent<UnitCombat>().isAlive())
        {
            targeting[3] = true;
        }
        if (Input.GetKeyUp(KeyCode.R) && targeting[3])
        {
            character = partySystem.getCharacter(2);
            character.GetComponent<UnitCombat>().castSpellInSlot(1);
            character.GetComponent<UnitMovement>().stop();
            targeting[3] = false;
        }

        if (Input.GetKeyDown(KeyCode.A) && partySystem.getCharacter(3).GetComponent<UnitCombat>().isAlive())
        {
            targeting[4] = true;
        }
        if (Input.GetKeyUp(KeyCode.A) && targeting[4])
        {
            character = partySystem.getCharacter(3);
            character.GetComponent<UnitCombat>().castSpellInSlot(0);
            character.GetComponent<UnitMovement>().stop();
            targeting[4] = false;
        }

        if (Input.GetKeyDown(KeyCode.S) && partySystem.getCharacter(3).GetComponent<UnitCombat>().isAlive())
        {
            targeting[5] = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && targeting[5])
        {
            character = partySystem.getCharacter(3);
            character.GetComponent<UnitCombat>().castSpellInSlot(1);
            character.GetComponent<UnitMovement>().stop();
            targeting[5] = false;
        }

        for (int i = 0; i < 8; i++)
        {
            int charID = Mathf.FloorToInt((i * 0.5f) + 1);
            int spellID = i % 2;
            character = partySystem.getCharacter(charID);
            string spell = character.GetComponent<UnitCombat>().getSpellList()[spellID].getSpellName();

            if (targeting[i])
            {
                if (spell.Equals("blot_out")) //Arrow rain skill
                    targetedAbilityIndicator.showIndicator(character, TargetedAbilityIndicator.Skills.arrow, PlayerMovement.getCurrentMousePos());
                else if (spell.Equals("charge")) //Charge skill
                    targetedAbilityIndicator.showIndicator(character, TargetedAbilityIndicator.Skills.charge, PlayerMovement.getCurrentMousePos());
                else if (spell.Equals("whirlwind")) //Whirlwind skill
                    targetedAbilityIndicator.showIndicator(character, TargetedAbilityIndicator.Skills.whirlwind, PlayerMovement.getCurrentMousePos());
            } else
            {
                if (spell.Equals("blot_out")) //Arrow rain skill
                    targetedAbilityIndicator.hideIndicator(character, TargetedAbilityIndicator.Skills.arrow);
                else if (spell.Equals("charge")) //Charge skill
                    targetedAbilityIndicator.hideIndicator(character, TargetedAbilityIndicator.Skills.charge);
                else if (spell.Equals("whirlwind")) //Whirlwind skill
                    targetedAbilityIndicator.hideIndicator(character, TargetedAbilityIndicator.Skills.whirlwind);
            }
        }
    }

    void OnGUI()
    {
        int width = Screen.width;
        //int height = Screen.height;

        GUI.Label(new Rect(10, 180, 300, 20), "Press F to Toggle Camera Lock to Selection");
        GUI.Label(new Rect(10, 200, 300, 20), "Press V to Toggle Show Healthbars");
        GUI.Label(new Rect(10, 220, 300, 20), "Press (Shift +) Num to (De)select a Character");
        GUI.Label(new Rect(10, 240, 300, 20), "Press § to Select All Characters");
        GUI.Label(new Rect(10, 260, 300, 20), "Press H to Heal Player Characters");
        GUI.Label(new Rect(10, 280, 300, 20), "Press Z to Stop Moving");
        float msec = Time.deltaTime * 1000.0f;
        float fps = 1.0f / Time.deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(new Rect(width - 110, 10, 110, 20), text);
        GUI.Label(new Rect(width - 110, 30, 110, 20), "Enemies left: " + GameObject.FindGameObjectsWithTag("Hostile").Length);
    }
}
