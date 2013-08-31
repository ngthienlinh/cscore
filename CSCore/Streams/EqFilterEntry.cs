﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    public class EqFilterEntry : IEnumerable<KeyValuePair<int, EqFilter>>
    {
        public static EqFilterEntry CreateFilter(int channels, int sampleRate, double frequency, int bandWidth, float gain)
        {
            EqFilterEntry result = new EqFilterEntry();
            for (int c = 0; c < channels; c++)
            {
                result.Filters.Add(c, new EqFilter(sampleRate, frequency, bandWidth, gain));
            }

            return result;
        }

        public EqFilterEntry()
        {
            Filters = new Dictionary<int, EqFilter>();
        }

        public EqFilterEntry(int channels, EqFilter filter)
            : this()
        {
            Filters.Add(0, filter);
            for (int c = 1; c < channels; c++)
            {
                Filters.Add(c, (EqFilter)filter.Clone());
            }
        }

        /// <summary>
        /// Key: Channel; Value: Filter
        /// </summary>
        public Dictionary<int, EqFilter> Filters { get; set; }

        public void SetGain(float gain)
        {
            foreach (var c in Filters)
            {
                c.Value.Gain = gain;
            }
        }

        public IEnumerator<KeyValuePair<int, EqFilter>> GetEnumerator()
        {
            return Filters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
