using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Collections;
using Jerarquia;

namespace XControls
{
    public partial class FunctionsViewer : UserControl
    {
        public FunctionsViewer()
        {
            InitializeComponent();

            // Subscribe to invalidate automatically when added or removed a new function.
            functions.CollectionChanged += new EventHandler(functions_CollectionChanged);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            
            BackColor = Color.SteelBlue;
        }

        void functions_CollectionChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        PointF center  = new PointF (0,0);

        /// <summary>
        /// Gets or sets the point of the plane this viewer will show at center.
        /// </summary>
        public PointF Center
        {
            get { return center; }
            set
            {
                center = value;
                Invalidate();
            }
        }

        float scale = 10;

        /// <summary>
        /// Gets or sets the scale of the viewer. Scale determines the size (in x coordinates) of the representation.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("No se puede escalar 0");

                scale = Math.Min(1000, Math.Max(0.0001f, value));

                Invalidate();
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(Center.X - scale, Center.Y - scale * this.Height / this.Width, scale * 2, scale * this.Height / this.Width * 2);
            }
        }

        public Point FromPlaneToClient(float x, float y)
        {
            return FromPlaneToClient(new PointF(x, y));
        }

        /// <summary>
        /// Converts a coordinate from plane to the client area of this control.
        /// </summary>
        public Point FromPlaneToClient(PointF pto)
        {
            // centered
            PointF p = new PointF((pto.X - center.X)/scale, (pto.Y - center.Y)/scale);
            
            // scaled
            p = new PointF(p.X * Width / 2, - p.Y * Width / 2);

            // translated
            p = new PointF(p.X + Width / 2, p.Y + Height / 2);

            return Point.Ceiling(p);
        }


        /// <summary>
        /// Converts a coordinate from the client area of this control to the plane.
        /// </summary>
        public PointF FromClientToPlane(Point pto)
        {
            PointF p = pto;

            // before translated
            p = new PointF(p.X - this.Width / 2, p.Y - this.Height / 2);

            // before scaled
            p = new PointF(2 * p.X / this.Width, - 2 * p.Y / this.Width);

            // before centered
            p = new PointF(p.X *scale + center.X, p.Y * scale + center.Y);

            return p;
        }

        #region Properties

        bool showAxis = true;

        /// <summary>
        /// Gets or sets when the axis are shown.
        /// </summary>
        [Category("Viewer")]
        [DefaultValue(true)]
        public bool ShowAxis
        {
            get { return showAxis; }
            set { showAxis = value; Invalidate(); }
        }

        #endregion

        FunctionCollection functions = new FunctionCollection();

        /// <summary>
        /// Gets the collection of functions. Use this property for adding or removing functions to be viewed.
        /// </summary>
        public FunctionCollection Functions { get { return functions; } }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = e.Graphics;

            DrawGrid(gr);

            if (ShowAxis)
                DrawAxis(gr);

            foreach (var f in functions)
                DrawFunction(gr, f);

            if (isDragging)
            {
                Brush selectionBrush = new SolidBrush(Color.FromArgb(100, Color.SlateBlue));

                gr.FillRectangle(selectionBrush, Math.Min(startDrag.X, endDrag.X), Math.Min(startDrag.Y, endDrag.Y), Math.Abs(endDrag.X - startDrag.X), Math.Abs(endDrag.Y - startDrag.Y));
                gr.DrawRectangle(Pens.LightSeaGreen, Math.Min(startDrag.X, endDrag.X), Math.Min(startDrag.Y, endDrag.Y), Math.Abs(endDrag.X - startDrag.X), Math.Abs(endDrag.Y - startDrag.Y));
            }

            PointF cursor = FromClientToPlane(PointToClient(Control.MousePosition));
            gr.DrawString(cursor.X + ":" + cursor.Y, Font, Brushes.DarkGray, Point.Add(PointToClient(Control.MousePosition), new Size(5, -15)));
        }

        const int NSamples = 3;

        private void DrawFunction(Graphics gr, FunctionInfo f)
        {
            Brush brush= new SolidBrush (f.Color);

            var bounds = Bounds;
            float step = bounds.Width / (this.Width*NSamples); // N samples by pixel
            for (float x = bounds.Left; x <= bounds.Right; x += step)
            {
                bool undefined = false;

                try
                {
                    float y = f.Function(x);
                    if (!float.IsNaN(y) && !float.IsInfinity(y))
                    {
                        var plot = FromPlaneToClient(new PointF(x, y));
                        gr.FillEllipse(brush, new Rectangle(plot.X - 1, plot.Y - 1, 3, 3));
                    }
                    else
                        undefined = true;
                }
                catch
                {
                    undefined = true;
                }

                if (undefined)
                {
                    var plot = FromPlaneToClient(new PointF(x, 0));
                    gr.FillRectangle(Brushes.Red, new Rectangle(plot.X - 1, plot.Y - 1, 3, 3));
                }
            }
        }

        private void DrawAxis(Graphics gr)
        {
            Point negativeX = FromPlaneToClient(new PointF(Bounds.Left, 0));
            Point positiveX = FromPlaneToClient(new PointF(Bounds.Right, 0));
            Point negativeY = FromPlaneToClient(new PointF(0, Bounds.Top));
            Point positiveY = FromPlaneToClient(new PointF(0, Bounds.Bottom));

            Pen arrowPen = new Pen (Color.Black, 1);
            arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            arrowPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

            gr.DrawLine(arrowPen, negativeX, positiveX);
            gr.DrawLine(arrowPen, negativeY, positiveY);
        }

        private void DrawGrid(Graphics gr)
        {
            float numberStep = (float)Math.Pow (10, Math.Ceiling(Math.Log10 (Scale)))/10;

            PointF centerMoved = new PointF((int)(Center.X / numberStep) * numberStep, (int)(Center.Y / numberStep) * numberStep);
            
            var bounds = Bounds;

            Pen gridPen = new Pen(Color.DimGray, 0.5f);
            gridPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            for (int i = -10 * this.Height / this.Width; i <= 10 * this.Height / this.Width; i++)
            {
                float y = centerMoved.Y + i * numberStep;

                Point left = FromPlaneToClient(bounds.Left, y);
                Point right = FromPlaneToClient(bounds.Right, y);

                gr.DrawLine(gridPen, left, right);

                gr.DrawString(y + "", Font, Brushes.DimGray, left);
            }

            for (int i = -10; i <= 10; i++)
            {
                float x =centerMoved.X + i * numberStep;

                Point top = FromPlaneToClient(x, bounds.Top);
                Point bottom = FromPlaneToClient(x, bounds.Bottom);

                gr.DrawLine(gridPen, top, bottom);

                gr.DrawString(x + "", Font, Brushes.DimGray, bottom);
            }
        }

        #region Drag Functionalities

        bool isDragging;
        Point startDrag;

        Point endDrag;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            /// Drag operation
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isDragging = true;

                startDrag = e.Location;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.Scale *= 2;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (isDragging)
            {

                if (Math.Abs(endDrag.X - startDrag.X) >= 10) // less number of pixels the drag and drop works....
                {
                    PointF startInPlane = FromClientToPlane(startDrag);
                    PointF endInPlane = FromClientToPlane(endDrag);

                    Scale = Math.Abs(endInPlane.X - startInPlane.X) / 2;
                    Center = new PointF((startInPlane.X + endInPlane.X) / 2, (startInPlane.Y + endInPlane.Y) / 2);
                }

                isDragging = false;
            }
            else
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    this.Scale /= 2;

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            endDrag = e.Location;

            Invalidate();

            this.Cursor = Cursors.Cross;
        }

        #endregion
    }

    /// <summary>
    /// Represents a function to be viewed.
    /// </summary>
    public class FunctionInfo
    {
        /// <summary>
        /// Gets or sets the name of the function. You will use this tag to represent the name of the function in the viewer in future implementations.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the color will be used to draw the function.
        /// </summary>
        public Color Color { get; set; }
        
        /// <summary>
        /// Gets or sets the delegate that encapsulates the function to be drawn.
        /// </summary>
        public Func<float, float> Function { get; set; }

        /// <summary>
        /// Initializes a function with some default values.
        /// </summary>
        public FunctionInfo()
        {
            Pow a=new Pow(new Identidad(), new Constante(2));
            Name = "f";
            Color = Color.White;
            Function = x => (float)a.Evaluar(x);
        }
    }

    /// <summary>
    /// Represents a function collection.
    /// </summary>
    public class FunctionCollection : Collection<FunctionInfo>
    {
        public FunctionCollection()
        {
        }

        public event EventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            var temp = CollectionChanged;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        protected override void InsertItem(int index, FunctionInfo item)
        {
            base.InsertItem(index, item);

            OnCollectionChanged(EventArgs.Empty);
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            OnCollectionChanged(EventArgs.Empty);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            OnCollectionChanged(EventArgs.Empty);
        }

        protected override void SetItem(int index, FunctionInfo item)
        {
            base.SetItem(index, item);

            OnCollectionChanged(EventArgs.Empty);
        }
    }
}
