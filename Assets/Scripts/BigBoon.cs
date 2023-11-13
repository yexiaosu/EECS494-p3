using UnityEngine;
using System.Collections;
using System.Numerics;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        if (CanLevelUp)
            CurrLevel++;
    }

    public override void LevelUpBoon()
    {
        PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
        if (CanLevelUp)
            CurrLevel++;
        else
            return;
        if (CurrLevel % 2 == 0)
            playerAttack.ProjectileDamageFactor += 0.4f;
        else
        {
            if (playerAttack.ProjectileCd > 1.0f)
            {
                playerAttack.ProjectileCd -= 0.3f;
                return;
            }
            else if (playerAttack.ProjectileSpeed < 4.0f)
            {
                playerAttack.ProjectileSpeed += 0.4f;
                return;
            }
            else
                playerAttack.ProjectileDamageFactor += 0.4f;
        }
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
        if (CanLevelUp)
            CurrLevel++;
    }

    public override void LevelUpBoon()
    {
        PlayerAttack playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
        if (CanLevelUp)
            CurrLevel++;
        else
            return;
        if (CurrLevel % 2 == 0)
            playerAttack.MissileAttackDamageFactor += 0.4f;
        else
        {
            if (playerAttack.ShootCd > 1.0f)
            {
                playerAttack.ShootCd -= 0.4f;
                return;
            }
            else if (playerAttack.MissileAttackSpeed < 6.0f)
            {
                playerAttack.MissileAttackSpeed += 0.4f;
                return;
            }
            else
                playerAttack.MissileAttackDamageFactor += 0.4f;
        }
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
        if (CanLevelUp)
            CurrLevel++;
    }

    public override void LevelUpBoon()
    {
        return;
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
        if (CanLevelUp)
            CurrLevel++;
    }

    public override void LevelUpBoon()
    {
        PlayerShield playerShield = GameObject.Find("Player").GetComponent<PlayerShield>();
        if (CanLevelUp)
            CurrLevel++;
        else
            return;
        if (CurrLevel % 2 == 0)
            playerShield.MaxShieldTimes += 1;
        else
        {
            if (playerShield.ReGeneratedCd > 1.0f)
            {
                playerShield.ReGeneratedCd -= 0.3f;
                return;
            }
            else
            {
                playerShield.MaxShieldTimes += 1;
                if (playerShield.MaxShieldTimes > 5)
                {
                    CanLevelUp = false;
                }
            }
        }
    }
}
