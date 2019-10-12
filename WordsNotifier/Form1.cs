using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Troschuetz.Random;

namespace WordsNotifier
{
    using Words = Dictionary<string, Translations>;
    using PlainWords = List<KeyValuePair<string, int>>;

    public partial class Form1 : Form
    {
        //-- Settings.
        private string mTextFile = @"unknown_words.txt";
        private int mTimeToShow = 10;
        private int mTimeToHide = 5;
        private Color mColor = Color.Red;

        //--
        private static readonly string kFileHandleSettings = "file";
        private static readonly string kTimeToShowHandleSettings = "timeToShow";
        private static readonly string kTimeToHideHandleSettings = "timeToHide";
        private static readonly string kBackgroundColorHandleSettings = "backgroundColor";

        //--
        private Words mTranslations;
        private PlainWords mPlainListWords;

        //--
        private BinomialDistribution mBinominalDistr;
        private TriangularDistribution mTriangleDistr;
        private DiscreteUniformDistribution mUniformDistr;
        private ContinuousUniformDistribution mContUniformDist;
        private Random mRnd;
        private int mCurrentWordIdx;

        private bool mStopTimer;

        public void StopShowTimer() { timerToShowWindow.Stop(); }

        public void StartShowTimer() { timerToShowWindow.Start(); }

        public Translations GetAllTranslations()
        {
            string word = mPlainListWords[mCurrentWordIdx].Key;
            return mTranslations[word];
        }

        public Color GetBackgroundColor() { return mColor; }

        public Form1()
        {
            InitializeComponent();

            //--
            mTranslations = new Dictionary<string, Translations>();
            mPlainListWords = new PlainWords();

            //--
            mBinominalDistr = new BinomialDistribution();
            mTriangleDistr = new TriangularDistribution();
            mUniformDistr = new DiscreteUniformDistribution();
            mContUniformDist = new ContinuousUniformDistribution();
            mRnd = new Random();

            mStopTimer = false;
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            startToolStripMenuItem.Enabled = mStopTimer;
            stopToolStripMenuItem.Enabled = !mStopTimer;
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mStopTimer = false;
            timerToShowWindow.Start();
        }

        private void StopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mStopTimer = true;
            timerToShowWindow.Stop();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowWindow();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
            ReadWords();
            SetupDistribution();
            SortWordsByFrequency();

            this.WindowState = FormWindowState.Minimized;
            TuneTimers();
        }

