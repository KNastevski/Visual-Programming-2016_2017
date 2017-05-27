using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Konverzija_na_valuti
{
    public partial class Menuvacnica : Form
    {
        public Menuvacnica()
        {
            InitializeComponent();
        }


        //Konvertor//

        public static decimal getCurrencyRate(string currFrom, string currTo)
        {
            decimal result;
            using (WebClient c = new WebClient())
            {
                string data = c.DownloadString(string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=sl1d1t1ba&e=.csv", currFrom, currTo));
                string rate = data.Split(',')[1];
                var style = NumberStyles.Number;
                var culture = CultureInfo.CreateSpecificCulture("en-US");
                decimal.TryParse(rate, style, culture, out result);
            }
            return result;
        }

        public string GetSubstringInBrackets(string a)
        {
            string s = a;
            int start = s.IndexOf("(") + 1;
            int end = s.IndexOf(")", start);
            string result = s.Substring(start, end - start);
            return result;
        }

        private void textBox_Suma_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',') { }

            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }

            if (e.KeyChar == ',')
            {
                if (!textBox_Suma.Text.Contains(",")) { }
                else
                    e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {

            textBox_Suma.Clear();
            comboBox_Od.SelectedIndex = -1;
            comboBox_Vo.SelectedIndex = -1;
            textBox_Rezultat.Clear();
            button_Convert.Enabled = false;
        }

        private void comboBox_Od_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((textBox_Suma.Text.Length > 0) && (comboBox_Od.SelectedIndex > -1)
            && (comboBox_Vo.SelectedIndex > -1))
            {
                button_Convert.Enabled = true;
            }
        }

        private void textBox_Suma_TextChanged(object sender, EventArgs e)
        {
            if ((textBox_Suma.Text.Length > 0) && (comboBox_Od.SelectedIndex > -1)
            && (comboBox_Vo.SelectedIndex > -1))
            {
                button_Convert.Enabled = true;
            }
        }

        private void comboBox_Vo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((textBox_Suma.Text.Length > 0) && (comboBox_Od.SelectedIndex > -1)
            && (comboBox_Vo.SelectedIndex > -1))
            {
                button_Convert.Enabled = true;
            }
        }

        private void button_Convert_Click(object sender, EventArgs e)
        {
            decimal a = decimal.Parse(textBox_Suma.Text);
            decimal rate = getCurrencyRate(GetSubstringInBrackets(comboBox_Od.SelectedItem.ToString()),
                GetSubstringInBrackets(comboBox_Vo.SelectedItem.ToString()));
            decimal result = a * rate;
            textBox_Rezultat.Text = result.ToString();
        }



        //Kalkulator//

        Double vrednost = 0;
        String operacija = "";
        bool kliknata_operacija = false;

        private void button_Click(object sender, EventArgs e)
        {
            if ((textBox_Result.Text == "0") || (kliknata_operacija))
                textBox_Result.Clear();
            kliknata_operacija = false;
            Button b = (Button)sender;

            if (b.Text == ",")
            {
                if(!textBox_Result.Text.Contains(","))
                {
                    textBox_Result.Text = textBox_Result.Text + b.Text; 
                }
            }
            else
                textBox_Result.Text = textBox_Result.Text + b.Text; 
        }

        private void button_CE_Click(object sender, EventArgs e)
        {
            textBox_Result.Text = string.Empty;
            textBox_Result.Text = "0";
        }

        private void operator_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if(vrednost != 0)
            {
                if (b.Text == "√")
                {
                    textBox_Result.Text = Math.Sqrt(Double.Parse(textBox_Result.Text)).ToString("#.000");
                }
                button_equals.PerformClick();
                kliknata_operacija = true;
                operacija = b.Text;
                label_operacija.Text = vrednost + "" + operacija;
            }
            else if (b.Text == "√")
            {
                textBox_Result.Text = Math.Sqrt(Double.Parse(textBox_Result.Text)).ToString("#.000000");
                vrednost = Math.Sqrt(Double.Parse(textBox_Result.Text)); 
            }
            else
            {
                operacija = b.Text;
                vrednost = double.Parse(textBox_Result.Text);
                kliknata_operacija = true;
                label_operacija.Text = vrednost + "" + operacija;
            }
        }

        private void button_equals_Click(object sender, EventArgs e)
        {
            label_operacija.Text = "";
            switch (operacija)
            {
                case "+":
                    textBox_Result.Text = (vrednost + double.Parse(textBox_Result.Text)).ToString();
                    break;
                case "-":
                    textBox_Result.Text = (vrednost - double.Parse(textBox_Result.Text)).ToString();
                    break;
                case "*":
                    textBox_Result.Text = (vrednost * double.Parse(textBox_Result.Text)).ToString();
                    break;
                case "/":
                    textBox_Result.Text = (vrednost / double.Parse(textBox_Result.Text)).ToString();
                    break;
                default:
                    break;
            }
            vrednost = Double.Parse(textBox_Result.Text);
            operacija = "";
        }

        private void button_C_Click(object sender, EventArgs e)
        {
            textBox_Result.Text = "0";
            vrednost = 0;
            label_operacija.Text = "";
        }

        private void textBox_Result_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',') { }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
            if (e.KeyChar == ',')
            {
                if (!textBox_Result.Text.Contains(",")) { }
                else
                  e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void panel_Kalkulator_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

        }

        private void Menuvacnica_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((textBox_Suma.Focused) || (comboBox_Od.Focused) || (comboBox_Vo.Focused))
            {
                switch (e.KeyChar)
                {
                    case (char)Keys.Decimal:
                        button_decimal.PerformClick();
                        break;
                    default:
                        break;
                }
            }

            else 
            {
                this.ActiveControl = null;

                switch (e.KeyChar)
                {
                    case (char)Keys.Return:
                        button_equals.PerformClick();
                        break;
                    case (char)Keys.Back:
                        button_Backspace.PerformClick();
                        break;
                    default:
                        break;
                }

                switch (e.KeyChar.ToString())
                {
                    case "0":
                        button10.PerformClick();
                        break;
                    case "1":
                        button1.PerformClick();
                        break;
                    case "2":
                        button2.PerformClick();
                        break;
                    case "3":
                        button3.PerformClick();
                        break;
                    case "4":
                        button4.PerformClick();
                        break;
                    case "5":
                        button5.PerformClick();
                        break;
                    case "6":
                        button6.PerformClick();
                        break;
                    case "7":
                        button7.PerformClick();
                        break;
                    case "8":
                        button8.PerformClick();
                        break;
                    case "9":
                        button9.PerformClick();
                        break;
                    case "+":
                        button_add.PerformClick();
                        break;
                    case "-":
                        button_subtract.PerformClick();
                        break;
                    case "*":
                        button_multiply.PerformClick();
                        break;
                    case "/":
                        button_divide.PerformClick();
                        break;
                    case ".":
                        button_decimal.PerformClick();
                        break;
                    default:
                        break;
                }
            }
        }

        private void panel_Menuvacnica_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void button_Backspace_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length > 1)
            {
                textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.Text.Length - 1, 1);
            }

            else if (textBox_Result.Text.Length == 1)
            {
                textBox_Result.Text = "0";
            }

            else { }
        }

        private void btnGame_Click(object sender, EventArgs e)
        {
            SnakeGame sg = new SnakeGame();
            sg.ShowDialog();
        }
    }
}
