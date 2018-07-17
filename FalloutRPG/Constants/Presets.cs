using FalloutRPG.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutRPG.Constants
{
    public class SpecialPresets
    {
        private static Random rand = new Random();

        // lowest special total = 29, highest special total = 44
        public static Special Raider => new Special
        {
            Strength     = 5 + RngAdd(2), // 5-7
            Perception   = 4 + RngAdd(2), // 4-6
            Endurance    = 5 + RngAdd(2), // 5-7
            Charisma     = 3 + RngAdd(2), // 3-5
            Intelligence = 3 + RngAdd(3), // 3-6
            Agility      = 5 + RngAdd(2), // 5-7
            Luck         = 4 + RngAdd(2), // 4-6
        };

        // lowest SPECIAL total = 36, highest SPECIAL total = 47
        public static Special RaiderVeteran => new Special
        {
            Strength     = 6 + RngAdd(2), // 6-8
            Perception   = 4 + RngAdd(2), // 4-6
            Endurance    = 6 + RngAdd(1), // 6-7
            Charisma     = 4 + RngAdd(1), // 4-5
            Intelligence = 5 + RngAdd(2), // 5-7
            Agility      = 7 + RngAdd(1), // 7-8
            Luck         = 4 + RngAdd(2), // 4-6
        };

        // lowest SPECIAL total = 40, highest SPECIAL = 51
        public static Special Mercenary => new Special
        {
            Strength     = 5 + RngAdd(2), // 5-7
            Perception   = 6 + RngAdd(1), // 6-7
            Endurance    = 6 + RngAdd(2), // 6-8
            Charisma     = 6 + RngAdd(1), // 6-7
            Intelligence = 6 + RngAdd(2), // 6-8
            Agility      = 7 + RngAdd(1), // 7-8
            Luck         = 4 + RngAdd(2), // 4-6
        };

        // lowest SPECIAL total = 41, highest SPECIAL = 46
        public static Special Merchant => new Special
        {
            Strength     = 6 + RngAdd(2), // 6-7
            Perception   = 5 + RngAdd(2), // 5-6
            Endurance    = 6,
            Charisma     = 7 + RngAdd(2), // 7-8
            Intelligence = 6 + RngAdd(2), // 6-7
            Agility      = 5 + RngAdd(2), // 5-6
            Luck         = 6,
        };

        private static int RngAdd(int highest)
        {
            return rand.Next(0, highest + 1);
        }
    }
}
