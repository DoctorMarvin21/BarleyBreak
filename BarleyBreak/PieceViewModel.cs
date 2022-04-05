using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace BarleyBreak
{
    public class PieceViewModel : INotifyPropertyChanged
    {
        private int column;
        private int row;
        private bool displayPiece;

        private double x;
        private double y;

        public PieceViewModel(BarelyBreakViewModel owner, BitmapSource piece, int index, bool isEmpty)
        {
            Owner = owner;
            Piece = piece;
            Index = index;
            IsEmpty = isEmpty;
        }

        public BarelyBreakViewModel Owner { get; set; }

        public BitmapSource Piece { get; }

        public int Index { get; }

        public bool IsEmpty { get; }

        public int Column
        {
            get => column;
            set
            {
                column = value;
                OnPropertyChanged();

                OldX = NewX;
                NewX = Column * Piece.Width;
            }
        }

        public int Row
        {
            get => row;
            set
            {
                row = value;
                OnPropertyChanged();

                OldY = NewY;
                NewY = Row * Piece.Height;
            }
        }

        public double OldX { get; set; }

        public double NewX { get; set; }

        public double OldY { get; set; }

        public double NewY { get; set; }

        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayPiece
        {
            get => displayPiece;
            set
            {
                displayPiece = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
