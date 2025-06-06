using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using AutoMapper;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Settings;
using GymRadar.Repository.Interface;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GymRadar.Service.Implement
{
    public class UploadService :  IUploadService
    {
        private readonly AppWriteSettings _appWriteSetting;
        private readonly Client _client;
        private readonly Storage _storage;
        private readonly HttpClient _httpClient;

        public UploadService(IOptions<AppWriteSettings> appWriteSetting, HttpClient httpClient)
        {
            _appWriteSetting = appWriteSetting.Value;
            _client = new Client()
            .SetEndpoint(_appWriteSetting.EndPoint)
            .SetProject(_appWriteSetting.ProjectId)
            .SetKey(_appWriteSetting.APIKey);

            _storage = new Storage(_client);
            _httpClient = httpClient;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only image files are allowed (.jpg, .jpeg, .png, .gif, .bmp).");


            try
            {
                using var formContent = new MultipartFormDataContent();

                formContent.Add(new StringContent("unique()"), "fileId");

                using var fileStream = file.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                formContent.Add(fileContent, "file", file.FileName);

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{_appWriteSetting.EndPoint}/storage/buckets/{_appWriteSetting.Bucket}/files");

                request.Headers.Add("X-Appwrite-Project", _appWriteSetting.ProjectId);
                request.Headers.Add("X-Appwrite-Key", _appWriteSetting.APIKey);
                request.Content = formContent;


                var response = await _httpClient.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var jsonDoc = JsonDocument.Parse(responseContent);
                    var fileId = jsonDoc.RootElement.GetProperty("$id").GetString();

                    var fileUrl = $"{_appWriteSetting.EndPoint}/storage/buckets/{_appWriteSetting.Bucket}/files/{fileId}/view?project={_appWriteSetting.ProjectId}";
                    return fileUrl;
                }
                else
                {
                    throw new Exception($"Upload failed with status {response.StatusCode}: {responseContent}");
                }
            }
            catch (AppwriteException ex)
            {
                throw new Exception($"Upload failed: {ex.Message}");
            }
        }
    }
}
