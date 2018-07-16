using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TasiYokan.Algorithm.DataStructure;
using UnityEngine;
using UnityEngine.Assertions;

namespace TasiYokan.Algorithm.Graph
{
    /// <summary>
    /// Denote the edge to certain vertex(the org is not included in this struct)
    /// It's an Incomplete stuct unless org pos was known before using ToEdge.
    /// </summary>
    public struct DstEdge
    {
        public int dstId;
        public float len;

        public DstEdge(int _vertex, float _len)
        {
            dstId = _vertex;
            len = _len;
        }
    }

    /// <summary>
    /// Connect two ends with a len denotes its' length
    /// </summary>
    public struct UndirectedEdge
    {
        public int orgId;
        public int dstId;
        public float len;

        /// <summary>
        /// If no _len was passed, we treat it to be a simple id edge
        /// </summary>
        /// <param name="_orgId"></param>
        /// <param name="_dstId"></param>
        /// <param name="_len"></param>
        public UndirectedEdge(int _orgId, int _dstId, float _len = -1)
        {
            orgId = _orgId;
            dstId = _dstId;
            len = _len;
        }
    }

    public static class TreeHelper
    {
        private static Dictionary<int, List<DstEdge>> m_treeAdjMap;
        // Useful array to record visited info
        private static bool[] m_vis;
        // Each child node will have only one parent node, the root's parent should be itself
        private static int[] m_parentMap;

        public static Dictionary<int, List<DstEdge>> ConvertUndirectedEdgesToAdjacencyMap(List<UndirectedEdge> _undirEdges)
        {
            Dictionary<int, List<DstEdge>> AdjacencyList =
                new Dictionary<int, List<DstEdge>>();

            Action<int, int, float> CreateDirectedEdge = (_orgId, _dstId, _len) =>
            {
                if (AdjacencyList.ContainsKey(_orgId) == false)
                {
                    AdjacencyList.Add(_orgId, new List<DstEdge>());
                }
                AdjacencyList[_orgId].Add(new DstEdge(_dstId, _len));
            };

            foreach (UndirectedEdge undirEdge in _undirEdges)
            {
                CreateDirectedEdge(undirEdge.orgId, undirEdge.dstId, undirEdge.len);
                CreateDirectedEdge(undirEdge.dstId, undirEdge.orgId, undirEdge.len);
            }

            return AdjacencyList;
        }

        #region Tree Diameter

        public static List<UndirectedEdge> GetTreeDiameter(List<UndirectedEdge> _edges, out int _startId, out int _endId)
        {
            m_treeAdjMap = ConvertUndirectedEdgesToAdjacencyMap(_edges);

            return GetTreeDiameter(m_treeAdjMap, out _startId, out _endId);
        }

        public static List<UndirectedEdge> GetTreeDiameter(
            Dictionary<int, List<DstEdge>> _treeAdjMap, out int _startId, out int _endId)
        {
            m_treeAdjMap = _treeAdjMap;
            List<UndirectedEdge> trunkEdges = new List<UndirectedEdge>();

            m_vis = new bool[m_treeAdjMap.Keys.Count];

            /// <summary>
            /// Key is the max distance from deepest leaf node to current index node. 
            /// Values stores the path from leaf node to current node.[inclusive]
            /// </summary>
            KeyValuePair<float, List<int>>[] nodeDepthInfo =
                new KeyValuePair<float, List<int>>[m_treeAdjMap.Keys.Count];
            Action<int> DfsDeepest = null;

            DfsDeepest = new Action<int>((_curId) =>
            {
                // Because it's a tree consists of undirected pathes, we need this to ensure we dfs from top to down
                m_vis[_curId] = true;

                bool isLeaf = true;

                foreach (DstEdge dstEdge in m_treeAdjMap[_curId])
                {
                    if (m_vis[dstEdge.dstId])
                    {
                        continue;
                    }
                    else
                    {
                        isLeaf = false;
                    }

                    // Make sure every child node has been visited before updating current node's info
                    if (nodeDepthInfo[dstEdge.dstId].Key < 0)
                        DfsDeepest(dstEdge.dstId);

                    float dist = nodeDepthInfo[dstEdge.dstId].Key + dstEdge.len;
                    if (dist > nodeDepthInfo[_curId].Key)
                    {
                        List<int> nodeList = nodeDepthInfo[dstEdge.dstId].Value;
                        nodeList.Add(_curId);
                        nodeDepthInfo[_curId] =
                            new KeyValuePair<float, List<int>>(dist, nodeList);
                    }
                }

                // Where treeAdjMap[_curId].Count == 1
                // But we can't use above to check if it's a leaf as root also has only one connection
                if (isLeaf)
                {
                    // For leaf node, the distance from bottom is 0 and the path only include itself
                    nodeDepthInfo[_curId] = new KeyValuePair<float, List<int>>(
                        0, new List<int>() { _curId });
                }

                return;
            });

            Action ResetMap = new Action(() =>
            {
                // nodeMaxDistInfo should cover all vertex in map so that we can use id of array to know which vertex it is
                nodeDepthInfo = new KeyValuePair<float, List<int>>[m_treeAdjMap.Keys.Count];
                m_vis = new bool[m_treeAdjMap.Keys.Count];
                for (int i = 0; i < m_treeAdjMap.Keys.Count; ++i)
                {
                    nodeDepthInfo[i] = new KeyValuePair<float, List<int>>(
                        Mathf.NegativeInfinity, null);
                    m_vis[i] = false;
                }
            });

            ResetMap();
            DfsDeepest(m_treeAdjMap.Keys.First());

            _endId = nodeDepthInfo[m_treeAdjMap.Keys.First()].Value[0];
            ResetMap();
            DfsDeepest(_endId);

            _startId = nodeDepthInfo[_endId].Value[0];

            List<int> finalNodeList = nodeDepthInfo[_endId].Value;
            for (int i = 0; i < finalNodeList.Count - 1; ++i)
            {
                trunkEdges.Add(new UndirectedEdge(finalNodeList[i], finalNodeList[i + 1], 0));
            }

            // Remember to clear static variable before return
            m_treeAdjMap = null;
            m_vis = null;

            return trunkEdges;
        }

