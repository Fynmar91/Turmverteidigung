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
		public Point MyRichtung { get; set; }
		int MyGeschwindigkeit { get; set; }

		public Gegner(Canvas spielbrett, Point punkt, int groesse, Point richtung, int leben, int geschwindigkeit)
		{
			MyPosition = punkt;
			MySpielbrett = spielbrett;
			MyHP = leben;
			MyRichtung = richtung;
			MyGeschwindigkeit = geschwindigkeit;

			MyForm = new Polygon();
			MyForm.Fill = Brushes.Blue;
			MyForm.Points.Add(new Point(0, groesse * 2));
			MyForm.Points.Add(new Point(groesse, -groesse));
			MyForm.Points.Add(new Point(-groesse, -groesse));
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
			Point neuPosition = new Point(MyPosition.X + MyRichtung.X * MyGeschwindigkeit * intervall, MyPosition.Y + MyRichtung.Y * MyGeschwindigkeit * intervall);
			MyPosition = neuPosition;

			Canvas.SetLeft(MyForm, MyPosition.X);
			Canvas.SetTop(MyForm, MyPosition.Y);
		}
	}
}
