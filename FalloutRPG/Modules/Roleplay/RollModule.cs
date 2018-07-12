using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Services.Roleplay;
using System.Threading.Tasks;

namespace FalloutRPG.Modules.Roleplay
{
    [Group("roll")]
    [Alias("r")]
    [Ratelimit(Globals.RATELIMIT_TIMES, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
    public class RollModule : ModuleBase<SocketCommandContext>
    {
        private readonly RollService _rollService;
        private readonly HelpService _helpService;

        public RollModule(RollService rollService, HelpService helpService)
        {
            _rollService = rollService;
            _helpService = helpService;
        }

        [Command]
        [Alias("help")]
        public async Task ShowRollHelpAsync()
        {
            await _helpService.ShowRollHelpAsync(Context);
        }

        #region SPECIAL Commands
        [Command("strength")]
        [Alias("str")]
        public async Task RollStrength() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Strength")} ({Context.User.Mention})");

        [Command("perception")]
        [Alias("per")]
        public async Task RollPerception() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Perception")} ({Context.User.Mention})");

        [Command("endurance")]
        [Alias("end")]
        public async Task RollEndurance() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Endurance")} ({Context.User.Mention})");

        [Command("charisma")]
        [Alias("cha")]
        public async Task RollCharisma() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Charisma")} ({Context.User.Mention})");

        [Command("intelligence")]
        [Alias("int")]
        public async Task RollIntelligence() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Intelligence")} ({Context.User.Mention})");

        [Command("agility")]
        [Alias("agi")]
        public async Task RollAgility() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Agility")} ({Context.User.Mention})");

        [Command("luck")]
        [Alias("luc", "lck")]
        public async Task RollLuck() => await ReplyAsync($"{await _rollService.GetSpRollAsync(Context.User, "Luck")} ({Context.User.Mention})");
        #endregion

        #region Skills Commands
        [Command("barter")]
        public async Task RollBarter() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Barter")} ({Context.User.Mention})"); 

        [Command("energy weapons")]
        [Alias("energy weapon", "energyweapons", "energyweapon", "energy")]
        public async Task RollEnergyWeapons() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "EnergyWeapons")} ({Context.User.Mention})");

        [Command("explosives")]
        public async Task RollExplosives() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Explosives")} ({Context.User.Mention})");

        [Command("guns")]
        public async Task RollGuns() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Guns")} ({Context.User.Mention})");

        [Command("lockpick")]
        public async Task RollLockpick() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Lockpick")} ({Context.User.Mention})");

        [Command("medicine")]
        [Alias("medic", "doctor")]
        public async Task RollMedicine() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Medicine")} ({Context.User.Mention})");

        [Command("meleeweapons")]
        [Alias("melee", "meleeweapon", "melee weapons", "melee weapons")]
        public async Task RollMeleeWeapons() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "MeleeWeapons")} ({Context.User.Mention})");

        [Command("repair")]
        public async Task RollRepair() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Repair")} ({Context.User.Mention})");

        [Command("science")]
        public async Task RollScience() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Science")} ({Context.User.Mention})");

        [Command("sneak")]
        public async Task RollSneak() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Sneak")} ({Context.User.Mention})");

        [Command("speech")]
        public async Task RollSpeech() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Speech")} ({Context.User.Mention})");

        [Command("survival")]
        public async Task RollSurvival() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Survival")} ({Context.User.Mention})");

        [Command("unarmed")]
        public async Task RollUnarmed() => await ReplyAsync($"{await _rollService.GetSkillRollAsync(Context.User, "Unarmed")} ({Context.User.Mention})");
        #endregion
    }
}
