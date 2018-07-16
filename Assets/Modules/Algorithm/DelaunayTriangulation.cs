using System.Collections;
using System.Collections.Generic;
using TasiYokan.Algorithm.Graph;
using UnityEngine;

namespace TasiYokan.Algorithm.Geometry
{
    public class DelaunayTriangulation
    {
        List<Vector2> m_vertice;
        List<HalfEdge> m_halfedges;

        public List<HalfEdge> Halfedges
        {
            get
            {
                return m_halfedges;
            }
        }

        public struct Edge
        {
            public Vector2 org;
            public Vector2 dst;

            public Edge(Vector2 _org, Vector2 _dst)
            {
                org = _org;
                dst = _dst;
            }
        }

        private void Init(ref List<Vector2> _vertice)
        {
            m_vertice = _vertice;
            SortVectice();
            m_halfedges = new List<HalfEdge>();
        }

        private void SortVectice()
        {
            m_vertice.Sort((lhs, rhs) =>
            {
                if ((lhs.x - rhs.x).Sgn() != 0)
                    return (lhs.x - rhs.x).Sgn();
                else
                    return (lhs.y - rhs.y).Sgn();
            });
        }

        /// <summary>
        /// Because we will sort vertice before triangulation. 
        /// The id in return list will be the updated ones.
        /// </summary>
        /// <param name="_vertice"></param>
        /// <returns></returns>
        public List<UndirectedEdge> MakeTriangulation(ref List<Vector2> _vertice)
        {
            Init(ref _vertice);
            Triangulate(0, _vertice.Count - 1, 0);

            List<UndirectedEdge> edges = new List<UndirectedEdge>();

            for (int i = 0; i < m_halfedges.Count; i += 2)
            {
                edges.Add(new UndirectedEdge(
                    m_halfedges[i].orgId,
                    m_halfedges[i].DstId,
                    (_vertice[m_halfedges[i].orgId] - _vertice[m_halfedges[i].DstId])
                        .magnitude));
            }

            return edges;
        }

        private TriangulatedSet Triangulate(int _beginId, int _endId, int _depth)
        {
            // Number of points in this set
            int size = _endId - _beginId + 1;

            if (size == 2) // Segment
            {
                HalfEdge h = CreateHalfEdge(_beginId, _endId);

                return new TriangulatedSet(h.twin, h.twin);
            }
            else if (size == 3) // Triangle
            {
                HalfEdge hA = CreateHalfEdge(_beginId, _beginId + 1);
                HalfEdge hB = CreateHalfEdge(_beginId + 1, _endId);
                ConnectHalfedges(hA, hB);

                // A,B,C are in CCW, which means A,B,C are inner halfedges
                if (IsCcw(m_vertice[_beginId], m_vertice[_beginId + 1], m_vertice[_endId]))
                {
                    // DONOT delete this line since it update other connection infomation
                    HalfEdge hC = BridgeHalfedges(hB, hA);
                    return new TriangulatedSet(hA.twin, hB.twin);
                }
                // A,B,C are in CW, which means A,B,C are outer halfedges
                else if (IsCcw(m_vertice[_beginId], m_vertice[_endId], m_vertice[_beginId + 1]))
                {
                    HalfEdge hC = BridgeHalfedges(hB, hA);
                    return new TriangulatedSet(hC, hC);
                }
                else
                {
                    return new TriangulatedSet(hA.twin, hB.twin);
                }
            }
            else // size >= 4, Separate
            {
                // Recursively delaunay triangulate L and R halves
                TriangulatedSet leftSet = Triangulate(_beginId, _beginId + (size / 2) - 1, _depth + 1);
                TriangulatedSet rightSet = Triangulate(_beginId + (size / 2), _endId, _depth + 1);
                HalfEdge LLh = leftSet.leftHalfedge;
                HalfEdge LRh = leftSet.rightHalfedge;
                HalfEdge RLh = rightSet.leftHalfedge;
                HalfEdge RRh = rightSet.rightHalfedge;

                // Compute the lowest common tangent of L and R
                while (true)
                {
                    if (IsLeftOf(RLh.DstId, LRh))
                    {
                        LRh = LRh.next;
                    }
                    else if (IsLeftOf(LRh.orgId, RLh))
                    {
                        RLh = RLh.Prev;
                    }
                    else
                    {
                        break;
                    }
                }

                HalfEdge baseH = BridgeHalfedges(RLh, LRh);
                if (LRh.orgId == LLh.DstId)
                {
                    LLh = baseH;
                }
                if (RLh.DstId == RRh.orgId)
                {
                    RRh = baseH;
                }

                // Merge
                while (true)
                {
                    // Locate the first L point (Lcand.Org) to be encountered by the rising bubble
                    // and delete L edges that fail the circle test
                    HalfEdge Lcand = baseH.twin.Prev;
                    if (IsLeftOf(Lcand.orgId, baseH.twin))
                    {
                        while (IsInCircle(
                            baseH.DstId,
                            baseH.orgId,
                            Lcand.orgId,
                            Lcand.twin.Prev.orgId))
                        {
                            HalfEdge nextLcand = Lcand.twin.Prev;
                            DeleteHalfedge(Lcand);
                            Lcand = nextLcand;
                        }
                    }

                    HalfEdge Rcand = baseH.twin.next;
                    if (IsLeftOf(Rcand.DstId, baseH.twin))
                    {
                        while (IsInCircle(
                            baseH.DstId,
                            baseH.orgId,
                            Rcand.DstId,
                            Rcand.twin.next.DstId))
                        {
                            HalfEdge nextRcand = Rcand.twin.next;
                            DeleteHalfedge(Rcand);
                            Rcand = nextRcand;
                        }
                    }

                    // If both Lcand and Rcand are invalid, then baseH is the upper common tangent
                    if (IsLeftOf(Lcand.orgId, baseH.twin) == false && IsLeftOf(Rcand.DstId, baseH.twin) == false)
                    {
                        break;
                    }

                    // The next cross halfedge is to be connected to either Lcand.Org or rcand.Dst
                    // if both are valid, then choose the appropriate one using the inCircle test
                    if (IsLeftOf(Lcand.orgId, baseH.twin) && IsInCircle(Lcand.orgId, baseH.DstId, baseH.orgId, Rcand.DstId) == false)
                    {
                        baseH = BridgeHalfedges(baseH.twin, Lcand);
                    }
                    else
                    {
                        baseH = BridgeHalfedges(Rcand, baseH.twin);
                    }
                }

                return new TriangulatedSet(LLh, RRh);
            }
        }

