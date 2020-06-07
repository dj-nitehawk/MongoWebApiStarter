using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Auth;
using MongoWebApiStarter.Api.Base;
using MongoWebApiStarter.Api.Extensions;
using MongoWebApiStarter.Biz.Models;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Api.Controllers
{
    public class ImageController : BaseController
    {
        [Permission(ImageModel.Perms.Write)]
        [HttpPost("api/image")]
        public async Task<ActionResult<string>> CreateAsync([FromForm] ImageModel model)
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

        [Permission(ImageModel.Perms.Write)]
        [HttpPatch("api/image")]
        public async Task<ActionResult> UpdateAsync([FromForm] ImageModel model)
        {
            try
            {
                if (model.ID.HasNoValue())
                    return Problem("Image ID is needed for patching!");

                await model.SaveAsync();
                return Ok(model.ID);
            }
            catch (Exception x)
            {
                return BadRequest(x.Message);
            }
        }

        [Permission(ImageModel.Perms.Delete)]
        [HttpDelete("api/image/{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var model = new ImageModel();
            await model.DeleteAsync(id);
            return Ok();
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