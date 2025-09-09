using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AgOpenGPS.Classes;
using OpenTK.Graphics.OpenGL;
using AgLibrary.Logging;

namespace AgOpenGPS.Forms.Field
{
    public partial class FormBuildBoundaryFromTracks : Form
    {
        #region Constants
        private const double VIEW_MARGIN_FACTOR = 0.1;
        private const double DEFAULT_VIEW_SIZE = 100;
        private const float SELECTED_TRACK_WIDTH = 5.0f;
        private const float NORMAL_TRACK_WIDTH = 1.0f;
        private const float BOUNDARY_LINE_WIDTH = 3.0f;
        private const float INTERSECTION_POINT_SIZE = 4.0f;
        private const int CIRCLE_SEGMENTS = 16;
        #endregion

        #region Fields
        private readonly FormGPS _mf;
        private readonly List<CTrk> _trackList;
        private readonly List<CTrk> _selectedTracks;
        private BoundaryBuilder _builder;
        private CTrk _activeTrack;
        private double _viewLeft, _viewRight, _viewTop, _viewBottom;
        private bool _showTrimmedOnly;
        private Form _parentForm;
        private bool _redrawPending;



        #endregion

        #region Constructor
        public FormBuildBoundaryFromTracks(FormGPS mf, Form parentForm)
        {
            InitializeComponent();
            _mf = mf;
            _parentForm = parentForm;
            _trackList = new List<CTrk>();
            _selectedTracks = new List<CTrk>();
            _showTrimmedOnly = false;
            _redrawPending = false;

            InitializeOpenGL();
            LoadTracks();
        }

        private void InitializeOpenGL()
        {

            SetStyle(ControlStyles.DoubleBuffer |
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint, true);
        }

        private void LoadTracks()
        {
            _trackList.Clear();
            var tempTrackList = new List<CTrk>();
            var originalTrackList = _mf.trk.gArr;

            try
            {
                _mf.trk.gArr = tempTrackList;
                _mf.FileLoadTracks();

                if (tempTrackList.Count == 0)
                {
                    _mf.TimedMessageBox(3000, "Track Info", "No tracks found.");
                    return;
                }
                _trackList.AddRange(tempTrackList);
            }
            catch (Exception ex)
            {
                _mf.TimedMessageBox(3000, "Track Load Error", ex.Message);
            }
            finally
            {
                _mf.trk.gArr = originalTrackList;
            }
        }
        #endregion

        #region Form Events
        private void FormBuildBoundaryFromTracks_Load(object sender, EventArgs e)
        {
            InitializeForm();
            InitializeBuilder();
            BuildTrackSelectorUI();
            UpdateTrackListHighlighting();
            UpdateViewBounds();
            RequestRedraw();
        }

        private void InitializeForm()
        {
            _activeTrack = _trackList.FirstOrDefault();
            _showTrimmedOnly = false;
            _selectedTracks.Clear();
            _selectedTracks.AddRange(_trackList);
            btnSave.Enabled = false;
        }


        #endregion

        #region Track Management
        private void BuildTrackSelectorUI()
        {
            flpTrackList.Controls.Clear();
            _selectedTracks.Clear();

            foreach (var trk in _trackList)
            {
                var chk = CreateTrackCheckbox(trk);
                _selectedTracks.Add(trk);
                flpTrackList.Controls.Add(chk);
            }
        }

        private CheckBox CreateTrackCheckbox(CTrk track)
        {
            var chk = new CheckBox
            {
                Text = $"{track.name} ({track.mode})",
                Checked = true,
                AutoSize = false,
                Width = flpTrackList.Width - 10,
                Height = 40,
                Tag = track,
                Font = new Font("Segoe UI", 16F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 8, 0, 0),
                BackColor = Color.Green,
                ForeColor = Color.Black
            };

            chk.CheckedChanged += (s, e) => OnTrackSelectionChanged(s as CheckBox);
            return chk;
        }