        #endregion

        #region Divide Subtree

        public struct DivideRule
        {
            public readonly int minNodeInSubtree;
            public readonly int maxNodeInSubtree;
            public readonly List<int> isolatedIds;

            public DivideRule(int _min, int _max, List<int> _isolatedIds = null)
            {
                minNodeInSubtree = _min;
                maxNodeInSubtree = _max;
                isolatedIds = _isolatedIds ?? new List<int>();
            }
        }

        // All nodes in the same depth of tree
        private static List<int>[] m_nodesInDepth;
        // All children in the subtree that starts from current node
        private static int[] m_childrenCount;
        private static List<List<int>> m_subtrees;
        private static DivideRule m_divideRule;

        public static List<List<int>> DivideNodesIntoSubTrees(
            List<UndirectedEdge> _edges, int _rootId,
            DivideRule _divideRule,
            out int[] _parentMap)
        {
            m_treeAdjMap = ConvertUndirectedEdgesToAdjacencyMap(_edges);

            return DivideNodesIntoSubTrees(m_treeAdjMap, _rootId, _divideRule, out _parentMap);
        }

        public static List<List<int>> DivideNodesIntoSubTrees(
            Dictionary<int, List<DstEdge>> _treeAdjMap, int _rootId,
            DivideRule _divideRule,
            out int[] _parentMap)
        {
            m_treeAdjMap = _treeAdjMap;
            // Reset to record wether a node has been divided into a subtree
            m_vis = new bool[m_treeAdjMap.Keys.Count];
            m_divideRule = _divideRule;

            m_subtrees = new List<List<int>>();

            m_nodesInDepth = new List<int>[m_treeAdjMap.Keys.Count];
            m_childrenCount = new int[m_treeAdjMap.Keys.Count];
            m_parentMap = new int[m_treeAdjMap.Keys.Count];
            for (int i = 0; i < m_treeAdjMap.Keys.Count; ++i)
            {
                m_childrenCount[i] = -1;
                m_parentMap[i] = -1;
                m_vis[i] = false;
            }

            DfsNodeInfo(_rootId, _rootId, 0);

            DivideNodeFromBottom();

            _parentMap = m_parentMap;

            return m_subtrees;
        }

        private static void DfsNodeInfo(int _curId, int _parent, int _depth)
        {
            m_parentMap[_curId] = _parent;
            if (m_nodesInDepth[_depth] == null)
                m_nodesInDepth[_depth] = new List<int>();
            m_nodesInDepth[_depth].Add(_curId);

            bool isLeaf = true;
            foreach (DstEdge dstEdge in m_treeAdjMap[_curId])
            {
                if (dstEdge.dstId == m_parentMap[_curId])
                {
                    continue;
                }
                else
                {
                    isLeaf = false;
                }

                DfsNodeInfo(dstEdge.dstId, _curId, _depth + 1);
            }

            if (isLeaf)
            {
                m_childrenCount[_curId] = 1;
            }
            return;
        }

