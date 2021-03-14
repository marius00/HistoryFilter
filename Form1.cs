using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HistoryFilter.Filters;
using HistoryFilter.Settings;
using HistoryFilter.Util;

namespace HistoryFilter {
    public partial class Form1 : Form {
        private readonly List<IFilter> _filters;
        private readonly AppSettings _settings;
        private MinimizeToTrayHandler _minimizeToTrayHandler;

        public Form1(AppSettings settings, List<IFilter> filters) {
            InitializeComponent();
            _filters = filters;
            _settings = settings;
            _minimizeToTrayHandler = new MinimizeToTrayHandler(this, notifyIcon1);
        }

        private void Form1_Load(object sender, EventArgs e) {
            ExceptionHandler.EnableLogUnhandledOnThread();

            var timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick; ;
            timer.Interval = 60000;
            timer.Start();

            foreach (var prefix in _settings.Prefixes ?? new List<string>()) {
                listBox1.Items.Add(prefix);
            }

            textBox1.KeyPress += TextBox1_KeyPress;
            this.FormClosing += Form1_FormClosing;
            cbStartOnBoot.Checked = StartupRegistrationService.IsInstalled();

            cbStartOnBoot.CheckedChanged += (o, args) => {
                var install = (o as CheckBox).Checked;
                if (install) {
                    StartupRegistrationService.Install();
                }
                else {
                    StartupRegistrationService.Uninstall();
                }
            };

#if DEBUG
            Text = Text + " (Debug)";
            notifyIcon1.Text = notifyIcon1.Text + " (Debug)";
#endif
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            _minimizeToTrayHandler?.Dispose();
            _minimizeToTrayHandler = null;
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == 13) {
                e.Handled = true;
                buttonAdd_Click(sender, e);
                return;
            }
        }

        private void Timer_Tick(object sender, EventArgs e) {
            foreach (var filter in _filters) {
                filter.Purge();
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(textBox1.Text)) {
                if (textBox1.Text.Length < 3) {
                    MessageBox.Show(@"The minimum prefix length is 3 characters (ex: Z:\)");
                }
                else {
                    listBox1.Items.Add(textBox1.Text);
                    _settings.AddPrefix(textBox1.Text);
                    textBox1.Text = "";
                    UpdateFilters();
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e) {
            var item = listBox1.SelectedItem;
            if (!string.IsNullOrEmpty(item?.ToString())) {
                listBox1.Items.Remove(item);
                _settings.RemovePrefix(item?.ToString());
                UpdateFilters();
            }
        }

        private void UpdateFilters() {
            var masks = listBox1.Items.Cast<string>().ToList();
            foreach (var filter in _filters) {
                filter.SetMasks(masks);
            }
        }

        private void cbStartOnBoot_CheckedChanged(object sender, EventArgs e) {

        }

        private void cbFilterMissingDrives_CheckedChanged(object sender, EventArgs e) {
            _settings.FilterMissingDrives = !_settings.FilterMissingDrives;
            foreach (var filter in _filters) {
                filter.SetFilterMissingDrives(_settings.FilterMissingDrives);
            }
        }
    }
}
