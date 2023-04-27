namespace Client.Health
{
    public class HealthSystem
    {
        public float Damage(float health, float damageAmount)
        {
            health -= damageAmount;
            // ...check for min health...
            return health;
        }

        public float Heal(float health, float healAmount)
        {
            health += healAmount;
            // ...check for max health...
            return health;
        }
    }
}