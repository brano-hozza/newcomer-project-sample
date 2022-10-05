using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApi.API
{
    public interface IHasClientSettings
    {
        string ClientSettings { get; set; }
    }

    public class ClientSettingsJsonConverter<T> : JsonConverter<T>
        where T : IHasClientSettings
    {
        private const string ClientSettingsPropertyName = "clientSettings";

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader);

            string json;
            using (var stream = new MemoryStream())
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Encoder = options.Encoder }))
            {
                WriteElement(writer, jsonDocument.RootElement);
                writer.Flush();
                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            var optionsCopy = CopyJsonSerializerOptionsWithoutConverters(options);
            return JsonSerializer.Deserialize<T>(json, optionsCopy);
        }

        private void WriteObject(Utf8JsonWriter writer, JsonElement jsonElement)
        {
            writer.WriteStartObject();
            foreach (JsonProperty property in jsonElement.EnumerateObject())
            {
                if (property.Name == ClientSettingsPropertyName)
                {
                    var parsedProperty = property.Value.ToString();
                    if (!String.IsNullOrEmpty(parsedProperty))
                    {
                        writer.WriteString(ClientSettingsPropertyName, parsedProperty);
                    }
                }
                else if (
                    property.Value.ValueKind == JsonValueKind.Object ||
                    property.Value.ValueKind == JsonValueKind.Array)
                {
                    writer.WritePropertyName(property.Name);
                    WriteElement(writer, property.Value);
                }
                else
                {
                    property.WriteTo(writer);
                }
            }
            writer.WriteEndObject();
        }

        private void WriteElement(Utf8JsonWriter writer, JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                writer.WriteStartArray();
                foreach (var itemELement in jsonElement.EnumerateArray())
                {
                    WriteElement(writer, itemELement);
                }
                writer.WriteEndArray();
            }
            else if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                WriteObject(writer, jsonElement);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            dynamic result = ToExpandoObject(value);
            result.clientSettings =
                value.ClientSettings != null
                    ? JsonSerializer.Deserialize<dynamic>(value.ClientSettings)
                    : null;
            JsonSerializer.Serialize(writer, result, options);
        }

        private static ExpandoObject ToExpandoObject(object obj)
        {
            var expando = new ExpandoObject();
            if (obj != null)
            {
                var publicProperties =
                    obj.GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanRead);
                foreach (PropertyInfo property in publicProperties)
                {
                    var name = char.ToLowerInvariant(property.Name[0]) + property.Name[1..];
                    expando.TryAdd(name, property.GetValue(obj));
                }
            }

            return expando;
        }

        private static JsonSerializerOptions CopyJsonSerializerOptionsWithoutConverters(JsonSerializerOptions options)
        {
            var copy = new JsonSerializerOptions(options);
            copy.Converters.Clear();
            return copy;
        }
    }

    public class ClientSettingsJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IHasClientSettings).IsAssignableFrom(typeToConvert);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(ClientSettingsJsonConverter<>).MakeGenericType(typeToConvert);

            return Activator.CreateInstance(converterType) as JsonConverter;
        }
    }
}