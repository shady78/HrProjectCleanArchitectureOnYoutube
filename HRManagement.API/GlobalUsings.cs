global using FluentValidation;
global using HRManagement.API.Common.Responses;
global using HRManagement.API.Filters;
global using HRManagement.API.GlobalHandling;
global using HRManagement.Application;
global using HRManagement.Application.Common.Exceptions;
global using HRManagement.Application.Common.Interfaces;
global using HRManagement.Application.Common.Settings;
global using HRManagement.Application.DTOs.Auth;
global using HRManagement.Application.Services.Auth.Interfaces;
global using HRManagement.Infrastructure;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.IdentityModel.Tokens;
global using Serilog;
global using Serilog.Events;
global using System.Text;
global using HRManagement.API.Authorization;
global using HRManagement.Application.Common;
global using HRManagement.Application.DTOs.Departments;
global using HRManagement.Application.Services.Departments;
global using HRManagement.Domain.Entities;
global using Microsoft.AspNetCore.Authorization;





global using HRManagement.Application.DTOs.Roles;
global using HRManagement.Application.Services.Roles.Services;

