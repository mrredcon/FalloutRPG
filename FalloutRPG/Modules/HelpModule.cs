using Discord.Addons.Interactive;
using Discord.Commands;
using FalloutRPG.Addons;
using FalloutRPG.Constants;
using FalloutRPG.Services;
using FalloutRPG.Util;
using System.Text;
using System.Threading.Tasks;

namespace FalloutRPG.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly HelpService _helpService;

        public HelpModule(HelpService helpService)
        {
            _helpService = helpService;
        }

        [Command]
        [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
        public async Task ShowHelpAsync()
        {
            await _helpService.ShowHelpAsync(Context);
        }

        [Group("character")]
        public class CharacterHelpModule : InteractiveBase<SocketCommandContext>
        {
            private readonly HelpService _helpService;

            public CharacterHelpModule(HelpService helpService)
            {
                _helpService = helpService;
            }

            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowCharacterHelpAsync()
            {
                await _helpService.ShowCharacterHelpAsync(Context);
            }
        }

        [Group("roll")]
        public class RollHelpModule : InteractiveBase<SocketCommandContext>
        {
            private readonly HelpService _helpService;

            public RollHelpModule(HelpService helpService)
            {
                _helpService = helpService;
            }

            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowRollHelpAsync()
            {
                await _helpService.ShowRollHelpAsync(Context);
            }
        }

        [Group("skills")]
        public class SkillsHelpModule : InteractiveBase<SocketCommandContext>
        {
            private readonly HelpService _helpService;

            public SkillsHelpModule(HelpService helpService)
            {
                _helpService = helpService;
            }

            [Command]
            [Ratelimit(1, Globals.RATELIMIT_SECONDS, Measure.Seconds)]
            public async Task ShowSkillsHelpAsync()
            {
                await _helpService.ShowSkillsHelpAsync(Context);
            }
        }
    }
}
