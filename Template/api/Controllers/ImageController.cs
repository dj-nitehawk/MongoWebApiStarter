using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Api.Base;
using MongoWebApiStarter.Biz.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Api.Controllers
{
    [NeedPermission(ImageModel.Perms.Full)]
    public class ImageController : BaseController
    {
        [HttpPost("api/image")]
        public async Task<ActionResult<string>> CreateAsync([FromForm]ImageModel model)
        {
            try
            {
                await model.SaveAsync();
                return Ok(model.ID);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }
        }

        [HttpPatch("api/image")]
        public async Task<ActionResult> UpdateAsync([FromForm]ImageModel model)
        {
            try
            {
                await model.SaveAsync();
                return Ok();
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("api/image/{id}.jpg")] //jpg extension is used so files can be cached by CDNs and browsers
        public async Task<ActionResult> Retrieve(string id)
        {
            var model = new ImageModel { ID = id };
            var stream = Response?.Body ?? new MemoryStream();
            try
            {
                if (Response == null) //handle unit test
                {
                    await model.WriteImageData(stream);
                    return File(stream, "image/jpeg");
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.ContentType = "image/jpeg";
                await model.WriteImageData(stream);
                return new EmptyResult();
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                Response.ContentType = "image/jpeg";
                return new EmptyResult();
            }
        }
    }
}