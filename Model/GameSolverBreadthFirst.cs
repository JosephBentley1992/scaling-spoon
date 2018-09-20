using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model
{
    public class GameSolverBreadthFirst
    {
        private Engine _model = new Engine();
        private Dictionary<int, Node> _tree = new Dictionary<int, Node>();
        private Node _winningNode = null;
        private DestinationCell _winningDestination;
        private List<Direction> _allDirections = new List<Direction> { Direction.Up, Direction.Down, Direction.Right, Direction.Left };
        private List<int> _robotsByPriority = new List<int>();
        private int _numberOfNodesEvaluated = 0;
        public GameSolverBreadthFirst(Engine e)
        {
            _model = e;
        }

        public List<RobotMove> FindSolution()
        {
            NodeData nodeData = new NodeData(null, new Dictionary<int, Cell>(_model.RobotCurrentLocations));
            Node root = new Node(nodeData, 0, null);
            _tree.Add(root.GetIndex(), root);
            _winningDestination = _model.CurrentWinningDestination;

            for (int i = 0; i < _model.RobotCurrentLocations.Count; i++)
                _robotsByPriority.Add(i);

            _robotsByPriority = _robotsByPriority.OrderBy(r => r == _model.CurrentWinningDestination.WinningRobotId ? 0 : 1).ToList();

            Recursive(0);
            if (_winningNode == null)
                return new List<RobotMove>();

            Node n = _winningNode;
            //Traverse up the tree, adding the moves made to get to the winning position.
            List<RobotMove> movesToWin = new List<RobotMove>();
            while (n.Previous != null)
            {
                movesToWin.Add(n.Data.Move);
                n = n.Previous;
            }

            //This list is backwards, so we need to reverse it.
            movesToWin.Reverse();
            Console.WriteLine(String.Format("Number of nodes evaluated: {0}", _numberOfNodesEvaluated));
            Console.WriteLine(String.Format("Number of Nodes: {0}", _tree.Count));
            return movesToWin;
        }

        private void Recursive(int depth)
        {
            Dictionary<int, Node> nodesToAdd = new Dictionary<int, Node>();
            //Process all nodes at this depth
            foreach (Node prev in _tree.Where(t => t.Value.Depth == depth).Select(t => t.Value))
            {
                SetRobotCurrentLocations(prev);
                foreach (int i in _robotsByPriority)
                {
                    foreach (Direction d in _allDirections)
                    {
                        _numberOfNodesEvaluated++;

                        RobotMove move = _model.MoveRobot(i, d).FirstOrDefault();
                        if (move == null)
                            continue; //an Invalid move not worth saving

                        NodeData nodeData = new NodeData(move, new Dictionary<int, Cell>(_model.RobotCurrentLocations));
                        Node next = new Node(nodeData, depth + 1, prev);
                        prev.Next.Add(next);

                        //This move found a solution, we're done processing.
                        if (_model.CurrentWinningDestination.X != _winningDestination.X && _model.CurrentWinningDestination.Y != _winningDestination.Y)
                        {
                            _winningNode = next;
                            return;
                        }

                        //Check for a repeating position in the tree. Any repeated position does not need to be evaluated.
                        //It either occurs in the same number of moves, or fewer, since this is a breadth first algorithm.
                        if (!_tree.ContainsKey(next.GetIndex()) && !nodesToAdd.ContainsKey(next.GetIndex()))
                            nodesToAdd.Add(next.GetIndex(), next);

                        _model.UndoMove();
                    }
                }
            }

            //If we didn't add any new nodes, we've searched everything.
            if (nodesToAdd.Count == 0)
                return;

            _tree = _tree.Concat(nodesToAdd).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (_winningNode == null)
                Recursive(depth + 1);
        }

        private void SetRobotCurrentLocations(Node n)
        {
            //Undo all moves, and then redo the moves to get to this point in the tree.
            //TODO refactor this to just allow setting a board state, probably.
            for (int i = 0; i < n.Depth; i++)
                _model.UndoMove();

            List<RobotMove> movesToReachNode = new List<RobotMove>();
            while (n.Previous != null)
            {
                movesToReachNode.Add(n.Data.Move);
                n = n.Previous;
            }

            movesToReachNode.Reverse();
            foreach (RobotMove move in movesToReachNode)
                _model.MoveRobot(move.RobotId, move.GetDirection());
        }
    }
}
