using FalloutRPG.Models;
using System;

namespace FalloutRPG.Constants
{
    public static class NpcSpecialPresets
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

        // lowest SPECIAL total = 39, highest SPECIAL = 46
        public static Special Mercenary => new Special
        {
            Strength     = 6 + RngAdd(1), // 6-7
            Perception   = 7,
            Endurance    = 4 + RngAdd(2), // 4-6
            Charisma     = 5 + RngAdd(1), // 5-6
            Intelligence = 4 + RngAdd(2), // 4-6
            Agility      = 8,
            Luck         = 5 + RngAdd(1), // 5-6
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

        // lowest SPECIAL total = 41, highest SPECIAL = 46
        public static Special Spy => new Special
        {
            Strength     = 4 + RngAdd(3), // 4-7
            Perception   = 7 + RngAdd(1), // 7-8
            Endurance    = 7 + RngAdd(2), // 7-9
            Charisma     = 4 + RngAdd(1), // 4-5
            Intelligence = 7 + RngAdd(1), // 7-8
            Agility      = 8 + RngAdd(1), // 8-9
            Luck         = 4,
        };

        // lowest SPECIAL total = 32, highest SPECIAL = 45
        public static Special SuperMutant => new Special
        {
            Strength     = 8 + RngAdd(3), // 8-11
            Perception   = 4 + RngAdd(2), // 4-6
            Endurance    = 7 + RngAdd(3), // 7-10
            Charisma     = 2 + RngAdd(1), // 2-3
            Intelligence = 2 + RngAdd(2), // 2-4
            Agility      = 4 + RngAdd(2), // 4-6
            Luck         = 5,
        };

        // lowest SPECIAL total = 23, highest SPECIAL = 32
        public static Special FeralGhoul => new Special
        {
            Strength     = 6 + RngAdd(2), // 6-8
            Perception   = 2 + RngAdd(2), // 2-4
            Endurance    = 3 + RngAdd(3), // 3-6
            Charisma     = 1,
            Intelligence = 1,
            Agility      = 7 + RngAdd(2), // 7-9
            Luck         = 3,
        };

        // lowest SPECIAL total = 41, highest SPECIAL = 42
        public static Special Protectron => new Special
        {
            Strength     = 7,
            Perception   = 6,
            Endurance    = 7,
            Charisma     = 5,
            Intelligence = 7,
            Agility      = 3 + RngAdd(1), // 3-4
            Luck         = 6,
        };

        // lowest SPECIAL total = 45, highest SPECIAL = 52
        public static Special Assaultron => new Special
        {
            Strength     = 7 + RngAdd(1), // 7-8
            Perception   = 7 + RngAdd(1), // 7-8
            Endurance    = 7 + RngAdd(2), // 7-9
            Charisma     = 5 + RngAdd(1), // 5-6
            Intelligence = 6 + RngAdd(1), // 6-7
            Agility      = 8 + RngAdd(1), // 8-9
            Luck         = 5,
        };

        // lowest SPECIAL total = 37, highest SPECIAL = 42
        public static Special Eyebot => new Special
        {
            Strength     = 3 + RngAdd(1), // 3-4
            Perception   = 7,
            Endurance    = 4 + RngAdd(2), // 4-6
            Charisma     = 4 + RngAdd(1), // 4-5
            Intelligence = 7,
            Agility      = 6 + RngAdd(1), // 6-7
            Luck         = 6,
        };

        // lowest SPECIAL total = 39, highest SPECIAL = 47
        public static Special MisterHandy => new Special
        {
            Strength     = 6, // 6
            Perception   = 7 + RngAdd(1), // 7-8
            Endurance    = 6 + RngAdd(2), // 6-8
            Charisma     = 4 + RngAdd(3), // 4-7
            Intelligence = 7 + RngAdd(1), // 7-8
            Agility      = 4 + RngAdd(1), // 4-5
            Luck         = 5,
        };

        // lowest SPECIAL total = 44, highest SPECIAL = 50
        public static Special MisterGutsy => new Special
        {
            Strength     = 6 + RngAdd(1), // 6-7
            Perception   = 7 + RngAdd(1), // 7-8
            Endurance    = 7 + RngAdd(1), // 7-8
            Charisma     = 4 + RngAdd(1), // 4-5
            Intelligence = 7 + RngAdd(1), // 7-8
            Agility      = 9 + RngAdd(1), // 9-10
            Luck         = 4,
        };

        // lowest SPECIAL total = 39, highest SPECIAL = 44
        public static Special Robobrain => new Special
        {
            Strength     = 4 + RngAdd(1), // 4-5
            Perception   = 7 + RngAdd(1), // 7-8
            Endurance    = 7,
            Charisma     = 4,
            Intelligence = 9 + RngAdd(1), // 9-10
            Agility      = 3 + RngAdd(2), // 3-5
            Luck         = 5,
        };

        // lowest SPECIAL total = 45, highest SPECIAL = 52
        public static Special SentryBot => new Special
        {
            Strength     = 8 + RngAdd(3), // 8-11
            Perception   = 7,
            Endurance    = 10 + RngAdd(2), // 10-12
            Charisma     = 2,
            Intelligence = 6 + RngAdd(1), // 6-7
            Agility      = 8 + RngAdd(1), // 8-9
            Luck         = 4,
        };

        // lowest SPECIAL total = 45, highest SPECIAL = 59
        public static Special Securitron => new Special
        {
            Strength     = 7 + RngAdd(1), // 7-8
            Perception   = 6 + RngAdd(2), // 6-8
            Endurance    = 8 + RngAdd(3), // 7-11
            Charisma     = 5 + RngAdd(1), // 5-6
            Intelligence = 5 + RngAdd(1), // 5-6
            Agility      = 7 + RngAdd(1), // 7-8
            Luck         = 5 + RngAdd(5), // vegas baby
        };

        private static int RngAdd(int highest)
        {
            return rand.Next(0, highest + 1);
        }
    }
}
