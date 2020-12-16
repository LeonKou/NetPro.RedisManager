﻿using System;
using System.Text;
using NetPro.RedisManager;
using Newtonsoft.Json;

namespace SNetPro.RedisManager
{
    /// <summary>
    /// JSon.Net implementation of <see cref="ISerializer"/>
    /// </summary>
    public class NewtonsoftSerializer : ISerializer
    {
        /// <summary>
        /// Encoding to use to convert string to byte[] and the other way around.
        /// </summary>
        /// <remarks>
        /// StackExchange.Redis uses Encoding.UTF8 to convert strings to bytes,
        /// hence we do same here.
        /// </remarks>
        private static readonly Encoding encoding = Encoding.UTF8;

        private readonly JsonSerializerSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftSerializer"/> class.
        /// </summary>
        public NewtonsoftSerializer()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftSerializer"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public NewtonsoftSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings ?? new JsonSerializerSettings();
        }

        /// <inheritdoc/>
        public byte[] Serialize(object item)
        {
            var type = item?.GetType();
            var jsonString = JsonConvert.SerializeObject(item, type, settings);
            jsonString = jsonString.TrimStart('\"').TrimEnd('\"');
            return encoding.GetBytes(jsonString);
        }

        /// <inheritdoc/>
        public T Deserialize<T>(byte[] serializedObject)
        {
            var jsonString = encoding.GetString(serializedObject);
            jsonString = jsonString.TrimStart('\"').TrimEnd('\"');
            if (typeof(T) == typeof(string) || typeof(T).IsValueType)            {                return (T)Convert.ChangeType(jsonString, typeof(T));            }
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }

        private object GetDefault(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var valueProperty = type.GetProperty("Value");
                type = valueProperty.PropertyType;
            }

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
