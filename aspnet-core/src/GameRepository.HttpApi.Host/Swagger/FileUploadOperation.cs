using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameRepository.Swagger
{
    public class FileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription;
            var actionParameters = descriptor.ActionDescriptor.Parameters;

            foreach (var parameter in actionParameters)
            {
                if (IsFileParameter(parameter.ParameterType))
                {
                    // 清除自动生成的参数
                    operation.Parameters.Clear();

                    // 设置请求体为 multipart/form-data
                    operation.RequestBody = new OpenApiRequestBody
                    {
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["multipart/form-data"] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Type = "object",
                                    Properties = GetFileFormProperties(actionParameters)
                                }
                            }
                        },
                        Required = true
                    };
                    Console.WriteLine("FileUploadFilter is applying...");
                    // 确保操作标记为已处理，避免重复处理
                    return;
                }
            }
        }

        private bool IsFileParameter(Type type)
        {
            return type == typeof(IFormFile) ||
                   type == typeof(List<IFormFile>) ||
                   type.Name == "IFormFile"; // 处理可能的接口类型
        }

        private Dictionary<string, OpenApiSchema> GetFileFormProperties(IList<Microsoft.AspNetCore.Mvc.Abstractions.ParameterDescriptor> parameters)
        {
            var properties = new Dictionary<string, OpenApiSchema>();

            foreach (var param in parameters)
            {
                if (param.ParameterType == typeof(IFormFile))
                {
                    properties.Add(param.Name, new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    });
                }
                else if (param.ParameterType == typeof(List<IFormFile>))
                {
                    properties.Add(param.Name, new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    });
                }
                else
                {
                    // 处理其他表单参数
                    properties.Add(param.Name, new OpenApiSchema
                    {
                        Type = GetSchemaType(param.ParameterType)
                    });
                }
            }

            return properties;
        }

        private string GetSchemaType(Type type)
        {

            if (type == typeof(int) || type == typeof(int?)) return "integer";
            if (type == typeof(bool) || type == typeof(bool?)) return "boolean";
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return "string";
            if (type == typeof(string)) return "string"; // 添加对 string 的支持
            return "string";
        }
    }
}
