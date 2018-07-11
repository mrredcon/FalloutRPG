using Discord;
using Discord.Commands;
using FalloutRPG.Services.Roleplay;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("admin")]
    [Alias("adm")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly CharacterService _charService;
        private readonly SkillsService _skillsService;
        private readonly SpecialService _specialService;

        public AdminModule(CharacterService charService,
            SkillsService skillsService,
            SpecialService specialService)
        {
            _charService = charService;
            _skillsService = skillsService;
            _specialService = specialService;
        }

        [Command]
        public async Task ShowAdminHelpAsync()
        {

        }

        [Command("givemoney")]
        public async Task GiveMoneyAsync(IUser user, int money)
        {
            if (money < 1) return;

            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            character.Money += money;

            await _charService.SaveCharacterAsync(character);
            await Context.Channel.SendMessageAsync("Gave money successfully");
        }

        [Command("giveskillpoints")]
        public async Task GiveSkillPointsAsync(IUser user, int points)
        {
            if (points < 1) return;

            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            character.SkillPoints += points;

            await _charService.SaveCharacterAsync(character);
            await Context.Channel.SendMessageAsync("Gave skill points successfully");
        }

        [Command("delete")]
        public async Task DeleteCharacterAsync(IUser user)
        {
            var character = await _charService.GetCharacterAsync(user.Id);
            if (character == null) return;

            await _charService.DeleteCharacterAsync(character);
            await Context.Channel.SendMessageAsync("Character deleted successfully");
        }
    }
}
