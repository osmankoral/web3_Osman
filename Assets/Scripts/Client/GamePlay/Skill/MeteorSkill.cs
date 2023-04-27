using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Skill;
using Client.GamePlay;

[CreateAssetMenu(fileName = "Meteor Skill", menuName = "Skill/AOE", order = 52)]
public class MeteorSkill : SkillData
{
    public GameObject meteorPrefab;

    public override void Perform(Transform performer, Transform[] targets)
    {
        var position = new Vector3(targets[0].transform.position.x, 0, targets[0].transform.position.z);
        Instantiate(meteorPrefab, position, Quaternion.identity);
        targets[0].GetComponent<EnemyPuppet>().Damage();
    }
}
