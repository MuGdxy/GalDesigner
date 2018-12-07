using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public struct GraphNode<T>
    {
        public T Data { get; }
        public HashSet<GraphNode<T>> Next { get; }

        public GraphNode(T data)
        {
            Data = data;

            Next = new HashSet<GraphNode<T>>();
        }
    }

    public class Graph<T>
    {
        protected Dictionary<T, GraphNode<T>> mNodes;
        
        public Graph()
        {
            mNodes = new Dictionary<T, GraphNode<T>>();
        }

        public void AddNode(GraphNode<T> graphNode)
        {
            mNodes.Add(graphNode.Data, graphNode);
        }

        public void RemoveNode(GraphNode<T> graphNode)
        {
            RemoveNode(graphNode.Data);
        }

        public void RemoveNode(T data)
        {
            if (mNodes.ContainsKey(data) is false) return;

            var removedNode = mNodes[data];

            mNodes.Remove(data);

            foreach (var node in mNodes)
            {
                RemoveEdge(node.Value, removedNode);
            }
        }
        
        public void AddEdge(GraphNode<T> fromNode, GraphNode<T> toNode)
        {
            fromNode.Next.Add(toNode);
        }

        public void AddEdge(T fromData, T toData)
        {
            mNodes[fromData].Next.Add(mNodes[toData]);
        }

        public void RemoveEdge(GraphNode<T> fromNode, GraphNode<T> toNode)
        {
            if (fromNode.Next.Contains(toNode) is false) return;

            fromNode.Next.Remove(toNode);
        }

        public void RemoveEdge(T fromData, T toData)
        {
            RemoveEdge(mNodes[fromData], mNodes[toData]);
        }

        public bool IsNodeExist(T data)
        {
            if (mNodes.ContainsKey(data) is true) return true;
            return false;
        }
    }
}
