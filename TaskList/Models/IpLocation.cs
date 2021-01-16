using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class IpLocation
    {
        [JsonProperty("geoname_id")]
        public string GeonameId { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }
    }
}
