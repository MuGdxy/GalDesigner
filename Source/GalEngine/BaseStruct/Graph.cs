using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.GraphData
{
    class GraphNode<Data,Edge>
    {
        private bool isNextEnd;

        private Data data;

        private Dictionary<GraphNode<Data, Edge>, Edge> edgeMap;

        public GraphNode(Data Data)
        {
            isNextEnd = false;

            data = Data;

            edgeMap = new Dictionary<GraphNode<Data, Edge>, Edge>();
        }

        public bool IsNextEnd { get => isNextEnd; set => isNextEnd = value; }

        public Data NodeData { get => data; set => data = value; }

        public Dictionary<GraphNode<Data, Edge>, Edge> Next => edgeMap;

        public void Translation(GraphNode<Data,Edge> next, Edge edge)
        {
            if (next is null) { isNextEnd = true; return; }

            edgeMap[next] = edge;
        }

        public void RemoveEdge(GraphNode<Data,Edge> next)
        {
            edgeMap.Remove(next);
        }
    }

    class Graph<Data, Edge>
    {
        private Dictionary<string, GraphNode<Data, Edge>> nodes = new Dictionary<string, GraphNode<Data, Edge>>();

        public Dictionary<string, GraphNode<Data, Edge>> Nodes => nodes;

        public void AddNode(string nodeName,Data data)
        {
            nodes[nodeName] = new GraphNode<Data, Edge>(data);
        }

        public void Translation(string fromNode, string nextNode, Edge edge)
        {
            nodes[fromNode].Translation(nodes[nextNode], edge);
        }

        public void Translation(GraphNode<Data, Edge> fromNode, GraphNode<Data, Edge> nextNode, Edge edge)
        {
#if DEBUG
            if (nodes.ContainsValue(fromNode) is false ||
                nodes.ContainsValue(nextNode) is false)
                throw new Exception("InternalError: The Node must be in Graph!!!");
#endif

            fromNode.Translation(nextNode, edge);
        }
    }
}
