using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A3
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool isDragging = false;
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Canvas_Drop(object sender, DragEventArgs e)
		{
			String[] droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];	// Array of dropped file addresses

			// Converts each dropped file to a bitmap, and adds it as an image to the canvas
			foreach(String path in droppedFiles)
			{
				BitmapImage bmap = new BitmapImage();
				bmap.BeginInit();
				bmap.UriSource = new Uri(path);
				bmap.EndInit();

				Image img = new Image();
				img.Source = bmap;
				img.Width = 100;
				img.Stretch = Stretch.Fill;
				// Offset by 50 so mouse is in middle of image
				img.SetValue(Canvas.LeftProperty, e.GetPosition(canvas).X - 50);
				img.SetValue(Canvas.TopProperty, e.GetPosition(canvas).Y - 50);
			
				this.canvas.Children.Add(img);

				DoubleAnimation move = new DoubleAnimation();
				img.BeginAnimation(LeftProperty, move);
				img.BeginAnimation(TopProperty, move);
			}
		}

		private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// TODO: Consider using a for loop to iterate backwards (items added are overlapped, so topmost have a larger index)
			foreach(Image img in this.canvas.Children)
			{
				if(img.IsMouseOver)
				{
					isDragging = true;
					// Remove and add image so that it appears on top
					this.canvas.Children.Remove(img);
					this.canvas.Children.Add(img);
					break;	// No need to check remaining children
				}
			}
		}

		private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isDragging = false;
		}

		private void canvas_MouseMove(object sender, MouseEventArgs e)
		{
			if(isDragging)
			{
				int index = this.canvas.Children.Count - 1;	// Get index of currently dragged image

				this.canvas.Children[index].SetValue(LeftProperty, e.GetPosition(canvas).X-50);
				this.canvas.Children[index].SetValue(TopProperty, e.GetPosition(canvas).Y-50);
			}
		}
	}
}