        private void OnTrackSelectionChanged(CheckBox checkbox)
        {
            if (checkbox?.Tag is CTrk track)
            {
                if (checkbox.Checked)
                {
                    if (!_selectedTracks.Contains(track))
                        _selectedTracks.Add(track);
                }
                else
                {
                    _selectedTracks.Remove(track);
                    if (track == _activeTrack)
                        _activeTrack = _selectedTracks.FirstOrDefault();
                }

                _builder.SetTracks(_selectedTracks);
                _builder.BuildSegments();
                UpdateTrackListHighlighting();
                RequestRedraw();
            }
        }

        private void UpdateTrackListHighlighting()
        {
            foreach (CheckBox cb in flpTrackList.Controls)
            {
                if (cb.Tag is CTrk trk)
                {
                    if (!_selectedTracks.Contains(trk))
                    {
                        cb.BackColor = Color.OrangeRed;
                        cb.ForeColor = Color.LightGray;
                    }
                    else if (trk == _activeTrack)
                    {
                        cb.BackColor = Color.Gold;
                        cb.ForeColor = Color.Black;
                    }
                    else
                    {
                        cb.BackColor = Color.Green;
                        cb.ForeColor = Color.Black;
                    }
                }
            }
        }


        #endregion

        #region Boundary Building
        private void InitializeBuilder()
        {
            _builder = new BoundaryBuilder();
            _builder.SetTracks(_selectedTracks);
        }

        private void InitializeBuilderAndRebuild()
        {
            _builder.SetTracks(_selectedTracks);
            _builder.BuildSegments();
            _builder.FindIntersections();
            _builder.TrimSegmentsToIntersections();

            RequestRedraw();
        }
        #endregion

        #region Track Manipulation
        private void ShiftSelectedTrackPoint(bool isPointA, double deltaMeters)
        {
            var trk = _activeTrack;
            if (trk == null) return;

            _showTrimmedOnly = false;

            try
            {
                if (trk.mode == TrackMode.AB)
                {
                    AdjustABTrackPoints(trk, isPointA, deltaMeters);
                }
                else if (trk.mode == TrackMode.Curve && trk.curvePts.Count >= 2)
                {
                    AdjustCurveTrackPoints(trk, isPointA, deltaMeters);
                }

                UpdateViewBounds();
                InitializeBuilderAndRebuild();
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Track adjustment failed: {ex}");
                _mf.TimedMessageBox(2000, "Error", "Track adjustment failed: " + ex.Message);
            }
        }


        private void AdjustABTrackPoints(CTrk track, bool isPointA, double deltaMeters)
        {
            vec2 direction = (track.ptB - track.ptA).Normalize();

            if (isPointA)
            {
                track.ptA += direction * deltaMeters;
            }
            else
            {
                track.ptB -= direction * deltaMeters;
            }
        }


        private void AdjustCurveTrackPoints(CTrk track, bool isStartPoint, double deltaMeters)
        {
            int steps = (int)Math.Abs(deltaMeters);
            bool isExtend = deltaMeters < 0;

            if (isStartPoint)
            {
                AdjustCurveStartPoint(track, steps, isExtend);
            }
            else
            {
                AdjustCurveEndPoint(track, steps, isExtend);
            }
        }

        private void AdjustCurveStartPoint(CTrk track, int steps, bool isExtend)
        {
            for (int s = 0; s < steps && (isExtend || track.curvePts.Count > 2); s++)
            {
                var pt0 = track.curvePts[0];
                var pt1 = track.curvePts[1];
                vec2 dir = (new vec2(pt0.easting, pt0.northing) - new vec2(pt1.easting, pt1.northing)).Normalize();

                if (isExtend)
                {
                    track.curvePts.Insert(0, new vec3(
                        pt0.easting + dir.easting,
                        pt0.northing + dir.northing,
                        pt0.heading));
                }
                else
                {
                    track.curvePts.RemoveAt(0);
                }
            }
        }

