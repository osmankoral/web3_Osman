using UnityEngine;
using System.Collections.Generic;
using Client.Skill;
using Client.Health;


public enum Name
{
    Aurelion,
    Katya,
    Zinzar
}

public enum Qualities
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum Types
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

[CreateAssetMenu(fileName = "New Hero", menuName = "Hero", order = 51)]
public class HeroSO : ScriptableObject
{
    // external fields
    [SerializeField] private List<SkillData> _skills;
    public HealthSystem _healthSystem;

    // internal fields
    [SerializeField] private Name _name;
    [SerializeField] private Qualities _quality;
    [SerializeField] private Types _type;
    [SerializeField] private int _level;
    [SerializeField] private int _health;
    [SerializeField] private float _damage;
    [SerializeField] private float _dodge;
    [SerializeField] private float _criticalChance;
    [SerializeField] private float _criticalHit;
    [SerializeField] private float _speed;
    [SerializeField] private float _defence;

    // properties
    public Name Name { get { return _name; } set { _name = value; } }
    public Qualities Quality { get { return _quality; } set { _quality = value; } }
    public Types Type { get { return _type; } set { _type = value; } }
    public int Level { get { return _level; } set { _level = value; } }
    public int Health { get { return _health; } set { _health = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float Dodge { get { return _dodge; } set { _dodge = value; } }
    public float CriticalChance { get { return _criticalChance; } set { _criticalChance = value; } }
    public float CriticalHit { get { return _criticalHit; } set { _criticalHit = value; } }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public float Defence { get { return _defence; } set { _defence = value; } }
}