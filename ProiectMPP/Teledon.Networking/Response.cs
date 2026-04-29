using System.Text.Json;

namespace Teledon.Networking
{
    public enum ResponseType
    {
        OK,
        ERROR,
        UPDATE,
        NEW_DONATION,
        DONOR_UPDATED
    }

    public class Response
    {
        public ResponseType Type { get; set; }
        public string ErrorMessage { get; set; }
        public string JsonData { get; set; }

        public void SetData<T>(T data)
        {
            JsonData = JsonSerializer.Serialize(data);
        }

        public T GetData<T>()
        {
            if (string.IsNullOrEmpty(JsonData))
                return default;
            return JsonSerializer.Deserialize<T>(JsonData);
        }
    }
}
