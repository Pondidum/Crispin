using System;
using System.Collections;
using System.Collections.Generic;

namespace Crispin.Infrastructure
{
	internal class LightweightCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private readonly IDictionary<TKey, TValue> _values;

		private readonly Func<TKey, TValue> _onMissing;

		public LightweightCache(Func<TKey, TValue> onMissing)
			: this(new Dictionary<TKey, TValue>(), onMissing)
		{
		}

		private LightweightCache(IDictionary<TKey, TValue> dictionary, Func<TKey, TValue> onMissing)
			: this(dictionary)
		{
			_onMissing = onMissing;
		}

		private LightweightCache(IDictionary<TKey, TValue> dictionary)
		{
			_values = dictionary;
		}

		public int Count => _values.Count;


		public TValue this[TKey key]
		{
			get
			{
				TValue value;

				if (!_values.TryGetValue(key, out value))
				{
					value = _onMissing(key);

					if (value != null)
					{
						_values[key] = value;
					}
				}

				return value;
			}
			set
			{
				if (_values.ContainsKey(key))
				{
					_values[key] = value;
				}
				else
				{
					_values.Add(key, value);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)this).GetEnumerator();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		public void Fill(TKey key, TValue value)
		{
			if (_values.ContainsKey(key))
			{
				return;
			}

			_values.Add(key, value);
		}

		public void Each(Action<TValue> action)
		{
			foreach (var pair in _values)
			{
				action(pair.Value);
			}
		}

		public void Each(Action<TKey, TValue> action)
		{
			foreach (var pair in _values)
			{
				action(pair.Key, pair.Value);
			}
		}

		public TValue[] GetAll()
		{
			var returnValue = new TValue[Count];
			_values.Values.CopyTo(returnValue, 0);

			return returnValue;
		}

		public void Clear()
		{
			_values.Clear();
		}
	}
}
