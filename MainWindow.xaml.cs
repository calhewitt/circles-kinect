using System;
using System.IO;
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
using System.Media;
using System.Xml;
using System.Text.RegularExpressions;
using WindowsInput;
using System.Net;
using System.Windows.Media.Animation;
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

        // SoundPlayer cache
        Dictionary<String, SoundPlayer> spcache;

        Point before;

        //Kinect Stuff
        Boolean tracking;
        public String newstate;
        public String coordinates = "hello";
        public String baseHTML = "";
        public String state;
        public int freq;
        public string sampledir = "D:\\Documents\\Development\\kinect music\\samples\\";
        public Boolean panelVisible;
        Boolean started = false;

        string filename = null;

        public MainWindow()
        {
            InitializeComponent();

            //Hide the selection panel
            SelectionPanel.Margin = new Thickness(-260, 40, 0, 0);

            circles = new Dictionary<Ellipse, String>();
            end_tracking();

            spcache = new Dictionary<String, SoundPlayer>();

            panelVisible = false;
            state = null;

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
            Toggletracking.Content = "Stop";

            image1.Visibility = Visibility.Visible;
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
        }
        public void end_tracking()
        {
            tracking = false;
            Indicator.Visibility = Visibility.Hidden;
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
            circle.Margin = new Thickness(300, 200, 0, 0);
            circle.Cursor = Cursors.SizeAll;
            circle.Name = "CIRCLE" + counter.ToString();
            circles.Add(circle, "");
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
            SelectionAction.Text = circles[selected];


            String action = SelectionAction.Text.Split(':')[0].ToLower();
            String data = "";
            if (SelectionAction.Text.Contains(":"))
            {
                data = SelectionAction.Text.Split(new char[] { ':' }, 2)[1];
            }
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
        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (down != null)
            {
                Window wnd = Window.GetWindow(this);
                Point currentLocation = e.MouseDevice.GetPosition(wnd);
                Ellipse ellipse = down;
                ellipse.Margin = new Thickness(currentLocation.X - 50, currentLocation.Y - 50, 0, 0);
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

        //Algorithmto check whether an indicator is inside a circle
        public Boolean inCircle(Ellipse circle)
        {
            Point totest = new Point(Indicator.Margin.Left + 34, Indicator.Margin.Top + 34);
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
                Indicator.Margin = new Thickness(rx, ry, 0, 0);
                newstate = null;
                foreach (FrameworkElement child in Circles.Children)
                {
                    Ellipse circle = (Ellipse)child;
                    if (inCircle(circle))
                    {
                        newstate = circles[circle];
                    }

                }
                if (state != newstate)
                {
         
                    //A new state has been reached, so an action needs to be triggered
                    if (newstate != null)
                    {
                        executeCommand(newstate);
                    }

                    //If original state is midi, release note  
                    if (state != null)
                    {
                        String action = state.Split(':')[0].ToLower();
                        String data = state.Split(new char[] { ':' }, 2)[1];
                        if (action == "midi")
                        {
                            //MessageBox.Show("midi off");
                            String[] midiparams = data.Split(',');
                            if (midiparams[3] == "true")
                            {
                                sendMidiOff(Convert.ToInt32(midiparams[0]), Convert.ToInt32(midiparams[1]), 0);
                            }
                        }
                    }
                }
                state = newstate;
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

        public void executeCommand(String command)
        {
            //MessageBox.Show("executing");
            if (command.Contains(":"))
            {
                String action = command.Split(':')[0].ToLower();
                String data = command.Split(new char[] { ':' }, 2)[1];

                if (data != "")
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new DoWorkEventHandler(
                    delegate(object o, DoWorkEventArgs args)
                    {
                        if (action == "play")
                        {
                            try
                            {
                                if (spcache.ContainsKey(data))
                                {
                                    spcache[data].Stop();
                                    spcache[data].Play();
                                    //MessageBox.Show("Sound file loaded from cache");
                                }
                                else
                                {
                                    SoundPlayer sound = new SoundPlayer(data);
                                    spcache.Add(data, sound);
                                    sound.Play();
                                }
                            }
                            catch (FileNotFoundException)
                            {
                                MessageBox.Show("the file cannot be found");
                            }
                        }
                        else if (action == "midi")
                        {
                            String[] midiparams = data.Split(',');
                            sendMidi(Convert.ToInt32(midiparams[0]), Convert.ToInt32(midiparams[1]), Convert.ToInt32(midiparams[2]));
                        }
                        else if (action == "keypress")
                        {
                            VirtualKeyCode key;
                            switch (data)
                            {
                                case "right":
                                    key = VirtualKeyCode.RIGHT;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "left":
                                    key = VirtualKeyCode.LEFT;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "up":
                                    key = VirtualKeyCode.UP;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "down":
                                    key = VirtualKeyCode.DOWN;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "space":
                                    key = VirtualKeyCode.SPACE;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "back":
                                    key = VirtualKeyCode.BACK;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "enter":
                                    key = VirtualKeyCode.RETURN;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                case "play":
                                    key = VirtualKeyCode.MEDIA_PLAY_PAUSE;
                                    InputSimulator.SimulateKeyPress(key);
                                    break;
                                default:
                                    InputSimulator.SimulateTextEntry(data);
                                    break;
                            }
                        }
                        else if (action == "webrequest")
                        {
                            new StreamReader(WebRequest.Create(data).GetResponse().GetResponseStream()).ReadToEnd();
                        }
                        else if (action == "execute")
                        {
                            System.Diagnostics.Process.Start("CMD.exe", "/C" + data);
                        }
                    });
                    bw.RunWorkerAsync();
                }
            }
        }

        //Function to play midi to output device
        public void sendMidi(int octave, int degreeOfScale, int velocity)
        {
            int note = ((octave + 2) * 12) + degreeOfScale;
            OutputDevice outputDevice = OutputDevice.InstalledDevices[1];
            outputDevice.Open();
            outputDevice.SendNoteOn(Channel.Channel1, (Note)note, velocity);
            //outputDevice.SendNoteOff(Channel.Channel1, (Note)note, 0);
            outputDevice.Close();
        }

        public void sendMidiOff(int octave, int degreeOfScale, int velocity)
        {
            int note = ((octave + 2) * 12) + degreeOfScale;
            OutputDevice outputDevice = OutputDevice.InstalledDevices[1];
            outputDevice.Open();
            outputDevice.SendNoteOff(Channel.Channel1, (Note)note, velocity);
            outputDevice.Close();
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
                        SkeletonPoint rxyz = HandRight.Position;
                        double rx = (double)rxyz.X;
                        double ry = (double)rxyz.Y;

                        SkeletonPoint lxyz = HandRight.Position;
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
    }
}
