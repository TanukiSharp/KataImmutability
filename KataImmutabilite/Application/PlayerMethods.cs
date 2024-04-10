using System.Collections.Immutable;
using KataImmutabilite.Models;

namespace KataImmutabilite.Application;

public static class PlayerExtensions
{
    public static Player AddTokens(this Player player, ImmutableList<QuestionType> tokens)
    {
        return player with
        {
            Tokens = player.Tokens.Union(tokens)
        };
    }
    
    public static Player MoveToBoardTile(this Player player, int boardIndex)
    {
        return player with
        {
            BoardIndex = boardIndex
        };
    }
    
    public static bool HasAllTokens(this Player player, ImmutableList<QuestionType> tokens)
    {
        return tokens.All(token => player.Tokens.Contains(token));
    }
}