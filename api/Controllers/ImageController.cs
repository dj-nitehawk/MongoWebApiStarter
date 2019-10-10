using App.Api.Auth;
using App.Api.Base;
using App.Biz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.Api.Controllers
{
    [NeedPermission(ImageModel.Perms.Save)]
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
            var model = new ImageModel() { ID = id };
            await model.LoadAsync();
            
            if (model.FileBytes == null) return NotFound();

            return File(model.FileBytes, "image/jpeg");
        }
    }
}