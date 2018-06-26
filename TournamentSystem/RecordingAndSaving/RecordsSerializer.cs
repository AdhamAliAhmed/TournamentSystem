using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TournamentSystem.Saving
{
    /// <summary>
    /// Serializes and Deserializes a collection of Records
    /// </summary>
    public class RecordsSerializer
    {
        /// <summary>
        /// Serializes a list of TournametRecord class
        /// </summary>
        /// <param name="jsonPath">Path of the Json file where the serialized data is stored</param>
        /// <param name="records">Records to serialize</param>
        public void Serialize(string jsonPath, List<TourrnamentRecord> records)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            using (var writer = new StreamWriter(new FileStream(jsonPath, FileMode.Create)))
            {
                var data = serializer.Serialize(records);
                writer.Write(data);
            }
        }

        /// <summary>
        /// Dersializes a list of TouramentRecord class from a given file
        /// </summary>
        /// <param name="jsonPath">File to pull out the serialized records from</param>
        /// <returns>List[TourrnamentRecord]</returns>
        public List<TourrnamentRecord> Deserialize(string jsonPath)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            using (var writer = new StreamReader(new FileStream(jsonPath, FileMode.Open)))
            {
                return serializer.Deserialize<List<TourrnamentRecord>>(writer.ReadToEnd());
            }
        }
    }
}
