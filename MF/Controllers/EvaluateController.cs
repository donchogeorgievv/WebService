namespace MF.Controllers
{
    using MF.Classes;
    using System.Web;
    using System.Web.Http;
    public class EvaluateController : ApiController
    {
        public int Get(string input)
        {
            var uri = HttpContext.Current.Request.QueryString;              //Work with raw data as the + sign gets trimmed
            string strToRemove = $"{nameof(input)}=";                       //To trim the input= part of the raw request
            string actualExpression = uri.ToString().Replace(strToRemove, string.Empty);
            var result = Parser.Parse(actualExpression).Evaluate(null);
            return result;
        }
    }
}
