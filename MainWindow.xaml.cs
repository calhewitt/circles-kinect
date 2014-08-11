using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Xml;
using System.Text.RegularExpressions;
using WindowsInput;
using System.Net;
using System.Windows.Media.Animation;
using System.Speech.Synthesis;
using System.ComponentModel;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf;
using Midi;

namespace Circles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ellipse down = null;
        Ellipse selected = null;
        String[] colors = { "#32ff32", "#ffa500", "#1E90FF", "#CD0000" };
        int counter = 0;
        Dictionary<Ellipse, String> circles;
        
        //Velocity
        double rVelocity;
        double lVelocity;


        // SoundPlayer cache
        Dictionary<String, SoundPlayer> spcache;

        Point before;

        //Kinect Stuff
        Boolean tracking;
        public String newstate;
        public String newlstate;
        public String coordinates = "hello";
        public String baseHTML = "";
        public String state;
        public String lstate;
        public int freq;
        public Boolean panelVisible;
        Boolean started = false;

        string filename = null;

        BrushConverter brushconverter;

        public MainWindow()
        {
            InitializeComponent();

            //Hide the selection panel
            SelectionPanel.Margin = new Thickness(-260, 40, 0, 0);

            circles = new Dictionary<Ellipse, String>();
            end_tracking();

            spcache = new Dictionary<String, SoundPlayer>();

            List<String> midiOptions = new List<String>();
            int counter = 0;
            foreach (OutputDevice device in OutputDevice.InstalledDevices) {
                midiOptions.Add(counter.ToString() + ": " + device.Name);
                counter++;
            }
            
            port_selector.ItemsSource = midiOptions;

            panelVisible = false;
            state = null;

            //Set up brush convertor
            brushconverter = new BrushConverter();

            //Set up combo boxes
            comboxBoxSetup();

        }
        public void toggle_tracking(object sender, RoutedEventArgs e)
        {
            if (tracking) end_tracking();
            else start_tracking();
        }
        public void start_tracking()
        {
            destroy_selection();
            tracking = true;
            Indicator.Visibility = Visibility.Visible;
            lIndicator.Visibility = Visibility.Visible;
            Toggletracking.Content = "Stop";

            //image1.Visibility = Visibility.Visible;
            //If a kinect is available, start using it
            if (KinectSensor.KinectSensors.Count != 0)
            {
                if (started == false)
                {
                    startKinect();
                }
            }
            else
            {
                debugMode();
                //There are no kinect sensors available, notify the user
            }
            velocity();
        }
        public void end_tracking()
        {
            tracking = false;
            Indicator.Visibility = Visibility.Hidden;
            lIndicator.Visibility = Visibility.Hidden;
            Toggletracking.Content = "Start";
            image1.Visibility = Visibility.Hidden;
        }
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
        public void new_circle()
        {
            //Create a new circle
            Ellipse circle = new Ellipse();
            circle.Width = 150;
            circle.Height = 150;
            circle.Opacity = 0.7;
            circle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[(counter % 4)]));
            circle.MouseDown += circle_Down;
            circle.MouseUp += circle_Up;
            circle.HorizontalAlignment = HorizontalAlignment.Left;
            circle.VerticalAlignment = VerticalAlignment.Top;
            circle.Cursor = Cursors.SizeAll;
            circle.Name = "CIRCLE" + counter.ToString();
            circle.Margin = new Thickness(300, 300, 0, 0);
            circles.Add(circle, "midi:C0,127,$DEFAULT_PORT");
            Circles.Children.Add(circle);
            counter++;
        }
        private void newcircle_click(object sender, RoutedEventArgs e)
        {
            new_circle();
        }
        private void circle_Down(object sender, MouseEventArgs e)
        {
            down = (Ellipse)sender;
            before = new Point(down.Margin.Left, down.Margin.Top);
        }
        private void circle_Up(object sender, MouseEventArgs e)
        {
            down = null;
            selected = (Ellipse)sender;
            if (new Point(selected.Margin.Left, selected.Margin.Top) == before)
            {
                selectionMade(selected);
            }
        }
        public void selectionMade(Ellipse selected)
        {
            selected.StrokeThickness = 2;
            selected.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
            showSelectionPanel();
            //SelectionName.Content = selected.Name;
            SelectionSize.Value = (selected.Width - 50) / 20;

            String actionValue = circles[selected];
            String action = actionValue.Split(':')[0].ToLower();
            String data = "";
            if (SelectionAction.Text.Contains(":"))
            {
                data = actionValue.Split(new char[] { ':' }, 2)[1];
            }

            //if the circle has a simple midi action, use the GUI selector
            string[] midiParams = data.Split(',');
            n1selector.SelectedValue = midiParams[0];
            VelocitySelector.Value = Convert.ToInt32(midiParams[1]);

            SelectionAction.Text = actionValue;
        }
        private void selectionAction_textChanged(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (selected != null)
            {
                circles[selected] = tb.Text;
            }
        }
        private void SelectionSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            selected.Width = (slider.Value * 20) + 50;
            selected.Height = (slider.Value * 20) + 50;
            //MessageBox.Show(slider.Value.ToString());
        }
        private void updateAction(object sender, RoutedEventArgs e)
        {
            SelectionAction.Text = "midi:" + n1selector.SelectedValue.ToString() + "," + VelocitySelector.Value.ToString() + ",$DEFAULT_PORT";
        }
        public void comboxBoxSetup()
        {
            for (int i = 0; i <= 7; i++)
            {
                n1selector.Items.Add("C" + i.ToString());
                n1selector.Items.Add("C#" + i.ToString());
                n1selector.Items.Add("D" + i.ToString());
                n1selector.Items.Add("D#" + i.ToString());
                n1selector.Items.Add("E" + i.ToString());
                n1selector.Items.Add("F" + i.ToString());
                n1selector.Items.Add("F#" + i.ToString());
                n1selector.Items.Add("G" + i.ToString());
                n1selector.Items.Add("G#" + i.ToString());
                n1selector.Items.Add("A" + i.ToString());
                n1selector.Items.Add("A#" + i.ToString());
                n1selector.Items.Add("B" + i.ToString());
            }
        }
        private void container_down(object sender, MouseEventArgs e)
        {
            destroy_selection();
        }
        public void destroy_selection()
        {
            hideSelectionPanel();
            if (selected != null)
            {
                selected.StrokeThickness = 0;
            }
        }
        public void showSelectionPanel()
        {
            if (panelVisible != true)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(-260, 40, 0, 0);
                animation.To = new Thickness(0, 40, 0, 0);
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTargetName(animation, SelectionPanel.Name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Panel.MarginProperty));
                storyboard.Begin(this);
                panelVisible = true;
            }
        }
        public void hideSelectionPanel()
        {
            if (panelVisible == true)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.To = new Thickness(-260, 40, 0, 0);
                animation.From = new Thickness(0, 40, 0, 0);
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTargetName(animation, SelectionPanel.Name);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Panel.MarginProperty));
                storyboard.Begin(this);
                panelVisible = false;
            }
        }

        private void test_actions(object sender, RoutedEventArgs e)
        {
            foreach (string command in circles[selected].Split(';'))
            {
                try
                {
                    executeCommand(command, new String[] { "127" });
                }
                catch
                {
                    MessageBox.Show("You must specify a midi output port");
                }
            }   
        }
        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (down != null)
            {
                Window wnd = Window.GetWindow(this);
                Point currentLocation = e.MouseDevice.GetPosition(wnd);
                Ellipse ellipse = down;
                ellipse.Margin = new Thickness(currentLocation.X - down.Width / 2, currentLocation.Y - down.Width / 2, 0, 0);
            }
        }

        private void closeButton_mouseEnter(object sender, MouseEventArgs e)
        {
            Label close = (Label)sender;
            close.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151515"));
        }

        private void closeButton_mouseLeave(object sender, MouseEventArgs e)
        {
            Label close = (Label)sender;
            close.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555"));
        }

        private void closeButton_click(object sender, MouseEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Algorithm to check whether an indicator is inside a circle
        public Boolean inCircle(Ellipse circle, Ellipse indicator)
        {
            Point totest = new Point(Canvas.GetLeft(indicator) + 34, Canvas.GetTop(indicator) + 34);
            Point circleCenter = new Point(circle.Margin.Left + (circle.Width / 2), circle.Margin.Top + (circle.Width / 2));
            double a = Math.Abs(totest.X - circleCenter.X);
            double b = Math.Abs(totest.Y - circleCenter.Y);
            double distance = Math.Sqrt((a*a) + (b*b));
            if (distance < (circle.Width / 2)) return true;
            else return false;
        }

        //Called when new coordinates are recieved
        public void gotCoordinates(double rx, double ry, double lx, double ly)
        {
            if (tracking)
            {
                Canvas.SetLeft(Indicator, rx);
                Canvas.SetTop(Indicator, ry);
                Canvas.SetLeft(lIndicator, lx);
                Canvas.SetTop(lIndicator, ly);
                newstate = null;
                newlstate = null;
                Ellipse activecircle = null;
                Ellipse lactivecircle = null;
                foreach (FrameworkElement child in Circles.Children)
                {
                    Ellipse circle = (Ellipse)child;
                    if (inCircle(circle, Indicator))
                    {
                        newstate = circles[circle];
                        activecircle = circle;
                    }
                    if (inCircle(circle, lIndicator)) {
                        newlstate = circles[circle];
                        lactivecircle = circle;
                    }
                }
                if (state != newstate)
                {
         
                    //A new state has been reached, so an action needs to be triggered
                    if (newstate != null)
                    {
                        foreach (string command in newstate.Split(';'))
                        {
                            
                            executeCommand(command, new String[] {rVelocity.ToString()});
                        }                        
                    }
                }
                state = newstate;
                
                //And the same for the left hand...
                if (lstate != newlstate)
                {
         
                    //A new state has been reached, so an action needs to be triggered
                    if (newlstate != null)
                    {
                        foreach (string command in newlstate.Split(';'))
                        {
                            executeCommand(newlstate, new String[] { lVelocity.ToString() });
                        }
                    }
                }
                lstate = newlstate;
            }
        }

        //Remove all circles from the canvas
        public void blank_canvas()
        {
            List<FrameworkElement> elementslist = new List<FrameworkElement>();
            foreach (FrameworkElement child in Circles.Children)
            {
                elementslist.Add(child);
            }
            foreach (FrameworkElement child in elementslist)
            {
                Ellipse circle = (Ellipse)child;
                Circles.Children.Remove(circle);
                destroy_selection();
            }
        }

        public void executeCommand(String command, String[] commandargs)
        {
            command = command.Trim();
            //MessageBox.Show("executing");
            if (command.Contains(":"))
            {
                command = command.Replace("$VELOCITY", commandargs[0]);
                if (port_selector.SelectedItem.ToString().Contains(":"))
                {
                    command = command.Replace("$DEFAULT_PORT", port_selector.SelectedItem.ToString().Split(':')[0]);
                }
                else
                {
                    command = command.Replace("$DEFAULT_PORT", "0");
                }
                String action = command.Split(':')[0].ToLower();
                String data = command.Split(new char[] { ':' }, 2)[1];

                if (data != "")
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new DoWorkEventHandler(
                    delegate(object o, DoWorkEventArgs args)
                    {                        
                        if (action == "midi")
                        {
                            String[] midiparams = data.Split(',');
                            sendMidi(midiparams[0], Convert.ToInt32(Math.Round(Convert.ToDouble(midiparams[1]))), Convert.ToInt32(midiparams[2]));
                        }
                    });
                    bw.RunWorkerAsync();
                }
            }
        }

        //Function to play midi to output device
        public void sendMidi(String notename, int velocity, int device)
        {            
            try
            {
                int octave, degreeOfScale;
                //parse note name
                Dictionary<string, int> notes = new Dictionary<string, int>()
                {
                    {"C", 0},
                    {"C#", 1},
                    {"D", 2},
                    {"D#", 3},
                    {"E", 4},
                    {"F", 5},
                    {"F#", 6},
                    {"G", 7},
                    {"G#", 8},
                    {"A", 9},
                    {"A#", 10},
                    {"B", 11},
                };
                notename = notename.ToUpper();
                octave = 0;
                if (notename[1] == '#')
                {
                    degreeOfScale = notes[notename.Substring(0, 2)];
                    octave = Convert.ToInt32(notename.Substring(2));
                }
                else
                {
                    degreeOfScale = notes[notename[0].ToString()];
                    octave = Convert.ToInt32(notename.Substring(1));
                }
                int note = ((octave + 2) * 12) + degreeOfScale;
                OutputDevice outputDevice = OutputDevice.InstalledDevices[device];
                if (outputDevice.IsOpen != true) outputDevice.Open();
                outputDevice.SendNoteOn(Channel.Channel1, (Note)note, velocity);
                //outputDevice.SendNoteOff(Channel.Channel1, (Note)note, 0);     
                outputDevice.Close();
            }
            catch {
                MessageBox.Show("An error occurred - check that the Midi port that you are trying to send on is open", "Midi Error");
            }            
        }

        private void save_to_file(object sender, RoutedEventArgs e)
        {
            if (filename == null)
            {
                save_as(null, null);
            }
            else
            {
                string xml = getXML();

                //Write xml to file
                System.IO.File.WriteAllText(@filename, xml);
            }
        }

        private void save_as(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".ckp"; // Default file extension
            dlg.Filter = "Circles for Kinect Projects |*.ckp"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
                //MessageBox.Show(filename);

                //Convert circles to xml
                string xml = getXML();

                //Write xml to file
                System.IO.File.WriteAllText(@filename, xml);
            }
        }

        private void open_file(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ckp";
            dlg.Filter = "Circles for Kinect Projects |*.ckp";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                blank_canvas();
                filename = dlg.FileName;
                string xml = System.IO.File.ReadAllText(@filename);
                loadXML(xml);
            }
        }

        public string getXML()
        {
            string xml = "<circles>\n";
            foreach (FrameworkElement child in Circles.Children)
            {
                Ellipse circle = (Ellipse)child;
                xml = xml + "\t<circle size = '" + circle.Width.ToString() + "' locx = '" + circle.Margin.Left.ToString() + "' locy = '" + circle.Margin.Top.ToString() + "' color = '" + circle.Fill.ToString() + "' action = '" + circles[circle] + "'/>\n";
            }
            xml = xml + "</circles>";
            return xml;
        }

        public void loadXML(string xml)
        {
            xml = xml.Replace("&", "&amp;");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            for (int i = 0; i < Regex.Split(xml, "<circle ").Length - 1; i++)
            {
                double size = Double.Parse(xmlDoc.SelectSingleNode("circles/circle[" + (i + 1).ToString() + "]/@size").InnerText);
                double locx = Int32.Parse(xmlDoc.SelectSingleNode("circles/circle[" + (i + 1).ToString() + "]/@locx").InnerText);
                double locy = Int32.Parse(xmlDoc.SelectSingleNode("circles/circle[" + (i + 1).ToString() + "]/@locy").InnerText);
                string color = xmlDoc.SelectSingleNode("circles/circle[" + (i + 1).ToString() + "]/@color").InnerText;
                string action = xmlDoc.SelectSingleNode("circles/circle[" + (i + 1).ToString() + "]/@action").InnerText.Replace("&amp;", "&");

                //Create a new circle on the canvas with the right dimensions etc
                Ellipse circle = new Ellipse();
                circle.Width = size;
                circle.Height = size;
                circle.Opacity = 0.7;
                circle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                circle.MouseDown += circle_Down;
                circle.MouseUp += circle_Up;
                circle.HorizontalAlignment = HorizontalAlignment.Left;
                circle.VerticalAlignment = VerticalAlignment.Top;
                circle.Margin = new Thickness(locx, locy, 0, 0);
                circle.Cursor = Cursors.SizeAll;
                circle.Name = "CIRCLE" + counter.ToString();
                circles.Add(circle, action);
                Circles.Children.Add(circle);
                counter++;
            }
        }


        private void window_keypress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    if (selected != null)
                    {
                        Circles.Children.Remove(selected);
                        destroy_selection();
                    }
                    break;
                case Key.Back:
                    if (selected != null)
                    {
                        Circles.Children.Remove(selected);
                        destroy_selection();
                    }
                    break;

                //Use arrow keys to emulate kinedct sensor, mainly for debugging
                case Key.Down:
                    gotCoordinates(Indicator.Margin.Left, Indicator.Margin.Top + 20, 0, 0);
                    break;
                case Key.Up:
                    gotCoordinates(Indicator.Margin.Left, Indicator.Margin.Top - 20, 0, 0);
                    break;
                case Key.Right:
                    gotCoordinates(Indicator.Margin.Left + 20, Indicator.Margin.Top, 0, 0);
                    break;
                case Key.Left:
                    gotCoordinates(Indicator.Margin.Left - 20, Indicator.Margin.Top, 0, 0);
                    break;
            }
        }

        //Kinect Setup code

        KinectSensor kinect = null;
        Skeleton[] skeletonData = null;

        void startKinect()
        {
            started = true;
            kinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
            kinect.SkeletonStream.Enable();
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            skeletonData = new Skeleton[kinect.SkeletonStream.FrameSkeletonArrayLength];

            kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
            kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinect_ColorFrameReady);

            kinect.Start();
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null && this.skeletonData != null)
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);
                }
            }
            getKinectData();
        }

        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            ColorImageFrame imageData = e.OpenColorImageFrame();
            image1.Source = imageData.ToBitmapSource();
        }


        //Process recieved data and retrieve details for right hand

        private void getKinectData()
        {
            foreach (Skeleton skeleton in this.skeletonData)
            {
                try
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {

                        Joint HandRight = skeleton.Joints[JointType.HandRight].ScaleTo(800, 600, .65f, .5f);
                        Joint HandLeft = skeleton.Joints[JointType.HandLeft].ScaleTo(800, 600, .65f, .5f);
                        SkeletonPoint rxyz = HandRight.Position;
                        double rx = (double)rxyz.X;
                        double ry = (double)rxyz.Y;

                        SkeletonPoint lxyz = HandLeft.Position;
                        double lx = (double)lxyz.X;
                        double ly = (double)lxyz.Y;

                        gotCoordinates(rx, ry, lx, ly);

                    }
                    else if (skeleton.TrackingState == SkeletonTrackingState.PositionOnly) { }
                }
                catch
                {
                    //Error...
                }
            }
        }
        public void debugMode()
        {
            MessageBox.Show("Debug mode started");
            BackgroundWorker bw = new BackgroundWorker();            
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                
                while (tracking == true)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        Point mousepos = Mouse.GetPosition(Application.Current.MainWindow);
                        gotCoordinates(mousepos.X - 34 , mousepos.Y - 34, 0, 0);
                    }, null);                    
                } 
            });
            bw.RunWorkerAsync();
        }
        
        //Creates a separate thread for tracking velocity
        public void velocity()
        {
            BackgroundWorker bw = new BackgroundWorker();            
            bw.DoWork += new DoWorkEventHandler(
            delegate(object o, DoWorkEventArgs args)
            {
                Point oldrPos = new Point(0,0);
                Point oldlPos = new Point(0,0);
                Application.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        oldrPos = new Point(Canvas.GetLeft(Indicator), Canvas.GetTop(Indicator));
                        oldlPos = new Point(Canvas.GetLeft(lIndicator), Canvas.GetTop(lIndicator));
                    });
                while (tracking)
                {
                    System.Threading.Thread.Sleep(60);
                    Application.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        Point rPos = new Point(Canvas.GetLeft(Indicator), Canvas.GetTop(Indicator));
                        Point lPos = new Point(Canvas.GetLeft(lIndicator), Canvas.GetTop(lIndicator));
                        double rxoffset = Math.Abs(rPos.X - oldrPos.X);
                        double ryoffset = Math.Abs(rPos.Y - oldrPos.Y);
                        double lxoffset = Math.Abs(lPos.X - oldlPos.X);
                        double lyoffset = Math.Abs(lPos.Y - oldlPos.Y);

                        double roffset = Math.Sqrt(Math.Pow(rxoffset, 2) + Math.Pow(ryoffset, 2));
                        double loffset = Math.Sqrt(Math.Pow(lxoffset, 2) + Math.Pow(lyoffset, 2));

                        if (roffset * 2.4 < 128) rVelocity = roffset * 2.4;
                        else rVelocity = 127;
                        if (loffset * 2.4 < 128) lVelocity = loffset * 2.4;
                        else lVelocity = 127;

                        oldrPos = rPos;
                        oldlPos = lPos;

                        rVelocityMonitor.Content = rVelocity.ToString();
                        lVelocityMonitor.Content = lVelocity.ToString();
                    });
                }
            });
            bw.RunWorkerAsync();
        }

    }
}