        /// <summary>
        /// Create a pair of halfedges between two vertex(ids)
        /// </summary>
        /// <param name="_orgId"></param>
        /// <param name="_dstId"></param>
        /// <returns></returns>
        HalfEdge CreateHalfEdge(int _orgId, int _dstId)
        {
            HalfEdge h = new HalfEdge(_orgId);
            HalfEdge twinH = new HalfEdge(_dstId);

            h.twin = twinH;
            twinH.twin = h;

            h.next = twinH;
            twinH.next = h;

            m_halfedges.Add(h);
            m_halfedges.Add(twinH);

            return h;
        }

        void DeleteHalfedge(HalfEdge _h)
        {
            DisconnectHalfedges(_h.Prev, _h);
            DisconnectHalfedges(_h.twin.Prev, _h.twin);

            m_halfedges.Remove(_h);
            m_halfedges.Remove(_h.twin);
        }

        /// <summary>
        /// Make a bridge btween two halfedges which should be both outer halfedges
        /// halfedge A's head should remotely connect to halfedge B's rear
        /// </summary>
        /// <param name="_hA"></param>
        /// <param name="_hB"></param>
        /// <returns></returns>
        private HalfEdge BridgeHalfedges(HalfEdge _hA, HalfEdge _hB)
        {
            HalfEdge hC = CreateHalfEdge(_hA.DstId, _hB.orgId);
            ConnectHalfedges(_hA, hC);
            ConnectHalfedges(hC, _hB);

            return hC;
        }

        /// <summary>
        /// Make halfedge A's head connect to halfedge B's rear
        /// </summary>
        /// <param name="_hA"></param>
        /// <param name="_hB"></param>
        private void ConnectHalfedges(HalfEdge _hA, HalfEdge _hB)
        {
            _hB.Prev.next = _hA.next;
            _hA.next = _hB;
        }

        /// <summary>
        /// _hB should always be the isolated one to detach, _hA should be the base one.
        ///    _hB\ \
        ///        \ \
        /// _hA ----   ---
        /// </summary>
        /// <param name="_hA"></param>
        /// <param name="_hB"></param>
        private void DisconnectHalfedges(HalfEdge _hA, HalfEdge _hB)
        {
            _hA.next = _hB.twin.next;
            _hB.twin.next = _hB;
        }

        public static float SignedArea(Vector2 _pA, Vector2 _pB, Vector2 _pC)
        {
            return (_pB.x - _pA.x) * (_pC.y - _pA.y) - (_pB.y - _pA.y) * (_pC.x - _pA.x);
        }

        public static bool IsCcw(Vector2 _pA, Vector2 _pB, Vector2 _pC)
        {
            return SignedArea(_pA, _pB, _pC).Sgn() > 0;
        }

        public static bool IsNotCw(Vector2 _pA, Vector2 _pB, Vector2 _pC)
        {
            return SignedArea(_pA, _pB, _pC).Sgn() >= 0;
        }

        public static bool IsInCircle(Vector2 _pA, Vector2 _pB, Vector2 _pC, Vector2 _pX)
        {
            if (_pX == _pA || _pX == _pB || _pX == _pC)
                return false;

            return _pA.sqrMagnitude * SignedArea(_pB, _pC, _pX)
                - _pB.sqrMagnitude * SignedArea(_pA, _pC, _pX)
                + _pC.sqrMagnitude * SignedArea(_pA, _pB, _pX)
                - _pX.sqrMagnitude * SignedArea(_pA, _pB, _pC) > 0;
        }

        /// <summary>
        /// Exclusive left
        /// </summary>
        /// <param name="_vId"></param>
        /// <param name="_h"></param>
        /// <returns></returns>
        private bool IsLeftOf(int _vId, HalfEdge _h)
        {
            return IsCcw(m_vertice[_vId], m_vertice[_h.orgId], m_vertice[_h.DstId]);
        }

        /// <summary>
        /// Exclusive right
        /// </summary>
        /// <param name="_vId"></param>
        /// <param name="_h"></param>
        /// <returns></returns>
        private bool IsRightOf(int _vId, HalfEdge _h)
        {
            return !IsNotCw(m_vertice[_vId], m_vertice[_h.orgId], m_vertice[_h.DstId]);
        }

        private bool IsInCircle(int _vAId, int _vBId, int _vCId, int _vXid)
        {
            return IsInCircle(
                m_vertice[_vAId], m_vertice[_vBId], m_vertice[_vCId], m_vertice[_vXid]);
        }
    }
}
