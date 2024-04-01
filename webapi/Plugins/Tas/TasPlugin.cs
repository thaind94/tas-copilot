// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CopilotChat.WebApi.Utilities;
using Microsoft.SemanticKernel;

namespace CopilotChat.WebApi.Plugins.Tas;

public class TasPlugin
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TasPlugin(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // [KernelFunction, Description("Retrieve list available roles (include role name and role id) to create a module")]
    // public async Task<string> GetRolesToCreateModule()
    // {
    //     var httpClient = _httpClientFactory.CreateClient("TAS");
    //     try
    //     {
    //         var httpResponseMessage = await httpClient.GetAsync(new Uri("api/modules/roles/", UriKind.Relative));
    //         if (httpResponseMessage.IsSuccessStatusCode)
    //         {
    //             await using var contentStream =
    //                 await httpResponseMessage.Content.ReadAsStreamAsync();
    //
    //             var response = await JsonSerializer.DeserializeAsync<RoleModuleResponse>(contentStream);
    //             if (response?.Success == true)
    //             {
    //                 return JsonSerializer.Serialize(response.Data.Roles);
    //             }
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    //
    //     return "";
    // }
    //
    // [KernelFunction, Description("Retrieve list available roles (include role name and role id) to create a project")]
    // public async Task<string> GetRolesToCreateProject()
    // {
    //     var httpClient = _httpClientFactory.CreateClient("TAS");
    //     try
    //     {
    //         var httpResponseMessage = await httpClient.GetAsync(new Uri("api/projects/roles/", UriKind.Relative));
    //         if (httpResponseMessage.IsSuccessStatusCode)
    //         {
    //             await using var contentStream =
    //                 await httpResponseMessage.Content.ReadAsStreamAsync();
    //
    //             var response = await JsonSerializer.DeserializeAsync<RoleProjectResponse>(contentStream);
    //             if (response?.Success == true)
    //             {
    //                 return JsonSerializer.Serialize(response.Data.Roles);
    //             }
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    //
    //     return "";
    // }

    [KernelFunction,
     Description("Get list emails for module creation.")]
    public async Task<string> GetEmailForModuleCreation()
    {
        var httpClient = _httpClientFactory.CreateClient("TAS");
        try
        {
            var httpResponseMessage = await httpClient.GetAsync(new Uri("api/modules/members/", UriKind.Relative));
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                await using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await JsonSerializer.DeserializeAsync<MemberModuleResponse>(contentStream);
                return JsonSerializer.Serialize(response.Items);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return "";
    }

    [KernelFunction,
     Description("Create a module. Ask user to name the module. Ask user choose email from available email list.")]
    public async Task<string> CreateModule(
        [Description("email to assign as module's owner role")]
        string email,
        [Description("Unique name of the module")]
        string moduleName = "")
    {
        if (string.IsNullOrEmpty(moduleName))
        {
            return "Create module failed: Module name is required.";
        }

        if (!(await ValidateModuleName(moduleName)))
        {
            return $"Create module failed: Module name {moduleName} is already existed. Please choose another name.";
        }

        var userId = await GetUserIdByEmail(email);
        if (string.IsNullOrEmpty(userId))
        {
            return $"Create module failed: User with email {email} not found.";
        }

        var httpClient = _httpClientFactory.CreateClient("TAS");
        try
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(new Uri("api/modules/", UriKind.Relative), new
            {
                name = moduleName,
                description = "Test Module Description",
                status = "Active",
                members = new[]
                {
                    new
                    {
                        userId,
                        roleId = "c441c5bd-a451-467b-9d42-89c9bb9d09f4"
                    }
                }
            });
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CreateModuleResponse>(content);
            if (response != null)
            {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Serialize(response.Data);
                    return result;
                }

                return $"Create module failed: {response.Message}";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return $"Create module failed: {e.Message}";
        }

        return "";
    }

    private async Task<string> GetUserIdByEmail(string email)
    {
        var httpClient = _httpClientFactory.CreateClient("TAS");
        try
        {
            var uri = PluginUtils.BuildUrlWithQueryStringUsingUriBuilder(
                $"{httpClient.BaseAddress.AbsoluteUri}/api/users/",
                new Dictionary<string, string>
                {
                    {"email", email},
                    {"page", "1"},
                    {"pageSize", "1"},
                    {"isDeleted", "false"},
                    {"status", "Active"}
                });
            var response = await httpClient.GetFromJsonAsync<ListUserResponse>(uri);
            if (response != null && response.Items.Any())
            {
                return response.Items.First().Id.ToString();
            }

            return string.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return string.Empty;
        }
    }

    private async Task<bool> ValidateModuleName(string moduleName)
    {
        var httpClient = _httpClientFactory.CreateClient("TAS");
        try
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync(
                new Uri("api/modules/validate_module_name", UriKind.Relative), new
                {
                    moduleName
                });
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<BaseResponse>(content);
            return response?.Success == true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
