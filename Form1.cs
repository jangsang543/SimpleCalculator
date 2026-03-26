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

            // 디자이너에서 비활성화(Enabled = false)되어 있던 나누기 버튼을 활성화합니다.
            Dividebtn.Enabled = true;
        }

        // --------------------------------------------------
        // [1] 숫자 입력 기능 (0 ~ 9 버튼 클릭 시)
        // --------------------------------------------------
        private void Number_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // 현재 화면이 "0"이거나, 직전에 연산자를 눌렀다면 화면을 비우고 새로 입력받음
            if (txtBox2.Text == "0" || isOperatorClicked)
            {
                txtBox2.Text = "";
                isOperatorClicked = false;
            }

            txtBox2.Text += btn.Text; // 누른 숫자 버튼의 글자를 이어붙임
        }

        // --------------------------------------------------
        // [2] 사칙연산 기호 입력 기능 (+, -, X, ÷ 버튼 클릭 시)
        // --------------------------------------------------
        private void Operator_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentOperator = btn.Text; // 클릭된 버튼의 기호를 저장 (+, -, X, ÷)

            // 하단 텍스트박스의 현재 글자를 정수(Int)로 변환하여 첫 번째 숫자로 저장
            firstNumber = int.Parse(txtBox2.Text);

            isOperatorClicked = true; // 다음 숫자를 누를 때 화면이 새로 지워지도록 설정

            // 상단 텍스트박스(txtBox1)에 누른 연산 과정 표시 (예: 10 -)
            txtBox1.Text = $"{firstNumber} {currentOperator}";
        }

        // --------------------------------------------------
        // [3] 계산 결과 출력 기능 (= 버튼 클릭 시)
        // --------------------------------------------------
        private void Answerbtn_Click(object sender, EventArgs e)
        {
            // 하단 텍스트박스의 현재 글자를 정수(Int)로 변환하여 두 번째 숫자로 저장
            secondNumber = int.Parse(txtBox2.Text);
            int result = 0;

            // 저장된 연산자 기호에 따라 사칙연산 계산 수행
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
                case "÷": // 디자이너에 설정된 특수문자 ÷ 기호와 일치시킴
                    if (secondNumber != 0)
                    {
                        result = firstNumber / secondNumber; // 정수형 나눗셈 (몫만 계산)
                    }
                    else
                    {
                        MessageBox.Show("0으로 나눌 수 없습니다!", "계산 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBox2.Text = "0";
                        return; // 0으로 나누려 할 시 계산을 중단함
                    }
                    break;
            }

            // 계산 결과값(Int)을 다시 문자열(String)로 변환하여 하단 텍스트박스에 표시
            txtBox2.Text = result.ToString();

            // 상단 텍스트박스에 최종 완성된 수식 출력 (예: 10 - 2 = 8)
            txtBox1.Text = $"{firstNumber} {currentOperator} {secondNumber} = {result}";

            isOperatorClicked = true; // 계산이 끝난 후 숫자를 누르면 새로운 계산이 시작되도록 플래그 설정
        }

        // --------------------------------------------------
        // [4] 지우기 기능 (CE, C, del 버튼 클릭 시)
        // --------------------------------------------------
        private void CEbtn_Click(object sender, EventArgs e)
        {
            txtBox2.Text = "0"; // 현재 입력 중인 하단창만 0으로 초기화
        }

        private void Cbtn_Click(object sender, EventArgs e)
        {
            txtBox1.Text = ""; // 상단 수식창 비우기
            txtBox2.Text = "0"; // 하단 결과창 0으로 초기화

            // 모든 연산 변수 초기화
            firstNumber = 0;
            secondNumber = 0;
            currentOperator = "";
            isOperatorClicked = false;
        }

        private void Delbtn_Click(object sender, EventArgs e)
        {
            if (txtBox2.Text.Length > 0)
            {
                // 마지막 한 글자를 잘라냄 (백스페이스 기능)
                txtBox2.Text = txtBox2.Text.Substring(0, txtBox2.Text.Length - 1);
            }

            // 다 지워서 빈 칸이 되거나 음수 부호만 남으면 0으로 리셋
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
            // 숫자 버튼 이벤트 연결
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

            // 사칙연산 버튼 이벤트 연결
            Plusbtn.Click += Operator_Click;
            Minusbtn.Click += Operator_Click;
            Multibtn.Click += Operator_Click;
            Dividebtn.Click += Operator_Click;

            // 결과(=) 버튼 이벤트 연결
            Answerbtn.Click += Answerbtn_Click;

            // 지우기 버튼 이벤트 연결
            CEbtn.Click += CEbtn_Click;
            Cbtn.Click += Cbtn_Click;
            Delbtn.Click += Delbtn_Click;
        }
    }
}