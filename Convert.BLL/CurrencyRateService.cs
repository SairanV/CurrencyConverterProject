﻿using Convert.DAL;
using System;
using System.Reflection;

namespace Convert.BLL
{
    public class CurrencyRateService
    {
        private readonly CurrencyRateRepository _repository;

        public CurrencyRateService(CurrencyRateRepository repository)
        {
            _repository = repository;
        }
        public void UpdateCurrencyRates()
        {
            _repository.UpdateCurrencyRates();
        }

        public Root GetCurrencyRates()
        {
            return _repository.GetCurrencyRates();
        }

        /// <summary>
        /// Получает курс обмена между двумя валютами.
        /// </summary>
        /// <param name="sourceCurrency">Исходная валюта.</param>
        /// <param name="targetCurrency">Целевая валюта.</param>
        /// <returns>Курс обмена.</returns>
        public decimal GetExchangeRate(string sourceCurrency, string targetCurrency)
        {
            var rates = GetCurrencyRates();

            sourceCurrency = sourceCurrency.ToUpper();
            targetCurrency = targetCurrency.ToUpper();

            if (rates != null && rates.Rates != null)
            {
                double sourceRate, targetRate;

                if (!TryGetRate(rates.Rates, sourceCurrency, out sourceRate) ||
                    !TryGetRate(rates.Rates, targetCurrency, out targetRate))
                {
                    throw new Exception("Не удалось найти курс для одной из валют.");
                }

                // В данном случае, предполагается, что курс обмена это отношение целевой валюты к исходной
                decimal exchangeRate = (decimal)targetRate / (decimal)sourceRate;

                return exchangeRate;
            }
            else
            {
                throw new Exception("Не удалось получить курсы валют.");
            }
        }

        /// <summary>
        /// Пытается получить курс валюты из объекта Rates.
        /// </summary>
        /// <param name="rates">Объект, содержащий курсы валют.</param>
        /// <param name="currency">Искомая валюта.</param>
        /// <param name="rate">Полученный курс валюты.</param>
        /// <returns>True, если курс валюты найден; в противном случае - false.</returns>
        private bool TryGetRate(Rates rates, string currency, out double rate)
        {
            rate = 0;

            PropertyInfo property = typeof(Rates).GetProperty(currency);
            if (property != null)
            {
                rate = (double)property.GetValue(rates);
                return true;
            }

            throw new Exception($"Не удалось найти курс для валюты {currency}");
            //return false;
        }
    }
}