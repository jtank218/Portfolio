using System;
namespace SampleMidiUse
{
    public class Player : Character
    {
        private Array weaponList; //Probably will be in a different class, but for the moment building it here
        private int totalDamage; //equal to damage from weapon plus base damage (damage in Character)
        private Boolean defend = false;
        public Player(int hp, int dmg) : base(hp, dmg)
        {
            totalDamage = dmg; // + weaponList.weaponDamage;
        }

        public void action(int action)
        {
            //player choses to defend, attack or use item defend = 0, attack = 1, item = 2
            if (action == 0)
            {
                defend = true; //when defend is true, the character will take reduced damage from an attack on the next turn
            }
            else if (action == 1)
            {
                //playerSings, probably will have some methods for that
                //dealDamage();
            }
            else if (action == 2)
            {
                //method to use items
            }
        }

        public int dealDamage(int diff)
        {
            int finalDmg;

            // alter totalDamage based on how close to the requested note the player is, set it to finalDamage.
            finalDmg = (int)(totalDamage * (1 - (diff * .2)));

            Console.WriteLine("\n" + "Dealt " + finalDmg + " damage!" + "\n");

            return finalDmg;
        }
    }
}
