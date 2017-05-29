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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace A2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Timer timerTimer;
		private int timerTime;

		public MainWindow()
		{
			InitializeComponent();
			
			timerTimer = new Timer(1000);		// Timer ticks once every second
			timerTimer.Elapsed += decrementTimer;
		}

		// Starts timer with input time
		private void button_Click(object sender, RoutedEventArgs e)
		{
			timerTimer.Enabled = false;			// Disable ticking (if resetting a running timer)

			// Pull data from textboxes
			int hour = Int32.Parse(textBoxHour.Text);
			int min = Int32.Parse(textBoxMin.Text);
			int sec = Int32.Parse(textBoxSec.Text);

			// Convert data into time in seconds
			timerTime = convertToSeconds(hour, min, sec);

			// Update display, start ticking
			updateTimer();
			timerTimer.Enabled = true;
		}
		
		// Decrement timer
		private void decrementTimer(object sender, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				timerTime--;					// Decrement 1 second from timer time
				if(timerTime >= 0)
					updateTimer();				// Change timer time label
				else
				{
					timerBlack();				// Reset timer color to black
					timerTimer.Enabled = false;	// Stop ticking
				}
			}));
		}
		
		// Change all timer label colors to black
		private void timerBlack()
		{
			timerHour.Foreground = Brushes.Black;
			timerColon1.Foreground = Brushes.Black;
			timerMin.Foreground = Brushes.Black;
			timerColon2.Foreground = Brushes.Black;
			timerSec.Foreground = Brushes.Black;
		}

		// Change all timer label colors to red
		private void timerRed()
		{
			timerHour.Foreground = Brushes.Red;
			timerColon1.Foreground = Brushes.Red;
			timerMin.Foreground = Brushes.Red;
			timerColon2.Foreground = Brushes.Red;
			timerSec.Foreground = Brushes.Red;
		}

		// Updates the labels for the timer, adding a zero in front of the hours/mins/secs of single digits (to maintain format)
		private void updateTimer ()
		{
			// Convert timer time from seconds into hours, minutes, and seconds
			int[] time = formatSeconds(timerTime);
			int timerTimeHour = time[0];
			int timerTimeMin = time[1];
			int timerTimeSec = time[2];

			// Timer color is red when 00:00:03 to 00:00:00, black otherwise
			if(timerTime >= 0 && timerTime <= 3)
				timerRed();
			else
				timerBlack();

			if(timerTimeHour < 10)
				timerHour.Content = "0" + timerTimeHour;
			else
				timerHour.Content = timerTimeHour;
			if(timerTimeMin < 10)
				timerMin.Content = "0" + timerTimeMin;
			else
				timerMin.Content = timerTimeMin;
			if(timerTimeSec < 10)
				timerSec.Content = "0" + timerTimeSec;
			else
				timerSec.Content = timerTimeSec;
		}

		private int[] formatSeconds (int sec)
		{
			int hour = 0;
			int min = 0;

			while(sec >= 3600)
			{
				hour++;
				sec -= 3600;
			}
			while(sec >= 60)
			{
				min++;
				sec -= 60;
			}

			int[] formattedTime = new int[3] {hour, min, sec};
			return formattedTime;
		}

		private int convertToSeconds(int hour, int min, int sec)
		{
			return (hour * 3600) + (min * 60) + sec;
		}

	}
}
