using System;
namespace CsPyMudServer
{
    public class Weapon : Item
    {

        // base damage
        public int damage = 0;
        // stat to use as modifier  { "atk", "mgc", "hp" }
        public string stat = "atk";
        // weapon type, unsure if will be used { "sword", "wand", "heavy", "other" }
        public string type = "sword";

        public virtual void getDefaults()
        {
            damage = 1;
        }

        // possible stats
        public string[] pStats = { "atk", "mgc", "hp" };
        // possible types (heavy is for bludgeoning weapons e.g. bat, staff)
        public string[] pTypes = { "sword", "wand", "heavy", "other" };

        public void Initialise()
        {
            getDefaults();

            // checks if using a valid stat
            foreach (string item in pStats)
            {
                if (item.Equals(stat) == false)
                {
                    stat = "atk";
                }
            }

            // checks if has a valid type
            foreach (string item in pTypes)
            {
                if (item.Equals(type) == false)
                {
                    type = "other";
                }
            }
        }

        // called before attacks, for special effects
        public virtual void preAttack(int damage,Enemy target, Player owner)
        {

        }

        // called after attacks, for special effects
        public virtual void postAttack(int damage, Enemy target, Player owner)
        {

        }
    }
}
