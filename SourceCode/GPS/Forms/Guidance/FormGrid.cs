using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Visuals;
using AgOpenGPS.Helpers;
using AgOpenGPS.Visuals;
using AgOpenGPS.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGrid : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;
        private GeoViewport _viewport;

        public GeoCoord? _coordA;
        public GeoCoord? _coordB;

        public FormGrid(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            mf.CalculateMinMax();
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            Size = Properties.Settings.Default.setWindow_gridSize;

            Location = Properties.Settings.Default.setWindow_gridLocation;
            FormABDraw_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.curve.isCurveValid = false;
            mf.ABLine.isABValid = false;

            mf.twoSecondCounter = 100;

            Properties.Settings.Default.setWindow_gridSize = Size;
            Properties.Settings.Default.setWindow_gridLocation = Location;
            Properties.Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.worldGrid.gridRotation = 0;
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            _coordA = null;
            _coordB = null;
            mf.curve.desList?.Clear();

            _viewport.ResetZoomPan();

            btnExit.Focus();
        }

        private void OnViewportMouseDown(object sender, GeoCoord mouseDownCoord)
        {
            if (!_coordA.HasValue)
            {
                _coordA = mouseDownCoord;
            }
            else
            {
                _coordB = mouseDownCoord;
                GeoDir abDir = new GeoDir(_coordA.Value, _coordB.Value);
                mf.worldGrid.gridRotation = abDir.AngleInDegrees;
            }
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            _viewport.BeginPaint();

            SectionsVisual.DrawSections(mf.triStrip);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                GeoCoord[] fenceLineEar = GeoRefactorHelper.ToGeoCoordArray(mf.bnd.bndList[j].fenceLineEar);
                bool isSelected = j == 0;
                FenceLineVisual.DrawFenceLine(fenceLineEar, isSelected);
            }
            VehicleDotVisual.DrawVehicleDot(mf.pivotAxlePos.ToGeoCoord());
            TouchPointsLineVisual.DrawTouchPoints(_coordA, _coordB);

            _viewport.EndPaint();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
        }

        private void btnAlignToTrack_Click(object sender, EventArgs e)
        {
            if (mf.trk.idx > -1)
            {
                mf.worldGrid.gridRotation = Math.Atan2(
                    mf.trk.gArr[mf.trk.idx].ptB.easting - mf.trk.gArr[mf.trk.idx].ptA.easting,
                    mf.trk.gArr[mf.trk.idx].ptB.northing - mf.trk.gArr[mf.trk.idx].ptA.northing);
                if (mf.worldGrid.gridRotation < 0) mf.worldGrid.gridRotation += glm.twoPI;
                mf.worldGrid.gridRotation = glm.toDegrees(mf.worldGrid.gridRotation);
            }
            Close();
        }

        private void FormABDraw_ResizeEnd(object sender, EventArgs e)
        {
            Width = (int)(Height * 1.09);
            oglSelf.Height = oglSelf.Width = Height - 40;

            oglSelf.Left = 1;
            oglSelf.Top = 0;

            _viewport.Resize(oglSelf.Width, oglSelf.Height);

            tlp1.Width = Width - oglSelf.Width - 10;
            tlp1.Left = oglSelf.Width - 2;
        }

        private void oglSelf_Resize(object sender, EventArgs e)
        {
            CreateViewport();
            _viewport.Resize(oglSelf.Width, oglSelf.Height);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            CreateViewport();
        }

        private void CreateViewport()
        {
            if (_viewport == null)
            {
                _viewport = new GeoViewport(mf.FieldBoundingBox, oglSelf);
                _viewport.MouseDownEventHandler += OnViewportMouseDown;
            }
        }

    }
}