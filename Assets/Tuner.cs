﻿using System.CodeDom;
using UnityEngine;
using System.Collections;

public class Tuner : MonoBehaviour
{
    //GAME SETTINGS DEFAULT VALEUS
    public static readonly int FPS_TARGET_FRAME_RATE = 1200;
    public static readonly bool SHOW_HEALTHBARS = true;
    public static readonly bool SHOW_CASTBARS = true;
    public static readonly int LEVEL_WIDTH_IN_TILES = 75;
    public static readonly int LEVEL_HEIGHT_IN_TILES = 75;
    public static readonly int TILE_WIDTH = 512;
    public static readonly int TILE_HEIGHT = 256;
    public static readonly int LEVEL_WIDTH_IN_WORLD_UNITS = LEVEL_WIDTH_IN_TILES * TILE_WIDTH;
    public static readonly int LEVEL_HEIGHT_IN_WORLD_UNITS = LEVEL_HEIGHT_IN_TILES * TILE_HEIGHT;
    public static readonly int LEVEL_AREA_DIVISIONS_WIDTH = 4;
    public static readonly int LEVEL_AREA_DIVISIONS_HEIGHT = 4;
    public static readonly int LEVEL_AREA_DIVISIONS = LEVEL_AREA_DIVISIONS_WIDTH * LEVEL_AREA_DIVISIONS_HEIGHT;
    public static readonly int DEFAULT_LAYER_ORDER_UNITS = 1;

    //KEYBOARD CONTROLS
    public static readonly KeyCode[] KEYS_CHARACTER_ABILITY = new KeyCode[8] { KeyCode.Q, KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E, KeyCode.D, KeyCode.R, KeyCode.F };

    //DEFAULT COLORS
    public static readonly Color ENEMY_RANGED_COLOR = new Color(0.8f, 0.8f, 0.5f);
    public static readonly Color ENEMY_MELEE_COLOR = new Color(1.0f, 0.5f, 0.5f);
    public static readonly Color TARGETED_ABILITY_INDICATOR_COLOR = new Color(0, 0, 1.0f, 0.5f);
    public static readonly Color TARGETED_ABILITY_INDICATOR_COLOR_FAIL = new Color(1.0f, 0, 0, 0.5f);

    //UNIT DEFAULT VALUES
    public static readonly float UNIT_BASE_HEALTH = 10f;
    public static readonly float UNIT_BASE_STAMINA = 100f;
    public static readonly float UNIT_BASE_MOVEMENT_SPEED = 500f;
    public static readonly float UNIT_BASE_RANGED_RANGE = 750f;
    public static readonly float UNIT_BASE_MELEE_RANGE = 25 * 5f;
    public static readonly int UNIT_BASE_DAMAGE_MELEE_DICES = 1;
    public static readonly int UNIT_BASE_DAMAGE_MELEE_SIDES = 1;
    public static readonly int UNIT_BASE_DAMAGE_RANGED_DICES = 1;
    public static readonly int UNIT_BASE_DAMAGE_RANGED_SIDES = 1;
    public static readonly float UNIT_BASE_ATTACK_SPEED_MELEE = 1.33f;
    public static readonly float UNIT_BASE_ATTACK_SPEED_RANGED = 2.0f;
    public static readonly float DEFAULT_MELEE_ATTACK_CONE_DEGREES = 135f;

    //LICH DEFAULT VALUES
    public static readonly float LICH_KILL_MANA_GAIN = 3f;
    public static readonly float LICH_ABILITY_HEAL_AMOUNT = 0.25f; //Percent of max health
    public static readonly int LICH_ABILITY_HEAL_COST = 25;

    //KNOCKBACK DEFAULT VALUES
    public static readonly float KNOCKBACK_DISTANCE = UNIT_BASE_MELEE_RANGE * 0.5f;
    public static readonly float KNOCKBACK_STUN_DURATION = 0.3f;

    //SPELL DEFAULT VALUES
    public static readonly float CAST_TIME_INSTANT = 0f;
    public static readonly float CAST_TIME_SHORT = 0.4f;
    public static readonly float CAST_TIME_MEDIUM = 0.8f;
    public static readonly float CAST_TIME_LONG = 1.2f;
    public static readonly float CAST_TIME_EXTRA_FOR_ENEMIES = 0.5f; //Extra time (in seconds) added to all ability casts for enemies
    public static readonly float CAST_TIME_EXTRA_ON_MELEE = 0.25f; //Extra time is added (proportion of max. cast time) to casting when hit by a melee attack
    public static readonly float CAST_TIME_EXTRA_ON_RANGED = 0.1f; //Extra time is added (proportion of max. cast time) to casting when hit by a ranged attack
    public static readonly float CAST_INTERRUPT_CHANCE = 0.25f; //How big of a chance of getting interrupted with every attack
    public static readonly float ABILITY_COOLDOWN_TIME_AFTER_INTERRUPT = 3f; //How long the ability will be on cooldown after an interrupt

    public static readonly float DEFAULT_SKILL_CAST_RANGE = 150f;
    public static readonly float DEFAULT_SKILL_CAST_TIME = CAST_TIME_INSTANT;
    public static readonly float DEFAULT_SKILL_RADIUS = UNIT_BASE_MELEE_RANGE;
    public static readonly float DEFAULT_SKILL_COOLDOWN = 1f; //?

