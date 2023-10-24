using System;

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum Player
{
    White,
    Black
}

public class ChessPiece
{
    public Player Player { get; set; }
    public PieceType Type { get; set; }

    public ChessPiece(Player player, PieceType type)
    {
        Player = player;
        Type = type;
    }
}

public class ChessBoard
{
    private ChessPiece[,] board;

    public ChessBoard()
    {
        board = new ChessPiece[8, 8];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Установка начальной позиции пешек.
        for (int col = 0; col < 8; col++)
        {
            board[1, col] = new ChessPiece(Player.Black, PieceType.Pawn);
            board[6, col] = new ChessPiece(Player.White, PieceType.Pawn);
        }

        //Ладьи
        board[0, 0] = new ChessPiece(Player.Black, PieceType.Rook);
        board[0, 7] = new ChessPiece(Player.Black, PieceType.Rook);
        board[7, 0] = new ChessPiece(Player.White, PieceType.Rook);
        board[7, 7] = new ChessPiece(Player.White, PieceType.Rook);

        //Кони
        board[0, 1] = new ChessPiece(Player.Black, PieceType.Knight);
        board[0, 6] = new ChessPiece(Player.Black, PieceType.Knight);
        board[7, 1] = new ChessPiece(Player.White, PieceType.Knight);
        board[7, 6] = new ChessPiece(Player.White, PieceType.Knight);

        //Слон
        board[0, 2] = new ChessPiece(Player.Black, PieceType.Bishop);
        board[0, 5] = new ChessPiece(Player.Black, PieceType.Bishop);
        board[7, 2] = new ChessPiece(Player.White, PieceType.Bishop);
        board[7, 5] = new ChessPiece(Player.White, PieceType.Bishop);

        //Ферзи
        board[0, 3] = new ChessPiece(Player.Black, PieceType.Queen);
        board[7, 3] = new ChessPiece(Player.White, PieceType.Queen);

        //Короли
        board[0, 4] = new ChessPiece(Player.Black, PieceType.King);
        board[7, 4] = new ChessPiece(Player.White, PieceType.King);
    }

