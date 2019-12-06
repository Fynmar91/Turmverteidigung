using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpielObjekte
{
	public class Projektil
	{
		Point MyPosition { get; set; }
		Canvas MySpielbrett { get; set; }
		public Polygon MyForm { get; set; }
		public Ellipse MyKollisionsbox { get; set; }
		public Gegner MyZiel { get; set; }
		double MySchaden { get; set; }

		public Projektil(Canvas spielbrett, Point punkt, Gegner gegner, double schaden)
		{
			MyPosition = punkt;
			MySpielbrett = spielbrett;
			MyZiel = gegner;
			MySchaden = schaden;

			MyForm = new Polygon();
			MyForm.Fill = Brushes.Black;
			MyForm.Points.Add(new Point(0, 2));
			MyForm.Points.Add(new Point(2, 0));
			MyForm.Points.Add(new Point(0, -2));
			MyForm.Points.Add(new Point(-2, 0));
			Canvas.SetLeft(MyForm, MyPosition.X);
			Canvas.SetTop(MyForm, MyPosition.Y);

			MyKollisionsbox = new Ellipse();
			MyKollisionsbox.Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
			MyKollisionsbox.Height = 10;
			MyKollisionsbox.Width = 10;
			Canvas.SetLeft(MyKollisionsbox, punkt.X - 10 / 2);
			Canvas.SetTop(MyKollisionsbox, punkt.Y - 10 / 2);

			MySpielbrett.Children.Add(MyForm);
			MySpielbrett.Children.Add(MyKollisionsbox);
		}

		public void Bewegen(double intervall)
		{
			RotateTransform rotateTransform = new RotateTransform(90 + Math.Atan2(MyPosition.X - MyZiel.MyPosition.X, MyPosition.Y - MyZiel.MyPosition.Y) / Math.PI * 180);

			double velX = Math.Cos((rotateTransform.Angle) * Math.PI / 180) * 200;
			double velY = Math.Sin((rotateTransform.Angle) * Math.PI / 180) * -200;

			Point neuPosition = new Point(MyPosition.X + velX * intervall, MyPosition.Y + velY * intervall);
			MyPosition = neuPosition;

			Canvas.SetLeft(MyForm, MyPosition.X);
			Canvas.SetTop(MyForm, MyPosition.Y);
			Canvas.SetLeft(MyKollisionsbox, MyPosition.X - 10 / 2);
			Canvas.SetTop(MyKollisionsbox, MyPosition.Y - 10 / 2);
		}

		public bool Getroffen(Gegner gegner)
		{
			if (MyKollisionsbox.RenderedGeometry.FillContains(new Point(gegner.MyPosition.X - (MyPosition.X - MyKollisionsbox.ActualWidth / 2),
																		gegner.MyPosition.Y - (MyPosition.Y - MyKollisionsbox.ActualHeight / 2))) && gegner.IstAmLeben())
			{
				gegner.Angreifen(MySchaden);
				return true;
			}

			return false;
		}
	}
}