    public static readonly float DEFAULT_PROJECTILE_VELOCITY = 15f;
    public static readonly float DEFAULT_PROJECTILE_DAMAGE = 1f;
    public static readonly float DEFAULT_PROJECTILE_RANGE = 30f;

    public static readonly float DEFAULT_PROJECTILE_OFFSET = 100f; //Y-axis, from the bottom of the sprite
    public static readonly float DEFAULT_PROJECTILE_HITBOX_RADIUS = 50f;

    public static readonly float BASE_WHIRLWIND_RADIUS = UNIT_BASE_MELEE_RANGE * 2f;
    public static readonly int BASE_WHIRLWIND_DAMAGE_MULTIPLIER = 2;
    public static readonly float BASE_WHIRLWIND_COOLDOWN = 5f;

    public static readonly int BASE_CHARGE_DAMAGE_MULTIPLIER = 2;
    public static readonly float BASE_CHARGE_MOVEMENT_SPEED = UNIT_BASE_MOVEMENT_SPEED * 4f;
    public static readonly float BASE_CHARGE_COOLDOWN = 10f;
    public static readonly float BASE_CHARGE_DURATION = 0.55f;
    public static readonly float BASE_CHARGE_RADIUS = 150f; //Units that are inside this range are damaged by the charger
    public static readonly float CHARGE_MAX_ANGLE = 90f; //If the charging unit would be turning more than this (in degrees), stop charging

    public static readonly int BASE_BLOT_OUT_DURATION = 3;
    public static readonly float BASE_BLOT_OUT_RADIUS = 440f;
    public static readonly int BASE_BLOT_OUT_DAMAGE_MULTIPLIER = 1;
    public static readonly float BASE_BLOT_OUT_COOLDOWN = 20f;
    public static readonly float BASE_BLOT_OUT_CAST_TIME = CAST_TIME_LONG;
    public static readonly float BASE_BLOT_OUT_CAST_RANGE = 1000f;

    //CAMERA DEFAULT VALUES
    public static readonly float CAMERA_MIN_DISTANCE = 100f;
    public static readonly float CAMERA_MAX_DISTANCE = 6000f;
    public static readonly float CAMERA_SCROLLING_SPEED = 5f;
    public static readonly float CAMERA_ZOOM_SPEED = 15f;

    //PARTYSYSTEM DEFAULT VALUES
    public static readonly float PARTY_SPACING = 200f;

    //PATHFINDING & MOVEMENT DEFAULT VALUES
    public static readonly float PATHFINDING_MINIMUM_DISTANCE_FROM_UNIT = 10f;
    public static readonly float ATTACKMOVE_MAX_SEARCH_DISTANCE_FROM_CLICK_POINT = 300f;
    public static readonly float WANDERING_MOVEMENT_SPEED = 250f;

    //ENEMY AI DEFAULT VALUES
    public static readonly float UNIT_AGGRO_RANGE = 1500f;
    public static readonly float UNIT_AGGRO_CALLOUT_RANGE = 1000f; //Enemy units within this range (of the aggroing unit) get aggroed too. This can chain, potentially aggroing all units on map
    public static readonly float IDLING_STATE_TIME_MIN = 3f; //The minimum time in seconds the enemy spends in idle mode before it wanders
    public static readonly float IDLING_STATE_TIME_MAX = 10f; //The maximum time in seconds the enemy spends in idle mode before it wanders
    public static readonly float WANDERING_DISTANCE_MAX = 1000f; //The maximum distance the enemy can wander from its starting position
    public static readonly float WANDERING_DISTANCE = 750f; //The maximum distance the enemy can wander at a time
    public static readonly float CHASING_TIME_MAX = 3f; //The maximum time in seconds the enemy spends chasing the player without seeing him before giving up and returning to its starting position
    public static readonly float ENEMY_ABILITY_START_COOLDOWN_MIN = 0f; //The minimum time in seconds that the enemy must have been in combat before using its abilities
    public static readonly float ENEMY_ABILITY_START_COOLDOWN_MAX = 5f; //The maximum time in seconds that the enemy must have been in combat before using its abilities

    //UNITY EDITOR DEFAULT VALUES
    public static readonly int LAYER_OBSTACLES_INT = 8;
    public static readonly int LAYER_OBSTACLES = 1 << LAYER_OBSTACLES_INT;
    public static readonly int LAYER_UNITS_INT = 9;
    public static readonly int LAYER_UNITS = 1 << LAYER_UNITS_INT;
    public static readonly int LAYER_WATER_INT = 4;
    public static readonly int LAYER_WATER = 1 << LAYER_WATER_INT;
    //public static readonly int LAYER_GROUND_INT = 11;
    //public static readonly int LAYER_GROUND = 1 << LAYER_GROUND_INT;
    public static readonly int LAYER_FLOOR_INT = 12;
    public static readonly int LAYER_FLOOR = 1 << LAYER_FLOOR_INT;

    //DIFFERENT DAMAGE TYPES
    public enum DamageType
    {
        def,
        melee,
        ranged,
        spell
    }

    //SPELL AI FOR ENEMIES
    public enum SpellBaseAI
    {
        arrowRain,
        charge,
        selfHeal,
        whirlwind
    }
}
