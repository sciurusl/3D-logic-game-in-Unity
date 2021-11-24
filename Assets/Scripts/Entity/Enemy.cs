using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private float staminaRefreshing = 0f;
    private float maxStamina = 10f;
    private Entity curEntity;
    private Entity player;
    public GameObject projectilePrefab;
    private int height = 1;
    private int maxDistance =20;
    public bool upWorld;

    private void Start()
    {
        player = GameManager.instance.player.GetComponent<Entity>();
        curEntity = GetComponent<Entity>();
        if(transform.parent.parent.name.Equals("UpWorld"))
            upWorld = true;
        else
            upWorld = false;
    }
   
    /// <summary>
    /// Sets stamina to 0
    /// </summary>
    public void UseStamina()
    {
        staminaRefreshing = 0f;
    }

    /// <summary>
    /// Performs attac and instantiates a projectile in the direction of the player
    /// </summary>
    public void Attack()
    {      
        if (staminaRefreshing >= curEntity.attackSpeed && curEntity.canAttack)
        {
            GameObject projectileInstance = (GameObject)Instantiate(projectilePrefab, transform.position + Vector3.Normalize(Vector3.Cross(transform.forward, transform.up)), transform.rotation);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();
            projectile.creator = transform;
            if (projectile != null)
                projectile.dir = transform.forward;

            UseStamina();
        }
    }
    
    /// <summary>
    /// Checks if the player is close enough to perform an attack
    /// </summary>
    void Update()
    {
        if(staminaRefreshing + Time.deltaTime <= maxStamina)
            staminaRefreshing += Time.deltaTime;

        if (gameObject.activeInHierarchy)
        {
            if(Vector3.Distance(player.transform.position, this.transform.position) < maxDistance) {
                if ((upWorld && GameManager.instance.inParallelWorld) || (!upWorld && !GameManager.instance.inParallelWorld))
                    return;
                gameObject.transform.LookAt(player.transform);
  
                Attack();

            }

        }
    }

}