    public bool IsKingInCheck(Player player)
    {
        int kingX = -1;
        int kingY = -1;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece piece = board[row, col];
                if (piece != null && piece.Type == PieceType.King && piece.Player == player)
                {
                    kingX = row;
                    kingY = col;
                    break;
                }
            }
            if (kingX != -1) break;
        }

        if (kingX == -1)
        {
            return false;
        }

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece piece = board[row, col];
                if (piece != null && piece.Player != player)
                {
                    if (IsMoveValid(row, col, kingX, kingY))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public ChessPiece GetPieceAtPosition(int x, int y)
    {
        return board[x, y];
    }

    public void DisplayBoard()
    {
        Console.Clear();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Console.Write(GetPieceSymbol(board[row, col]) + " ");
            }
            Console.WriteLine();
        }
    }

    private string GetPieceSymbol(ChessPiece piece)
    {
        if (piece == null) return " ";

        switch (piece.Type)
        {
            case PieceType.Pawn:
                return (piece.Player == Player.White) ? "P" : "p";
            case PieceType.Rook:
                return (piece.Player == Player.White) ? "R" : "r";
            case PieceType.Knight:
                return (piece.Player == Player.White) ? "N" : "n";
            case PieceType.Bishop:
                return (piece.Player == Player.White) ? "B" : "b";
            case PieceType.King:
                return (piece.Player == Player.White) ? "K" : "k";
            case PieceType.Queen:
                return (piece.Player == Player.White) ? "Q" : "q";
            default:
                return " ";
        }
    }

    public bool IsMoveValid(int startX, int startY, int endX, int endY)
    {
        ChessPiece piece = board[startX, startY];

        if (piece == null)
            return false;

        //логика пешек
        if (piece.Type == PieceType.Pawn)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            int direction = (piece.Player == Player.White) ? -1 : 1;
            if (startX + direction == endX && startY == endY && board[endX, endY] == null)
                return true;
            if (startX + direction == endX && Math.Abs(startY - endY) == 1 && board[endX, endY] != null && board[endX, endY].Player != piece.Player)
                return true;
            if (startX + 2 * direction == endX && startY == endY && board[startX + direction, startY] == null && board[endX, endY] == null)
                return true;
        }

        //Логика ладьи
        else if (piece.Type == PieceType.Rook)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            if (startX == endX && startY != endY)
            {
                int step = (startY < endY) ? 1 : -1;
                for (int y = startY + step; y != endY; y += step)
                {
                    if (board[startX, y] != null)
                        return false;
                }
                return true;
            }

            if (startY == endY && startX != endX)
            {
                int step = (startX < endX) ? 1 : -1;
                for (int x = startX + step; x != endX; x += step)
                {
                    if (board[x, startY] != null)
                        return false;
                }
                return true;
            }
        }

        //Логика коня
        else if (piece.Type == PieceType.Knight)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);

            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }

        //логика слона
        else if (piece.Type == PieceType.Bishop)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);

            return dx == dy;
        }

        //Логика Ферзя
        else if (piece.Type == PieceType.Queen)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);

            //Логика движения как у ладьи
            if ((dx == 0 && dy != 0) || (dx != 0 && dy == 0))
            {
                int stepX = (dx == 0) ? 0 : (endX > startX ? 1 : -1);
                int stepY = (dy == 0) ? 0 : (endY > startY ? 1 : -1);

                for (int x = startX + stepX, y = startY + stepY; x != endX || y != endY; x += stepX, y += stepY)
                {
                    if (board[x, y] != null)
                        return false;
                }
                return true;
            }

            //Логика движения как у слона
            if (dx == dy)
            {
                int stepX = (endX > startX) ? 1 : -1;
                int stepY = (endY > startY) ? 1 : -1;

                for (int x = startX + stepX, y = startY + stepY; x != endX || y != endY; x += stepX, y += stepY)
                {
                    if (board[x, y] != null)
                        return false;
                }
                return true;
            }
        }

        //Логика короля
        else if (piece.Type == PieceType.King)
        {
            if (board[endX, endY] != null && board[endX, endY].Player == piece.Player)
                return false;

            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);

            if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1) || (dx == 1 && dy == 1))
            {
                return true;
            }
        }

        return false;
    }

    public bool MakeMove(int startX, int startY, int endX, int endY)
    {
        if (IsMoveValid(startX, startY, endX, endY))
        {
            ChessPiece capturedPiece = board[endX, endY];

            if (capturedPiece != null && capturedPiece.Type == PieceType.King)
            {
                if (capturedPiece.Player == Player.White)
                {
                    Console.WriteLine("Белые победили!");
                }
                else
                {
                    Console.WriteLine("Черные победили!");
                }

                return true;
            }

            board[endX, endY] = board[startX, startY];
            board[startX, startY] = null;

            Player opponent = (board[endX, endY].Player == Player.White) ? Player.Black : Player.White;
            if (IsKingInCheck(opponent))
            {
                Console.WriteLine("Шах!");
            }

            return true;
        }

        return false;
    }
}

class Program
{
    static void Main()
    {
        ChessBoard board = new ChessBoard();
        board.DisplayBoard();

        bool isWhiteTurn = true;

        while (true)
        {
            string input;

            if (isWhiteTurn)
                Console.WriteLine("Ход белых (формат: начальная_позиция конечная_позиция): ");
            else
                Console.WriteLine("Ход черных (формат: начальная_позиция конечная_позиция): ");

            input = Console.ReadLine();

            if (input.Length != 5)
            {
                Console.WriteLine("Неверный формат ввода. Попробуйте еще раз.");
                continue;
            }

            string start = input.Substring(0, 2);
            string end = input.Substring(3, 2);

            int startX, startY, endX, endY;

            if (!ParsePosition(start, out startX, out startY) || !ParsePosition(end, out endX, out endY))
            {
                Console.WriteLine("Неверный формат позиции. Попробуйте еще раз.");
                continue;
            }

            ChessPiece piece = board.GetPieceAtPosition(startX, startY);

            if (piece == null || (isWhiteTurn && piece.Player != Player.White) || (!isWhiteTurn && piece.Player != Player.Black))
            {
                Console.WriteLine("Неверная фигура. Попробуйте еще раз.");
                continue;
            }

            if (!board.MakeMove(startX, startY, endX, endY))
            {
                Console.WriteLine("Недопустимый ход. Попробуйте еще раз.");
                continue;
            }

            isWhiteTurn = !isWhiteTurn;
            board.DisplayBoard();
        }
    }

    private static bool ParsePosition(string input, out int x, out int y)
    {
        x = -1;
        y = -1;

        if (input.Length != 2)
            return false;

        char colChar = input[0];
        char rowChar = input[1];

        if (colChar < 'a' || colChar > 'h' || rowChar < '1' || rowChar > '8')
            return false;

        x = '8' - rowChar;
        y = colChar - 'a';

        return true;
    }
}