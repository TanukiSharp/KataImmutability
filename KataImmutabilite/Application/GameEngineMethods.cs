using KataImmutabilite.Models;

namespace KataImmutabilite.Application;

public static class GameEngineMethods
{
    public static GameEngine MoveCurrentPlayer(this GameEngine engine, int diceThrow)
    {
        var currentPlayer = engine.GetCurrentPlayer();
        var currentTile = engine.GetCurrentTile();

        if (!currentTile.CanPlayerPass(currentPlayer))
        {
            return engine;
        }

        // 6 - (currentPlayer.BoardIndex + diceThrow) % 6
        var nextChallengeIndex = engine.Board.Tiles.FindIndex(currentPlayer.BoardIndex + 1, tile => tile.IsBlocking);
        var newBoardIndex = int.Min(currentPlayer.BoardIndex + diceThrow, nextChallengeIndex);
        var movedPlayer = currentPlayer.MoveToBoardTile(newBoardIndex);
        
        return engine with
        {
            Board = engine.Board with
            {
                Players = engine.Board.Players.SetItem(engine.CurrentPlayerIndex, movedPlayer)
            }
        };
    }
    
    public static GameEngine GrantTile(this GameEngine engine)
    {
        var currentPlayer = engine.GetCurrentPlayer();
        var currentTile = engine.GetCurrentTile();
        if (currentTile.IsGrandChallengeTile())
        {
            return engine with { IsFinished = true };
        }

        if (currentTile.IsBlocking)
        {
            return engine with
            {
                Board = engine.Board with
                {
                    Players = engine.Board.Players.SetItem(engine.CurrentPlayerIndex, currentPlayer.AddTokens(currentTile.QuestionTypes)),
                },
                CurrentPlayerIndex = engine.GetNextPlayerIndex()
            };
        }

        return engine;
    }
    
    public static GameEngine DenyTile(this GameEngine engine)
    {
        return engine with
        {
            CurrentPlayerIndex = engine.GetNextPlayerIndex()
        };
    }
    
    public static BoardTile GetCurrentTile(this GameEngine engine)
    {
        return engine.Board.GetTile(engine.Board.Players[engine.CurrentPlayerIndex].BoardIndex);
    }
    
    public static Player GetCurrentPlayer(this GameEngine engine)
    {
        return engine.Board.Players[engine.CurrentPlayerIndex];
    }
    
    public static int GetNextPlayerIndex(this GameEngine engine)
    {
        return (engine.CurrentPlayerIndex + 1) % engine.Board.Players.Count;
    }
}