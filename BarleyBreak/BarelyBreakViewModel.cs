using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BarleyBreak
{
    public class BarelyBreakViewModel
    {
        public const int Rows = 4;
        public const int Columns = 4;
        public const int PiecesCount = Rows * Columns;

        public BarelyBreakViewModel()
        {
            Pieces = new ObservableCollection<PieceViewModel>[Rows];

            for (int i = 0; i < Rows; i++)
            {
                Pieces[i] = new ObservableCollection<PieceViewModel>();

                for (int j = 0; j < Columns; j++)
                {
                    int index = i * Columns + j;

                    string tag;
                    bool isEmpty = index == PiecesCount - 1;

                    if (isEmpty)
                    {
                        tag = string.Empty;
                    }
                    else
                    {
                        tag = (index + 1).ToString();
                    }

                    Pieces[i].Add(new PieceViewModel
                    {
                        Owner = this,
                        Row = i,
                        Column = j,
                        Index = index,
                        Tag = tag,
                        IsEmpty = isEmpty
                    });
                }
            }

            Shuffle(0);
        }

        public ObservableCollection<PieceViewModel>[] Pieces { get; }

        public async void Shuffle(int delay = 0)
        {
            // Do this in non-real array
            Random rnd = new Random(Environment.TickCount);

            var emptyPiece = Pieces[Rows - 1][Columns - 1];

            for (int i = 0; i < 1000 * PiecesCount; i++)
            {
                int direction = rnd.Next(0, 4);

                if (direction == 0 && emptyPiece.Column > 0)
                {
                    var left = Pieces[emptyPiece.Row][emptyPiece.Column - 1];

                    Pieces[emptyPiece.Row].Move(emptyPiece.Column, emptyPiece.Column - 1);

                    left.Column++;
                    emptyPiece.Column--;

                    await Task.Delay(delay);
                }
                else if (direction == 1 && emptyPiece.Column < Columns - 1)
                {
                    var right = Pieces[emptyPiece.Row][emptyPiece.Column + 1];

                    Pieces[emptyPiece.Row].Move(emptyPiece.Column, emptyPiece.Column + 1);

                    emptyPiece.Column++;
                    right.Column--;

                    await Task.Delay(delay);
                }
                else if (direction == 2 && emptyPiece.Row > 0)
                {
                    var up = Pieces[emptyPiece.Row - 1][emptyPiece.Column];

                    Pieces[emptyPiece.Row].Remove(emptyPiece);
                    Pieces[emptyPiece.Row - 1].Remove(up);

                    Pieces[emptyPiece.Row].Insert(emptyPiece.Column, up);
                    Pieces[emptyPiece.Row - 1].Insert(emptyPiece.Column, emptyPiece);

                    emptyPiece.Row--;
                    up.Row++;

                    await Task.Delay(delay);
                }
                else if (direction == 3 && emptyPiece.Row < Columns - 1)
                {
                    var down = Pieces[emptyPiece.Row + 1][emptyPiece.Column];

                    Pieces[emptyPiece.Row].Remove(emptyPiece);
                    Pieces[emptyPiece.Row + 1].Remove(down);

                    Pieces[emptyPiece.Row].Insert(emptyPiece.Column, down);
                    Pieces[emptyPiece.Row + 1].Insert(emptyPiece.Column, emptyPiece);

                    emptyPiece.Row++;
                    down.Row--;

                    await Task.Delay(delay);
                }
            }
        }
    }

    public class PieceViewModel
    {
        public BarelyBreakViewModel Owner { get; set; }

        public int Index { get; set; }

        public string Tag { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public bool IsEmpty { get; set; }
    }
}
