using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Skill;
using Client.GamePlay;

[CreateAssetMenu(fileName = "Heal Skill", menuName = "Skill/Heal", order = 52)]
public class HealSkill : SkillData
{
    public GameObject healPrefab;

    public override void Perform(Transform performer, Transform[] targets)
    {
        Instantiate(healPrefab, performer.position, Quaternion.identity);
        performer.GetComponent<HeroPuppet>()._heroSO.Health = 5;
        // Update UI ...
    }
}
