using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BarleyBreak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MoveAnimationTime = 200;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PieceMouseDown(object sender, MouseButtonEventArgs e)
        {
            var piece = (PieceViewModel)((FrameworkElement)e.Source).DataContext;

            var direction = piece.Owner.ClickMove(piece);

            if (direction == MoveDirection.Left || direction == MoveDirection.Right)
            {
                MoveX(piece, (Image)e.Source);
            }
            else if (direction == MoveDirection.Up || direction == MoveDirection.Down)
            {
                MoveY(piece, (Image)e.Source);
            }
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (BarelyBreakViewModel)DataContext;

            switch (e.Key)
            {
                case Key.Left:
                    {
                        var result = viewModel.MoveEmptyPiece(MoveDirection.Right);

                        if (result != null)
                        {
                            var image = BarelyBreakUtils.FindVisualChilds<Image>(this, (obj) => obj.DataContext == result).First();
                            MoveX(result, image);
                        }

                        break;
                    }
                case Key.Right:
                    {
                        var result = viewModel.MoveEmptyPiece(MoveDirection.Left);

                        if (result != null)
                        {
                            var image = BarelyBreakUtils.FindVisualChilds<Image>(this, (obj) => obj.DataContext == result).First();
                            MoveX(result, image);
                        }

                        break;
                    }
                case Key.Up:
                    {
                        var result = viewModel.MoveEmptyPiece(MoveDirection.Down);

                        if (result != null)
                        {
                            var image = BarelyBreakUtils.FindVisualChilds<Image>(this, (obj) => obj.DataContext == result).First();
                            MoveY(result, image);
                        }

                        break;
                    }
                case Key.Down:
                    {
                        var result = viewModel.MoveEmptyPiece(MoveDirection.Up);

                        if (result != null)
                        {
                            var image = BarelyBreakUtils.FindVisualChilds<Image>(this, (obj) => obj.DataContext == result).First();
                            MoveY(result, image);
                        }

                        break;
                    }
            }
        }

        public static void MoveX(PieceViewModel piece, Image target)
        {
            var parent = (UIElement)target.Parent;

            TranslateTransform transform = new TranslateTransform();
            parent.RenderTransform = transform;
            DoubleAnimation xAnimation = new DoubleAnimation(0, piece.NewX - piece.OldX, TimeSpan.FromMilliseconds(MoveAnimationTime));

            xAnimation.Completed += (s, e) =>
            {
                parent.RenderTransform = Transform.Identity;
                piece.X = piece.NewX;
                piece.Owner.CheckComplete();
            };

            transform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
        }

        public static void MoveY(PieceViewModel piece, Image target)
        {
            var parent = (UIElement)target.Parent;

            TranslateTransform transform = new TranslateTransform();
            parent.RenderTransform = transform;
            DoubleAnimation yAnimation = new DoubleAnimation(0, piece.NewY - piece.OldY, TimeSpan.FromMilliseconds(MoveAnimationTime));

            yAnimation.Completed += (s, e) =>
            {
                parent.RenderTransform = Transform.Identity;
                piece.Y = piece.NewY;
                piece.Owner.CheckComplete();
            };

            transform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (BarelyBreakViewModel)DataContext;
            viewModel.Shuffle();
        }
    }
}