        private void AdjustCurveEndPoint(CTrk track, int steps, bool isExtend)
        {
            for (int s = 0; s < steps && (isExtend || track.curvePts.Count > 2); s++)
            {
                int last = track.curvePts.Count - 1;
                var ptN = track.curvePts[last];
                var ptN1 = track.curvePts[last - 1];
                vec2 dir = (new vec2(ptN.easting, ptN.northing) - new vec2(ptN1.easting, ptN1.northing)).Normalize();

                if (isExtend)
                {
                    track.curvePts.Add(new vec3(
                        ptN.easting + dir.easting,
                        ptN.northing + dir.northing,
                        ptN.heading));
                }
                else
                {
                    track.curvePts.RemoveAt(last);
                }
            }
        }
        #endregion

        #region View Management
        private void UpdateViewBounds()
        {
            CalculateViewBounds();
            ApplyViewMargins();
        }

        private void CalculateViewBounds()
        {
            _viewLeft = double.PositiveInfinity;
            _viewRight = double.NegativeInfinity;
            _viewTop = double.NegativeInfinity;
            _viewBottom = double.PositiveInfinity;

            foreach (var trk in _selectedTracks)
            {
                if (trk.mode == TrackMode.AB)
                {
                    UpdateMinMax(trk.ptA);
                    UpdateMinMax(trk.ptB);
                }
                else if (trk.mode == TrackMode.Curve)
                {
                    foreach (var pt in trk.curvePts)
                        UpdateMinMax(new vec2(pt.easting, pt.northing));
                }
            }

            if (double.IsInfinity(_viewLeft) || double.IsInfinity(_viewRight) ||
                double.IsInfinity(_viewTop) || double.IsInfinity(_viewBottom))
            {
                SetDefaultViewBounds();
            }
        }

        private void UpdateMinMax(vec2 pt)
        {
            _viewLeft = Math.Min(_viewLeft, pt.easting);
            _viewRight = Math.Max(_viewRight, pt.easting);
            _viewBottom = Math.Min(_viewBottom, pt.northing);
            _viewTop = Math.Max(_viewTop, pt.northing);
        }

        private void SetDefaultViewBounds()
        {
            _viewLeft = -DEFAULT_VIEW_SIZE;
            _viewRight = DEFAULT_VIEW_SIZE;
            _viewTop = DEFAULT_VIEW_SIZE;
            _viewBottom = -DEFAULT_VIEW_SIZE;
        }

        private void ApplyViewMargins()
        {
            double margin = Math.Max(10, (_viewRight - _viewLeft) * VIEW_MARGIN_FACTOR);
            _viewLeft -= margin;
            _viewRight += margin;
            _viewTop += margin;
            _viewBottom -= margin;
        }

        private void RequestRedraw()
        {
            if (!_redrawPending)
            {
                _redrawPending = true;
                BeginInvoke((Action)(() =>
                {
                    glControlPreview.Invalidate();
                    _redrawPending = false;
                }));
            }
        }
        #endregion

        #region OpenGL Rendering
        private void glControlPreview_Load(object sender, EventArgs e)
        {
            InitializeGL();
            SetupViewport();
        }

        private void InitializeGL()
        {
            glControlPreview.MakeCurrent(); // Ensure the GL context is active
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
        }


