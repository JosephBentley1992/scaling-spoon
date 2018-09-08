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
        private List<Node> _tree = new List<Node>();
        private List<Node> _winningNodes = new List<Node>();
        private int _fastestWin = -1;
        private DestinationCell _winningDestination;
        private List<Direction> _allDirections = new List<Direction> { Direction.Up, Direction.Down, Direction.Right, Direction.Left };
        public GameSolver(Engine e)
        {
            _model = e;
        }

        public List<RobotMove> FindSolution()
        {
            _model.MoveRobot(0, Direction.Right);
            return new List<RobotMove>() { new RobotMove(0, _model.Board[2, 1], _model.Board[2, 2]) };
            //return new List<RobotMove>();
        }

        private void Recursive(Node prev)
        {
            //Don't attempt to find any paths that are longer than the fastest path discovered so far.
            if (prev.Depth > _fastestWin)// || prev.Depth > 30)
                return;

            Direction prevDirection = prev.Data.Move.GetDirection();
            for (int i = 0; i < _model.RobotCurrentLocations.Count; i++)
            {
                foreach(Direction d in _allDirections)
                {
                    //The same robot never needs to move in the same dimension consecutively.
                    if (prev.Previous != null && prev.Data.Move.RobotId == i && 
                        ((d == Direction.Up || d == Direction.Down) && (prevDirection == Direction.Up || prevDirection == Direction.Down)
                        || (d == Direction.Left || d == Direction.Right) && (prevDirection == Direction.Left || prevDirection == Direction.Right)))
                        continue;

                    RobotMove move = _model.MoveRobot(i, d).FirstOrDefault();
                    if (move == null)
                        continue; //an Invalid move not worth saving

                    NodeData nodeData = new NodeData(move, new Dictionary<int, Cell>(_model.RobotCurrentLocations));
                    Node next = new Node(nodeData, prev.Depth + 1, prev);
                    prev.Next.Add(next);
                    _tree.Add(next);

                    //This move found a solution
                    if (_model.CurrentWinningDestination.X != _winningDestination.X && _model.CurrentWinningDestination.Y != _winningDestination.Y)
                    {
                        if (_fastestWin == -1)
                            _fastestWin = next.Depth;

                        if (_fastestWin > next.Depth)
                            _fastestWin = next.Depth;

                        _winningNodes.Add(next);
                    }

                    //Check for a repeating position in the tree. Repeated positions do not need to do any more recursive calls.
                    //If the current depth is quicker than the repeated positions depth, 
                    // we should use this path to the position. Rebalance the tree + update depths.
                    Node repeatedNode = _tree.FirstOrDefault(n => IsRepeatedPosition(n, next));
                    if (repeatedNode != null)
                    {
                        if (repeatedNode.Depth > next.Depth)
                            SwapRepeatingNode(repeatedNode, next);
                    }
                    else
                    {
                        Recursive(next);
                    }

                    //Before trying other paths from this node, Undo the current move.
                    _model.UndoMove();
                }
            }
        }

        private bool IsRepeatedPosition(Node a, Node b)
        {
            for (int i = 0; i < a.Data.CurrentRobotLocations.Count; i++)
                if (!a.Data.CurrentRobotLocations[i].Equals(b.Data.CurrentRobotLocations[i]))
                    return false;
            return true;
        }

        private void SwapRepeatingNode(Node repeatingNode, Node next)
        {

        }
    }
}
