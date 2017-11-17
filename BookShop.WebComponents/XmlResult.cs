using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookShop.WebComponents
{
    public class XmlResult : ActionResult
    {
        public XmlResult(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (this.Value != null)
            {
                var serializer = new XmlSerializer(this.Value.GetType());

                using (var stream = new MemoryStream())
                {
                    serializer.Serialize(stream, this.Value);

                    var data = stream.ToArray();

                    context.HttpContext.Response.ContentType = "application/xml";
                    context.HttpContext.Response.ContentLength = data.Length;
                    context.HttpContext.Response.Body.Write(data, 0, data.Length);
                }
            }

            return base.ExecuteResultAsync(context);
        }
    }
}
