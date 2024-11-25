using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiggableDice
{
    internal class Dice : Actor
    {
        private bool _isRolling = false;
        private double _timer;
        private int _displayedNumber;
        private int _maxRoll;
        private int _rollCount;
        private int _result;

        public Dice(int maxRoll)
        {
            _maxRoll = maxRoll;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            // if it is rolling, change the displayed number every half a second
            // and increment the roll count

            // when the roll count is equal to five or something, stop rolling and land on a number
            // if the dice was rigged, make it the specified value

        }
    }
}
