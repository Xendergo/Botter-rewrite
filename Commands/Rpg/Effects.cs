using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Commands {
  class Effects : ICommand {
    public string help {get;} = "See a list of all status effects";
    public string helpShort {get;} = "See a list of all status effects";
    public string[] signature {get;} = new string[] {};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("effects", 1)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      // There's no effects registry so I can't auto-generate the help message for them
      await msg.RespondAsync(@"All status effects possible

Illness - Reduces the amount of damage you deal in a fight
Regen - Regenerate health over time
Stun - Prevents you from executing battle commands system");
    }
  }
}