using System;
using System.Data;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Form1 : Form
    {
        private bool isOperatorClicked = false;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            AttachEvents();
        }

        // --------------------------------------------------
        // [키보드 실시간 입력 제어]
        // --------------------------------------------------
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                if (e.KeyCode == Keys.D9) { AddBracketToTextBox("("); e.SuppressKeyPress = true; e.Handled = true; return; }
                if (e.KeyCode == Keys.D0) { AddBracketToTextBox(")"); e.SuppressKeyPress = true; e.Handled = true; return; }
                if (e.KeyCode == Keys.Oemplus) { AppendTextToTextBox("+"); e.SuppressKeyPress = true; e.Handled = true; return; }
            }

            bool handled = false;
            string pressedNumber = "";

            if (!e.Shift && e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                pressedNumber = (e.KeyCode - Keys.D0).ToString();
                handled = true;
            }
            else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                pressedNumber = (e.KeyCode - Keys.NumPad0).ToString();
                handled = true;
            }

            if (handled)
            {
                AppendTextToTextBox(pressedNumber);
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }

            // ★ 키보드로 곱하기(*)나 나누기(/)를 쳐도 화면에는 'X'와 '÷'로 뜨게 바꿨습니다!
            if (e.KeyCode == Keys.Add) { AppendTextToTextBox("+"); e.SuppressKeyPress = true; e.Handled = true; }
            else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus) { AppendTextToTextBox("-"); e.SuppressKeyPress = true; e.Handled = true; }
            else if (e.KeyCode == Keys.Multiply) { AppendTextToTextBox("X"); e.SuppressKeyPress = true; e.Handled = true; }
            else if (e.KeyCode == Keys.Divide || e.KeyCode == Keys.OemQuestion) { AppendTextToTextBox("÷"); e.SuppressKeyPress = true; e.Handled = true; }

            else if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; e.Handled = true; Answerbtn_Click(null, null); }
            else if (e.KeyCode == Keys.Back) { Delbtn_Click(null, null); e.SuppressKeyPress = true; e.Handled = true; }
            else if (e.KeyCode == Keys.Escape) { Cbtn_Click(null, null); e.SuppressKeyPress = true; e.Handled = true; }
            else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) { AppendTextToTextBox("."); e.SuppressKeyPress = true; e.Handled = true; }
        }


        private void AppendTextToTextBox(string text)
        {
            if (txtBox2.Text == "0" || txtBox2.Text == "에러" || txtBox2.Text == "0으로 나눌 수 없습니다.")
                txtBox2.Text = "";

            if (isOperatorClicked)
            {
                txtBox2.Text = "";
                isOperatorClicked = false;
            }

            int cursorPosition = txtBox2.SelectionStart;
            string beforeText = txtBox2.Text.Substring(0, cursorPosition);
            string afterText = txtBox2.Text.Substring(cursorPosition);

            txtBox2.Text = beforeText + text + afterText;
            txtBox2.SelectionStart = cursorPosition + text.Length;

            txtBox1.Text = txtBox2.Text;
        }

        private void AddBracketToTextBox(string bracket)
        {
            if (txtBox2.Text == "0" || txtBox2.Text == "에러" || txtBox2.Text == "0으로 나눌 수 없습니다.")
                txtBox2.Text = "";

            int cursorPosition = txtBox2.SelectionStart;
            string beforeText = txtBox2.Text.Substring(0, cursorPosition);
            string afterText = txtBox2.Text.Substring(cursorPosition);

            txtBox2.Text = beforeText + bracket + afterText;
            txtBox2.SelectionStart = cursorPosition + 1;

            txtBox1.Text = txtBox2.Text;
        }


        private void Number_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            AppendTextToTextBox(btn.Text);
        }

        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            AppendTextToTextBox(btn.Text); // 화면에는 X나 ÷가 그대로 찍힘!
        }


        // --------------------------------------------------
        // [★ 마법의 정점] 내부에서만 몰래 컴퓨터 기호로 치환해서 계산
        // --------------------------------------------------
        private void Answerbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string expression = txtBox2.Text;

                // ★ 화면에 보이는 X와 ÷를 컴퓨터가 읽을 수 있는 *와 /로 치환합니다!
                expression = expression.Replace("X", "*").Replace("÷", "/");

                DataTable dt = new DataTable();
                var result = dt.Compute(expression, "");

                txtBox1.Text = txtBox2.Text + " ="; // 상단에는 사람이 보던 'X' 수식 그대로 출력
                txtBox2.Text = result.ToString();

                isOperatorClicked = true;
            }
            catch (DivideByZeroException)
            {
                txtBox2.Text = "0으로 나눌 수 없습니다.";
                isOperatorClicked = true;
            }
            catch
            {
                txtBox2.Text = "에러";
                isOperatorClicked = true;
            }
        }


        private void CEbtn_Click(object sender, EventArgs e)
        {
            txtBox2.Text = "0";
            txtBox1.Text = "0";
        }

        private void Cbtn_Click(object sender, EventArgs e)
        {
            txtBox1.Text = "";
            txtBox2.Text = "0";
            isOperatorClicked = false;
        }

        private void Delbtn_Click(object sender, EventArgs e)
        {
            if (txtBox2.Text == "에러" || txtBox2.Text == "0으로 나눌 수 없습니다.")
            {
                txtBox2.Text = "0";
                return;
            }

            int cursorPosition = txtBox2.SelectionStart;

            if (cursorPosition > 0 && txtBox2.Text.Length > 0)
            {
                string beforeText = txtBox2.Text.Substring(0, cursorPosition - 1);
                string afterText = txtBox2.Text.Substring(cursorPosition);

                txtBox2.Text = beforeText + afterText;
                txtBox2.SelectionStart = cursorPosition - 1;
            }

            if (txtBox2.Text == "") txtBox2.Text = "0";
            txtBox1.Text = txtBox2.Text;
        }


        private void AttachEvents()
        {
            number0.Click += Number_Click;
            number1.Click += Number_Click;
            number2.Click += Number_Click;
            number3.Click += Number_Click;
            number4.Click += Number_Click;
            number5.Click += Number_Click;
            number6.Click += Number_Click;
            number7.Click += Number_Click;
            number8.Click += Number_Click;
            number9.Click += Number_Click;

            Plusbtn.Click += Operator_Click;
            Minusbtn.Click += Operator_Click;
            Multibtn.Click += Operator_Click;
            Dividebtn.Click += Operator_Click;

            Answerbtn.Click += Answerbtn_Click;

            CEbtn.Click += CEbtn_Click;
            Cbtn.Click += Cbtn_Click;
            Delbtn.Click += Delbtn_Click;
        }
    }
}