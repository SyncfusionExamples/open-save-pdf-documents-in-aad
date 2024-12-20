﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using Syncfusion.Pdf.Parsing;
using SkiaSharp;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Syncfusion.Pdf;
using ControllerPractice;
using Microsoft.Identity.Client;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Files.Shares;
using Azure.Identity;
using Azure.Storage.Blobs;
using System.Text.Json.Nodes;


namespace PdfViewerService2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PdfViewerController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        //Initialize the memory cache object   
        public IMemoryCache _cache;

        string tenantId = "Provide the tenant id here";
        string clientId = "Provide the clientid here";
        string clientSecret = "Provide the client secret here";
        string blobServiceEndpoint = "https://your-storage-account.blob.core.windows.net";
        string containerName = "Provide the container name here";

        public PdfViewerController(IHostingEnvironment hostingEnvironment, IMemoryCache cache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
            Console.WriteLine("PdfViewerController initialized");
        }

        [HttpPost("Load")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/Load")]
        //Post action for loading the PDF documents 
        public async Task<IActionResult> Load([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            MemoryStream stream = new MemoryStream();
            object jsonResult = new object();
            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                if (bool.Parse(jsonObject["isFileName"]))
                {
                    string documentPath = GetDocumentPath(jsonObject["document"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        stream = new MemoryStream(bytes);
                    }
                    else
                    {
                        string fileName = jsonObject["document"].Split(new string[] { "://" }, StringSplitOptions.None)[0];
                        if (fileName == "http" || fileName == "https")
                        {
                            WebClient WebClient = new WebClient();
                            byte[] pdfDoc = WebClient.DownloadData(jsonObject["document"]);
                            stream = new MemoryStream(pdfDoc);
                        }
                        else
                        {
                            return this.Content(jsonObject["document"] + " is not found");
                        }

                    }
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                    stream = new MemoryStream(bytes);
                }
            }
            jsonResult = pdfviewer.Load(stream, jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }
             
        [HttpPost("LoadFromAAD")]
        [Route("[controller]/LoadFromAAD")]
        public async Task<IActionResult> LoadFromAAD(string fileName)
        {
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), clientSecretCredential);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            // Download the PDF file to a local stream
            using MemoryStream pdfStream = new MemoryStream();
            await blobClient.DownloadToAsync(pdfStream);
            var base64 = Convert.ToBase64String(pdfStream.ToArray());
            return Content("data:application/pdf;base64," + base64);
        }

        [AcceptVerbs("SaveToAAD")]
        [HttpPost("SaveToAAD")]
        [Route("[controller]/SaveToAAD")]
        public async Task<IActionResult> SaveToAAD([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var fileName = jsonObject.ContainsKey("documentId") ? jsonObject["documentId"] : "Test.pdf";
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            string convertedBase = documentBase.Substring(documentBase.LastIndexOf(',') + 1);
            // Decode the Base64 string to a byte array
            byte[] byteArray = Convert.FromBase64String(convertedBase);

            // Create a MemoryStream from the byte array
            MemoryStream stream = new MemoryStream(byteArray);
            // Create a new BlobServiceClient using the DefaultAzureCredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), clientSecretCredential);

            // Get a reference to the container
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(fileName);

            //FileStream uploadFileStream = new FileStream();
            await blobClient.UploadAsync(stream, true);
            stream.Close();
            return Content(string.Empty);
        }

        [AcceptVerbs("Post")]
        [HttpPost("Bookmarks")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/Bookmarks")]
        //Post action for processing the bookmarks from the PDF documents
        public IActionResult Bookmarks([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonResult = pdfviewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("RenderPdfPages")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/RenderPdfPages")]
        //Post action for processing the PDF documents  
        public IActionResult RenderPdfPages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetPage(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("RenderPdfTexts")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/RenderPdfTexts")]
        //Post action for processing the PDF texts  
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetDocumentText(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("RenderThumbnailImages")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/RenderThumbnailImages")]
        //Post action for rendering the thumbnail images
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        [AcceptVerbs("Post")]
        [HttpPost("RenderTaggedContent")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/RenderTaggedContent")]
        //Post action for processing the PDF texts  
        public IActionResult RenderTaggedContent([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetTaggedElements(jsonObject);
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(jsonResult));
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("RenderAnnotationComments")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/RenderAnnotationComments")]
        //Post action for rendering the annotations
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("ExportAnnotations")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/ExportAnnotations")]
        //Post action to export annotations
        public IActionResult ExportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
            return Content(jsonResult);
        }

        [AcceptVerbs("Post")]
        [HttpPost("ImportAnnotations")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/ImportAnnotations")]
        //Post action to import annotations
        public IActionResult ImportAnnotations([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = string.Empty;
            object JsonResult;
            if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            {
                string documentPath = GetDocumentPath(jsonObject["fileName"]);
                if (!string.IsNullOrEmpty(documentPath))
                {
                    jsonResult = System.IO.File.ReadAllText(documentPath);
                    string[] searchStrings = { "textMarkupAnnotation", "measureShapeAnnotation", "freeTextAnnotation", "stampAnnotations", "signatureInkAnnotation", "stickyNotesAnnotation", "signatureAnnotation", "AnnotationType" };
                    bool isnewJsonFile = !searchStrings.Any(jsonResult.Contains);
                    if (isnewJsonFile)
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        jsonResult = JsonConvert.SerializeObject(JsonResult);
                    }
                }
                else
                {
                    return this.Content(jsonObject["document"] + " is not found");
                }
            }
            else
            {
                string extension = System.IO.Path.GetExtension(jsonObject["importedData"]);
                if (extension != ".xfdf")
                {
                    JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                    return Content(JsonConvert.SerializeObject(JsonResult));
                }
                else
                {
                    string documentPath = GetDocumentPath(jsonObject["importedData"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        return Content(JsonConvert.SerializeObject(JsonResult));
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
            }
            return Content(jsonResult);
        }

        [AcceptVerbs("Post")]
        [HttpPost("ExportFormFields")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/ExportFormFields")]
        //Post action to export form fields
        public IActionResult ExportFormFields([FromBody] Dictionary<string, string> jsonObject)

        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string jsonResult = pdfviewer.ExportFormFields(jsonObject);
            return Content(jsonResult);
        }

        [AcceptVerbs("Post")]
        [HttpPost("ImportFormFields")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/ImportFormFields")]
        //Post action to import form fields
        public IActionResult ImportFormFields([FromBody] Dictionary<string, string> jsonObject)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            if (jsonObject["formFieldDataFormat"] == "Json")
            {
                try
                {
                    jsonObject["data"] = JsonConvert.DeserializeObject(jsonObject["data"]).ToString();
                }
                catch (Exception e)
                {
                    jsonObject["data"] = jsonObject["data"];
                    jsonObject["data"] = GetDocumentPath(jsonObject["data"]);
                }

            }
            object jsonResult = pdfviewer.ImportFormFields(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        [AcceptVerbs("Post")]
        [HttpPost("Unload")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/Unload")]
        //Post action for unloading and disposing the PDF document resources  
        public IActionResult Unload([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
        }


        [HttpPost("Download")]
        /*        [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]*/
        [Route("[controller]/Download")]
        //Post action for downloading the PDF documents
        public IActionResult Download([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            return Content(documentBase);
        }

        [HttpPost("PrintImages")]
        //[Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]
        [Route("[controller]/PrintImages")]
        //Post action for printing the PDF documents
        public IActionResult PrintImages([FromBody] Dictionary<string, string> jsonObject)
        {
            //Initialize the PDF Viewer object with memory cache object
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            object pageImage = pdfviewer.GetPrintImage(jsonObject);
            return Content(JsonConvert.SerializeObject(pageImage));
        }

        //Returns the PDF document path
        private string GetDocumentPath(string document)
        {
            string documentPath = string.Empty;
            if (!System.IO.File.Exists(document))
            {
                var path = _hostingEnvironment.ContentRootPath;
                if (System.IO.File.Exists(path + "/Data/" + document))
                    documentPath = path + "/Data/" + document;
            }
            else
            {
                documentPath = document;
            }
            Console.WriteLine(documentPath);
            return documentPath;
        }

        //GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}

