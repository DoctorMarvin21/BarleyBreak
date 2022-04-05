using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BarleyBreak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var piece = (PieceViewModel)((Label)e.Source).DataContext;

            if (piece.Column > 0)
            {
                var left = piece.Owner.Pieces[piece.Row][piece.Column - 1];

                if (left.IsEmpty)
                {
                    piece.Owner.Pieces[piece.Row].Move(piece.Column, piece.Column - 1);

                    left.Column++;
                    piece.Column--;

                    return;
                }
            }

            if (piece.Column < BarelyBreakViewModel.Columns - 1)
            {
                var right = piece.Owner.Pieces[piece.Row][piece.Column + 1];

                if (right.IsEmpty)
                {
                    piece.Owner.Pieces[piece.Row].Move(piece.Column, piece.Column + 1);

                    piece.Column++;
                    right.Column--;

                    return;
                }
            }

            if (piece.Row > 0)
            {
                var up = piece.Owner.Pieces[piece.Row - 1][piece.Column];

                if (up.IsEmpty)
                {
                    piece.Owner.Pieces[piece.Row].Remove(piece);
                    piece.Owner.Pieces[piece.Row - 1].Remove(up);

                    piece.Owner.Pieces[piece.Row].Insert(piece.Column, up);
                    piece.Owner.Pieces[piece.Row - 1].Insert(piece.Column, piece);

                    piece.Row--;
                    up.Row++;

                    return;
                }
            }

            if (piece.Row < BarelyBreakViewModel.Columns - 1)
            {
                var down = piece.Owner.Pieces[piece.Row + 1][piece.Column];

                if (down.IsEmpty)
                {
                    piece.Owner.Pieces[piece.Row].Remove(piece);
                    piece.Owner.Pieces[piece.Row + 1].Remove(down);

                    piece.Owner.Pieces[piece.Row].Insert(piece.Column, down);
                    piece.Owner.Pieces[piece.Row + 1].Insert(piece.Column, piece);

                    piece.Row++;
                    down.Row--;

                    return;
                }
            }
        }
    }
}
