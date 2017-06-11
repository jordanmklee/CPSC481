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
		Vector mousePosition = new Vector();	// Mouse position relative to image
		
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
				img.Height = 100;
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
			// Find the visibly-topmost Image object the mouse is clicking on
			foreach(Image img in this.canvas.Children)
			{
				if(img.IsMouseOver)
				{
					// Start dragging picture
					isDragging = true;

					// Clear animations so that properties can be changed
					img.BeginAnimation(Canvas.LeftProperty, null);
					img.BeginAnimation(Canvas.TopProperty, null);

					// Remove and add image so that it appears on top
					this.canvas.Children.Remove(img);
					this.canvas.Children.Add(img);

					// Update mouse position (relative to image, for offsets)
					mousePosition.X = e.GetPosition(img).X;
					mousePosition.Y = e.GetPosition(img).Y;

					break;	// No need to check remaining Images
				}
			}
		}

		private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// Stop dragging picture
			isDragging = false;
		}

		private void canvas_MouseMove(object sender, MouseEventArgs e)
		{
			if(isDragging)
			{
				int index = this.canvas.Children.Count - 1;	// Get index of currently dragged image (ie. last index; topmost)
				
				// Offset by saved mouse position 
				this.canvas.Children[index].SetValue(LeftProperty, e.GetPosition(canvas).X - mousePosition.X);
				this.canvas.Children[index].SetValue(TopProperty, e.GetPosition(canvas).Y - mousePosition.Y);
			}
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			// Row and Column counters
			int row = 0;
			int col = 0;

			// Animate each image moving to their corresponding grid locations
			foreach(Image img in this.canvas.Children)
			{
				// Calcuate pixel coordinates based on cell 
				double xCoord = col * 100;
				double yCoord = row * 100;

				// DoubleAnimation to move image to calculated x coordinate
				DoubleAnimation animateX = new DoubleAnimation();
				animateX.To = xCoord;
				animateX.From = Convert.ToInt64(img.GetValue(LeftProperty));
				animateX.Duration = new Duration(TimeSpan.FromMilliseconds(500));
				
				// DoubleAnimation to move image to calcualted y coordinate
				DoubleAnimation animateY = new DoubleAnimation();
				animateY.To = yCoord;
				animateY.From = Convert.ToInt64(img.GetValue(TopProperty));
				animateY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
				
				// Start animation
				img.BeginAnimation(Canvas.LeftProperty, animateX);
				img.BeginAnimation(Canvas.TopProperty, animateY);

				// Increment cell counter
				if((xCoord + 2*(100)) > canvas.Width)
				{
					col = 0;
					row++;
				}
				else
					col++;
			}
		}

		// Dynamically resizes canvas to take up entire window
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			canvas.Width = e.NewSize.Width;
			canvas.Height = e.NewSize.Height;
		}
	}
}
