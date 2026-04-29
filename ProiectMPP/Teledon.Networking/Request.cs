using System.Text.Json;

namespace Teledon.Networking
{
    public enum RequestType { LOGIN, LOGOUT, GET_CASES, ADD_DONATION, UPDATE_DONOR, SEARCH_DONORS }
    
    public class Request
    {
        public RequestType Type { get; set; }
        
        public string JsonData { get; set; } 
        
        public void SetData<T>(T data)
        {
            JsonData = JsonSerializer.Serialize(data);
        }

        public T GetData<T>()
        {
            return JsonSerializer.Deserialize<T>(JsonData);
        }
    }
}