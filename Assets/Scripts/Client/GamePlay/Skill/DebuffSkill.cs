using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Skill;

[CreateAssetMenu(fileName = "Debuff Skill", menuName = "Skill/Debuff", order = 52)]
public class DebuffSkill : SkillData
{
    public GameObject debuffPrefab;
    public override void Perform(Transform performer, Transform[] targets)
    {
        Instantiate(debuffPrefab, targets[0].position, Quaternion.identity);
    }
}
