using ClearCut.Support.Witness.Watchers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ClearCut.Main.Views
{
    public class RefreshTimer
    {
        private TimeSpan _time;
        private DispatcherTimer _timer;
        private Label countDown;
        private SiteWatcher _siteWatcher;

        public RefreshTimer(Label countDown, SiteWatcher siteWatcher)
        {
            this.countDown = countDown;
            _siteWatcher = siteWatcher;
            InitCountdown();
        }

        private void InitCountdown()
        {
            _time = TimeSpan.FromSeconds(10);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.SystemIdle, delegate
            {
                this.countDown.Content = ClearCut.Support.Settings.Constants.Ui.TitlePrefix + _time.Seconds;
                if (_time == TimeSpan.Zero)
                {
                    if (_siteWatcher != null)
                    {
                        _siteWatcher.TriggerDataChanged();
                    }

                    _time = TimeSpan.FromSeconds(10);
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
    }
}