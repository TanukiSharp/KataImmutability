using KataImmutabilite.Models;

namespace KataImmutabilite.Application;

public static class BoardTileMethods
{
    public static bool IsGrandChallengeTile(this BoardTile tile)
    {
        return tile.QuestionTypes.Count == Enum.GetValues<QuestionType>().Length;
    }
    
    public static bool CanPlayerPass(this BoardTile tile, Player player)
    {
        return !tile.IsGrandChallengeTile() && (!tile.IsBlocking || player.HasAllTokens(tile.QuestionTypes));
    }
}