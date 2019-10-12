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
        private IntPtr _hrgn;
        private WordsNotifier.Form1 _mainForm;
        private bool _showFullWords;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="duration"></param>
        /// <param name="animation"></param>
        /// <param name="direction"></param>
        public Notification(WordsNotifier.Form1 form, int duration, FormAnimator.AnimationMethod animation, FormAnimator.AnimationDirection direction, Position position)
        {
            InitializeComponent();

            _mainForm = form;

            if (duration < 0)
                duration = int.MaxValue;
            else
                duration = duration * 1000;

            WordsNotifier.Word w = _mainForm.GetWord(true);

            switch (_mainForm.GetMode())
            {
                case WordsNotifier.Mode.Default:
                {
                        _showFullWords = true;
                        labelTitle.Text = w.word + "[" + w.part + "]";
                        labelBody.Text = w.translation;
                        break;
                }
                case WordsNotifier.Mode.HideTranslation:
                {
                        _showFullWords = false;
                        labelTitle.Text = w.word + "[" + w.part + "]";
                        labelBody.Text = "[click to show translation]";
                        break;
                }
                case WordsNotifier.Mode.Inverse:
                {
                        _showFullWords = false;
                        labelTitle.Text = w.translation + "[" + w.part + "]";
                        labelBody.Text = "[click to show the word]";
                        break;
                }
            }

            lifeTimer.Interval = duration;

            _position = position;

            _animator = new FormAnimator(this, animation, direction, 500);

            _hrgn = NativeMethods.CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 20, 20);
            Region = Region.FromHrgn(_hrgn);
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

            this.BackColor = _mainForm.GetBackgroundColor();

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

            Region.ReleaseHrgn(_hrgn);
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

        private void Notification_MouseClick(object sender, MouseEventArgs e)
        {
            if (_showFullWords)
            {
                WordsNotifier.Word w = _mainForm.GetWord(false);
                labelTitle.Text = w.word + "[" + w.part + "]";

                ShowFullListWords();
            }
            else
            {
                switch (_mainForm.GetMode())
                {
                    case WordsNotifier.Mode.HideTranslation:
                        {
                            WordsNotifier.Word w = _mainForm.GetWord(false);
                            labelBody.Text = w.translation;
                            _showFullWords = true;
                            break;
                        }
                    case WordsNotifier.Mode.Inverse:
                        {
                            WordsNotifier.Word w = _mainForm.GetWord(false);
                            labelBody.Text = w.word;
                            _showFullWords = true;
                            break;
                        }
                }
            }
        }

        private void ShowFullListWords()
        {
            WordsNotifier.Translations translations = _mainForm.GetAllTranslations();

            //--
            Dictionary<string, List<string>> partsToTrans = new Dictionary<string, List<string>>();
            foreach (var t in translations.translations)
            {
                if (!partsToTrans.ContainsKey(t.part))
                {
                    partsToTrans[t.part] = new List<string>();
                }
                partsToTrans[t.part].Add(t.translation);
            }

            //--
            labelBody.Text = "";
            foreach (var part in partsToTrans)
            {
                labelBody.Text += "[" + part.Key + "]: " + Environment.NewLine;
                foreach (var t in part.Value)
                {
                    labelBody.Text += "\t" + t + Environment.NewLine;
                }
            }

            //--
            int height = Math.Max(labelBody.Font.Height * (translations.translations.Count + partsToTrans.Count), labelBody.Size.Height) + 20;
            this.labelBody.Size = new Size(this.labelBody.Size.Width, height);
            this.labelBody.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.Size = new Size(this.Size.Width, this.labelTitle.Size.Height + this.labelBody.Size.Height);

            //--
            Region.ReleaseHrgn(_hrgn);
            NativeMethods.DeleteObject(_hrgn);

            _hrgn = NativeMethods.CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 20, 20);
            Region = Region.FromHrgn(_hrgn);
        }

        #endregion // Event Handlers
    }
}