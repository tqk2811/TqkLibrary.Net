using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace TqkLibrary.Net
{
  public class IgnoreStringEmptyContractResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      var prop = base.CreateProperty(member, memberSerialization);
      if (prop.PropertyType == typeof(string) && prop.Readable)
      {
        prop.ShouldSerialize = obj =>
        {
          string val = (string)prop.ValueProvider.GetValue(obj);
          return !string.IsNullOrEmpty(val);//!val.Equals("N/A", StringComparison.OrdinalIgnoreCase);
        };
      }
      return prop;
    }
  }
}