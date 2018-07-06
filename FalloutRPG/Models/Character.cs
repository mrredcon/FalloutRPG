namespace FalloutRPG.Models
{
    public class Character : BaseModel
    {
        public ulong DiscordId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }
        public string Story { get; set; }

        public int Experience { get; set; }
        public int SkillPoints { get; set; }
        public int PerkPoints { get; set; }

        public float SkillPoints { get; set; }

        public Special Special { get; set; }
        public SkillSheet Skills { get; set; }
    }
}
