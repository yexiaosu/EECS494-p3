using UnityEngine;
using System.Collections;
using System.Numerics;

public abstract class BigBoon
{
    public string Name;
    public string Description;
    public int CurrLevel = 0;
    public bool CanLevelUp = false;

    public abstract void GetBoon();

    public abstract void LevelUpBoon();
}

public class RangedProjectiles : BigBoon
{
    public RangedProjectiles()
    {
        Name = "Ranged Projectiles";
        Description = "Get ranged auto tracking projectiles.";
        CanLevelUp = true;
    }

    public override void GetBoon()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerAttack>().ProjectileEnabled = true;
    }

    public override void LevelUpBoon()
    {
        throw new System.NotImplementedException();
    }
}
