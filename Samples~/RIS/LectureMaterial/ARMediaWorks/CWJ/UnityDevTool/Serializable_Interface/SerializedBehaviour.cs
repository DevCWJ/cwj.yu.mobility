namespace CWJ.Serializable
{
    //public abstract class SerializedBehaviour : MonoBehaviour, ISerializationCallbackReceiver
    //{
    //    private Dictionary<string, UnityObject> serializedObjects = new Dictionary<string, UnityObject>();
    //    private Dictionary<string, string> serializedStrings = new Dictionary<string, string>();
    //    private BinaryFormatter serializer = new BinaryFormatter();
    //    public void OnAfterDeserialize()
    //    {
    //        Deserialize();
    //    }
    //    public void OnBeforeSerialize()
    //    {
    //        Serialize();
    //    }
    //    private void Serialize()
    //    {
    //        foreach (var field in GetInterfaces())
    //        {
    //            var value = field.GetValue(this);
    //            if (value == null)
    //                continue;
    //            string name = field.Name;
    //            var obj = value as UnityObject;
    //            if (obj != null)
    //            {
    //                serializedObjects[name] = obj;
    //            }
    //            else
    //            {
    //                using (var stream = new MemoryStream())
    //                {
    //                    serializer.Serialize(stream, value);
    //                    stream.Flush();
    //                    serializedObjects.Remove(name);
    //                    serializedStrings[name] = Convert.ToBase64String(stream.ToArray());
    //                }
    //            }
    //        }
    //    }
    //    private void Deserialize()
    //    {
    //        foreach (var field in GetInterfaces())
    //        {
    //            object result = null;
    //            string name = field.Name;

    //            UnityObject obj;
    //            if (serializedObjects.TryGetValue(name, out obj))
    //            {
    //                result = obj;
    //            }
    //            else
    //            {
    //                string serializedString;
    //                if (serializedStrings.TryGetValue(name, out serializedString))
    //                {
    //                    byte[] bytes = Convert.FromBase64String(serializedString);
    //                    using (var stream = new MemoryStream(bytes))
    //                        result = serializer.Deserialize(stream);
    //                }
    //            }
    //            field.SetValue(this, result);
    //        }
    //    }
    //    private IEnumerable<FieldInfo> GetInterfaces()
    //    {
    //        return GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
    //                        .Where(f => !f.IsDefined(typeof(HideInInspector)) && (f.IsPublic || f.IsDefined(typeof(SerializeField))))
    //                        .Where(f => f.FieldType.IsInterface);
    //    }
    //}
}