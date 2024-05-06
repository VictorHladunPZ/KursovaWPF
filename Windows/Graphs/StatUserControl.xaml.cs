using KursovaWPF.Models;
using KursovaWPF.MVVM.CoreViewModels;
using KursovaWPF.Resources;
using KursovaWPF.Resources.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursovaWPF
{
    /// <summary>
    /// Interaction logic for StatUserControl.xaml
    /// </summary>
    public partial class StatUserControl : UserControl
    {
        private List<Category> Categories { get; set; }
        public StatUserControl()
        {
            InitializeComponent();
            
        }
        public void InitDrawing()
        {
            mainCanvas.Children.Clear();
            float pieWidth = 450, pieHeight = 450, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;
            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;
            int id = int.Parse(SearchTextBox.Text.ToString());
            Decimal Cost = ContractRepository.LoadContracts().Where(c => c.ContractId == id).Select(c => c.Cost).First();
            Decimal PaidPart = PaymentRepository.GetContractSum(id);
            float paidPerc = PaidPart > Cost ? 100 : (float)((PaidPart / Cost) * 100);
            float unpaidPerc = paidPerc >= 100 ? 0 : 100 - paidPerc;
            Categories = new List<Category>()
            {
                new Category
                {
                    Title = "Unpaid",
                    Percentage = (float)Math.Ceiling(unpaidPerc),
                    ColorBrush = Brushes.Red,
                },
                new Category
                {
                    Title = "Paid",
                    Percentage = (float)Math.Ceiling(paidPerc),
                    ColorBrush = Brushes.Green,
                },
            };
            detailsItemsControl.ItemsSource = Categories;
            
        }
        private void DrawPie()
        {
            float pieWidth = (float)mainCanvas.Width;
            float pieHeight = (float)mainCanvas.Height;
            float centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;
            float angle = 0, prevAngle = 0;
            if(Categories.Any(p => p.Percentage >= 100))
            {
                Ellipse circle = new Ellipse();
                circle.Width = radius * 2;
                circle.Height = radius * 2;
                circle.Fill = Categories.Where(p => p.Percentage >= 100).Select(p => p.ColorBrush).First(); // Set the fill color to transparent for an outline only
                circle.Stroke = Brushes.Black; // Set the border color
                circle.StrokeThickness = 2; // Set the border thickness

                // Set the position of the circle on the canvas
                Canvas.SetLeft(circle, centerX - radius);
                Canvas.SetTop(circle, centerY - radius);

                // Add the circle to the canvas
                mainCanvas.Children.Add(circle);

            }
            else foreach (var category in Categories)
            {
                double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                angle = category.Percentage * (float)360 / 100 + prevAngle;
                Debug.WriteLine(angle);

                double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                double arcWidth = radius, arcHeight = radius;
                bool isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment()
                {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc,
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>()
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                    },
                    true);

                var pathFigures = new List<PathFigure>() { pathFigure, };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new System.Windows.Shapes.Path()
                {
                    Fill = category.ColorBrush,
                    Data = pathGeometry,
                };
                mainCanvas.Children.Add(path);

                prevAngle = angle;


                // draw outlines
                var outline1 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };
                var outline2 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = Brushes.White,
                    StrokeThickness = 5,
                };

                mainCanvas.Children.Add(outline1);
                mainCanvas.Children.Add(outline2);
            }

        }

        private void GraphButton_Click(object sender, RoutedEventArgs e)
        {
            InitDrawing();
            DrawPie();
        }
    }
}