        private void TimerToShowWindow_Tick(object sender, EventArgs e)
        {
            ShowPopupWord();

            if (mContUniformDist.NextDouble() > 0.5)
            {
                SortWordsByFrequency();
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            HideWindow();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            //--
            mTimeToShow = int.Parse(txtTimeShow.Text);
            mTimeToHide = int.Parse(txtTimeHide.Text);

            //--
            timerToShowWindow.Interval = mTimeToShow * 1000;

            //--
            HideWindow();
        }

        private void BtnOpenFie_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\";
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mTextFile = dialog.FileName;
                txtFile.Text = mTextFile;

                Reload();
            }
        }

        private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dump();
        }

        private void TuneTimers()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                timerToShowWindow.Start();
            }
            else
            {
                timerToShowWindow.Stop();
            }
        }

        private void ShowPopupWord()
        {
            Word w = SelectWord();

            ToastNotifications.Notification toastNotification = new ToastNotifications.Notification(this, w.word + (" [" + w.part + "]"), w.translation, mTimeToHide,
                ToastNotifications.FormAnimator.AnimationMethod.Center, ToastNotifications.FormAnimator.AnimationDirection.Up,
                ToastNotifications.Position.VerticalTop | ToastNotifications.Position.HorizontalCenter);
            toastNotification.Show();
        }

        private Word SelectWord()
        {
            Debug.Assert(mPlainListWords.Count == mTranslations.Count);

            if (mTranslations.Count == 0)
            {
                return new Word("nothing", "to do", "here");
            }

            //--
            mCurrentWordIdx = Convert.ToInt32(mTriangleDistr.NextDouble() * (mPlainListWords.Count - 1));
            string word = mPlainListWords[mCurrentWordIdx].Key;

            //--
            Translations translations = mTranslations[word];
            translations.frequency++;

            //-- update the frequency in the plain list.
            mPlainListWords[mCurrentWordIdx] = new KeyValuePair<string, int>(word, translations.frequency);

            //--
            int idxTranslation = mRnd.Next(0, translations.translations.Count - 1);
            Translation t = translations.translations[idxTranslation];
            t.frequency++;

            return new Word(word + " " + mCurrentWordIdx.ToString(), t.part, t.translation);
        }

        private void ReadWords()
        {
            Dictionary<string, int> frequencies = new Dictionary<string, int>();
            if (File.Exists(getPathDb()))
            {
                using (StreamReader file = new StreamReader(getPathDb()))
                {
                    string line;
                    string[] separator = { " " };
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] values = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        frequencies[values[0]] = int.Parse(values[1]);
                    }
                }
            }

            string textFile = Path.IsPathRooted(mTextFile) ? mTextFile : exePath() + @"\\" + mTextFile;
            if (File.Exists(textFile))
            {
                using (StreamReader file = new StreamReader(textFile))
                {
                    string line;
                    Regex regex = new Regex(@"([^;:\[\]\s]+[^;\]\[]+)");
                    while ((line = file.ReadLine()) != null)
                    {
                        /*var parts = Regex.Matches(line, @"([^\s]+) - (.+)");.First().Groups.Select(x => x.Value).Skip(1).ToArray();
                        var mainWord = parts.First();
                        var regResult = Regex.Matches(parts[1], @"([^;:\[\]\s]+[^;\]\[]+)(( - \[)|([\];]))");
                        var resultText = regResult.Select(x => x.Groups[1].Value).ToList();*/

                        var matches = regex.Matches(line);
                        //--
                        string word = matches[0].ToString();
                        word = word.Substring(0, word.IndexOf(" "));

                        int frequency = frequencies.ContainsKey(word) ? frequencies[word] : 0;

                        if (!mTranslations.ContainsKey(word))
                        {
                            mPlainListWords.Add(new KeyValuePair<string, int>(word, frequency));
                        }

                        mTranslations[word] = new Translations(frequency);

                        //--
                        for (int i = 1; i < matches.Count; i += 2)
                        {
                            //--
                            string part = matches[i + 0].ToString();

                            //--
                            string[] separator = { ", " };
                            var translations = matches[i + 1].ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var translation in translations)
                            {
                                mTranslations[word].translations.Add(new Translation(part, translation));
                            }
                        }
                    }
                }
            }
        }

        private void Reload()
        {
            mTranslations.Clear();

            ReadWords();
            SetupDistribution();
        }

        private void SortWordsByFrequency()
        {
            mPlainListWords.Sort((a, b) => a.Value.CompareTo(b.Value));
        }

        private void SetupDistribution()
        {
            //--
            mBinominalDistr.Alpha = 0.25;
            mBinominalDistr.Beta = mPlainListWords.Count - 1;

            //--
            mTriangleDistr.Alpha = 0.0;
            mTriangleDistr.Beta = 1.0;
            mTriangleDistr.Gamma = 0.1;

            //--
            mUniformDistr.Alpha = 0;
            mUniformDistr.Beta = mPlainListWords.Count - 1;

            //--
            mContUniformDist.Alpha = 0;
            mContUniformDist.Beta = 1;
        }

        private void Dump()
        {
            //--
            string pathDb = getPathDb();
            using (var writer = new StreamWriter(pathDb))
            {
                foreach (var word in mTranslations)
                {
                    writer.WriteLine(word.Key + " " + word.Value.frequency);
                }
            }

            //--
            string pathSettings = getPathSettings();
            using (var writer = new StreamWriter(pathSettings))
            {
                writer.WriteLine("{0} = {1}", kFileHandleSettings, mTextFile);
                writer.WriteLine("{0} = {1}", kTimeToShowHandleSettings, mTimeToShow.ToString());
                writer.WriteLine("{0} = {1}", kTimeToHideHandleSettings, mTimeToHide.ToString());
                writer.WriteLine("{0} = {1}", kBackgroundColorHandleSettings, mColor.ToArgb());
            }
        }

        private void LoadSettings()
        {
            string pathSettings = getPathSettings();
            if (File.Exists(pathSettings))
            {
                using (var reader = new StreamReader(pathSettings))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] splits = line.Split('=');
                        splits[0] = splits[0].Trim();
                        splits[1] = splits[1].Trim();

                        if (splits[0] == kFileHandleSettings)
                        {
                            mTextFile = splits[1];
                        }
                        else if (splits[0] == kTimeToShowHandleSettings)
                        {
                            mTimeToShow = Convert.ToInt32(splits[1]);
                        }
                        else if (splits[0] == kTimeToHideHandleSettings)
                        {
                            mTimeToHide = Convert.ToInt32(splits[1]);
                        }
                        else if (splits[0] == kBackgroundColorHandleSettings)
                        {
                            mColor = Color.FromArgb(Convert.ToInt32(splits[1]));
                        }
                    }
                }
            }
        }

        private void ShowWindow()
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();

            txtFile.Text = mTextFile;
            txtTimeShow.Text = mTimeToShow.ToString();
            txtTimeHide.Text = mTimeToHide.ToString();
            btnColor.BackColor = mColor;
        }

        private void HideWindow()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private string exePath()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        private string getPathDb()
        {
            return exePath() + "\\info.db";
        }

        private string getPathSettings()
        {
            return exePath() + "\\settings.txt";
        }

        private void SortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortWordsByFrequency();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnColor.BackColor = colorDialog1.Color;
                mColor = colorDialog1.Color;
            }
        }
    }
}
