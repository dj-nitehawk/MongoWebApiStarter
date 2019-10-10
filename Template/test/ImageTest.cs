using MongoWebApiStarter.Api.Controllers;
using MongoWebApiStarter.Biz.Models;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Test
{
    [TestClass]
    public class ImageTest
    {
        private static readonly string validImage = @"iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAIAAAAiOjnJAAAACXBIWXMAAAsTAAALEwEAmpwYAAAGf2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDIgNzkuMTYwOTI0LCAyMDE3LzA3LzEzLTAxOjA2OjM5ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgeG1wOk1vZGlmeURhdGU9IjIwMTktMDgtMTBUMTI6MjE6NDkrMDU6MzAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6ODkzZmI0YjMtZDg3OC03YTQwLWIyYmMtYjlmOWY0YWM4Njc4IiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZTIwN2Y0NDQtYjllMC03YjQzLTlhZTctMzQyNDBlNWNlMzFmIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZDIxMWY5NWYtZjMzNy0zZTQzLWEzMDItMGUwYWVlZjg1M2Y1IiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIiBwaG90b3Nob3A6SUNDUHJvZmlsZT0ic1JHQiBJRUM2MTk2Ni0yLjEiIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDpkMjExZjk1Zi1mMzM3LTNlNDMtYTMwMi0wZTBhZWVmODUzZjUiIHN0RXZ0OndoZW49IjIwMTktMDgtMTBUMTI6MjE6NDkrMDU6MzAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAoV2luZG93cykiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjg5M2ZiNGIzLWQ4NzgtN2E0MC1iMmJjLWI5ZjlmNGFjODY3OCIgc3RFdnQ6d2hlbj0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPHBob3Rvc2hvcDpUZXh0TGF5ZXJzPiA8cmRmOkJhZz4gPHJkZjpsaSBwaG90b3Nob3A6TGF5ZXJOYW1lPSJNQVhJTSBMT0dJQ1MiIHBob3Rvc2hvcDpMYXllclRleHQ9Ik1BWElNIExPR0lDUyIvPiA8L3JkZjpCYWc+IDwvcGhvdG9zaG9wOlRleHRMYXllcnM+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+FrSdrAAACTFJREFUeNrtnTuKXUcQhrUB70V4FdYe7CUYrUDgBTiYVIlCgcCBwGAQGAQKhFEwgRACJ5YTBZICJ57EybigoSj1o7q6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";
        private static readonly string invalidImage = @"iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAIAAAAiOjnJAAAACXBIWXMAAAsTAAALEwEAmpwYAAAGf2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDIgNzkuMTYwOTI0LCAyMDE3LzA3LzEzLTAxOjA2OjM5ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgeG1wOk1vZGlmeURhdGU9IjIwMTktMDgtMTBUMTI6MjE6NDkrMDU6MzAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6ODkzZmI0YjMtZDg3OC03YTQwLWIyYmMtYjlmOWY0YWM4Njc4IiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZTIwN2Y0NDQtYjllMC03YjQzLTlhZTctMzQyNDBlNWNlMzFmIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InhtcC5kaWQ6ZDIxMWY5NWYtZjMzNy0zZTQzLWEzMDItMGUwYWVlZjg1M2Y1IiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIiBwaG90b3Nob3A6SUNDUHJvZmlsZT0ic1JHQiBJRUM2MTk2Ni0yLjEiIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIj4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iY3JlYXRlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDpkMjExZjk1Zi1mMzM3LTNlNDMtYTMwMi0wZTBhZWVmODUzZjUiIHN0RXZ0OndoZW49IjIwMTktMDgtMTBUMTI6MjE6NDkrMDU6MzAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAoV2luZG93cykiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjg5M2ZiNGIzLWQ4NzgtN2E0MC1iMmJjLWI5ZjlmNGFjODY3OCIgc3RFdnQ6d2hlbj0iMjAxOS0wOC0xMFQxMjoyMTo0OSswNTozMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPHBob3Rvc2hvcDpUZXh0TGF5ZXJzPiA8cmRmOkJhZz4gPHJkZjpsaSBwaG90b3Nob3A6TGF5ZXJOYW1lPSJNQVhJTSBMT0dJQ1MiIHBob3Rvc2hvcDpMYXllclRleHQ9Ik1BWElNIExPR0lDUyIvPiA8L3JkZjpCYWc+IDwvcGhvdG9zaG9wOlRleHRMYXllcnM+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+FrSdrAAACTFJREFUeNrtnTuKXUcQhrUB70V4FdYe7CUYrUDgBTiYVIlCgcCBwGAQGAQKhFEwgRACJ5YTBZICJ57EybigoSj1o7q6zz1nZm5/PxcxM7rnVfWdqn73vWuEdtA9TIAACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALARYCLIQACwEWAiyEAAsBFgIshAALLQzWN98+08+Tp++CR336cmUP/OXXPyNH/fX3P/Yo+dX5spwzfn57Pw8fvWo9Y/U8R1pgUbC++/634FHW63GzituGvPjDj7+nb95/8Pzfq/+cb/708x96WnH5NFh7W2BRsOTz9v3nyFHigAmzZkd1vSg3o1++eHwZ+ZqPThesvS2wCliamyQkdP1XHqXGjZhVj5Loopfzs2E3FGWBTe6nGtgc9x9pgYXA0nddyiXJsvJvPKNpOog4wx6luHSzocCkLi8LT6IXLz8oN6/ffPSjcun+Iy2wIljiaXV2yz0qdYD1Sjx3yOuu/oiUaWxpJstTEp/0tM49RMA6wAIrgiUWD758YnT9WtysNuNk1cNuNnToscy1EmUQrL0tsBZYakr5IfN9S2JNfa3jZtXcIVEhC2CRGr7Nd/JzmSX9Io4D1mEWWBcs6+xWLpDgob60r7sUn4N5ULFQ1II1fPFc9n11cPcMQbB2tcDSYKmzW7lAI0cKPLaeH2wX1VrbUDbMriW3Ydsnuw0EcbB2ssByYOlLn8yqpYdWLtDibcs2LO33OBPLC/pslQ33qTmXbeVuC1hBlwt/rV0tjgdLz1l2NcpfNjaXANahYDmdvjcFlhJf3tsNZsa7CpZd9XpLGUtf62PAsgl3j2yV9Ypub+alVri1Vmj/GO/JcRaT7Rbe96vHZR2RgDUAVnDjmi6U1YaAeAYZBcsGy117Y+wzbunxXA4s6yF/t61SrQ0mIhtPbATr2jTD7hpLItuYAVZdtjAx11eYRaaJzscJsA4b8QJYk4awTp0Y3VAu7W/f8uBolgmwbKO5MyzshPa8kbrh3R6PZetoo0X+6vdt0JKI2PX6BFjZbcyNxxoqg1LGGgbLjpgbGkHainDZQGf52ffKHFjXXw987V6lel2/ABrc7hWwmsq6zCTMdMe8+0Gi7Ce5eHxZbQ1KE2zmwMoIdq6SxrxnQxjkkVsj8eX9sQZhls61bXpJs01an2zASXW+SpqlI/7I/CeHd1NPOe2nvLGyF2X0PZGrZI2Z2VVas3RsmNSHyr5Py3vFE91Pmcgis/PEYUOGli+Xrmp9yiFcwQAslASvosGpOmmsfNibHUhzJmApXkKDRCk7wDJFuOmejZSJsmnQ6bRptrH4b/tMaHuVLA61JluXh6QJiVse9jzBQuckwEKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFAAsBFkKAhQALARZCgIUACwEWQoCFjtX/njDZyDY0yRAAAAAASUVORK5CYII=";
        private static readonly ImageController controller = new ImageController();

        [TestMethod]
        public async Task uploading_corrupt_image()
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(invalidImage)))
            {
                var file = new FormFile(stream, 0, stream.Length, "test", "test.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var editor = new ImageModel
                {
                    File = file,
                    Height = 100,
                    Width = 100
                };
                (await controller.CreateAsync(editor))
                                 .Result
                                 .Should()
                                 .BeOfType<BadRequestObjectResult>();
            }
        }

        [TestMethod]
        public void validating_bad_image()
        {
            var validator = new ImageModel.Validator();

            validator.ShouldHaveValidationErrorFor(i => i.Width, 10);
        }

        [TestMethod]
        public async Task uploading_valid_image()
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(validImage)))
            {
                var file = new FormFile(stream, 0, stream.Length, "test", "test.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var editor = new ImageModel
                {
                    File = file,
                    Height = 200,
                    Width = 200
                };
                var res = (await controller.CreateAsync(editor)).Result;
                res.Should().BeOfType<OkObjectResult>();
            }
        }

        [TestMethod]
        public async Task retrieving_by_id()
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(validImage)))
            {
                var file = new FormFile(stream, 0, stream.Length, "test", "test.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var editor = new ImageModel
                {
                    File = file,
                    Height = 200,
                    Width = 200
                };
                var id = ((await controller.CreateAsync(editor)).Result as OkObjectResult).Value.ToString();

                var result = await controller.Retrieve(id);
                result.Should().BeOfType<FileContentResult>();

                var type = (result as FileContentResult).ContentType;
                type.Should().Be("image/jpeg");
            }
        }

        [TestMethod]
        public async Task updating_existing_image()
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(validImage)))
            {
                var file = new FormFile(stream, 0, stream.Length, "test", "test.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var editor = new ImageModel
                {
                    File = file,
                    Height = 200,
                    Width = 200
                };
                var res = await controller.UpdateAsync(editor);
                res.Should().BeOfType<OkResult>();
            }
        }
    }
}
