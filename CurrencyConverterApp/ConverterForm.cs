using System;
using System.Reflection;
using System.Windows.Forms;
using Convert.BLL;
using Convert.DAL;

namespace CurrencyConverterApp
{
    public partial class ConverterForm : Form
    {
        private readonly CurrencyRateService _rateService;
        private readonly CurrencyConverter _converter;

        public ConverterForm()
        {
            InitializeComponent();

            // Инициализация службы курсов валют и конвертера
            var repository = new CurrencyRateRepository();
            _rateService = new CurrencyRateService(repository);
            _converter = new CurrencyConverter(_rateService);

            // Начальное заполнение комбобоксов валют
            FillCurrencyComboBoxes();
        }

        private void FillCurrencyComboBoxes()
        {
            // Получение списка валют
            var rates = _rateService.GetCurrencyRates();
            if (rates != null && rates.Rates != null)
            {
                foreach (var property in rates.Rates.GetType().GetProperties())
                {
                    sourceCurrencyComboBox.Items.Add(property.Name);
                    targetCurrencyComboBox.Items.Add(property.Name);
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить курсы валют.");
            }
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            // Получение выбранных валют и суммы
            string sourceCurrency = sourceCurrencyComboBox.Text;
            string targetCurrency = targetCurrencyComboBox.Text;

            if (decimal.TryParse(amountTextBox.Text, out decimal amount))
            {
                // Конвертация и вывод результата
                decimal exchangeRate = _rateService.GetExchangeRate(sourceCurrency, targetCurrency);
                if (exchangeRate != 0)
                {
                    decimal convertedAmount = _converter.ConvertCurrency(sourceCurrency, targetCurrency, amount);

                    resultLabel.Text = $"Курс обмена {sourceCurrency} -> {targetCurrency}: {exchangeRate:F3}\n" +
                                       $"{amount} {sourceCurrency} равно {convertedAmount:F3} {targetCurrency}";
                }
                else
                {
                    MessageBox.Show($"Конвертация в {targetCurrency} не выполнена из-за ошибки в получении курсов валют.");
                }
            }
            else
            {
                MessageBox.Show("Некорректная сумма для конвертации.");
            }
        }
    }
}