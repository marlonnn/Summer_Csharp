using System.Drawing;
using System; 

namespace Summer.UI.Line
{
    public interface IShape
    {
        /// <summary>
        /// Gets the number of points in the shape.
        /// </summary>
        int PointCount { get; }
        /// <summary>
        /// Gets the point at the index <paramref name="i"/>.
        /// </summary>
        /// <param name="i">Index of the point to return.</param>
        /// <returns>Point at the index or Point.Empty if it doesn't exist</returns>
        Point GetPoint(int i);
        /// <summary>
        /// Sets the point at the specified index <paramref name="i"/>.
        /// </summary>
        /// <param name="i">Index of the point to set.</param>
        /// <param name="p">Point value.</param>
        void SetPoint(int i, Point p);

        /// <summary>
        /// Raised when the number of points changes in the shape.
        /// </summary>
        event EventHandler PointCountChanged;
    }
}
