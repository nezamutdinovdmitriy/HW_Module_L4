using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;

namespace Assets._Project.Develop.Runtime.Meta.Features
{
    public class WalletService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly Dictionary<CurrencyType, ReactiveVariable<int>> _currencies;

        public WalletService(Dictionary<CurrencyType, ReactiveVariable<int>> currencies, PlayerDataProvider playerDataProvider)
        {
            _currencies = new Dictionary<CurrencyType, ReactiveVariable<int>>(currencies);

            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }

        public List<CurrencyType> AvailableCurrencies => _currencies.Keys.ToList();

        public IReadOnlyVariable<int> GetCurrency(CurrencyType currencyTypes) => _currencies[currencyTypes];

        public void Add(CurrencyType currencyTypes, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[currencyTypes].Value += amount;
        }

        public void Spend(CurrencyType currencyTypes, int amount)
        {
            if (Enough(currencyTypes, amount) == false)
                throw new InvalidOperationException("Not enough: " + currencyTypes.ToString());

            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            _currencies[currencyTypes].Value -= amount;
        }

        public bool Enough(CurrencyType currencyTypes, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            return _currencies[currencyTypes].Value >= amount;
        }

        public void WriteTo(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, ReactiveVariable<int>> currency in _currencies)
            {
                if (data.WalletData.ContainsKey(currency.Key))
                    data.WalletData[currency.Key] = currency.Value.Value;
                else
                    data.WalletData.Add(currency.Key, currency.Value.Value);
            }
        }

        public void ReadFrom(PlayerData data)
        {
            foreach (KeyValuePair<CurrencyType, int> currency in data.WalletData)
            {
                if (_currencies.ContainsKey(currency.Key))
                    _currencies[currency.Key].Value = currency.Value;
                else
                    _currencies.Add(currency.Key, new ReactiveVariable<int>(currency.Value));
            }
        }
    }
}