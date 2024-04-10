using KataImmutabilite.Models;

namespace KataImmutabilite.Application;

public static class BoardExtensions
{
    public static BoardTile GetTile(this Board board, int index)
    {
        return board.Tiles[index];
    }
}