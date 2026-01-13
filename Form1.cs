using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class frmCalculator : Form
    {
        CalculatorEngine engine = new CalculatorEngine();
        private Label lblDisplay = new Label();

        private string currentInput = "";
        private bool hasPendingOperator = false;

        public frmCalculator()
        {
            InitializeComponent();
        }

        private Label CreateDisplay()
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 100,
                Text = "0",
                TextAlign = ContentAlignment.BottomRight,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.Black,
                BackColor = Color.White,
            };
        }

        private Button CreateButton(string text, Color backColor)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Black,
                BackColor = backColor,
            };
        }

        private Button AddButton(TableLayoutPanel panel, string text, Color color, byte col, byte row, EventHandler onClick)
        {
            Button btn = CreateButton(text, color);
            if (onClick != null)
                btn.Click += onClick;

            panel.Controls.Add(btn, col, row);
            return btn;
        }

        TableLayoutPanel CreateButtonsPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel()
            {
                RowCount = 5,
                ColumnCount = 4,
                Dock = DockStyle.Fill
            };

            for (byte i = 0; i < 5; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            for (byte i = 0; i < 4; i++)
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            Color operatorColor = Color.Orange;
            Color numberColor = Color.White;

            // Row 0
            AddButton(panel, "Back", operatorColor, 0, 0, BackspaceButton_Click);
            AddButton(panel, "%", operatorColor, 1, 0, PercentButton_Click);
            AddButton(panel, "/", operatorColor, 2, 0, OperatorButton_Click);

            // Row 1
            AddButton(panel, "7", numberColor, 0, 1, NumberButton_Click);
            AddButton(panel, "8", numberColor, 1, 1, NumberButton_Click);
            AddButton(panel, "9", numberColor, 2, 1, NumberButton_Click);
            AddButton(panel, "x", operatorColor, 3, 1, OperatorButton_Click);

            // Row 2
            AddButton(panel, "4", numberColor, 0, 2, NumberButton_Click);
            AddButton(panel, "5", numberColor, 1, 2, NumberButton_Click);
            AddButton(panel, "6", numberColor, 2, 2, NumberButton_Click);
            AddButton(panel, "-", operatorColor, 3, 2, OperatorButton_Click);

            // Row 3
            AddButton(panel, "1", numberColor, 0, 3, NumberButton_Click);
            AddButton(panel, "2", numberColor, 1, 3, NumberButton_Click);
            AddButton(panel, "3", numberColor, 2, 3, NumberButton_Click);
            AddButton(panel, "+", operatorColor, 3, 3, OperatorButton_Click);

            // Row 4
            AddButton(panel, "C", operatorColor, 0, 4, ClearButton_Click);
            AddButton(panel, "0", numberColor, 1, 4, NumberButton_Click);
            AddButton(panel, ".", operatorColor, 2, 4, DotButton_Click);
            AddButton(panel, "=", operatorColor, 3, 4, EqualButton_Click);

            return panel;
        }

        private void frmCalculator_Load(object sender, EventArgs e)
        {
            lblDisplay = CreateDisplay();
            TableLayoutPanel panel = CreateButtonsPanel();

            this.Controls.Add(panel);
            this.Controls.Add(lblDisplay);
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string digit = btn.Text;

            if (currentInput == "" || currentInput == "0")
                currentInput = digit;
            else
                currentInput += digit;

            lblDisplay.Text = currentInput;
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string op = btn.Text;

            if(hasPendingOperator && currentInput != "" && currentInput != "0")
            {
                engine.InputNumber(Convert.ToSingle(currentInput));
                float tempResult = engine.GetResult();

                currentInput = tempResult.ToString();
                lblDisplay.Text = currentInput;

                engine.InputNumber(Convert.ToSingle(currentInput));
            }
            else if (hasPendingOperator && (currentInput == "" || currentInput == "0"))
            {

            }
            else
            {
                engine.InputNumber(Convert.ToSingle(currentInput));
            }

            switch(op)
            {
                case "+": engine.SetOperator(CalculatorEngine.enOperator.Add); break;
                case "-": engine.SetOperator(CalculatorEngine.enOperator.Subtract); break;
                case "/": engine.SetOperator(CalculatorEngine.enOperator.Divide); break;
                case "x": engine.SetOperator(CalculatorEngine.enOperator.Multiply); break;
                default: return;
            }

            hasPendingOperator = true;
            currentInput = "0";
        }

        private void EqualButton_Click(object sender, EventArgs e)
        {
            if (hasPendingOperator && (currentInput == "" || currentInput == "0"))
                return;

            engine.InputNumber(Convert.ToSingle(currentInput));
            float result = engine.GetResult();

            lblDisplay.Text = result.ToString();
            currentInput = result.ToString();

            hasPendingOperator = false;
        }

        private void BackspaceButton_Click(object sender, EventArgs e)
        {
            if (currentInput.Length > 1)
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            else
                currentInput = "0";

            lblDisplay.Text = currentInput;
        }

        private void PercentButton_Click(object sender, EventArgs e)
        {
            float value = Convert.ToSingle( currentInput);
            value = value / 100f;

            currentInput = value.ToString();
            lblDisplay.Text = currentInput;
        }

        private void DotButton_Click(object sender, EventArgs e)
        {
            if (!currentInput.Contains("."))
            {
                if (currentInput == "" || currentInput == "0")
                    currentInput = "0.";
                else
                    currentInput += ".";

                lblDisplay.Text = currentInput;
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            currentInput = "0";
            lblDisplay.Text = "0";

            engine.Reset();
            hasPendingOperator = false;
        }
    }
}
