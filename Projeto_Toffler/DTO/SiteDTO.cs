using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    [Serializable]
    public class SiteDTO
    {
        public int SiteNrSequencia { get; set; }
        public string SiteTxColigada { get; set; }
        public string SiteTxDescricao { get; set; }
        public string SiteCnpj { get; set; }
        public string SiteStStatus { get; set; }
        public string SiteIpAd { get; set; }
        public string SiteAliasAd { get; set; }
    }
}
