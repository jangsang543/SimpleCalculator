using System;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Form1 : Form
    {
        // 1. 계산에 필요한 전역 변수들
        private int firstNumber = 0;   // 첫 번째 피연산자
        private int secondNumber = 0;  // 두 번째 피연산자
        private string currentOperator = ""; // 현재 연산자 (+, -, X, ÷)
        private bool isOperatorClicked = false; // 연산자가 클릭되었는지 여부

        public Form1()
        {
            InitializeComponent();
            AttachEvents(); // 프로그램 실행 시 버튼 이벤트를 연결합니다.
        }

        // 2. 숫자 버튼 클릭 시 작동하는 기능 (숫자 이어붙이기)
        private void Number_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (txtBox2.Text == "0" || isOperatorClicked)
            {
                txtBox2.Text = "";
                isOperatorClicked = false;
            }

            txtBox2.Text += btn.Text;
        }

        // 3. 사칙연산 (+, -, X, ÷) 버튼 클릭 시 작동하는 기능
        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentOperator = btn.Text;

            firstNumber = int.Parse(txtBox2.Text);

            isOperatorClicked = true;

            txtBox1.Text = $"{firstNumber} {currentOperator}";
        }

        // 4. 결과 (=) 버튼 클릭 시 작동하는 기능
        private void Answerbtn_Click(object sender, EventArgs e)
        {
            secondNumber = int.Parse(txtBox2.Text);
            int result = 0;

            switch (currentOperator)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "-": result = firstNumber - secondNumber; break;
                case "X": result = firstNumber * secondNumber; break;
                case "÷":
                    if (secondNumber != 0) result = firstNumber / secondNumber;
                    break;
            }

            txtBox2.Text = result.ToString();

            txtBox1.Text = $"{firstNumber} {currentOperator} {secondNumber} = {result}";

            isOperatorClicked = true;
        }

        // 5. 버튼과 기능을 묶어주는 연결 함수
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
        }
    }
}