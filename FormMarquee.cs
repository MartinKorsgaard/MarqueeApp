namespace MarqueeApp
{
    public partial class FormMarquee : Form
    {

        private static Point _point;
        private static DateTime _maxRuntime = DateTime.MaxValue; // You can control the maximum time the message is displayed
        private static DateTime _minRuntime = DateTime.Now; // you can control if clicking the message will close it immediatly or not until a minimum time has passed
        private static Size _messageSize;
        private static Font _font;
        private static SolidBrush _brush;
        private static StringFormat _drawFormat = new StringFormat() { FormatFlags = StringFormatFlags.NoWrap };
        private static string _message;

        public FormMarquee(string message)
        {
            _message = message;

            //_maxRuntime= DateTime.Now.AddMinutes(1);
            //_minRuntime= DateTime.Now.AddSeconds(15);

            // if message is empty, close app
            if (string.IsNullOrEmpty(_message))
            {
                Application.Exit();
            }

            InitializeComponent();

            // dont show in taskbar
            this.ShowInTaskbar = false;

            // always on top
            this.TopMost = true;

            // remove border
            this.FormBorderStyle = FormBorderStyle.None;

            // enable double buffering to eliminate flickering
            this.DoubleBuffered = true;

            // set backgroundcolor
            this.BackColor = ColorTranslator.FromHtml("#ffffff");

            // set a default font and size
            _font = new Font("Arial", 64);

            // set foregoundcolor
            _brush = new SolidBrush(ColorTranslator.FromHtml("#000000"));

            // set initial size to same as workingarea
            this.Left = Screen.PrimaryScreen.WorkingArea.Left;
            this.Top = Screen.PrimaryScreen.WorkingArea.Top;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;

            this.Show();
        }

        private void SetStart()
        {
            // set message starting point to width of form
            _point.X = this.Width;
        }

        private void FormMarquee_Load(object sender, EventArgs e)
        {
            // now we can calculate how tall the messagebox will be
            _messageSize = TextRenderer.MeasureText(_message, _font);

            // set form height to height of messagebox
            this.Height = _messageSize.Height;

            // set top to height of screen minus the height of the font
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height; 

            // set message top to 0
            _point.Y = 0;

            // set starting point of the messagebox
            SetStart();

            // 33.3 = 30 fps
            // 16.6 = 60 fps
            timerMarquee.Interval = 33;

            // start timer
            timerMarquee.Enabled = true;
        }

        private void timerMarquee_Tick(object sender, EventArgs e)
        {
            // move point backwards
            _point.X -= 1;

            // if point is beyond the leftmost edge minus the messagebox width, then reset
            if (_point.X < this.Left - _messageSize.Width) 
            {
                SetStart();
            }

            if (DateTime.Now > _maxRuntime)
            {
                TransitionOut();
            }

            // force redraw
            this.Refresh();
        }

        private void FormMarquee_Paint(object sender, PaintEventArgs e)
        {
            // Draw string to screen.
            e.Graphics.DrawString(_message, _font, _brush, _point.X, _point.Y, _drawFormat);
        }

        private void FormMarquee_MouseClick(object sender, MouseEventArgs e)
        {
            // if minimum runtime has passed
            if (DateTime.Now > _minRuntime)
                TransitionOut();
        }
        private void TransitionOut()
        {
            // start transition timer
            timerTransitionOut.Enabled = true;
        }

        private void timerTransitionOut_Tick(object sender, EventArgs e)
        {
            // add 1 pixel to the top while taking 1 from the height to make it seem like the form is shrinking downwards
            this.Top += 1;
            this.Height -= 1;

            // if height gets below 2 just close the app
            if (this.Height <= 2)
            {
                Application.Exit();
            }
        }
    }
}