        private void glControlPreview_Resize(object sender, EventArgs e)
        {
            glControlPreview.MakeCurrent();
            SetupViewport();
            RequestRedraw();
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, glControlPreview.Width, glControlPreview.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(_viewLeft, _viewRight, _viewBottom, _viewTop, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void glControlPreview_Paint(object sender, PaintEventArgs e)
        {
            RenderScene();
        }

        private void RenderScene()
        {
            glControlPreview.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SetupViewport();

            if (!_showTrimmedOnly)
            {
                DrawRawSegments();
                DrawTracks();
            }

            DrawBoundary();
            DrawFinalBoundary();

            glControlPreview.SwapBuffers();
        }

        private void DrawTracks()
        {
            for (int i = 0; i < _selectedTracks.Count; i++)
            {
                var trk = _selectedTracks[i];
                bool isSelected = (trk == _activeTrack);
                Color color = isSelected ? Color.Yellow : Color.Gray;
                float width = isSelected ? SELECTED_TRACK_WIDTH : NORMAL_TRACK_WIDTH;

                if (trk.mode == TrackMode.AB)
                {
                    DrawLine(trk.ptA, trk.ptB, color, width);
                }
                else if (trk.mode == TrackMode.Curve)
                {
                    DrawPolyline(trk.curvePts.Select(p => new vec2(p.easting, p.northing)).ToList(), color, width);
                }
            }
        }

        private void DrawBoundary()
        {
            if (_builder == null) return;

            DrawTrimmedSegments();
            DrawIntersectionPoints();
        }

        private void DrawTrimmedSegments()
        {
            GL.LineWidth(2.5f);
            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);

            foreach (var seg in _builder.TrimmedSegments)
            {
                GL.Vertex2(seg.Start.easting, seg.Start.northing);
                GL.Vertex2(seg.End.easting, seg.End.northing);
            }

            GL.End();
        }

        private void DrawIntersectionPoints()
        {
            foreach (var pt in _builder.IntersectionPoints)
            {
                DrawCircle(pt, Color.Red, INTERSECTION_POINT_SIZE);
            }
        }

        private void DrawFinalBoundary()
        {
            if (_builder?.FinalBoundary == null || _builder.FinalBoundary.Count < 2)
                return;

            GL.LineWidth(BOUNDARY_LINE_WIDTH);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.LineLoop);

            foreach (var pt in _builder.FinalBoundary)
            {
                GL.Vertex2(pt.easting, pt.northing);
            }

            GL.End();
        }

        private void DrawLine(vec2 ptA, vec2 ptB, Color color, float width = 1.0f)
        {
            GL.LineWidth(width);
            GL.Color3(color);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(ptA.easting, ptA.northing);
            GL.Vertex2(ptB.easting, ptB.northing);
            GL.End();
        }

        private void DrawPolyline(List<vec2> points, Color color, float width = 1.0f)
        {
            if (points.Count < 2) return;

            GL.LineWidth(width);
            GL.Color3(color);
            GL.Begin(PrimitiveType.LineStrip);

            foreach (var pt in points)
                GL.Vertex2(pt.easting, pt.northing);

            GL.End();
        }

        private void DrawCircle(vec2 center, Color color, float radius)
        {
            GL.Color3(color);
            GL.Begin(PrimitiveType.LineLoop);

            for (int i = 0; i < CIRCLE_SEGMENTS; i++)
            {
                double angle = i * Math.PI * 2.0 / CIRCLE_SEGMENTS;
                GL.Vertex2(
                    center.easting + Math.Cos(angle) * radius,
                    center.northing + Math.Sin(angle) * radius);
            }

            GL.End();
        }

        private void DrawRawSegments()
        {
            if (_builder?.Segments == null || _showTrimmedOnly) return;

            GL.LineWidth(1.5f);
            GL.Color3(Color.Gray);
            GL.Begin(PrimitiveType.Lines);

            foreach (var seg in _builder.Segments)
            {
                GL.Vertex2(seg.Start.easting, seg.Start.northing);
                GL.Vertex2(seg.End.easting, seg.End.northing);
            }

            GL.End();
        }
        #endregion

        #region Event Handlers
        private void btnResetPreview_Click(object sender, EventArgs e)
        {
            InitializeForm();
            _trackList.Clear();
            LoadTracks();
            BuildTrackSelectorUI();
            RefreshTrackSelection();
            InitializeBuilder();
            UpdateViewBounds();
            UpdateTrackListHighlighting();
            RequestRedraw();
        }


