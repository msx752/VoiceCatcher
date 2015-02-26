using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;

namespace GoogleSpeech
{
    public class JSon
    {
        [DataContract]
        public class RecognizedItem
        {
            [DataMember]
            public string utterance;

            [DataMember]
            public float confidence;
        }

        [DataContract]
        public  class RecognitionResult
        {
            [DataMember]
             public string status;

            [DataMember]
            public string id;

            [DataMember]
            public RecognizedItem[] hypotheses;
        }

        public static RecognitionResult Parse(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RecognitionResult));

            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(toParse));
            
            RecognitionResult result= (RecognitionResult)ser.ReadObject(stream1);
            return result;
        }
    }

}
