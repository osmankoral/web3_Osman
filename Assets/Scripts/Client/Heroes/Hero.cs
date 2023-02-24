using UnityEngine;

namespace Client.Heroes
{
    public abstract class Hero : MonoBehaviour
    {
        protected enum Qualities
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary
        }

        protected enum Types
        {
            Warrior,
            Knight,
            Guardian,
            Paladin,
            Duelist,
            Assassin,
            Priest,
            Shaman
        }

        protected Qualities Quality { get; set; }
        protected Types Type { get; set; }
        protected int Level { get; set; }
        protected float Hp { get; set; }
        protected float Damage { get; set; }
        protected float Dodge { get; set; }
        protected float CriticalChance { get; set; }
        protected float CriticalHit { get; set; }
        protected float Speed { get; set; }
        protected float Defence { get; set; }
    }
}
