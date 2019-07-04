using ScalingSpoon.Model;
using ScalingSpoon.Model.Bus;
namespace ScalingSpoon.Control
{
    public class Controller
    {
        private Engine _model;

        public Controller()
        {
            _model = new Engine();
        }

        public void StartDefaultGame()
        {
            _model.ConstructBoard(16, 16, 16, 4, 0, 0);
        }
    }
}
