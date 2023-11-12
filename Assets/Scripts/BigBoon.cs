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

public class MissileAttack : BigBoon
{
    public MissileAttack()
    {
        Name = "Missile Attack";
        Description = "Get missile attack ability. Right click to shoot.";
        CanLevelUp = true;
    }

    public override void GetBoon()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerAttack>().MissileAttackEnabled = true;
    }

    public override void LevelUpBoon()
    {
        throw new System.NotImplementedException();
    }
}

public class DoubleJump : BigBoon
{
    public DoubleJump()
    {
        Name = "Double Jump";
        Description = "Get the ability to double jump.";
        CanLevelUp = false;
    }

    public override void GetBoon()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().DoubleJumpEnabled = true;
    }

    public override void LevelUpBoon()
    {
        throw new System.NotImplementedException();
    }
}

public class Shield : BigBoon
{
    public Shield()
    {
        Name = "Shield";
        Description = "The shield will protect you from damage. It will be regenerated after a while if it's broken.";
        CanLevelUp = true;
    }

    public override void GetBoon()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerShield>().ShieldEnabled = true;
        player.GetComponent<PlayerShield>().Shield.SetActive(true);
    }

    public override void LevelUpBoon()
    {
        throw new System.NotImplementedException();
    }
}
