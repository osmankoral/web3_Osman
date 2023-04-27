using UnityEngine;

namespace Client.Skill
{
    public abstract class SkillData : ScriptableObject
    {
        public abstract void Perform(Transform performer, Transform[] targets);
    }
}
