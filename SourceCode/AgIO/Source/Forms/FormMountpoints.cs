using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>The mountpoints of a caster, nearest first, to pick the base station from.</summary>
    public partial class FormMountpoints : Form
    {
        private readonly NtripProfile profile;
        private readonly double latitude, longitude;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>The mountpoint the user chose.</summary>
        public string SelectedMount { get; private set; } = "";

        internal FormMountpoints(NtripProfile profile, double latitude, double longitude)
        {
            this.profile = profile;
            this.latitude = latitude;
            this.longitude = longitude;
            InitializeComponent();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            lvMounts.Columns.Add("Mountpoint", 220);
            lvMounts.Columns.Add("Place", 280);
            lvMounts.Columns.Add("Distance", 130);
            lvMounts.Columns.Add("Needs your position", 190);

            try
            {
                SourcetableResult result = await Sourcetable.FetchAsync(profile, new NtripTimeouts(), cts.Token);
                if (IsDisposed) return;

                if (!result.Success)
                {
                    lblMountStatus.Text = result.Error.Message;
                    return;
                }

                bool known = latitude != 0 || longitude != 0;
                var rows = new List<Tuple<double, ListViewItem>>();
                foreach (MountpointInfo m in result.Mountpoints)
                {
                    double km = known ? m.DistanceKm(latitude, longitude) : double.NaN;
                    string distance = double.IsNaN(km) ? "" : km.ToString("F0", CultureInfo.InvariantCulture) + " km";
                    var item = new ListViewItem(new[] { m.Mount, m.Name, distance, m.NeedsPosition ? "yes" : "" }) { Tag = m.Mount };
                    rows.Add(Tuple.Create(double.IsNaN(km) ? double.MaxValue : km, item));
                }
                rows.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                foreach (var row in rows) lvMounts.Items.Add(row.Item2);

                lblMountStatus.Text = rows.Count + " mountpoints" + (known ? ", nearest first." : ".");
            }
            catch (OperationCanceledException)
            {
                // closed
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cts.Cancel();
            base.OnFormClosed(e);
        }

        private void Choose()
        {
            if (lvMounts.SelectedItems.Count == 0) return;
            SelectedMount = (string)lvMounts.SelectedItems[0].Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnMountSelect_Click(object sender, EventArgs e) { Choose(); }
        private void lvMounts_DoubleClick(object sender, EventArgs e) { Choose(); }
    }
}
