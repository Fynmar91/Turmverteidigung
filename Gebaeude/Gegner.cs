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
	public class Gegner
	{
		public Point MyPosition { get; set; }
		Canvas MySpielbrett { get; set; }
		public Polygon MyForm { get; set; }
		double MyHP { get; set; }

		public Gegner(Canvas spielbrett, Point punkt)
		{
			MyPosition = punkt;
			MySpielbrett = spielbrett;

			MyHP = 5;

			MyForm = new Polygon();
			MyForm.Fill = Brushes.Blue;
			MyForm.Points.Add(new Point(0, 10));
			MyForm.Points.Add(new Point(5, -5));
			MyForm.Points.Add(new Point(-5, -5));
			Canvas.SetLeft(MyForm, MyPosition.X);
			Canvas.SetTop(MyForm, MyPosition.Y);

			MySpielbrett.Children.Add(MyForm);
		}

		public bool IstAmLeben()
		{
			return MyHP > 0;
		}

		public void Angreifen(double schaden)
		{
			MyHP -= schaden;
		}

		public void Bewegen(double intervall)
		{
			Point neuPosition = new Point(MyPosition.X + 0 * intervall, MyPosition.Y + -100 * intervall);
			MyPosition = neuPosition;

			Canvas.SetLeft(MyForm, MyPosition.X);
			Canvas.SetTop(MyForm, MyPosition.Y);
		}
	}
}
