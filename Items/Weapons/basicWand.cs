using System;
namespace CsPyMudServer
{
    public class basicWand : Weapon
    {


        public override void getDefaults()
        {
            damage = 5;
            stat = "mgc";
            rarity = 1;
            name = "Basic Wand";
            type = "wand";
        }
    }
}
