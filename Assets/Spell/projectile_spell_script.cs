﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class projectile_spell_script : Spell
{
    public float velocity;
    public float damage;
    public Vector2 dir;
    private GameObject parent;
    private string ownerTag;

    private Vector2 lastPosition;
    public int direction = 0;

    protected string spriteName = "arrow_sprite";

    void Awake()
    {
        spellID = 0;
        velocity = Tuner.DEFAULT_PROJECTILE_VELOCITY;
        damage = Tuner.DEFAULT_PROJECTILE_DAMAGE;
        Destroy(gameObject, Tuner.DEFAULT_PROJECTILE_RANGE / Tuner.DEFAULT_PROJECTILE_VELOCITY);
        transform.position = new Vector3(transform.position.x, transform.position.y + Tuner.DEFAULT_PROJECTILE_OFFSET, transform.position.y / 100.0f + 800.0f);
        transform.localScale = new Vector3(50f, 50f, 0); //TODO: Fix this
    }

    public void initAttack(Vector3 enemy, GameObject parent, bool handleOffset)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/attack_bow", AudioScript.get3DAudioPositionVector3(transform.position));
        castLocation = enemy;
        this.parent = parent;
        ownerTag = parent.tag;
        if (handleOffset)
            castLocation.y += Tuner.DEFAULT_PROJECTILE_OFFSET;
        //dir = new Vector2(castLocation.x - transform.position.x, castLocation.y - transform.position.y).normalized;

        //print("Dir1: " + dir);

        dir = Ellipse.isometricDirection(castLocation, transform.position);

        //print("Dir2: " + dir2);

        transform.Rotate(new Vector3(0, 0, Mathf.Atan2(transform.position.y - castLocation.y, transform.position.x - castLocation.x) * 180f / Mathf.PI));
        transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y - Tuner.DEFAULT_PROJECTILE_OFFSET) / 100.0f + 800.0f);
    }

    private bool checkCollision(Vector2 start, Vector2 end, bool ignoreObstacles)
    {
        RaycastHit2D hit = Physics2D.Linecast(start, end, Tuner.LAYER_UNITS | Tuner.LAYER_OBSTACLES);
        Debug.DrawLine(start, end, Color.cyan, Time.fixedDeltaTime);
        if (hit.collider != null)
        {
            if (!ignoreObstacles && hit.collider.name.Equals("Collision"))
            {
                Destroy(gameObject);
                return false;
            }
            else if (!hit.collider.name.Equals("Collision") && !hit.collider.gameObject.tag.Equals(ownerTag))
            {
                hit.collider.gameObject.GetComponent<UnitCombat>().takeDamage(damage, parent);
                Destroy(gameObject);
                return false;
            }
        }
        return true;
    }

    void FixedUpdate()
    {
        //Sprite sprite = Resources.LoadAll<Sprite>("Spell Sprites/" + spriteName)[getDirection()-1];
        Sprite sprite = Resources.Load<Sprite>("Spell Sprites/" + spriteName);
        GetComponent<SpriteRenderer>().sprite = sprite;

        //Liikuttaa projektiiliä kohteen suuntaan
        if (dir != Vector2.zero)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            transform.position += new Vector3(dir.x, dir.y, 0) * velocity;
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.y - Tuner.DEFAULT_PROJECTILE_OFFSET) / 100.0f + 800.0f);
            Vector2 pos2DNew = new Vector2(transform.position.x, transform.position.y);

            Vector2 start = pos2D - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET);
            Vector2 end = pos2DNew - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET);

            if (checkCollision(start, end, false))
            {
                Vector2 offset = Quaternion.Euler(0, 0, 90) * new Vector2(dir.x, dir.y * 2f) * Tuner.DEFAULT_PROJECTILE_HITBOX_RADIUS * 0.5f;
                start = pos2D - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET) + offset;
                end = pos2DNew - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET) + offset;
                if (checkCollision(start, end, true))
                {
                    start = pos2D - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET) - offset;
                    end = pos2DNew - new Vector2(0, Tuner.DEFAULT_PROJECTILE_OFFSET) - offset;
                    checkCollision(start, end, true);
                }
            }
        }
    }

    public int getDirection()
    {
        //Palauttaa suunnan mihin unitti on suuntaamassa.
        //	
        //		 8	 7   6
        //		  \  |  /
        //	   1-----------5
        //		  /	 |  \
        //		 2	 3   4
        //
        //
        float movementAngle = Mathf.Atan2(transform.position.y + dir.y * 10 - transform.position.y, transform.position.x + dir.x * 10 - transform.position.x) + Mathf.PI;

        float qrt = Mathf.PI * 2 / 16;

        if (movementAngle > 0f && movementAngle < qrt || movementAngle > qrt * 15 && movementAngle < qrt * 16)
        {
            return 1;
        }
        else if (movementAngle > qrt && movementAngle < qrt * 3)
        {
            return 2;
        }
        else if (movementAngle > qrt * 3 && movementAngle < qrt * 5)
        {
            return 3;
        }
        else if (movementAngle > qrt * 5 && movementAngle < qrt * 7)
        {
            return 4;
        }
        else if (movementAngle > qrt * 7 && movementAngle < qrt * 9)
        {
            return 5;
        }
        else if (movementAngle > qrt * 9 && movementAngle < qrt * 11)
        {
            return 6;
        }
        else if (movementAngle > qrt * 11 && movementAngle < qrt * 13)
        {
            return 7;
        }
        else if (movementAngle > qrt * 13 && movementAngle < qrt * 15)
        {
            return 8;
        }

        //Palautata oletuksena Länsisuunnan.
        return 0;

    }
}