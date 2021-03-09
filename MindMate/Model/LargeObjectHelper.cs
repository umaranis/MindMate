using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Model
{
    public static class LargeObjectHelper
    {
        public static string GenerateNewKey<T>() where T : ILargeObject
        {
            return typeof(T).Name + "-" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Instantiates correct type of ILargeObject based on the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILargeObject CreateFromKey(string key)
        {
            if (key.StartsWith(typeof(ImageLob).Name))
            {
                return new ImageLob();
            }
            else
            {
                return new BytesLob();
            }
        }
    }
}
