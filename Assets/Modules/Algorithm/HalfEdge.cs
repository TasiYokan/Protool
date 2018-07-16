using UnityEngine;
using System.Collections;

namespace TasiYokan.Algorithm.Geometry
{
    public class HalfEdge
    {
        public HalfEdge next;
        public HalfEdge twin;
        public int orgId;

        public HalfEdge(int _orgId)
        {
            orgId = _orgId;
        }

        public HalfEdge Prev
        {
            get
            {
                HalfEdge h = next;
                while (h.next != this)
                    h = h.next;
                return h;
            }
        }

        public int DstId
        {
            get
            {
                return twin.orgId;
            }
        }
    }

    public class TriangulatedSet
    {
        /// <summary>
        /// Outer halfedge which is the twin of the halfedge that starts from leftmost point of convex hull to its increasing id neighbour
        /// </summary>
        public HalfEdge leftHalfedge;
        /// <summary>
        /// Outer halfedge which starts from rightmost point of convex hull and direct to its decreasing id neighbour
        /// </summary>
        public HalfEdge rightHalfedge;

        public TriangulatedSet(HalfEdge _lh, HalfEdge _rh)
        {
            leftHalfedge = _lh;
            rightHalfedge = _rh;
        }
    }
}
