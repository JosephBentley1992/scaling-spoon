using ScalingSpoon.Model.Bus;
using ScalingSpoon.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingSpoon.Model
{
    public class GameSolver
    {
        private Engine _model = new Engine();
        private Dictionary<int, Node> _tree = new Dictionary<int, Node>();
        private List<Node> _winningNodes = new List<Node>();
        private int _fastestWin = -1;
        private DestinationCell _winningDestination;
        private List<Direction> _allDirections = new List<Direction> { Direction.Up, Direction.Down, Direction.Right, Direction.Left };
        private List<int> _robotsByPriority = new List<int>();
        private int _numberOfNodesEvaluated = 0;
        public GameSolver(Engine e)
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

            Recursive(root);
            Node winner = _winningNodes.OrderBy(n => n.Depth).FirstOrDefault();
            if (winner == null)
                return null;

            //Traverse up the tree, adding the moves made to get to the winning position.
            List<RobotMove> movesToWin = new List<RobotMove>();
            while (winner.Previous != null)
            {
                movesToWin.Add(winner.Data.Move);
                winner = winner.Previous;
            }

            //This list is backwards, so we need to reverse it.
            movesToWin.Reverse();
            Console.WriteLine(String.Format("Number of nodes evaluated: {0}", _numberOfNodesEvaluated));
            Console.WriteLine(String.Format("Number of Nodes: {0}", _tree.Count));
            return movesToWin;
        }

        private void Recursive(Node prev)
        {
            //Don't attempt to find any paths that are longer than the fastest path discovered so far.
            if (_fastestWin != -1 && prev.Depth >= _fastestWin || prev.Depth > 30)
                return;

            Direction prevDirection = Direction.Up;
            if (prev.Data.Move != null)
                prevDirection = prev.Data.Move.GetDirection();

            foreach (int i in _robotsByPriority)
            {
                foreach (Direction d in _allDirections)
                {
                    _numberOfNodesEvaluated++;

                    //The same robot never needs to move in the same dimension consecutively.
                    //But because im swapping tree nodes... my prev.Data.Move is changing, which is causing the recursive calls to not follow a line it would otherwise be following.
                    //if (prev.Previous != null && prev.Data.Move != null && prev.Data.Move.RobotId == i && 
                    //    ((d == Direction.Up || d == Direction.Down) && (prevDirection == Direction.Up || prevDirection == Direction.Down)
                    //    || (d == Direction.Left || d == Direction.Right) && (prevDirection == Direction.Left || prevDirection == Direction.Right)))
                    //    continue;

                    RobotMove move = _model.MoveRobot(i, d).FirstOrDefault();
                    if (move == null)
                        continue; //an Invalid move not worth saving

                    NodeData nodeData = new NodeData(move, new Dictionary<int, Cell>(_model.RobotCurrentLocations));
                    Node next = new Node(nodeData, prev.Depth + 1, prev);
                    prev.Next.Add(next);

                    //Check for a repeating position in the tree. Repeated positions do not need to do any more recursive calls.
                    //If the current depth is quicker than the repeated positions depth, 
                    // we should use this path to the position. Rebalance the tree + update depths.
                    Node repeatedNode = null;
                    bool shouldRecurse = false;
                    if (_tree.ContainsKey(next.GetIndex()))
                        repeatedNode = _tree[next.GetIndex()];

                    if (repeatedNode != null)
                    {
                        if (repeatedNode.Depth > next.Depth)
                            SwapRepeatingNode(repeatedNode, next);
                    }
                    else
                    {
                        //Eww refactor this
                        shouldRecurse = true;
                        _tree.Add(next.GetIndex(), next);
                    }

                    //This move found a solution
                    if (_model.CurrentWinningDestination.X != _winningDestination.X && _model.CurrentWinningDestination.Y != _winningDestination.Y)
                    {
                        if (_fastestWin == -1)
                            _fastestWin = next.Depth;

                        if (_fastestWin > next.Depth)
                            _fastestWin = next.Depth;

                        _winningNodes.Add(next);
                        _model.UndoMove();
                        return;
                    }

                    if (shouldRecurse)
                        Recursive(next);

                    //Before trying other paths from this node, Undo the current move.
                    _model.UndoMove();
                }
            }
        }

        private void SwapRepeatingNode(Node slowerNode, Node fasterNode)
        {
            fasterNode.Next = slowerNode.Next;

            foreach (Node n in fasterNode.Next)
                n.Previous = fasterNode;

            //I think these two have the same reference, so doing the below loses a bunch of paths.
            //Maybe a DeepCopy and then nulling this would work, but for now we'll just keep the reference.
            //repeatingNode.Next = new List<Node>();
            _tree.Remove(slowerNode.GetIndex());
            _tree.Add(fasterNode.GetIndex(), fasterNode);

            //Update depths, though.
            RecursiveUpdateDepth(fasterNode, fasterNode.Depth);
        }

        private void RecursiveUpdateDepth(Node currentNode, int depth)
        {
            currentNode.Depth = depth;
            if (_winningNodes.Contains(currentNode) && _fastestWin > currentNode.Depth)
                _fastestWin = depth;

            if (currentNode.Next == null)
                return;

            foreach (Node n in currentNode.Next)
                RecursiveUpdateDepth(n, depth + 1);
        }
    }
}
