using System;
using System.Linq;
namespace BarleyBreak
{
    public enum MoveDirection
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public class BarelyBreakViewModel
    {
        public const int Rows = 4;
        public const int Columns = 4;
        public const int PiecesCount = Rows * Columns;

        public BarelyBreakViewModel()
        {
            Pieces = new PieceViewModel[PiecesCount];
            var image = BarelyBreakUtils.LoadImage("Image.png");
            var cut = BarelyBreakUtils.CutImage(image, Rows, Columns);
            Width = image.Width;
            Height = image.Height;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int index = i * Columns + j;
                    bool isEmpty = index == PiecesCount - 1;

                    Pieces[index] = new PieceViewModel(this, cut[i, j], index, isEmpty)
                    {
                        Row = i,
                        Column = j,
                        DisplayPiece = !isEmpty
                    };
                }
            }

            Shuffle(1000 * PiecesCount);
            CheckComplete();
        }

        public bool Done { get; private set; }

        public double Width { get; }

        public double Height { get; }

        public PieceViewModel[] Pieces { get; }

        public void Shuffle(int steps)
        {
            // Actual moving is faster then search so let's move pieces and then re-order
            Random rnd = new Random(Environment.TickCount);

            var emptyPiece = Pieces[PiecesCount - 1];

            for (int i = 0; i < steps; i++)
            {
                var direction = (MoveDirection)rnd.Next(0, 5);
                int emptyIndex = emptyPiece.Row * Columns + emptyPiece.Column;

                if (direction == MoveDirection.Left && emptyPiece.Column > 0)
                { 
                    var left = Pieces[emptyIndex - 1];

                    Pieces[emptyIndex] = left;
                    Pieces[emptyIndex - 1] = emptyPiece;

                    left.Column++;
                    emptyPiece.Column--;
                }
                else if (direction == MoveDirection.Right && emptyPiece.Column < Columns - 1)
                {
                    var right = Pieces[emptyIndex + 1];

                    Pieces[emptyIndex] = right;
                    Pieces[emptyIndex + 1] = emptyPiece;

                    emptyPiece.Column++;
                    right.Column--;
                }
                else if (direction == MoveDirection.Up && emptyPiece.Row > 0)
                {
                    var up = Pieces[emptyIndex - Columns];

                    Pieces[emptyIndex] = up;
                    Pieces[emptyIndex - Columns] = emptyPiece;

                    emptyPiece.Row--;
                    up.Row++;
                }
                else if (direction == MoveDirection.Down && emptyPiece.Row < Columns - 1)
                {
                    var down = Pieces[emptyIndex + Columns];

                    Pieces[emptyIndex] = down;
                    Pieces[emptyIndex + Columns] = emptyPiece;

                    emptyPiece.Row++;
                    down.Row--;
                }
            }

            for (int i = 0; i < Pieces.Length; i++)
            {
                Pieces[i].X = Pieces[i].NewX;
                Pieces[i].Y = Pieces[i].NewY;
            }

            Array.Sort(Pieces, (x, y) => x.Index - y.Index);
            Done = false;
        }

        public MoveDirection ClickMove(PieceViewModel piece)
        {
            if (Done)
            {
                return MoveDirection.None;
            }

            if (piece.Column > 0)
            {
                var left = Pieces.First(x => x.Row == piece.Row && x.Column == piece.Column - 1);

                if (left.IsEmpty)
                {
                    left.Column++;
                    piece.Column--;

                    return MoveDirection.Left;
                }
            }

            if (piece.Column < Columns - 1)
            {
                var right = Pieces.First(x => x.Row == piece.Row && x.Column == piece.Column + 1);

                if (right.IsEmpty)
                {
                    piece.Column++;
                    right.Column--;

                    return MoveDirection.Right;
                }
            }

            if (piece.Row > 0)
            {
                var up = Pieces.First(x => x.Row == piece.Row - 1 && x.Column == piece.Column);

                if (up.IsEmpty)
                {
                    piece.Row--;
                    up.Row++;

                    return MoveDirection.Up;
                }
            }

            if (piece.Row < Columns - 1)
            {
                var down = Pieces.First(x => x.Row == piece.Row + 1 && x.Column == piece.Column);

                if (down.IsEmpty)
                {
                    piece.Row++;
                    down.Row--;

                    return MoveDirection.Down;
                }
            }

            return MoveDirection.None;
        }

        public PieceViewModel? MoveEmptyPiece(MoveDirection direction)
        {
            var emptyPiece = Pieces[^1];

            switch (direction)
            {
                case MoveDirection.Left:
                    {
                        if (emptyPiece.Column > 0)
                        {
                            var left = Pieces.First(x => x.Row == emptyPiece.Row && x.Column == emptyPiece.Column - 1);

                            left.Column++;
                            emptyPiece.Column--;

                            return left;
                        }

                        break;
                    }
                case MoveDirection.Right:
                    {
                        if (emptyPiece.Column < Columns - 1)
                        {
                            var right = Pieces.First(x => x.Row == emptyPiece.Row && x.Column == emptyPiece.Column + 1);

                            right.Column--;
                            emptyPiece.Column++;

                            return right;
                        }

                        break;
                    }
                case MoveDirection.Up:
                    {
                        if (emptyPiece.Row > 0)
                        {
                            var up = Pieces.First(x => x.Row == emptyPiece.Row - 1 && x.Column == emptyPiece.Column);

                            emptyPiece.Row--;
                            up.Row++;

                            return up;
                        }

                        break;
                    }
                case MoveDirection.Down:
                    {
                        if (emptyPiece.Row < Columns - 1)
                        {
                            var down = Pieces.First(x => x.Row == emptyPiece.Row + 1 && x.Column == emptyPiece.Column);

                            emptyPiece.Row++;
                            down.Row--;

                            return down;
                        }

                        break;
                    }
            }

            return null;
        }

        public void CheckComplete()
        {
            for (int i = 0; i < Pieces.Length; i++)
            {
                var piece = Pieces[i];

                if (piece.Row * Columns + piece.Column != piece.Index)
                {
                    return;
                }
            }

            Done = true;

            var lastPiece = Pieces[^1];

            lastPiece.X = lastPiece.NewX;
            lastPiece.Y = lastPiece.NewY;

            lastPiece.DisplayPiece = true;
        }
    }
}
