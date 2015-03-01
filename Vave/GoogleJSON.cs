using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vave
{
    #region DeSerializeJson CLASSES
    //istisnasız tüm json değişkenleri tanımlandı
    public class GoogleJSON
    {
        [JsonProperty("results")]
        public List<Results> Results { get; set; }
    }
    public class Results
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }

        [JsonProperty("result_index")]
        public string Result_index { get; set; }
    }
    public class Result
    {
        [JsonProperty("alternative")]
        public List<Alternative> Alternatives { get; set; }

        [JsonProperty("stability")]
        public string Stability { get; set; }

        [JsonProperty("final")]
        public string Final { get; set; }
    }
    public class Alternative
    {
        [JsonProperty("transcript")]
        public string Transcript { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }
    }
    #endregion

}
