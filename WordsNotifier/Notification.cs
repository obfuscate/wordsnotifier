// =====COPYRIGHT=====
// Code originally retrieved from http://www.vbforums.com/showthread.php?t=547778 - no license information supplied
// =====COPYRIGHT=====
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ToastNotifications
{
    public enum Position
    {
        HorizontalLeft = 0x0001,
        HorizontalCenter = 0x0002,
        HorizontalRight = 0x0008,
        VerticalTop = 0x0004,
        VerticalCenter = 0x0010,
        VerticalBottom = 0x0020,
    }

    public partial class Notification : Form
    {
        private static readonly List<Notification> OpenNotifications = new List<Notification>();
        private bool _allowFocus;
        private readonly FormAnimator _animator;
        private Position _position;
        private IntPtr _currentForegroundWindow;

        private WordsNotifier.Form1 _mainForm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="duration"></param>
        /// <param name="animation"></param>
        /// <param name="direction"></param>
        public Notification(WordsNotifier.Form1 form, string title, string body, int duration, FormAnimator.AnimationMethod animation, FormAnimator.AnimationDirection direction, Position position)
        {
            InitializeComponent();

            if (duration < 0)
                duration = int.MaxValue;
            else
                duration = duration * 1000;

            lifeTimer.Interval = duration;
            labelTitle.Text = title;
            labelBody.Text = body;

            _position = position;

            _animator = new FormAnimator(this, animation, direction, 500);

            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 20, 20));

            _mainForm = form;
        }

        #region Methods

        /// <summary>
        /// Displays the form
        /// </summary>
        /// <remarks>
        /// Required to allow the form to determine the current foreground window before being displayed
        /// </remarks>
        public new void Show()
        {
            // Determine the current foreground window so it can be reactivated each time this form tries to get the focus
            _currentForegroundWindow = NativeMethods.GetForegroundWindow();

            base.Show();
        }

        #endregion // Methods

        #region Event Handlers

        private void Notification_Load(object sender, EventArgs e)
        {
            Point location = new Point();
            if (_position.HasFlag(Position.HorizontalLeft))
            {
                location.X = 0;
            }
            else if (_position.HasFlag(Position.HorizontalCenter))
            {
                location.X = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            }
            else if (_position.HasFlag(Position.HorizontalRight))
            {
                location.X = Screen.PrimaryScreen.WorkingArea.Width - Width;
            }

            if (_position.HasFlag(Position.VerticalTop))
            {
                location.Y = 0;
            }
            else if (_position.HasFlag(Position.VerticalCenter))
            {
                location.Y = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
            }
            else if (_position.HasFlag(Position.VerticalBottom))
            {
                location.Y = Screen.PrimaryScreen.WorkingArea.Height - Height;
            }

            // Display the form just above the system tray.
            Location = location;

            // Move each open form upwards to make room for this one
            foreach (Notification openForm in OpenNotifications)
            {
                openForm.Top -= Height;
            }

            OpenNotifications.Add(this);
            lifeTimer.Start();
        }

        private void Notification_Activated(object sender, EventArgs e)
        {
            // Prevent the form taking focus when it is initially shown
            if (!_allowFocus)
            {
                // Activate the window that previously had focus
                NativeMethods.SetForegroundWindow(_currentForegroundWindow);
            }
        }

        private void Notification_Shown(object sender, EventArgs e)
        {
            // Once the animation has completed the form can receive focus
            _allowFocus = true;

            // Close the form by sliding down.
            _animator.Duration = 0;
            _animator.Direction = FormAnimator.AnimationDirection.Down;
        }

        private void Notification_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Move down any open forms above this one
            foreach (Notification openForm in OpenNotifications)
            {
                if (openForm == this)
                {
                    // Remaining forms are below this one
                    break;
                }
                openForm.Top += Height;
            }

            OpenNotifications.Remove(this);
        }

        private void lifeTimer_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
            _mainForm.StartShowTimer();
        }

        private void Notification_MouseEnter(object sender, EventArgs e)
        {
            lifeTimer.Stop();
            _mainForm.StopShowTimer();
        }

        private void Notification_MouseLeave(object sender, EventArgs e)
        {
            lifeTimer.Start();
            _mainForm.StartShowTimer();
        }

        #endregion // Event Handlers
    }
}