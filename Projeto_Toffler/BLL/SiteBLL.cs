using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BLL
{
    public class SiteBLL
    {
        #region Singleton

        private static SiteBLL _instance;

        public static SiteBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(SiteBLL))
                    {
                        if (_instance == null)
                        {
                            _instance = new SiteBLL();
                        }
                    }
                }
                return _instance;
            }
        }

        //Entidades utilizadas por essa classe

        #endregion

        private readonly SiteDAL _siteDal;
        /// <summary>
        ///     Construtor Estatico
        ///     Faz parte do Singleton. Inicialize aqui os objetos uteis
        /// </summary>
        private SiteBLL()
        {
            _siteDal = SiteDAL.Instance;
        }
        /// <summary>
        /// Método para obter lista de sites
        /// </summary>
        /// <returns></returns>
        public DataTable GetSite()
        {
            return _siteDal.GetSite();
        }

    }
}
