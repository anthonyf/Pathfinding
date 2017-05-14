using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AF.Pathfinding
{
    public interface IGraph<TNode>
    {
        List<TNode> GetAllNodes();
        List<TNode> GetNeighbors(TNode node);
        float GetEdgeCost(TNode a, TNode b);
    }
}