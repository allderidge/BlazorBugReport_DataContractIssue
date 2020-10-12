using System;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Xml;

namespace DataContractIssue
{
    internal static class DeseriliseIssue
    {
        internal static string TryCreateIssue()
        {
            string message;
            var serialiser = new DataContractSerializer(typeof(AuthOutcome), new DataContractSerializerSettings());

            using (var output = new StringWriter())
            using (var writer = new XmlTextWriter(output) { Formatting = Formatting.None })
            {
                serialiser.WriteObject(writer, new AuthOutcome());
                message = output.GetStringBuilder().ToString();
            }

            try
            {
                using var reader = XmlReader.Create(new StringReader(message));
                serialiser.ReadObject(reader, false);
                // also causes the issue
                //serialiser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(message)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Deserialise Exception: " + e.ToString();
            }

            return "No Exception";
        }
    }

    public class AuthOutcome
    {
        //public string PropertyDeserialisedWithoutIssue { get; set; }
        public bool PropertyTriggeringIssue { get; set; }
    }
}
