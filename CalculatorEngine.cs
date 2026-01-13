
namespace Calculator
{
    class CalculatorEngine
    {
        private float _number1; 
        private float _number2;
        private bool _isSecondNumber;
        private enOperator _operator;

        public enum enOperator
        {
            Add,
            Subtract,
            Multiply,
            Divide
        }

        public void InputNumber(float number)
        {
            if (!_isSecondNumber)
                _number1 = number;
            else
                _number2 = number;
        }

        public void SetOperator(enOperator Op)
        {
            _operator = Op;
            _isSecondNumber = true;
        }

        public float GetResult()
        {
            float result = 0;

            switch(_operator)
            {
                case enOperator.Add: result = _number1 + _number2; break;
                case enOperator.Subtract: result = _number1 - _number2; break;
                case enOperator.Multiply: result = _number1 * _number2; break;
                case enOperator.Divide: result = _number2 != 0 ? _number1 / _number2 : 0; break;
            }

            Reset();
            return result;
        }

        public void Reset()
        {
            _number1 = 0;
            _number2 = 0;
            _isSecondNumber = false;
        }
    }
}
