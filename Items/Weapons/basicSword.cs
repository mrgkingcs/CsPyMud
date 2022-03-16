using System;
namespace CsPyMudServer
{
    public class basicSword : Weapon
    {


        public override void getDefaults()
        {
            damage = 5;
            stat = "atk";
            rarity = 1;
            name = "Basic Sword";
            type = "sword";
        }
    }
}
