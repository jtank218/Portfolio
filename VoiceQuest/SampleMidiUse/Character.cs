using System;

namespace SampleMidiUse
{
    public class Character
    {
        protected int maxHealth;
        protected int currentHealth;
        protected int speed;
        protected int damage;
        public Character(int hp, int dmg)
        {
            maxHealth = hp;
            currentHealth = maxHealth;
            damage = dmg;
        }

        public void takeDamage(int dmgTaken)
        {
            int tempHP = currentHealth - dmgTaken;
            if (tempHP > 0)
            {
                currentHealth = tempHP;
            }
            else
            {
                currentHealth = 0;
            }
        }

        public int dealDamage(bool isDefending)
        {
            if (isDefending) { damage /= 2; }
            return damage;
        }

        public int getHealth() { return currentHealth; }
    }
}
