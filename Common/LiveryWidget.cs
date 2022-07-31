using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UI
{
    public class LiveryWidget
    {
        public CarColor Primary { get; set; }
        public CarColor Secondary { get; set; }
        public CarColor Ternary { get; set; }

        public Grid LiveryRacing(int size)
        {
            if (size % 2 == 1) size -= 1;
            if (size < 10) size = 10;
            int coord = size * 3 / 5;
            int fat = size / 5;

            Grid UC = new Grid();
            UC.Width = UC.Height = size;

            Polygon prime = new Polygon();
            prime.Fill = (Primary == null) ? Brushes.Transparent : Primary.Brush;
            prime.Height = prime.Width = size;
            prime.HorizontalAlignment = HorizontalAlignment.Center;
            prime.VerticalAlignment = VerticalAlignment.Center;
            Ellipse sec = new Ellipse();
            sec.Fill = (Secondary == null) ? Brushes.Transparent : Secondary.Brush;
            sec.Height = sec.Width = coord;
            sec.HorizontalAlignment = HorizontalAlignment.Center;
            sec.VerticalAlignment = VerticalAlignment.Center;
            Polygon ter = new Polygon();
            ter.Height = ter.Width = size;
            ter.HorizontalAlignment = HorizontalAlignment.Center;
            ter.VerticalAlignment = VerticalAlignment.Center;
            ter.Fill = (Ternary == null) ? Brushes.Transparent : Ternary.Brush;

            prime.Points = new PointCollection() {
                new Point(0, 0),
                new Point(size, 0),
                new Point(size, size),
                new Point(0, size),
            };

            ter.Points = new PointCollection() {
                new Point(size / 2 - fat / 2, 0),
                new Point(size / 2 + fat / 2, 0),
                new Point(size / 2 + fat / 2, size),
                new Point(size / 2 - fat / 2, size)
            };

            UC.Children.Add(prime);
            UC.Children.Add(ter);
            UC.Children.Add(sec);

            return UC;
        }

        public Grid LiveryClassic(int size)
        {
            if (size % 2 == 1) size -= 1;
            if (size < 10) size = 10;
            int coord = size * 2 / 3;
            int fat = size / 10;

            Grid UC = new Grid();
            UC.Width = UC.Height = size;

            Polygon prime = new Polygon();
            prime.Height = prime.Width = size;
            prime.HorizontalAlignment = HorizontalAlignment.Center;
            prime.VerticalAlignment = VerticalAlignment.Center;
            prime.Fill = (Primary == null) ? Brushes.Transparent : Primary.Brush;

            Polygon sec = new Polygon();
            sec.Height = sec.Width = size;
            sec.HorizontalAlignment = HorizontalAlignment.Center;
            sec.VerticalAlignment = VerticalAlignment.Center;
            sec.Fill = (Secondary == null) ? Brushes.Transparent : Secondary.Brush;

            Polygon ter = new Polygon();
            ter.Height = ter.Width = size;
            ter.HorizontalAlignment = HorizontalAlignment.Center;
            ter.VerticalAlignment = VerticalAlignment.Center;
            ter.Fill = (Ternary == null) ? Brushes.Transparent : Ternary.Brush;

            prime.Points = new PointCollection() {
                new Point(0, 0),
                new Point(size, 0),
                new Point(size, size),
                new Point(0, size),
            };

            sec.Points = new PointCollection() {
                new Point(size, 0),
                new Point(size, size / 2),
                new Point(size / 2, size),
                new Point(0, size),
            };

            ter.Points = new PointCollection() {
                new Point(size, coord),
                new Point(size, coord + fat),
                new Point(coord + fat, size),
                new Point(coord, size),
            };

            UC.Children.Add(prime);
            UC.Children.Add(sec);
            UC.Children.Add(ter);

            return UC;
        }

        public Grid LiveryGradient(int size)
        {
            if (size % 2 == 1) size -= 1;
            if (size < 10) size = 10;
            int coord = size * 2 / 3;
            int fat = size / 10;

            Grid UC = new Grid();
            UC.Width = UC.Height = size;

            Polygon prime = new Polygon();
            prime.Height = prime.Width = size;
            prime.HorizontalAlignment = HorizontalAlignment.Center;
            prime.VerticalAlignment = VerticalAlignment.Center;

            SolidColorBrush c1 = (Primary == null) ? Brushes.Transparent : Primary.Brush;
            SolidColorBrush c2 = (Secondary == null) ? Brushes.Transparent : Secondary.Brush;
            GradientStopCollection gsc = new GradientStopCollection(new List<GradientStop>()
            {
                new GradientStop(c1.Color, 0),
                new GradientStop(c1.Color, .15),
                new GradientStop(c2.Color, .85),
                new GradientStop(c2.Color, 1)
            }); ;
            //LinearGradientBrush gradient = new LinearGradientBrush(c1.Color, c2.Color, 135);
            LinearGradientBrush gradient = new LinearGradientBrush(gsc);
            gradient.StartPoint = new Point(1, 0);
            gradient.EndPoint = new Point(0, 1);

            prime.Fill = gradient;

            //Polygon sec = new Polygon();
            //sec.Height = sec.Width = size;
            //sec.HorizontalAlignment = HorizontalAlignment.Center;
            //sec.VerticalAlignment = VerticalAlignment.Center;
            //sec.Fill =;

            Polygon ter = new Polygon();
            ter.Height = ter.Width = size;
            ter.HorizontalAlignment = HorizontalAlignment.Center;
            ter.VerticalAlignment = VerticalAlignment.Center;
            ter.Fill = (Ternary == null) ? Brushes.Transparent : Ternary.Brush;

            prime.Points = new PointCollection() {
                new Point(0, 0),
                new Point(size, 0),
                new Point(size, size),
                new Point(0, size),
            };

            ter.Points = new PointCollection() {
                new Point(size, coord),
                new Point(size, coord + fat),
                new Point(coord + fat, size),
                new Point(coord, size),
            };

            UC.Children.Add(prime);
            UC.Children.Add(ter);

            return UC;
        }
    }
}
