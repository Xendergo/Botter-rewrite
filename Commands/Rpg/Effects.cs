using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using StatusEffects;

namespace Commands {
  [Command(22)]
  class Effects : ICommand {
    public string help {get;} = "See a list of all status effects";
    public string helpShort {get;} = "See a list of all status effects";
    public string[] signature {get;} = new string[] {};
    public TypoableString[] aliases {get;} = new TypoableString[] {new TypoableString("effects", 1)};
    public string category {get;} = "Rpg";
    public bool admin {get;} = false;
    private string effects = String.Join('\n', from effect in TypesWithAttribute.GetOrderedTypesWithAttribute<EffectAttribute, IStatusEffect>() select $"`{effect.Item1.name}` - {effect.Item1.description}");
    public async Task Exec(DiscordClient client, Args args, DiscordMessage msg, Guild guild, User user) {
      await msg.RespondAsync(@$"All status effects possible

{effects}");
    }
  }
}