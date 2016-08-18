using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKGoldenAPI.Types;
using HKGoldenAPI.Exceptions;
using HtmlAgilityPack;

namespace HKGoldenAPI
{
    /// <summary>
    /// Provides access to the API
    /// </summary>
    public class Manager
    {
        NetworkHandler netHandler;

        public Manager()
        {
            netHandler = new NetworkHandler();
        }

        public async Task<TermsOfService> GetTermsOfServiceAsync()
        {
            try
            {
                HtmlDocument document = await netHandler.GETRequestAsDocumentAsync(Constants.URL_TERMSOFSERVICE);
                TermsOfService tos = new TermsOfService(document);
                return tos;
            }
            catch
            {
                throw new NetworkError();
            }
        }

        public async Task LoadHomePage()
        {

        }
    }
}
