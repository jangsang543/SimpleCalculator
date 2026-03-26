using System;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Form1 : Form
    {
        // 계산에 필요한 전역 변수들
        private int firstNumber = 0;   // 첫 번째 피연산자
        private int secondNumber = 0;  // 두 번째 피연산자
        private string currentOperator = ""; // 현재 연산자 (+, -, X, ÷)
        private bool isOperatorClicked = false; // 연산자가 클릭되었는지 여부 (새 숫자 입력용)

        public Form1()
        {
            InitializeComponent();
            AttachEvents(); // 프로그램 실행 시 버튼과 기능들을 연결합니다.

            Dividebtn.Enabled = true; // 디자이너에서 꺼져있던 나누기 버튼 활성화
        }

        // --------------------------------------------------
        // [1] 숫자 입력 기능 (0 ~ 9 버튼 클릭 시)
        // --------------------------------------------------
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

        // --------------------------------------------------
        // [2] 사칙연산 기호 입력 기능 (+, -, X, ÷ 버튼 클릭 시)
        // --------------------------------------------------
        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentOperator = btn.Text;

            firstNumber = int.Parse(txtBox2.Text);

            isOperatorClicked = true;

            txtBox1.Text = $"{firstNumber} {currentOperator}";
        }

        // --------------------------------------------------
        // [3] 계산 결과 출력 기능 (= 버튼 클릭 시)
        // --------------------------------------------------
        private void Answerbtn_Click(object sender, EventArgs e)
        {
            secondNumber = int.Parse(txtBox2.Text);
            int result = 0;

            switch (currentOperator)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "X":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber != 0)
                    {
                        result = firstNumber / secondNumber;
                    }
                    else
                    {
                        MessageBox.Show("0으로 나눌 수 없습니다!", "계산 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBox2.Text = "0";
                        return;
                    }
                    break;
            }

            txtBox2.Text = result.ToString();
            txtBox1.Text = $"{firstNumber} {currentOperator} {secondNumber} = {result}";

            isOperatorClicked = true;
        }

        // --------------------------------------------------
        // [4] 수정 및 삭제 기능 (CE, C, del 버튼 클릭 시)
        // --------------------------------------------------

        // CE 버튼 : 마지막에 입력한 하단 텍스트박스(피연산자) 값만 초기화
        private void CEbtn_Click(object sender, EventArgs e)
        {
            txtBox2.Text = "0";
        }

        // C 버튼 : 메모리 변수와 모든 텍스트박스를 처음 상태로 초기화
        private void Cbtn_Click(object sender, EventArgs e)
        {
            txtBox1.Text = "";
            txtBox2.Text = "0";

            firstNumber = 0;
            secondNumber = 0;
            currentOperator = "";
            isOperatorClicked = false;
        }

        // Del 버튼 : 마지막에 입력된 숫자 글자 하나만 제거
        private void Delbtn_Click(object sender, EventArgs e)
        {
            if (txtBox2.Text.Length > 0)
            {
                // 문자열의 마지막 한 글자를 잘라냅니다.
                txtBox2.Text = txtBox2.Text.Substring(0, txtBox2.Text.Length - 1);
            }

            // 다 지워져서 공백이 되거나 음수 부호만 남을 시 "0"으로 보정합니다.
            if (txtBox2.Text == "" || txtBox2.Text == "-")
            {
                txtBox2.Text = "0";
            }
        }

        // --------------------------------------------------
        // [5] 디자이너에 생성된 버튼과 실제 코드를 묶어주는 연결 함수
        // --------------------------------------------------
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

            // 수정 및 삭제 이벤트 바인딩
            CEbtn.Click += CEbtn_Click;
            Cbtn.Click += Cbtn_Click;
            Delbtn.Click += Delbtn_Click;
        }
    }
}