        private static void DivideNodeFromBottom()
        {
            for (int depth = m_nodesInDepth.Length - 1; depth >= 0; depth--)
            {
                if (m_nodesInDepth[depth] == null)
                    continue;

                for (int i = 0; i < m_nodesInDepth[depth].Count; ++i)
                {
                    int candidateId = m_nodesInDepth[depth][i];
                    int parentId = m_parentMap[candidateId];
                    // Initialize parent's children count
                    if (m_childrenCount[parentId] < 0)
                        m_childrenCount[parentId] = 1;

                    // If has reached root
                    if (candidateId == parentId)
                    {
                        m_subtrees.Add(DetachFromParentNode(candidateId));
                        continue;
                    }

                    // TODO: if parent is a unconnectable node e.g. an isolated node, we should detach from it

                    // If parent's children count has exceed limit after connecting, we should detach from it
                    if (m_childrenCount[parentId] + m_childrenCount[candidateId] > m_divideRule.maxNodeInSubtree)
                    {
                        m_subtrees.Add(DetachFromParentNode(candidateId));
                        continue;
                    }

                    if (m_childrenCount[candidateId] >= m_divideRule.minNodeInSubtree
                        && UnityEngine.Random.Range(0f, 1f) > 0.7f)
                    {
                        m_subtrees.Add(DetachFromParentNode(candidateId));
                        continue;
                    }

                    m_childrenCount[parentId] += m_childrenCount[candidateId];
                }
            }
        }

        /// <summary>
        /// Push current sub tree into final list.
        /// Update parent's childNodesCount.
        /// </summary>
        /// <param name="_curId"></param>
        private static List<int> DetachFromParentNode(int _curId)
        {
            Assert.IsTrue(m_childrenCount[_curId] >= 0,
                "Child node" + _curId + " should have been updated before detaching");

            List<int> nodesInSubtree = new List<int>();

            Queue<int> bfsQueue = new Queue<int>();
            bfsQueue.Enqueue(_curId);

            while (bfsQueue.Count > 0)
            {
                int id = bfsQueue.Dequeue();
                nodesInSubtree.Add(id);
                m_vis[id] = true;

                foreach (DstEdge dstEdge in m_treeAdjMap[id])
                {
                    if (dstEdge.dstId == m_parentMap[id] || m_vis[dstEdge.dstId])
                    {
                        continue;
                    }
                    bfsQueue.Enqueue(dstEdge.dstId);
                }
            }

            Assert.IsTrue(nodesInSubtree.Count == m_childrenCount[_curId], "Count in subtree " + _curId + "  should be the same as its updated childrenCount");
            return nodesInSubtree;
        }

        #endregion Divide Subtree
    }

    public class MinimumSpanningTree
    {
        Dictionary<int, List<DstEdge>> m_orgAdjMap;

        private class SortByLen : IComparer<UndirectedEdge>
        {
            int IComparer<UndirectedEdge>.Compare(UndirectedEdge _lhs, UndirectedEdge _rhs)
            {
                return _lhs.len.CompareTo(_rhs.len);
            }
        }

        public MinimumSpanningTree(Dictionary<int, List<DstEdge>> _adjacencyMap)
        {
            m_orgAdjMap = _adjacencyMap;
        }

        public List<UndirectedEdge> GenerateMst()
        {
            List<UndirectedEdge> mstEdgeList = new List<UndirectedEdge>();

            // Denote each vertex's minimum distance to existing Mst
            PriorityQueue<UndirectedEdge> EdgesToMst = new PriorityQueue<UndirectedEdge>(0, new SortByLen());
            bool[] vertexInMst = new bool[m_orgAdjMap.Keys.Count];
            float[] vertexMinDist = new float[m_orgAdjMap.Keys.Count];

            for (int i = 0; i < m_orgAdjMap.Keys.Count; ++i)
            {
                vertexInMst[i] = false;
                vertexMinDist[i] = Mathf.Infinity;
            }

            UndirectedEdge curEdge;
            // Here the length denotes the min distance to mst, instead of edge itself's length
            curEdge = new UndirectedEdge(m_orgAdjMap.Keys.First(), m_orgAdjMap.Keys.First(), 0);
            EdgesToMst.Push(curEdge);

            while (EdgesToMst.isEmpty() == false)
            {
                curEdge = EdgesToMst.Pop();

                if (vertexInMst[curEdge.dstId])
                    continue;

                vertexInMst[curEdge.dstId] = true;
                vertexMinDist[curEdge.dstId] = 0;
                if (curEdge.orgId != curEdge.dstId)
                {
                    curEdge.len = m_orgAdjMap[curEdge.orgId].Find((edge) => edge.dstId == curEdge.dstId).len;
                    mstEdgeList.Add(curEdge);
                }

                foreach (DstEdge nextEdge in m_orgAdjMap[curEdge.dstId])
                {
                    if (!vertexInMst[nextEdge.dstId] && nextEdge.len < vertexMinDist[nextEdge.dstId])
                    {
                        vertexMinDist[nextEdge.dstId] = nextEdge.len;
                        UndirectedEdge next = new UndirectedEdge(curEdge.dstId, nextEdge.dstId, nextEdge.len);
                        EdgesToMst.Push(next);
                    }
                }
            }
            
            return mstEdgeList;
        }
    }
}