        private void RefreshTrackSelection()
        {
            _showTrimmedOnly = false;
            _selectedTracks.Clear();

            foreach (CheckBox cb in flpTrackList.Controls)
            {
                if (cb.Checked && cb.Tag is CTrk trk)
                {
                    _selectedTracks.Add(trk);
                }
            }
        }

        private void btnSelectPrevious_Click(object sender, EventArgs e)
        {
            if (_selectedTracks.Count == 0 || _activeTrack == null) return;

            int idx = _selectedTracks.IndexOf(_activeTrack);
            idx = (idx - 1 + _selectedTracks.Count) % _selectedTracks.Count;
            _activeTrack = _selectedTracks[idx];

            RequestRedraw();
            UpdateTrackListHighlighting();
        }

        private void btnSelectNext_Click(object sender, EventArgs e)
        {
            if (_selectedTracks.Count == 0 || _activeTrack == null) return;

            int idx = _selectedTracks.IndexOf(_activeTrack);
            idx = (idx + 1) % _selectedTracks.Count;
            _activeTrack = _selectedTracks[idx];

            RequestRedraw();
            UpdateTrackListHighlighting();
        }




        private void btnExtendForward_Click(object sender, EventArgs e) => ShiftSelectedTrackPoint(false, -10);
        private void btnExtendBackward_Click(object sender, EventArgs e) => ShiftSelectedTrackPoint(true, -10);
        private void btnShrinkA_Click(object sender, EventArgs e) => ShiftSelectedTrackPoint(true, +10);
        private void btnShrinkB_Click(object sender, EventArgs e) => ShiftSelectedTrackPoint(false, +10);

        private void btnBuildBoundary_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            BuildBoundary();
        }

        private void BuildBoundary()
        {
            InitializeBuilderAndRebuild();

            try
            {
                var result = _builder.BuildTrimmedBoundary();
                var finalized = _builder.FinalizedBoundary;

                if (result.Count < 3 || finalized == null)
                {
                    _mf.TimedMessageBox(2000, "Error", "No valid boundary could be generated");
                    btnSave.Enabled = false;
                    return;
                }

                _showTrimmedOnly = true;
                UpdateMainApplicationBoundary(finalized);
                RequestRedraw();

                _mf.TimedMessageBox(2000, "Succes!", $"Boundary built with {result.Count} points");
                btnSave.Enabled = true;

            }
            catch (Exception ex)
            {
                Log.EventWriter($"Boundary build failed: {ex}");
                _mf.TimedMessageBox(2000, "Error", "Build failed" + ex.ToString());
            }
        }

        private void UpdateMainApplicationBoundary(CBoundaryList boundary)
        {
            _mf.bnd.bndList.Clear();
            _mf.bnd.bndList.Add(boundary);
            _mf.bnd.BuildTurnLines();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveBoundary();
            Close();


        }

        private void btnAutofind_click(object sender, EventArgs e)
        {
            _builder.ExtendAllTracks(50.0);
            InitializeBuilderAndRebuild();
            _mf.TimedMessageBox(3000, "Finding Intersections...", $"All green? Press Build or manually correct!");


        }

        private void SaveBoundary()
        {
            try
            {
                ValidateSaveConditions();
                _builder.SaveToBoundaryFile(_mf.currentFieldDirectory);
                _mf.TimedMessageBox(2000, "Succes!", "Boundary saved successfully.");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Save failed: {ex}");
                _mf.TimedMessageBox(2000, "Error", ex.Message);
            }
        }

        private bool ValidateSaveConditions()
        {
            if (string.IsNullOrEmpty(_mf.currentFieldDirectory))
            {
                _mf.TimedMessageBox(3000, "Save Error", "Please select a field before saving.");
                return false;
            }

            if (_builder?.FinalizedBoundary == null || _builder.FinalizedBoundary.fenceLine.Count < 3)
            {
                _mf.TimedMessageBox(3000, "Save Error", "No valid boundary to save");
                return false;
            }

            return true;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}