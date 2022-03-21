using lab20calcWpfApp1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using lab20calcWpfApp1.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using static lab20calcWpfApp1.Models.CalcOperations;

namespace lab20calcWpfApp1.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private double number1 = 0;
        private double number2 = 0;
        private CalcOper calcOp = CalcOper.None;

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string strData = "0";
        public string StrData
        {
            get => strData;
            set
            {
                double valueDouble = 0;
                if (StrToOper.DataStrToDouble(value, ref valueDouble))
                {
                    strData = value;
                }
                else
                {
                    strData = "0";
                }
                OnPropertyChanged();
            }
        }

        private string strCalc;
        public string StrCalc
        {
            get => strCalc;
            set
            {
                if (value != null)
                {
                    strCalc = value;
                }
                else
                {
                    strCalc = "0";
                }
                OnPropertyChanged();
            }
        }

        public ICommand BackSpace { get; }
        private void OnBackSpaceExecute(object p)
        {
            StrData = StrToOper.StrBackSpace(StrData);
        }
        private bool CanBackSpaceExecuted(object p)
        {
            if (StrData != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Команда для цифрового набора
        public ICommand NumberButtonCommand { get; }
        private void OnNumberButtonCommandExecute(object p)
        {
            //В параметр передана вызывающая кнопка, чтобы обработать ее название в качестве набираемого значения
            string dataStr = StrData;
            StrData = StrToOper.StrEnterData(dataStr, (p as Button).Content.ToString());
        }

        //Команда выбора операций 
        //Необходима для формирования расчетной цифры (number1) и определения дальнейшей операции вычисления
        public ICommand OperButtonCommand { get; }
        private void OnOperButtonCommandExecute(object p)
        {
            //Проверка на наличие данных в переменных, если есть - промежуточный расчет   
            string dataStr = StrData;
            StrData = "0";
            double tempNumber = 0;
            if (number1 != 0)
            {
                StrToOper.DataStrToDouble(dataStr, ref number2);
                tempNumber = number2;
                number1 = CalcOperations.Calculator(calcOp, number1, number2);
            }
            else
            {
                StrToOper.DataStrToDouble(dataStr, ref number1);
                tempNumber = number1;
            }

            calcOp = StrToOper.ConvertStrToCalcOper((p as Button).Content.ToString());

            string res;
            if (tempNumber < 0)
            {
                res = " (" + dataStr + ")";
            }
            else
            {
                res = " " + dataStr;
            }
            string calcStr = StrCalc;

            StrCalc = StrToOper.StrEnterData(calcStr, res + " " + (p as Button).Content.ToString());
        }
        
        
        public ICommand MonoButtonCommand { get; }      //Команда для моноопераций
        private void OnMonoButtonCommandExecute(object p)
        {
            string dataStr = StrData;
            CalcOper opStr = StrToOper.ConvertStrToCalcOper((p as Button).Content.ToString());
            //моно операции используются только для текущего рассчета, то его переводим во временную переменную.
            double tempNum = 0;
            StrToOper.DataStrToDouble(dataStr, ref tempNum);
            double res = CalcOperations.Calculator(opStr, tempNum);
            StrData = res.ToString();
        }

        public ICommand NegativeButtonCommand { get; }
        private void OnNegativeButtonCommandExecute(object p)
        {
            string dataStr = StrData;
            string operStr = (p as Button).Content.ToString();
            CalcOper calc = StrToOper.ConvertStrToCalcOper(operStr);

            // т.к. отрицательния/не отрицательния используется в пределах текущего набора цифр,
            // то результат выполнения выводим в StrData (нижний TexBox)
            double num = 0;
            StrToOper.DataStrToDouble(dataStr, ref num);
            double res = CalcOperations.Calculator(calc, num);

            StrData = res.ToString();
        }

        //Команда для расчета процента
        public ICommand ProcentButtonCommand { get; }
        private void OnProcentButtonCommandExecute(object p)
        {
            string dataStr = StrData;
            // Поскольку операция процента используется только после выбора операции вычисления и совместно с ней,
            // то определение самого процента выделяем во временной переменной, а результат выводим в StrData (нижний TexBox)
            double procNum = 0;
            StrToOper.DataStrToDouble(dataStr, ref procNum);
            CalcOper temCalc = StrToOper.ConvertStrToCalcOper((p as Button).Content.ToString());
            double res = CalcOperations.Calculator(temCalc, number1, procNum);
            StrData = res.ToString();
        }
        private bool CanProcentButtonCommandExecuted(object p)
        {
            if (number1 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Команда для расчета
        public ICommand CalculatorButtonCommand { get; }
        private void OnCalculatorButtonCommandExecute(object p)
        {
            string dataStr = StrData;

            StrToOper.DataStrToDouble(dataStr, ref number2);
            double res = CalcOperations.Calculator(calcOp, number1, number2);
            StrData = res.ToString();
            StrCalc = "";
            number1 = number2 = 0;
        }
        private bool CanCalculatorButtonCommandExecuted(object p)
        {
            if (number1 != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Команда для обнудения
        public ICommand ClearButtonCommand { get; }
        private void OnClearButtonCommandExecute(object p)
        {
            string clearStr = (p as Button).Content.ToString();

            if (clearStr == "CE")
            {
                StrCalc = "";
                number1 = number2 = 0;
            }
            StrData = "0";
        }

        public MainWindowViewModel()
        {
            NumberButtonCommand = new RelayCommand(OnNumberButtonCommandExecute, null);
            NegativeButtonCommand = new RelayCommand(OnNegativeButtonCommandExecute, null);

            OperButtonCommand = new RelayCommand(OnOperButtonCommandExecute, null);
            MonoButtonCommand = new RelayCommand(OnMonoButtonCommandExecute, null);
            ProcentButtonCommand = new RelayCommand(OnProcentButtonCommandExecute, CanProcentButtonCommandExecuted);
            CalculatorButtonCommand = new RelayCommand(OnCalculatorButtonCommandExecute, CanCalculatorButtonCommandExecuted);

            BackSpace = new RelayCommand(OnBackSpaceExecute, CanBackSpaceExecuted);
            ClearButtonCommand = new RelayCommand(OnClearButtonCommandExecute, null);
        }

    }
}
