using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [Header("Skill")]
    public SkillBase nowSkill;

    [Header("Ultimate")]
    public UltBase nowUltimate;

    private int count = 0;
    public int nowUltCount
    {
        get
        {
            return count;
        }
        set
        {
            count = Mathf.Clamp(value, 0 , maxUltCount);
        }
    }
    public int maxUltCount = 3;

    public void UseSkill()
    {
        nowSkill.UseSkill(ChargeUltimate);
    }

    public void ChargeUltimate()
    {
        nowUltCount += 1;
    }

    public void UseUltimate()
    {
        if (maxUltCount > nowUltCount)
        {
            nowUltCount = 0;
            nowUltimate.UseUltimate();
        }
    }
}
